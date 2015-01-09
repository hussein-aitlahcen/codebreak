using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
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
    public sealed class InventoryItemRepository : Repository<InventoryItemRepository, InventoryItemDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, InventoryItemDAO> m_itemById;

        /// <summary>
        /// 
        /// </summary>
        public InventoryItemRepository()
        {
            m_itemById = new Dictionary<long, InventoryItemDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public override void OnObjectAdded(InventoryItemDAO item)
        {
            m_itemById.Add(item.Id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public override void OnObjectRemoved(InventoryItemDAO item)
        {
            m_itemById.Remove(item.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public InventoryItemDAO GetById(long itemId)
        {
            if (m_itemById.ContainsKey(itemId))
                return m_itemById[itemId];
            return base.Load("Id=@ItemId", new { ItemId = itemId });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerType"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public IEnumerable<InventoryItemDAO> GetByOwner(int ownerType, long ownerId)
        {
            return m_dataObjects.Where(item => item.OwnerType == ownerType && item.OwnerId == ownerId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        public void EntityRemoved(int type, long id)
        {
            m_dataObjects.RemoveAll(item => item.OwnerType == type && item.OwnerId == id);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateAll()
        {         
            lock(m_syncLock)    
                Remove(m_dataObjects.Where(item => item.OwnerId == -1).ToList());
            base.UpdateAll();
        }
    }
}
