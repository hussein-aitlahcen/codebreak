using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Game.Area
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SubAreaInstance : MessageDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
        private SubAreaDAO m_subAreaRecord;
        
        /// <summary>
        /// 
        /// </summary>
        private AreaInstance m_area;

        /// <summary>
        /// 
        /// </summary>
        public AreaInstance Area
        {
            get
            {
                if (m_area == null)
                    m_area = AreaManager.Instance.GetArea(m_subAreaRecord.AreaId);
                return m_area;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        public SubAreaInstance(SubAreaDAO record)
        {
            m_subAreaRecord = record;
        }
    }
}
