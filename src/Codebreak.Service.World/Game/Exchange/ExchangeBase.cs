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
    /// Type d'echange possible
    /// </summary>
    public enum ExchangeTypeEnum
    {
        EXCHANGE_SHOP = 0, // Merchant/npc
        EXCHANGE_PLAYER = 1,
        EXCHANGE_NPC = 2,
        EXCHANGE_MERCHANT = 4,
        EXCHANGE_STORAGE = 5,
        EXCHANGE_TAXCOLLECTOR = 8,
        EXCHANGE_PERSONAL_SHOP_EDIT = 6,
        EXCHANGE_AUCTION_HOUSE_SELL = 10,
        EXCHANGE_AUCTION_HOUSE_BUY = 11,
        EXCHANGE_MOUNT_STORAGE = 16,
        EXCHANGE_MOUNT = 15
    }
    
    /// <summary>
    /// 
    /// </summary>
    public abstract class ExchangeBase : MessageDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
        public ExchangeTypeEnum Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public ExchangeBase(ExchangeTypeEnum type)
        {
            Type = type;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Create()
        {
            base.Dispatch(WorldMessage.EXCHANGE_CREATE(Type, SerializeAs_ExchangeCreate()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        public virtual void Leave(bool success = false)
        {
            base.Dispatch(WorldMessage.EXCHANGE_LEAVE(success));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual string SerializeAs_ExchangeCreate()
        {
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="guid"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        public virtual int AddItem(EntityBase actor, long guid, int quantity, long price = -1)
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="guid"></param>
        /// <param name="quantity"></param>
        public virtual int RemoveItem(EntityBase actor, long guid, int quantity)
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="quantity"></param>
        public virtual long MoveKamas(EntityBase actor, long quantity)
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        public virtual void BuyItem(EntityBase actor, long id, int quantity)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        public virtual void SellItem(EntityBase actor, long id, int quantity, long price = -1)
        {
        }
    }
}
