using Codebreak.Service.World.Database.Structure;
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
    public sealed class PersonalShopExchange : ExchangeBase
    {
        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity Character
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public PersonalShopExchange(CharacterEntity character)
            : base(ExchangeTypeEnum.EXCHANGE_PERSONAL_SHOP_EDIT)
        {
            Character = character;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Create()
        {
            base.Create();
            SendItemsList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="guid"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public override int AddItem(EntityBase actor, long guid, int quantity, long price = -1)
        {
            if (price < 1)
                return 0;

            InventoryItemDAO item = null;
            if (quantity > 0)
                item = Character.Inventory.RemoveItem(guid, quantity);
            else
                item = Character.PersonalShop.GetItem(guid);

            if(item == null)
            {
                base.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return 0;
            }
            
            item.MerchantPrice = price;

            if (quantity > 0)
            {
                Character.PersonalShop.AddItem(item, false);
                Character.RefreshPersonalShopTaxe();
            }

            SendItemsList();

            return item.Quantity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="guid"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public override int RemoveItem(EntityBase actor, long guid, int quantity)
        {
            if (quantity < 1)
                return 0;

            var item = Character.PersonalShop.RemoveItem(guid, quantity);

            if (item == null)
            {
                base.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return 0;
            }

            item.MerchantPrice = -1;

            Character.Inventory.AddItem(item);
            Character.RefreshPersonalShopTaxe();

            SendItemsList();

            return item.Quantity;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void SendItemsList()
        {
            base.Dispatch(WorldMessage.EXCHANGE_PERSONAL_SHOP_ITEMS_LIST(Character.PersonalShop.Items));
        }
    }
}
