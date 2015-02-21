using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MapTriggerRepository : Repository<MapTriggerRepository, MapTriggerDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, List<MapTriggerDAO>> m_triggersByMap;

        /// <summary>
        /// 
        /// </summary>
        public MapTriggerRepository()
        {
            m_triggersByMap = new Dictionary<int, List<MapTriggerDAO>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapTrigger"></param>
        public override void OnObjectAdded(MapTriggerDAO mapTrigger)
        {
            if (!m_triggersByMap.ContainsKey(mapTrigger.MapId))
                m_triggersByMap.Add(mapTrigger.MapId, new List<MapTriggerDAO>());
            m_triggersByMap[mapTrigger.MapId].Add(mapTrigger);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapTrigger"></param>
        public override void OnObjectRemoved(MapTriggerDAO mapTrigger)
        {
            m_triggersByMap.Remove(mapTrigger.MapId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        public List<MapTriggerDAO> GetTriggers(int mapId)
        {
            if (m_triggersByMap.ContainsKey(mapId))
                return m_triggersByMap[mapId];
            return new List<MapTriggerDAO>();
        }


        public override void UpdateAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }

        public override void DeleteAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }

        public override void InsertAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }
    }
}
