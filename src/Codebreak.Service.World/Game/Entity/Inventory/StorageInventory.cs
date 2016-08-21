using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Entity.Inventory
{
    /// <summary>
    /// 
    /// </summary>
    public class StorageInventory : PersistentInventory
    {
        /// <summary>
        /// 
        /// </summary>
        public long ActualUser
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public StorageInventory(int ownerType = (int)EntityTypeEnum.TYPE_STORAGE, long ownerId = -1)
            : base(ownerType, ownerId)
        {
            ActualUser = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public override void OnItemAdded(ItemDAO item)
        {
            Dispatch(WorldMessage.EXCHANGE_STORAGE_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_ADD, item.ToExchangeString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="quantity"></param>
        public override void OnItemQuantity(long itemId, int quantity)
        {
            OnItemAdded(GetItem(itemId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        public override void OnItemRemoved(long itemId)
        {
            Dispatch(WorldMessage.EXCHANGE_STORAGE_MOVEMENT(ExchangeMoveEnum.MOVE_OBJECT, OperatorEnum.OPERATOR_REMOVE, itemId.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public override void OnKamasAdded(long value)
        {
            Dispatch(WorldMessage.EXCHANGE_STORAGE_KAMAS_VALUE(Kamas));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public override void OnKamasSubstracted(long value)
        {
            Dispatch(WorldMessage.EXCHANGE_STORAGE_KAMAS_VALUE(Kamas));
        }
    }
}
