using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TaxCollectorInventory : StorageInventory
    {
        /// <summary>
        /// 
        /// </summary>
        public override long Kamas
        {
            get
            {
                return m_taxCollector.Kamas;
            }
            set
            {
                m_taxCollector.Kamas = value;
            }        
        }
        
        /// <summary>
        /// 
        /// </summary>
        private TaxCollectorEntity m_taxCollector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollecor"></param>
        public TaxCollectorInventory(TaxCollectorEntity taxCollecor)
            : base((int)EntityTypeEnum.TYPE_TAX_COLLECTOR, taxCollecor.Id)
        {
            m_taxCollector = taxCollecor;
        }
    }
}
