using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight.Challenge;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Game.Fight.Ending;

namespace Codebreak.Service.World.Game.Fight
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MonsterFight : AbstractFight, IDisposable
    {        
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
        private string m_serializedFlag;

        /// <summary>
        /// 
        /// </summary>
        public MonsterFight(MapInstance map, long id, CharacterEntity character, MonsterGroupEntity monsterGroup)
            : base(FightTypeEnum.TYPE_PVM, 
                  map, 
                  id, 
                  character.Id,
                  -1, 
                  character.CellId, 
                  monsterGroup.Id, 
                  -1,
                  monsterGroup.CellId, 
                  WorldConfig.PVM_START_TIMEOUT, 
                  WorldConfig.PVM_TURN_TIME, 
                  false, 
                  false, 
                  new LootMonsterBehavior())
        {
            Character = character;
            MonsterGroup = monsterGroup;
            
            JoinFight(Character, Team0);
            foreach(var monster in monsterGroup.Monsters)
                JoinFight(monster, Team1);

            foreach (var challenge in ChallengeManager.Instance.Generate(WorldConfig.PVM_CHALLENGE_COUNT))
                Team0.AddChallenge(challenge);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public override bool CanJoin(CharacterEntity character)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="kick"></param>
        /// <returns></returns>
        public override FightActionResultEnum FightQuit(CharacterEntity character, bool kick = false)
        {
            if (LoopState == FightLoopStateEnum.STATE_WAIT_END || LoopState == FightLoopStateEnum.STATE_ENDED)
                return FightActionResultEnum.RESULT_NOTHING;

            switch (State)
            {
                case FightStateEnum.STATE_PLACEMENT:
                    if (kick)
                    {
                        character.Fight.Dispatch(WorldMessage.FIGHT_FLAG_UPDATE(OperatorEnum.OPERATOR_REMOVE, character.Team.LeaderId, character));
                        character.Fight.Dispatch(WorldMessage.GAME_MAP_INFORMATIONS(OperatorEnum.OPERATOR_REMOVE, character));
                        character.EndFight(true);
                        character.Dispatch(WorldMessage.FIGHT_LEAVE());
                        return FightActionResultEnum.RESULT_NOTHING;
                    }

                    if (TryKillFighter(character, character, true, true) != FightActionResultEnum.RESULT_END)
                    {
                        character.EndFight();
                        character.Dispatch(WorldMessage.FIGHT_LEAVE());
                        return FightActionResultEnum.RESULT_DEATH;
                    }

                    return FightActionResultEnum.RESULT_END;

                case FightStateEnum.STATE_FIGHTING:
                    if (character.IsSpectating)
                    {
                        character.EndFight(kick);
                        character.Dispatch(WorldMessage.FIGHT_LEAVE());

                        return FightActionResultEnum.RESULT_NOTHING;
                    }

                    if (TryKillFighter(character, character, true, true) != FightActionResultEnum.RESULT_END)
                    {
                        Result.AddResult(character);
                        character.EndFight();
                        character.Dispatch(WorldMessage.FIGHT_LEAVE());

                        return FightActionResultEnum.RESULT_DEATH;
                    }

                    return FightActionResultEnum.RESULT_END;
            }

            return FightActionResultEnum.RESULT_NOTHING;
        }
        
        /// <summary>
        /// 
        /// </summary>
        protected override void FightEnd()
        {
            if (WinnerTeam == Team0)
                Map.SpawnMonsters();
            else
                Map.SpawnEntity(MonsterGroup);
            base.FightEnd();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void SerializeAs_FightList(StringBuilder message)
        {
            message.Append(Id.ToString()).Append(';');
            message.Append(UpdateTime).Append(';');
            message.Append("0,");
            message.Append(Team0.AlignmentId).Append(',');
            message.Append(Team0.AliveFighters.Count()).Append(';');
            message.Append("1,");
            message.Append(Team0.AlignmentId).Append(',');
            message.Append(Team1.AliveFighters.Count()).Append(';');
            message.Append('|');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void SerializeAs_FightFlag(StringBuilder message)
        {
            if (m_serializedFlag == null)
            {
                var m_serialized = new StringBuilder();
                m_serialized.Append(Id).Append(';');
                m_serialized.Append((int)Type).Append('|');
                m_serialized.Append(Team0.LeaderId).Append(';');
                m_serialized.Append(Team0.FlagCellId).Append(';');
                m_serialized.Append('0').Append(';');
                m_serialized.Append(Team0.AlignmentId).Append('|');
                m_serialized.Append(Team1.LeaderId).Append(';');
                m_serialized.Append(Team1.FlagCellId).Append(';');
                m_serialized.Append("1").Append(';');
                m_serialized.Append(Team1.AlignmentId);
                m_serializedFlag = m_serialized.ToString();
            }

            message.Append(m_serializedFlag);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            Character = null;
            MonsterGroup = null;
            m_serializedFlag = null;
            base.Dispose();
        }
    }
}
