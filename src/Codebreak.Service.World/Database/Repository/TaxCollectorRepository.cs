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
    public sealed class TaxCollectorRepository : Repository<TaxCollectorRepository, TaxCollectorDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        public long NextTaxCollectorId
        {
            get
            {
                lock (m_syncLock)
                    return m_nextTaxCollectorId++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private long m_nextTaxCollectorId;

        /// <summary>
        /// 
        /// </summary>
        public TaxCollectorRepository()
        {
            m_nextTaxCollectorId = 1000;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public override void OnObjectAdded(TaxCollectorDAO obj)
        {
            if (obj.Id >= m_nextTaxCollectorId)
                m_nextTaxCollectorId = obj.Id + 1;
        }
    }
}
