using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight.Challenges;
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
    public sealed class ChallengerFight : FightBase
    {
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
        public CharacterEntity Defender
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
        public ChallengerFight(MapInstance map, long id, CharacterEntity attacker, CharacterEntity defender)
            : base(FightTypeEnum.TYPE_CHALLENGE, map, id, attacker.Id, attacker.CellId, defender.Id, defender.CellId, 60 * 1000, 30 * 1000, true)
        {
            Attacker = attacker;
            Defender = defender;

            JoinFight(Attacker, Team0);
            JoinFight(Defender, Team1);

            base.Team0.AddChallenge(ChallengeManager.Instance.Generate());
            base.Team1.AddChallenge(ChallengeManager.Instance.Generate());
            base.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <returns></returns>
        public override bool CanJoin(FighterBase fighter)
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
                    if (fighter.IsLeader)
                    {
                        foreach (var teamFighter in fighter.Team.Fighters)
                        {
                            if (base.TryKillFighter(teamFighter, teamFighter.Id, true, true) == FightActionResultEnum.RESULT_END)
                            {
                                return FightActionResultEnum.RESULT_END;
                            }
                        }

                        return FightActionResultEnum.RESULT_END;
                    }
                    else
                    {
                        fighter.Fight.Dispatch(WorldMessage.FIGHT_FLAG_UPDATE(OperatorEnum.OPERATOR_REMOVE, fighter.Team.LeaderId, fighter));
                        fighter.Fight.Dispatch(WorldMessage.GAME_MAP_INFORMATIONS(OperatorEnum.OPERATOR_REMOVE, fighter));
                        fighter.LeaveFight(true);
                        fighter.Dispatch(WorldMessage.FIGHT_LEAVE());

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
            message.Append("0,0,"); // TODO : Alignement etc
            message.Append(Team0.AliveFighters.Count()).Append(';');
            message.Append("0,0,"); // TODO : Valeur monster "," Alignement monstre
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
                _serializedFlag.Append("-1").Append('|');
                _serializedFlag.Append(Team1.LeaderId).Append(';');
                _serializedFlag.Append(Team1.FlagCellId).Append(';');
                _serializedFlag.Append('0').Append(';');
                _serializedFlag.Append("-1");
            }

            message.Append(_serializedFlag.ToString());
        }
    }
}
