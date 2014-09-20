using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TaxCollectorFight : FightBase, IDisposable
    {
        public const int PVT_TELEPORT_DEFENDERS_TIMEOUT = 45000;
        public const int PVT_START_TIMEOUT = 60000;
        public const int PVT_TURN_TIME = 30000;

        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity Attacker
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public TaxCollectorEntity TaxCollector
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanDefend
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        private StringBuilder _serializedFlag;

        /// <summary>
        /// 
        /// </summary>
        public TaxCollectorFight(MapInstance map, long id, CharacterEntity attacker, TaxCollectorEntity taxCollector)
            : base(FightTypeEnum.TYPE_PVT, map, id, attacker.Id, attacker.CellId, taxCollector.Id, taxCollector.CellId, PVT_START_TIMEOUT, PVT_TURN_TIME)
        {
            CanDefend = true;
            Attacker = attacker;
            TaxCollector = taxCollector;
            
            JoinFight(Attacker, Team0);
            JoinFight(TaxCollector, Team1);

            base.AddTimer(PVT_TELEPORT_DEFENDERS_TIMEOUT, TeleportDefenders, true);
            base.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void TeleportDefenders()
        {
            CanDefend = false;

            foreach(var defender in TaxCollector.Defenders)
            {
                var character = defender.Character;
                if(character != null)
                {
                    character.AddMessage(() =>
                    {
                        character.StopAction(GameActionTypeEnum.TAXCOLLECTOR_AGGRESSION);

                        JoinFight(character, Team1);
                    });
                }
            }

            TaxCollector.Defenders.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public override void OnFighterJoin(FighterBase fighter)
        {
            TaxCollector.Guild.TaxCollectorAttackerJoin(TaxCollector.Id, fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <returns></returns>
        public override bool CanJoin(CharacterEntity character)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="kick"></param>
        /// <returns></returns>
        public override FightActionResultEnum FightQuit(FighterBase fighter, bool kick = false)
        {
            if (LoopState == FightLoopStateEnum.STATE_WAIT_END || LoopState == FightLoopStateEnum.STATE_ENDED)
                return FightActionResultEnum.RESULT_NOTHING;

            switch (State)
            {
                case FightStateEnum.STATE_PLACEMENT:
                    if (base.TryKillFighter(fighter, fighter.Id, true, true) == FightActionResultEnum.RESULT_END)
                    {
                        return FightActionResultEnum.RESULT_END;
                    }
                    else
                    {
                        if (kick)
                        {
                            fighter.Fight.Dispatch(WorldMessage.FIGHT_FLAG_UPDATE(OperatorEnum.OPERATOR_REMOVE, fighter.Team.LeaderId, fighter));
                            fighter.Fight.Dispatch(WorldMessage.GAME_MAP_INFORMATIONS(OperatorEnum.OPERATOR_REMOVE, fighter));
                            fighter.LeaveFight(true);
                            fighter.Dispatch(WorldMessage.FIGHT_LEAVE());
                        }

                        TaxCollector.Guild.TaxColectorAttackerLeave(TaxCollector.Id, fighter);

                        return FightActionResultEnum.RESULT_NOTHING;
                    }

                case FightStateEnum.STATE_FIGHTING:
                    if (fighter.IsSpectating)
                    {
                        fighter.LeaveFight(kick);
                        fighter.Dispatch(WorldMessage.FIGHT_LEAVE());

                        return FightActionResultEnum.RESULT_NOTHING;
                    }

                    if (TryKillFighter(fighter, fighter.Id, true, true) != FightActionResultEnum.RESULT_END)
                    {
                        fighter.LeaveFight();
                        fighter.Dispatch(WorldMessage.FIGHT_LEAVE());

                        return FightActionResultEnum.RESULT_DEATH;
                    }

                    return FightActionResultEnum.RESULT_END;
            }

            return FightActionResultEnum.RESULT_NOTHING;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitEndCalculation()
        {
            foreach (var fighter in _winnerTeam.Fighters)
            {
                Result.AddResult(fighter, true);
            }
            foreach (var fighter in _loserTeam.Fighters)
            {
                Result.AddResult(fighter, false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ApplyEndCalculation()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void SerializeAs_FightList(StringBuilder message)
        {
            message.Append(Id.ToString()).Append(';');
            message.Append(UpdateTime).Append(';'); // TODO : Time;
            message.Append("0,-1,"); // TODO : Alignement etc
            message.Append(Team0.AliveFighters.Count()).Append(';');
            message.Append("3,-1,"); // TODO : Valeur monster "," Alignement monstre
            message.Append(Team1.AliveFighters.Count()).Append(';');
            message.Append('|');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void SerializeAs_FightFlag(StringBuilder message)
        {
            if (_serializedFlag == null)
            {
                _serializedFlag = new StringBuilder();
                _serializedFlag.Append(Id).Append(';');
                _serializedFlag.Append((int)Type).Append('|');
                _serializedFlag.Append(Team0.LeaderId).Append(';');
                _serializedFlag.Append(Team0.FlagCellId).Append(';');
                _serializedFlag.Append('0').Append(';');
                _serializedFlag.Append("-1").Append('|'); // neutral
                _serializedFlag.Append(Team1.LeaderId).Append(';');
                _serializedFlag.Append(Team1.FlagCellId).Append(';');
                _serializedFlag.Append('3').Append(';');
                _serializedFlag.Append("-1");
            }

            message.Append(_serializedFlag.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            Attacker = null;
            TaxCollector = null;

            _serializedFlag.Clear();
            _serializedFlag = null;

            base.Dispose();
        }
    }
}
