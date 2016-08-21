using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Database.Repository;

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
        private IEnumerable<MonsterSpawnDAO> m_spawns;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<MonsterSpawnDAO> Spawns
        {
            get
            {
                if (m_spawns == null)
                    m_spawns = MonsterSpawnRepository.Instance.GetById(ZoneTypeEnum.TYPE_SUBAREA, m_subAreaRecord.Id);
                return m_spawns;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id => m_subAreaRecord.Id;

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
        public BasicTaskProcessor IOQueue
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public TaxCollectorEntity TaxCollector
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        public SubAreaInstance(SubAreaDAO record)
        {
            m_subAreaRecord = record;
            
            IOQueue = new BasicTaskProcessor("SubArea[" + record.Name + "]");
            IOQueue.AddUpdatable(this);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public new void AddHandler(Action<string> method)
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
