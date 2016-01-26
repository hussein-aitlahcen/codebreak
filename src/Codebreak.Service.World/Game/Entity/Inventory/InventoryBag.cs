using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Game.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class InventoryBag : MessageDispatcher, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract long Kamas
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract List<ItemDAO> Items
        {
            get;
        }
                    
        /// <summary>
        /// 
        /// </summary>
        public virtual void OnKamasAdded(long value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnKamasSubstracted(long value)
        {          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void OnItemAdded(ItemDAO item)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void OnOwnerChange(ItemDAO item)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="quantity"></param>
        public virtual void OnItemQuantity(long itemId, int quantity)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        public virtual void OnItemRemoved(long itemId)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void AddKamas(long value)
        {
            if (value < 0)
                throw new ArgumentException("InventoryBag::AddKamas value should be > 0 : " + value);
            Kamas += value;
            OnKamasAdded(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SubKamas(long value)
        {
            if (value < 0)
                throw new ArgumentException("InventoryBag::SubKamas value should be > 0 : " + value);
            Kamas -= value;
            OnKamasSubstracted(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="merge"></param>
        /// <returns></returns>
        public bool AddItem(ItemDAO item, bool merge = true)
        {
            if (Items.Contains(item))
                return false;

            if (merge)
                if (TryMerge(item))
                    return true;
            
            Items.Add(item);
            OnItemAdded(item);
            OnOwnerChange(item);

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryMerge(ItemDAO item)
        {
            var sameItem = Items.Find(
                entry => entry.TemplateId == item.TemplateId && 
                    entry.StringEffects == item.StringEffects && 
                    entry.Id != item.Id &&
                    entry.SlotId == item.SlotId &&
                    !ItemDAO.IsEquipedSlot(entry.Slot));
            
            if(sameItem != null)
            {
                sameItem.Quantity += item.Quantity;
                OnItemQuantity(sameItem.Id, sameItem.Quantity);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public ItemDAO MoveQuantity(ItemDAO item, int quantity, ItemSlotEnum slot = ItemSlotEnum.SLOT_INVENTORY)
        {
            if(quantity >= item.Quantity)            
                return RemoveItem(item.Id, item.Quantity);                        
            item.Quantity -= quantity;
            OnItemQuantity(item.Id, item.Quantity);
            return item.Clone(quantity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public bool IsEquipedOf(long guid, int templateId)
        {
            return Items.Any(item => item.Id != guid && item.IsEquiped && item.TemplateId == templateId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public bool HasTemplate(int templateId)
        {
            return Items.Any(item => item.TemplateId == templateId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public bool NotHasTemplate(int templateId)
        {
            return !HasTemplate(templateId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public bool HasTemplateEquiped(int templateId)
        {
            return Items.Any(item => item.TemplateId == templateId && item.IsEquiped);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public virtual IEnumerable<ItemDAO> RemoveItems()
        {
            foreach (var item in Items.ToArray())
            {
                Items.Remove(item);
                OnItemRemoved(item.Id);
                item.OwnerId = -1;
                yield return item;
            }
        }
               
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public virtual ItemDAO RemoveItem(long itemId, int quantity = 1)
        {
            var item = Items.Find(entry => entry.Id == itemId);
            if (item == null)
                return null;

            if (quantity >= item.Quantity)
            {
                Items.Remove(item);
                OnItemRemoved(item.Id);
            }
            else
            {
                item = MoveQuantity(item, quantity);
            }
            // set to delete on next save if not given to another entity
            item.OwnerId = -1;

            return item;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ItemDAO GetItem(long id)
        {
            return Items.Find(item => item.Id == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SerializeAs_BagContent(StringBuilder message)
        {
            foreach (var item in Items)            
                item.SerializeAs_BagContent(message);            
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
