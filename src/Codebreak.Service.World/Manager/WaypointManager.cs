using Codebreak.Framework.Generic;
using Codebreak.Service.World.Game.Interactive.Type;
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
    public sealed class WaypointManager : Singleton<WaypointManager>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<int, Waypoint> m_waypointByMap;

        /// <summary>
        /// 
        /// </summary>
        public WaypointManager()
        {
            m_waypointByMap = new Dictionary<int, Waypoint>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapId"></param>
        /// <param name="zaap"></param>
        public void AddWaypoint(int mapId, Waypoint zaap)
        {
            m_waypointByMap.Add(mapId, zaap);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Waypoint> All()
        {
            return m_waypointByMap.Values;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        public Waypoint GetByMapId(int mapId)
        {
            if (m_waypointByMap.ContainsKey(mapId))
                return m_waypointByMap[mapId];
            return null;
        }
    }
}
