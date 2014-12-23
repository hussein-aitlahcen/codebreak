using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repository
{
    public sealed class MapTriggerRepository : Repository<MapTriggerRepository, MapTriggerDAO>
    {
        private Dictionary<int, List<MapTriggerDAO>> _triggersByMap;

        public MapTriggerRepository()
        {
            _triggersByMap = new Dictionary<int, List<MapTriggerDAO>>();
        }

        public override void OnObjectAdded(MapTriggerDAO mapTrigger)
        {
            if (!_triggersByMap.ContainsKey(mapTrigger.MapId))
                _triggersByMap.Add(mapTrigger.MapId, new List<MapTriggerDAO>());
            _triggersByMap[mapTrigger.MapId].Add(mapTrigger);
        }

        public override void OnObjectRemoved(MapTriggerDAO mapTrigger)
        {
            _triggersByMap.Remove(mapTrigger.MapId);
        }

        public List<MapTriggerDAO> GetTriggers(int mapId)
        {
            if (_triggersByMap.ContainsKey(mapId))
                return _triggersByMap[mapId];
            return new List<MapTriggerDAO>();
        }
    }
}
