using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Spawn
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SpawnQueue : Updatable
    {
        /// <summary>
        /// 
        /// </summary>
        private List<MapInstance> m_maps;
        
        /// <summary>
        /// 
        /// </summary>
        private List<MonsterSpawnDAO> m_monsters;

        /// <summary>
        /// 
        /// </summary>
        private int m_counter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spawns"></param>
        public SpawnQueue(IEnumerable<MonsterSpawnDAO> spawns)
        {
            m_maps = new List<MapInstance>();
            m_monsters = new List<MonsterSpawnDAO>(spawns);

            base.AddTimer(WorldConfig.SPAWN_CHECK_INTERVAL, InternalUpdate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public void RegisterMap(MapInstance map)
        {
            AddMessage(() =>
                {
                    m_maps.Add(map);
                    m_counter += WorldConfig.SPAWN_MAX_GROUP_PER_MAP;
                });
        }

        /// <summary>
        /// 
        /// </summary>
        private void InternalUpdate()
        {
            if(m_counter > 0)
            {
                if(m_maps.Count > 0)
                {
                    while (m_counter > 0)
                    {
                        foreach (var map in m_maps)
                        {
                            map.SpawnMonsters(m_monsters);
                        }
                        m_counter--;
                    }
                }
            }
        }
    }
}
