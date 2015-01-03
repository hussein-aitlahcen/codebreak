using Codebreak.Framework.Database;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("characterwaypoint")]
    public sealed class CharacterWaypointDAO : DataAccessObject<CharacterWaypointDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long CharacterId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int MapId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private MapInstance m_mapInstance;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MapInstance GetMap()
        {
            if (m_mapInstance == null)
                m_mapInstance = MapManager.Instance.GetById(MapId);
            return m_mapInstance;
        }
    }
}
