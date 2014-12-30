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
        public long Kamas
        {
            get
            {
                return Entity.Kamas;
            }
            set
            {
                Entity.Kamas = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract List<InventoryItemDAO> Items
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public EntityBase Entity
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        private StringBuilder m_entityLookCache;
        private bool m_entityLookRefresh;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public InventoryBag(EntityBase entity)
        {
            Entity = entity;

            AddHandler(entity.Dispatch);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            foreach (var item in Items)
                if (item.IsEquiped())
                    Entity.Statistics.Merge(item.GetStatistics());
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void AddKamas(long value)
        {
            if (value < 0)
                throw new ArgumentException("InventoryBag::AddKamas value should be > 0 : " + Entity.Name);
            Kamas += value;

            if(Entity.Type == EntityTypeEnum.TYPE_CHARACTER)
            {
                base.Dispatch(WorldMessage.ACCOUNT_STATS((CharacterEntity)Entity));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SubKamas(long value)
        {
            if (value < 0)
                throw new ArgumentException("InventoryBag::SubKamas value should be > 0 : " + Entity.Name);
            Kamas -= value;

            if (Entity.Type == EntityTypeEnum.TYPE_CHARACTER)
            {
                base.Dispatch(WorldMessage.ACCOUNT_STATS((CharacterEntity)Entity));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="merge"></param>
        /// <returns></returns>
        public bool AddItem(InventoryItemDAO item, bool merge = true)
        {
            Logger.Debug("InventoryBad::AddItem adding item to inventory : " + Entity.Name);
            if (merge)
                if (TryMerge(item))
                    return true;

            item.OwnerId = Entity.Id;

            InventoryItemRepository.Instance.Update(item);

            Items.Add(item);

            if (Entity.Type == EntityTypeEnum.TYPE_CHARACTER)            
                base.Dispatch(WorldMessage.OBJECT_ADD_SUCCESS(item));            

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryMerge(InventoryItemDAO item)
        {
            var sameItem = Items.Find(
                entry => entry.TemplateId == item.TemplateId && 
                    entry.StringEffects == item.StringEffects && 
                    entry.Id != item.Id &&
                    entry.GetSlot() == ItemSlotEnum.SLOT_INVENTORY);
            
            if(sameItem != null)
            {
                Logger.Debug("InventoryBag::TryMerge merged item : " + Entity.Name);
                sameItem.Quantity += item.Quantity;
                if(Entity.Type == EntityTypeEnum.TYPE_CHARACTER)
                    base.Dispatch(WorldMessage.OBJECT_QUANTITY_UPDATE(sameItem.Id, sameItem.Quantity));
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
        public InventoryItemDAO MoveQuantity(InventoryItemDAO item, int quantity, ItemSlotEnum slot = ItemSlotEnum.SLOT_INVENTORY)
        {
            if(quantity >= item.Quantity)
            {
                Logger.Debug("InventoryBag::MoveQuantity moving full quantity : " + Entity.Name);
                return RemoveItem(item.Id, item.Quantity);
            }

            Logger.Debug("InventoryBag::MoveQuantity moving less quantity than object quantity : " + Entity.Name);

            item.Quantity -= quantity;

            if(Entity.Type == EntityTypeEnum.TYPE_CHARACTER)
                base.Dispatch(WorldMessage.OBJECT_QUANTITY_UPDATE(item.Id, item.Quantity));

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
            return Items.Any(item => item.Id != guid && item.IsEquiped() && item.TemplateId == templateId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public bool HasTemplateEquiped(int templateId)
        {
            return Items.Any(item => item.TemplateId == templateId && item.IsEquiped());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="slot"></param>
        public void MoveItem(InventoryItemDAO item, ItemSlotEnum slot)
        {
            if (slot == item.GetSlot())
                return;

            if (item.IsEquiped() && slot == ItemSlotEnum.SLOT_INVENTORY)
            {
                Logger.Debug("InventoryBag::MoveItem moving item from entity to inventory : " + Entity.Name);
                
                item.SlotId = (int)slot;
                m_entityLookRefresh = true;
                bool merged = AddItem(MoveQuantity(item, 1));

                Entity.Statistics.UnMerge(item.GetStatistics());
                
                Entity.MovementHandler.Dispatch(WorldMessage.ENTITY_OBJECT_ACTUALIZE(Entity));
                               
                // send new stats
                if (Entity.Type == EntityTypeEnum.TYPE_CHARACTER)
                {
                    if (!merged)                    
                        base.Dispatch(WorldMessage.OBJECT_MOVE_SUCCESS(item.Id, slot));                    
                    base.Dispatch(WorldMessage.ACCOUNT_STATS((CharacterEntity)Entity));
                }
                return;
            }
            else if (!item.IsEquiped() && InventoryItemDAO.IsEquipedSlot(slot))
            {
                if (!ItemTemplateDAO.CanPlaceInSlot((ItemTypeEnum)item.GetTemplate().Type, slot))
                {
                    Logger.Debug("InventoryBag::MoveItem trying to equip an item to bad slot: " + Entity.Name);
                    base.Dispatch(WorldMessage.OBJECT_MOVE_ERROR());
                    return;
                }

                // level required
                if (Entity.Level < item.GetTemplate().Level)
                {
                    Logger.Debug("InventoryBag::MoveItem trying to equip a higher level item : " + Entity.Name);
                    base.Dispatch(WorldMessage.OBJECT_MOVE_ERROR_REQUIRED_LEVEL());
                    return;
                }

                // Already equiped template                    
                if (HasTemplateEquiped(item.TemplateId))
                {
                    Logger.Debug("InventoryBag::MoveItem already equipped template : " + Entity.Name);
                    base.Dispatch(WorldMessage.OBJECT_MOVE_ERROR_ALREADY_EQUIPED());
                    return;
                }
                
                var equipedItem = Items.Find(entry => entry.SlotId == (int)slot && entry.Id != item.Id);

                // already equiped in slot ? remove it
                if (equipedItem != null)
                {
                    Logger.Debug("InventoryBag::MoveItem removing already equipped item : " + Entity.Name);
                    MoveItem(equipedItem, ItemSlotEnum.SLOT_INVENTORY);
                }

                Logger.Debug("InventoryBag::MoveItem equipped an item : " + Entity.Name);

                m_entityLookRefresh = true;
                var newItem = MoveQuantity(item, 1);
                newItem.SlotId = (int)slot;
                AddItem(newItem, false);

                Entity.Statistics.Merge(newItem.GetStatistics());

                Entity.MovementHandler.Dispatch(WorldMessage.ENTITY_OBJECT_ACTUALIZE(Entity));

                // send new stats
                if (Entity.Type == EntityTypeEnum.TYPE_CHARACTER)                
                    base.Dispatch(WorldMessage.ACCOUNT_STATS((CharacterEntity)Entity));                
            }
            else
            {
                if (Entity.Type == EntityTypeEnum.TYPE_CHARACTER)    
                    base.Dispatch(WorldMessage.OBJECT_MOVE_ERROR());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public InventoryItemDAO RemoveItem(long id, int quantity = 1)
        {
            var item = Items.Find(x => x.Id == id);
            if(item != null)
            {
                if(item.GetSlot() != ItemSlotEnum.SLOT_INVENTORY)
                {
                    Logger.Debug("InventoryBag::RemoveItem moving item into inventory before deleting : " + Entity.Name);
                    MoveItem(item, ItemSlotEnum.SLOT_INVENTORY);
                }

                if(quantity >= item.Quantity)
                {
                    Logger.Debug("InventoryBag::RemoveItem removing item with full quantity : " + Entity.Name);
                    Items.Remove(item);

                    if (Entity.Type == EntityTypeEnum.TYPE_CHARACTER)    
                        base.Dispatch(WorldMessage.OBJECT_REMOVE_SUCCESS(item.Id));
                }
                else
                {
                    Logger.Debug("InventoryBag::RemoveItem splitting merged item to delete specified quantity : " + Entity.Name);
                    item = MoveQuantity(item, quantity);
                }

                // set to delete on next save if not given to another entity
                item.OwnerId = -1;
            }
            return item;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InventoryItemDAO GetItem(long id)
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
            {
                item.SerializeAs_BagContent(message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            Entity = null;
            m_entityLookCache = null;

            base.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SerializeAs_ActorLookMessage(StringBuilder message)
        {
            if (m_entityLookRefresh || m_entityLookCache == null)
            {
                m_entityLookCache = new StringBuilder();

                var weapon = Items.Find(entry => entry.GetSlot() == ItemSlotEnum.SLOT_WEAPON);
                var hat = Items.Find(entry => entry.GetSlot() == ItemSlotEnum.SLOT_HAT);
                var cape = Items.Find(entry => entry.GetSlot() == ItemSlotEnum.SLOT_CAPE);
                var pet = Items.Find(entry => entry.GetSlot() == ItemSlotEnum.SLOT_PET);
                var shield = Items.Find(entry => entry.GetSlot() == ItemSlotEnum.SLOT_SHIELD);

                if (weapon != null)
                    m_entityLookCache.Append(weapon.TemplateId.ToString("x"));
                m_entityLookCache.Append(',');
                if (hat != null)
                    m_entityLookCache.Append(hat.TemplateId.ToString("x"));
                m_entityLookCache.Append(',');
                if (cape != null)
                    m_entityLookCache.Append(cape.TemplateId.ToString("x"));
                m_entityLookCache.Append(',');
                if (pet != null)
                    m_entityLookCache.Append(pet.TemplateId.ToString("x"));
                m_entityLookCache.Append(',');
                if (shield != null)
                    m_entityLookCache.Append(shield.TemplateId.ToString("x"));
                m_entityLookCache.Append(',');

                m_entityLookRefresh = false;
            }

            message.Append(m_entityLookCache.ToString());
        }
    }
}
