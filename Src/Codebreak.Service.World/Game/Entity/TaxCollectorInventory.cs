using Codebreak.Service.World.Database.Structure;
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
    public sealed class TaxCollectorInventory : InventoryBag
    {
        /// <summary>
        /// 
        /// </summary>
        public override long Kamas
        {
            get
            {
                return _taxCollector.DatabaseRecord.Kamas;
            }
            set
            {
                _taxCollector.DatabaseRecord.Kamas = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override List<InventoryItemDAO> Items
        {
            get 
            {
                return _taxCollector.Items;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private TaxCollectorEntity _taxCollector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollector"></param>
        public TaxCollectorInventory(TaxCollectorEntity taxCollector) 
            : base(taxCollector)
        {
            _taxCollector = taxCollector;
        }
    }
}
