using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.Database.Repository;

namespace Codebreak.Service.World.Game.Area
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AreaInstance : MessageDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
        private AreaDAO m_areaRecord;

        /// <summary>
        /// 
        /// </summary>
        private SuperAreaInstance m_superArea;

        /// <summary>
        /// 
        /// </summary>
        private IEnumerable<MonsterSpawnDAO> m_spawns;

        /// <summary>
        /// 
        /// </summary>
        public SuperAreaInstance SuperArea
        {
            get
            {
                if (m_superArea == null)
                    m_superArea = AreaManager.Instance.GetSuperArea(m_areaRecord.SuperAreaId);
                return m_superArea;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<MonsterSpawnDAO> Spawns
        {
            get
            {
                if (m_spawns == null)
                    m_spawns = MonsterSpawnRepository.Instance.GetById(ZoneTypeEnum.TYPE_AREA, m_areaRecord.Id);
                return m_spawns;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        public AreaInstance(AreaDAO record)
        {
            m_areaRecord = record;
        }
    }
}
