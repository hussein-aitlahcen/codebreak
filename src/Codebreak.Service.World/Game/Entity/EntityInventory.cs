using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityInventory : PersistentInventory
    {
        /// <summary>
        /// 
        /// </summary>
        public override long Kamas
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
        public EntityBase Entity
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        private StringBuilder m_entityLookCache;

        /// <summary>
        /// 
        /// </summary>
        private bool m_entityLookRefresh;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public EntityInventory(EntityBase entity, int type, long id)
            : base(type, id)
        {
            Entity = entity;
            AddHandler(Entity.Dispatch);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            foreach (var item in Items)
                if (item.IsEquiped)
                    Entity.Statistics.Merge(item.Statistics);
        }
               
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public override InventoryItemDAO RemoveItem(long itemId, int quantity = 1)
        {
            var item = Items.Find(entry => entry.Id == itemId);
            if (item == null)
                return null;

            if (item.IsEquiped)
                MoveItem(item, ItemSlotEnum.SLOT_INVENTORY);            

            return base.RemoveItem(itemId, quantity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="slot"></param>
        public void MoveItem(InventoryItemDAO item, ItemSlotEnum slot, int quantity = 1)
        {
            if (slot == item.Slot)
                return;

            if (quantity > item.Quantity || quantity < 1)
                quantity = item.Quantity;

            if (item.IsEquiped && !InventoryItemDAO.IsEquipedSlot(slot))
            {
                item.SlotId = (int)slot;
                m_entityLookRefresh = true;
                bool merged = AddItem(MoveQuantity(item, 1));

                Entity.Statistics.UnMerge(item.Statistics);

                Entity.MovementHandler.Dispatch(WorldMessage.ENTITY_OBJECT_ACTUALIZE(Entity));

                // send new stats
                if (Entity.Type == EntityTypeEnum.TYPE_CHARACTER)
                {
                    if (!merged)
                        base.Dispatch(WorldMessage.OBJECT_MOVE_SUCCESS(item.Id, item.SlotId));
                    base.Dispatch(WorldMessage.ACCOUNT_STATS((CharacterEntity)Entity));
                }
                return;
            }
            else if (!item.IsEquiped && InventoryItemDAO.IsEquipedSlot(slot))
            {
                if (!ItemTemplateDAO.CanPlaceInSlot((ItemTypeEnum)item.Template.Type, slot))
                {
                    base.Dispatch(WorldMessage.OBJECT_MOVE_ERROR());
                    return;
                }

                // level required
                if (Entity.Level < item.Template.Level)
                {
                    base.Dispatch(WorldMessage.OBJECT_MOVE_ERROR_REQUIRED_LEVEL());
                    return;
                }

                // Already equiped template                    
                if (HasTemplateEquiped(item.TemplateId))
                {
                    base.Dispatch(WorldMessage.OBJECT_MOVE_ERROR_ALREADY_EQUIPED());
                    return;
                }

                var equipedItem = Items.Find(entry => entry.SlotId == (int)slot && entry.Id != item.Id);

                // already equiped in slot ? remove it
                if (equipedItem != null)
                {
                    MoveItem(equipedItem, ItemSlotEnum.SLOT_INVENTORY);
                }


                m_entityLookRefresh = true;
                var newItem = MoveQuantity(item, 1);
                newItem.SlotId = (int)slot;
                AddItem(newItem, false);

                Entity.Statistics.Merge(newItem.Statistics);

                Entity.MovementHandler.Dispatch(WorldMessage.ENTITY_OBJECT_ACTUALIZE(Entity));

                // send new stats
                if (Entity.Type == EntityTypeEnum.TYPE_CHARACTER)
                    base.Dispatch(WorldMessage.ACCOUNT_STATS((CharacterEntity)Entity));
            }
            else
            {
                var newItem = MoveQuantity(item, quantity);
                newItem.SlotId = (int)slot;
                if(!AddItem(newItem, false))
                   base.Dispatch(WorldMessage.OBJECT_MOVE_SUCCESS(item.Id, item.SlotId));
            }
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

                var weapon = Items.Find(entry => entry.Slot == ItemSlotEnum.SLOT_WEAPON);
                var hat = Items.Find(entry => entry.Slot == ItemSlotEnum.SLOT_HAT);
                var cape = Items.Find(entry => entry.Slot == ItemSlotEnum.SLOT_CAPE);
                var pet = Items.Find(entry => entry.Slot == ItemSlotEnum.SLOT_PET);
                var shield = Items.Find(entry => entry.Slot == ItemSlotEnum.SLOT_SHIELD);

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
