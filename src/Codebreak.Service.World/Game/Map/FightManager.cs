using System;
using System.Linq;
using System.Collections.Generic;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Game.Condition;
using Codebreak.Service.World.Game.Action;

namespace Codebreak.Service.World.Game.Map
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FightManager : MessageDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, FightBase> m_fightList;
        private List<FightActionDAO> m_fightActions;
        private MapInstance m_map;
        private long m_fightId = 1;

        /// <summary>
        /// 
        /// </summary>
        public long FightCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<FightBase> Fights
        {
            get
            {
                return m_fightList.Values;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public FightManager(MapInstance map)
        {
            m_map = map;
            m_map.AddUpdatable(this);
            m_map.SubArea.AddHandler(base.Dispatch);
            m_fightList = new Dictionary<long, FightBase>();
            m_fightActions = new List<FightActionDAO>(FightActionRepository.Instance.GetById(ZoneTypeEnum.TYPE_MAP, m_map.Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanStartFight(CharacterEntity character)
        {
            if (m_map.FightTeam0Cells.Count == 0 || m_map.FightTeam1Cells.Count == 0)
            {
                character.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Unable to start fight withouth fightCells"));
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        public void StartChallenge(CharacterEntity attacker, CharacterEntity defender)
        {
            if(CanStartFight(attacker))
                Add(new ChallengerFight(m_map, m_fightId++, attacker, defender));            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggressor"></param>
        /// <param name="victim"></param>
        public void StartAggression(CharacterEntity aggressor, CharacterEntity victim)
        {
            if (CanStartFight(aggressor))
                Add(new AlignmentFight(m_map, m_fightId++, aggressor, victim));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggressor"></param>
        /// <param name="victim"></param>
        public bool StartAggression(MonsterGroupEntity monsters, CharacterEntity victim)
        {
            if (CanStartFight(victim))
            {
                monsters.StopAction(GameActionTypeEnum.MAP);
                Add(new AlignmentFight(m_map, m_fightId++, monsters, victim));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="monsterGroup"></param>
        public bool StartMonsterFight(CharacterEntity character, MonsterGroupEntity monsterGroup)
        {
            if (!CanStartFight(character))
                return false;
            
            if (!character.CanGameAction(GameActionTypeEnum.FIGHT))
            {
                character.Dispatch(WorldMessage.IM_ERROR_MESSAGE(InformationEnum.ERROR_YOU_ARE_AWAY));
                return false;
            }

            monsterGroup.StopAction(GameActionTypeEnum.MAP);
            Add(new MonsterFight(m_map, m_fightId++, character, monsterGroup));
            
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="taxCollector"></param>
        public bool StartTaxCollectorAggression(CharacterEntity attacker, TaxCollectorEntity taxCollector)
        {
            if (!CanStartFight(attacker))
                return false;

            Add(new TaxCollectorFight(m_map, m_fightId++, attacker, taxCollector));

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fightType"></param>
        /// <param name="state"></param>
        /// <param name="characters"></param>
        public void ExecuteFightActions(FightTypeEnum fightType, FightStateEnum state, CharacterEntity character)
        {
            foreach(var fightAction in m_fightActions.Where(faction => faction.Fight == fightType && faction.State == state))
            {
                if (ConditionParser.Instance.Check(fightAction.Conditions, character))
                {
                    foreach (var action in fightAction.ActionsList)
                    {
                        ActionEffectManager.Instance.ApplyEffect(character, action.Key, action.Value);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fightId"></param>
        /// <returns></returns>
        public FightBase GetFight(long fightId)
        {
            if (m_fightList.ContainsKey(fightId))
                return m_fightList[fightId];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        private void Add(FightBase fight)
        {
            FightCount++;
            m_fightList.Add(fight.Id, fight);
            m_map.Dispatch(WorldMessage.FIGHT_COUNT(FightCount));
            base.AddHandler(fight.Dispatch);
            base.AddUpdatable(fight);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        public void Remove(FightBase fight)
        {
            FightCount--;
            m_fightList.Remove(fight.Id);
            m_map.Dispatch(WorldMessage.FIGHT_COUNT(FightCount));
            base.RemoveHandler(fight.Dispatch);
            base.RemoveUpdatable(fight);
        }
    }
}
