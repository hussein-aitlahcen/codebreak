using Codebreak.Service.World.Database.Repository;
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
    public class PersistentInventory : AbstractInventory
    {     
        /// <summary>
        /// 
        /// </summary>
        public int OwnerType
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public long OwnerId
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public override long Kamas
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override List<ItemDAO> Items => m_items;

        /// <summary>
        /// 
        /// </summary>
        private readonly List<ItemDAO> m_items;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerType"></param>
        /// <param name="ownerId"></param>
        public PersistentInventory(int ownerType, long ownerId)
        {
            m_items = new List<ItemDAO>();
            m_items.AddRange(InventoryItemRepository.Instance.GetByOwner(ownerType, ownerId));

            OwnerType = ownerType;
            OwnerId = ownerId;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public override void OnOwnerChange(ItemDAO item)
        {
            item.OwnerId = OwnerId;
            item.OwnerType = OwnerType;
        }
    }
}
