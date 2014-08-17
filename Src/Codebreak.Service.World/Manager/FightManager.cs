using System.Collections.Generic;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Map;

namespace Codebreak.Service.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FightManager : Singleton<FightManager>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, FightBase> _fightList;
        private Dictionary<int, List<FightBase>> _fightByMap;
        private long _fightId = 1;

        /// <summary>
        /// 
        /// </summary>
        public FightManager()
        {
            _fightList = new Dictionary<long, FightBase>();
            _fightByMap = new Dictionary<int, List<FightBase>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        public void CreateChallenger(MapInstance map, CharacterEntity attacker, CharacterEntity defender)
        {
            Add(new ChallengerFight(map, _fightId++, attacker, defender));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        public List<FightBase> GetFights(int mapId)
        {
            if (_fightByMap.ContainsKey(mapId))
                return _fightByMap[mapId];
            return new List<FightBase>();
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
            _fightList.Add(fight.Id, fight);
            if (!_fightByMap.ContainsKey(fight.Map.Id))
                _fightByMap.Add(fight.Map.Id, new List<FightBase>());
            _fightByMap[fight.Map.Id].Add(fight);

            fight.Map.AddUpdatable(fight);
            fight.Map.Dispatch(WorldMessage.FIGHT_COUNT(_fightByMap[fight.Map.Id].Count));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        public void Remove(FightBase fight)
        {
            _fightList.Remove(fight.Id);
            _fightByMap[fight.Map.Id].Remove(fight);

            fight.ClearMessages();
            fight.Map.RemoveUpdatable(fight);
            fight.Map.Dispatch(WorldMessage.FIGHT_COUNT(_fightByMap[fight.Map.Id].Count));
        }
    }
}
