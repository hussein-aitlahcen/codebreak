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
        EXCHANGE_STORAGE = 5,
        EXCHANGE_TAXCOLLECTOR = 8,
        EXCHANGE_PERSONAL_SHOP_EDIT = 6,
        EXCHANGE_BIGSTORE_SELL = 10,
        EXCHANGE_BIGSTORE_BUY = 11,
        EXCHANGE_MOUNT_STORAGE = 16,
        EXCHANGE_MOUNT = 15
    }
    
    /// <summary>
    /// 
    /// </summary>
    public abstract class ExchangeBase : MessageDispatcher
    {
        public ExchangeTypeEnum Type
        {
            get;
            private set;
        }

        public ExchangeBase(ExchangeTypeEnum type)
        {
            Type = type;
        }

        public virtual void Create()
        {
            base.Dispatch(WorldMessage.EXCHANGE_CREATE(Type, SerializeAs_ExchangeCreate()));
        }

        public virtual void Leave(bool success = false)
        {
            base.Dispatch(WorldMessage.EXCHANGE_LEAVE(success));
        }

        protected virtual string SerializeAs_ExchangeCreate()
        {
            return "";
        }

        public virtual void AddItem(EntityBase actor, long guid, int quantity)
        {
        }

        public virtual void RemoveItem(EntityBase actor, long guid, int quantity)
        {
        }

        public virtual void MoveKamas(EntityBase actor, long quantity)
        {
        }

        public virtual void BuyItem(EntityBase actor, int templateId, int quantity)
        {
        }

        public virtual void SellItem(EntityBase actor, long guid, int quantity)
        {
        }
    }
}
