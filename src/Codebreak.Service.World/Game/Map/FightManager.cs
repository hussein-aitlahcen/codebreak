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
        private Dictionary<long, FightBase> _fightList;
        private MapInstance _map;
        private long _fightId = 1;

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
                return _fightList.Values;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public FightManager(MapInstance map)
        {
            _map = map;
            _map.AddUpdatable(this);
            _map.SubArea.Area.AddHandler(base.Dispatch);
            _fightList = new Dictionary<long, FightBase>();   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        public void StartChallenge(CharacterEntity attacker, CharacterEntity defender)
        {
            Add(new ChallengerFight(_map, _fightId++, attacker, defender));            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="monsterGroup"></param>
        public void StartMonsteFight(CharacterEntity character, MonsterGroupEntity monsterGroup)
        {
            Add(new MonsterFight(_map, _fightId++, character, monsterGroup));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="taxCollector"></param>
        public void StartTaxCollectorAggression(CharacterEntity attacker, TaxCollectorEntity taxCollector)
        {
            Add(new TaxCollectorFight(_map, _fightId++, attacker, taxCollector));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fightId"></param>
        /// <returns></returns>
        public FightBase GetFight(long fightId)
        {
            if (_fightList.ContainsKey(fightId))
                return _fightList[fightId];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        private void Add(FightBase fight)
        {
            FightCount++;
            _fightList.Add(fight.Id, fight);
            _map.Dispatch(WorldMessage.FIGHT_COUNT(FightCount));
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
            _fightList.Remove(fight.Id);
            _map.Dispatch(WorldMessage.FIGHT_COUNT(FightCount));
            base.RemoveHandler(fight.Dispatch);
            base.RemoveUpdatable(fight);
        }
    }
}
