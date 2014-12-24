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
    public sealed class InventoryItemRepository : Repository<InventoryItemRepository, InventoryItemDAO>
    {
        private Dictionary<long, InventoryItemDAO> m_itemById;

        public InventoryItemRepository()
        {
            m_itemById = new Dictionary<long, InventoryItemDAO>();
        }

        public override void OnObjectAdded(InventoryItemDAO item)
        {
            m_itemById.Add(item.Id, item);
        }

        public override void OnObjectRemoved(InventoryItemDAO item)
        {
            m_itemById.Remove(item.Id);
        }

        public InventoryItemDAO GetById(long itemId)
        {
            if (m_itemById.ContainsKey(itemId))
                return m_itemById[itemId];
            return base.Load("Id=@ItemId", new { ItemId = itemId });
        }

        public IEnumerable<InventoryItemDAO> GetByOwner(int ownerType, long ownerId)
        {
            return _dataObjects.Where(item => item.OwnerType == ownerType && item.OwnerId == ownerId);
        }

        public override void UpdateAll()
        {         
            lock(_syncLock)    
                Remove(_dataObjects.Where(item => item.OwnerId == -1));
            base.UpdateAll();
        }
    }
}
