using System.Collections.Generic;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Network;

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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        public void StartChallenge(CharacterEntity attacker, CharacterEntity defender)
        {
            Add(new ChallengerFight(m_map, m_fightId++, attacker, defender));            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="monsterGroup"></param>
        public void StartMonsterFight(CharacterEntity character, MonsterGroupEntity monsterGroup)
        {
            Add(new MonsterFight(m_map, m_fightId++, character, monsterGroup));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="taxCollector"></param>
        public void StartTaxCollectorAggression(CharacterEntity attacker, TaxCollectorEntity taxCollector)
        {
            Add(new TaxCollectorFight(m_map, m_fightId++, attacker, taxCollector));
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
