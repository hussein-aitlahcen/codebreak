using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Game.Spawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SpawnManager : Singleton<SpawnManager>
    {
        private Dictionary<SpawnTypeEnum, Dictionary<int, SpawnQueue>> m_spawnQueueById;

        /// <summary>
        /// 
        /// </summary>
        public SpawnManager()
        {
            m_spawnQueueById = new Dictionary<SpawnTypeEnum, Dictionary<int, SpawnQueue>>();
            m_spawnQueueById.Add(SpawnTypeEnum.TYPE_SUBAREA, new Dictionary<int, SpawnQueue>());
            m_spawnQueueById.Add(SpawnTypeEnum.TYPE_AREA, new Dictionary<int, SpawnQueue>());
            m_spawnQueueById.Add(SpawnTypeEnum.TYPE_SUPERAREA, new Dictionary<int, SpawnQueue>());
            m_spawnQueueById.Add(SpawnTypeEnum.TYPE_MAP, new Dictionary<int, SpawnQueue>());
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            foreach(var subArea in AreaManager.Instance.SubAreas)
            {
                if (subArea.Spawns.Count() > 0)
                {
                    var spawnQueue = new SpawnQueue(subArea.Spawns);
                    subArea.AddUpdatable(spawnQueue);
                    m_spawnQueueById[SpawnTypeEnum.TYPE_SUBAREA].Add(subArea.Id, spawnQueue);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public void RegisterMap(MapInstance map)
        {
            if (m_spawnQueueById[SpawnTypeEnum.TYPE_SUBAREA].ContainsKey(map.SubAreaId))
                m_spawnQueueById[SpawnTypeEnum.TYPE_SUBAREA][map.SubAreaId].RegisterMap(map);
        }
    }
}
