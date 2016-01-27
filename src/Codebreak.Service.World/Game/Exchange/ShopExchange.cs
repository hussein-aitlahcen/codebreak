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
    public sealed class ShopExchange : AbstractExchange
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
        public NonPlayerCharacterEntity Npc
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buyer"></param>
        /// <param name="shop"></param>
        public ShopExchange(CharacterEntity character, NonPlayerCharacterEntity npc)
            : base(ExchangeTypeEnum.EXCHANGE_SHOP)
        {
            Character = character;
            Npc = npc;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Create()
        {
            base.Create();
            base.Dispatch(WorldMessage.EXCHANGE_SHOP_LIST(Npc));

            Npc.OnBeginTrade(Character);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        public override void Leave(bool success = false)
        {
            base.Leave(success);

            Npc.OnLeaveTrade(Character);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="templateId"></param>
        /// <param name="quantity"></param>
        public override void BuyItem(AbstractEntity entity, long templateId, int quantity)
        {
            if (quantity < 1)
            {
                Logger.Debug("ShopExchange unable to buy, quantity < 0 : " + entity.Name);
                Character.Dispatch(WorldMessage.EXCHANGE_BUY_ERROR());
                return;
            }

            var template = Npc.ShopItems.Find(x => x.Id == templateId);
            if (template == null)
            {
                Logger.Debug("ShopExchange unable to buy null template : " + entity.Name);
                Character.Dispatch(WorldMessage.EXCHANGE_BUY_ERROR());
                return;
            }

            var price = template.Price * quantity;

            if (Character.Inventory.Kamas < price)
            {
                Logger.Debug("ShopExchange no enought kamas to buy item : " + entity.Name);
                Character.Dispatch(WorldMessage.EXCHANGE_BUY_ERROR());
                return;
            }

            var instance = template.Create(quantity);
            if (instance == null)
            {
                Logger.Debug("ShopExchange error while creating object : " + entity.Name);
                Character.Dispatch(WorldMessage.EXCHANGE_BUY_ERROR());
                return;
            }

            Character.CachedBuffer = true;
            Character.Inventory.SubKamas(price);
            Character.Inventory.AddItem(instance);
            Character.CachedBuffer = false;

            Npc.OnBuyTrade(Character);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="guid"></param>
        /// <param name="quantity"></param>
        public override void SellItem(AbstractEntity entity, long guid, int quantity, long price = -1)
        {
            if (quantity < 1)
            {
                Logger.Debug("ShopExchange unable to sell, quantity < 1 : " + entity.Name);
                entity.Dispatch(WorldMessage.EXCHANGE_SELL_ERROR());
                return;
            }

            var item = entity.Inventory.Items.Find(entry => entry.Id == guid);

            if (item == null)
            {
                Logger.Debug("ShopExchange unable to sell null item : " + entity.Name);
                entity.Dispatch(WorldMessage.EXCHANGE_SELL_ERROR());
                return;
            }

            if (quantity > item.Quantity)
                quantity = item.Quantity;

            var sellPrice = (item.Template.Price / 10) * quantity;

            entity.Inventory.RemoveItem(guid, quantity);
            entity.Inventory.AddKamas(sellPrice);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string SerializeAs_ExchangeCreate()
        {
            return Npc.Id.ToString();
        }
    }
}
