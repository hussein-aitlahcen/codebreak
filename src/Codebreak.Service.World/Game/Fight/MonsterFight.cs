using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Network;
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
    public sealed class MonsterFight : FightBase, IDisposable
    {        
        public const int MONSTERFIGHT_START_TIMEOUT = 60000;
        public const int MONSTERFIGHT_TURN_TIME = 30000;

        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity Character
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public MonsterGroupEntity MonsterGroup
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
        public MonsterFight(MapInstance map, long id, CharacterEntity character, MonsterGroupEntity monsterGroup)
            : base(FightTypeEnum.TYPE_PVM, map, id, character.Id, 0, character.CellId, monsterGroup.Id, monsterGroup.Monsters.First().Grade.GetTemplate().Alignment, monsterGroup.CellId, MONSTERFIGHT_START_TIMEOUT, MONSTERFIGHT_TURN_TIME)
        {
            Character = character;
            MonsterGroup = monsterGroup;

            JoinFight(Character, Team0);
            foreach(var monster in monsterGroup.Monsters)            
                JoinFight(monster, Team1);           

            base.Start();
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
            foreach (var fighter in m_winnerTeam.Fighters)
            {
                Result.AddResult(fighter, true);
            }
            foreach (var fighter in m_loserTeam.Fighters)
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
            message.Append(UpdateTime).Append(';');
            message.Append("0,-1,");
            message.Append(Team0.AliveFighters.Count()).Append(';');
            message.Append("1,");
            message.Append(MonsterGroup.Monsters.ElementAt(0).Grade.GetTemplate().Alignment).Append(',');
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
                _serializedFlag.Append('1').Append(';');
                _serializedFlag.Append(MonsterGroup.Monsters.ElementAt(0).Grade.GetTemplate().Alignment);
            }

            message.Append(_serializedFlag.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            Character = null;
            MonsterGroup = null;
            
            _serializedFlag.Clear();
            _serializedFlag = null;

            base.Dispose();
        }
    }
}
