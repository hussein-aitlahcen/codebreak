using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Map;
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
    public sealed class AlignmentFight : AbstractFight
    {
        /// <summary>
        /// 
        /// </summary>
        public MonsterGroupEntity Monsters
        {
            get;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private StringBuilder m_serializedFlag;

       /// <summary>
       /// 
       /// </summary>
       /// <param name="map"></param>
       /// <param name="id"></param>
       /// <param name="aggressor"></param>
       /// <param name="victim"></param>
        public AlignmentFight(MapInstance map, long id, AbstractFighter aggressor, CharacterEntity victim)
            : base(FightTypeEnum.TYPE_AGGRESSION, map, id, aggressor.Id, aggressor.AlignmentId, aggressor.CellId, victim.Id, victim.AlignmentId, victim.CellId, WorldConfig.AGGRESSION_START_TIMEOUT, WorldConfig.AGGRESSION_TURN_TIME, false, true, new HonorGainBehavior())
        {
            IsNeutralAgression = victim.AlignmentId == (int)AlignmentTypeEnum.ALIGNMENT_NEUTRAL;

            JoinFight(aggressor, Team0);
            JoinFight(victim, Team1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="id"></param>
        /// <param name="monsters"></param>
        /// <param name="victim"></param>
        public AlignmentFight(MapInstance map, long id, MonsterGroupEntity monsters, CharacterEntity victim)
            : base(FightTypeEnum.TYPE_AGGRESSION, map, id, monsters.Id, monsters.AlignmentId, monsters.CellId, victim.Id, victim.AlignmentId, victim.CellId, WorldConfig.AGGRESSION_START_TIMEOUT, WorldConfig.AGGRESSION_TURN_TIME, false, true)
        {
            IsNeutralAgression = victim.AlignmentId == (int)AlignmentTypeEnum.ALIGNMENT_NEUTRAL;
            Monsters = monsters;

            foreach (var monster in monsters.Monsters)
                JoinFight(monster, Team0);

            JoinFight(victim, Team1);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnFightStart()
        {
            if (IsNeutralAgression && Monsters == null)
            {
                var aggressors = Team0.AliveFighters.ToList();
                if (aggressors.Any())
                {
                    var averageLevel = (int)aggressors.Average(aggressor => aggressor.Level);
                    var knighLevel = 0;

                    if (averageLevel < 50)
                        knighLevel = 0;
                    else if (averageLevel < 80)
                        knighLevel = 1;
                    else if (averageLevel < 110)
                        knighLevel = 2;
                    else if (averageLevel < 140)
                        knighLevel = 3;
                    else if (averageLevel < 170)
                        knighLevel = 4;
                    else
                        knighLevel = 5;

                    var knight = MonsterRepository.Instance.GetById(WorldConfig.AGGRESSION_KNGIHT_MONSTER_ID);
                    if (knight != null)
                        if (knight.Grades.Count() > knighLevel)
                            if (Team1.FreePlace != null)
                                SummonFighter(new MonsterEntity(NextFighterId, knight.Grades.ElementAt(knighLevel)), Team1, Team1.FreePlace.Id);
                }

                foreach (var character in Team0.Fighters.OfType<CharacterEntity>())
                    character.AddDishonour(1);
            }

            base.OnFightStart();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="team"></param>
        public override void OnCharacterJoin(CharacterEntity character, FightTeam team)
        {
            if (!IsNeutralAgression)
                character.EnableAlignment();
            else
                if (team.AlignmentId != (int)AlignmentTypeEnum.ALIGNMENT_NEUTRAL)
                    character.EnableAlignment();
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
            }

            return FightActionResultEnum.RESULT_NOTHING;
        }

        protected override void FightEnd()
        {
            if (Monsters != null)
            {
                if (WinnerTeam == Team1)
                {
                    Map.SpawnMonsters();
                }
                else
                {
                    Map.SpawnEntity(Monsters);
                }
            }
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
            message.Append("0").Append(',');
            message.Append(Team0.AlignmentId).Append(",");
            message.Append(Team0.AliveFighters.Count()).Append(';');
            message.Append("0,");
            message.Append(Team1.AlignmentId).Append(",");
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
                m_serializedFlag = new StringBuilder();
                m_serializedFlag.Append(Id).Append(';');
                m_serializedFlag.Append((int)Type).Append('|');

                m_serializedFlag.Append(Team0.LeaderId).Append(';');
                m_serializedFlag.Append(Team0.FlagCellId).Append(';');
                m_serializedFlag.Append("0").Append(';');
                m_serializedFlag.Append(Team0.AlignmentId).Append('|');

                m_serializedFlag.Append(Team1.LeaderId).Append(';');
                m_serializedFlag.Append(Team1.FlagCellId).Append(';');
                m_serializedFlag.Append('0').Append(';');
                m_serializedFlag.Append(Team1.AlignmentId);
            }
            message.Append(m_serializedFlag.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            m_serializedFlag.Clear();
            m_serializedFlag = null;

            base.Dispose();
        }
    }
}
