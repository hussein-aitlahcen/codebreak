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
    public sealed class MapRepository : Repository<MapRepository, MapTemplateDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, MapTemplateDAO> m_mapById;

        /// <summary>
        /// 
        /// </summary>
        public MapRepository()
        {
            m_mapById = new Dictionary<int, MapTemplateDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public override void OnObjectAdded(MapTemplateDAO map)
        {
            m_mapById.Add(map.Id, map);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public override void OnObjectRemoved(MapTemplateDAO map)
        {
            m_mapById.Remove(map.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<MapTemplateDAO> GetMaps()
        {
            return _dataObjects;
        }
    }
}
