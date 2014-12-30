using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Game.Area
{
    /// <summary>
    /// 
    /// </summary>
    public sealed  class SuperAreaInstance : MessageDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
        private SuperAreaDAO m_superAreaRecord;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        public SuperAreaInstance(SuperAreaDAO record)
        {
            m_superAreaRecord = record;
        }
    }
}
