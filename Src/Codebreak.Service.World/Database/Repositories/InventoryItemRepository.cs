using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Database.Repository
{
    public sealed class InventoryItemRepository : Repository<InventoryItemRepository, InventoryItemDAO>
    {
        private Dictionary<long, InventoryItemDAO> _itemById;
        private Dictionary<long, List<InventoryItemDAO>> _itemsByOwner;

        public InventoryItemRepository()
        {
            _itemById = new Dictionary<long, InventoryItemDAO>();
            _itemsByOwner = new Dictionary<long, List<InventoryItemDAO>>();
        }

        public override void OnObjectAdded(InventoryItemDAO item)
        {
            _itemById.Add(item.Id, item);
            if (!_itemsByOwner.ContainsKey(item.OwnerId))
                _itemsByOwner.Add(item.OwnerId, new List<InventoryItemDAO>());
            _itemsByOwner[item.OwnerId].Add(item);
        }

        public override void OnObjectRemoved(InventoryItemDAO item)
        {
            _itemById.Remove(item.Id);
            _itemsByOwner.Remove(item.Id);
        }

        public InventoryItemDAO GetById(long itemId)
        {
            if (_itemById.ContainsKey(itemId))
                return _itemById[itemId];
            return base.Load("Id=@ItemId", new { ItemId = itemId });
        }

        public List<InventoryItemDAO> GetByOwner(long ownerId)
        {
            List<InventoryItemDAO> items = new List<InventoryItemDAO>();
            if (_itemsByOwner.ContainsKey(ownerId))
                items.AddRange(_itemsByOwner[ownerId]);
            else
                items.AddRange(base.LoadMultiple("OwnerId=@OwnerId", new { OwnerId = ownerId }));
            return items;
        }

        public override void UpdateAll()
        {
            for (int i = _dataObjects.Count - 1; i > -1; i--)
            {
                if (_dataObjects[i].OwnerId == -1)
                {
                    Remove(_dataObjects[i]);
                }
            }
            base.UpdateAll();
        }
    }
}
