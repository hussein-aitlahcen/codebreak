using Codebreak.Framework.Generic;
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
        public BasicTaskProcessor IOQueue
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        public AreaInstance(AreaDAO record)
        {
            m_areaRecord = record;

            IOQueue = new BasicTaskProcessor("Area[" + record.Name + "]");
            IOQueue.AddUpdatable(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public override void AddHandler(Action<string> method)
        {
            IOQueue.AddMessage(() => base.AddHandler(method));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public override void RemoveHandler(Action<string> method)
        {
            IOQueue.AddMessage(() => base.RemoveHandler(method));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void Dispatch(string message)
        {
            IOQueue.AddMessage(() => base.Dispatch(message));
        }
    }
}
