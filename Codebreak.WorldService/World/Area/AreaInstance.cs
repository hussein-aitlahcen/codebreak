using Codebreak.Framework.Generic;
using Codebreak.WorldService.World.Database.Structure;
using Codebreak.WorldService.World.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Area
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AreaInstance : MessageDispatcher
    {
        private AreaDAO _areaRecord;
        private SuperAreaInstance _superArea;
        
        /// <summary>
        /// 
        /// </summary>
        public SuperAreaInstance SuperArea
        {
            get
            {
                if (_superArea == null)
                    _superArea = AreaManager.Instance.GetSuperArea(_areaRecord.SuperAreaId);
                return _superArea;
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
            _areaRecord = record;

            IOQueue = new BasicTaskProcessor("Area[" + record.Name + "] Task Queue");
            IOQueue.AddUpdatable(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public override void AddHandler(Action<string> method)
        {
            IOQueue.AddMessage(() => AddMessage(() => base.AddHandler(method)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public override void RemoveHandler(Action<string> method)
        {
            IOQueue.AddMessage(() => AddMessage(() => base.RemoveHandler(method)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void Dispatch(string message)
        {
            IOQueue.AddMessage(() => AddMessage(() => base.Dispatch(message)));
        }
    }
}
