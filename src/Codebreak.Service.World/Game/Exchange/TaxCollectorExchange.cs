using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Exchange
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TaxCollectorExchange : StorageExchange
    {
        /// <summary>
        /// 
        /// </summary>
        public TaxCollectorEntity TaxCollector
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="taxCollector"></param>
        public TaxCollectorExchange(CharacterEntity character, TaxCollectorEntity taxCollector)
            : base(character, taxCollector.Storage, ExchangeTypeEnum.EXCHANGE_TAXCOLLECTOR)
        {
            TaxCollector = taxCollector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string SerializeAs_ExchangeCreate()
        {
            return TaxCollector.Id.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        public override void Leave(bool success = false)
        {
            base.Leave(success);
            Character.GuildMember.FarmTaxCollector(TaxCollector);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public override long MoveKamas(AbstractEntity actor, long quantity)
        {
            // Can only remove kamas
            if (quantity > 0)
                return 0;

            return base.MoveKamas(actor, quantity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="guid"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public override int AddItem(AbstractEntity actor, long guid, int quantity, long price = -1)
        {
            return 0; // should not be able to add an item
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="guid"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public override int RemoveItem(AbstractEntity actor, long guid, int quantity)
        {
            var item = Storage.GetItem(guid);
            if(item == null)
                return 0;

            var templateId = item.TemplateId;

            quantity = base.RemoveItem(actor, guid, quantity);

            if(quantity > 0)
            {
                if (!TaxCollector.FarmedItems.ContainsKey(templateId))
                    TaxCollector.FarmedItems.Add(templateId, 0);
                TaxCollector.FarmedItems[templateId] += quantity;
            }
            return quantity;
        }
    }
}
