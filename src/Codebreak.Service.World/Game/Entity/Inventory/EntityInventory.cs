using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Stats;
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
        private Dictionary<int, int> m_equippedSets;

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
            m_equippedSets = new Dictionary<int, int>();

            Entity = entity;
            AddHandler(Entity.Dispatch);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            foreach (var item in Items)
            {
                if (item.IsEquiped)
                {
                    AddSet(item);
                    if (item.IsBoostEquiped)
                        Entity.Statistics.Merge(StatsType.TYPE_BOOST, item.Statistics);
                    else
                        Entity.Statistics.Merge(StatsType.TYPE_ITEM, item.Statistics);

                    if (item.Slot == ItemSlotEnum.SLOT_WEAPON)
                    {
                        if (Entity.Type == EntityTypeEnum.TYPE_CHARACTER)
                        {
                            var character = (CharacterEntity)Entity;
                            character.CharacterJobs.ToolEquipped(item.TemplateId);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public override IEnumerable<InventoryItemDAO> RemoveItems()
        {
            foreach (var item in Items.ToArray())
            {
                if (item.IsEquiped)
                    if (item.IsBoostEquiped)
                        Entity.Statistics.UnMerge(StatsType.TYPE_BOOST, item.Statistics);
                    else
                        Entity.Statistics.UnMerge(StatsType.TYPE_ITEM, item.Statistics);

                item.SlotId = (int)ItemSlotEnum.SLOT_INVENTORY;

                yield return base.RemoveItem(item.Id, item.Quantity);
            }

            m_entityLookRefresh = true;
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
                if (item.IsBoostEquiped)
                    Entity.Statistics.UnMerge(StatsType.TYPE_BOOST, item.Statistics);
                else
                    Entity.Statistics.UnMerge(StatsType.TYPE_ITEM, item.Statistics);

                if (item.Slot == ItemSlotEnum.SLOT_WEAPON)
                {
                    Entity.Dispatch(WorldMessage.JOB_TOOL_EQUIPPED());
                }

                item.SlotId = (int)slot;
                m_entityLookRefresh = true;
                bool merged = AddItem(MoveQuantity(item, 1));

                RemoveSet(item);

                // send new stats
                if (Entity.Type == EntityTypeEnum.TYPE_CHARACTER)
                {
                    Entity.MovementHandler.Dispatch(WorldMessage.ENTITY_OBJECT_ACTUALIZE(Entity));

                    base.CachedBuffer = true;
                    if (!merged)
                        base.Dispatch(WorldMessage.OBJECT_MOVE_SUCCESS(item.Id, item.SlotId));
                    base.Dispatch(WorldMessage.ACCOUNT_STATS((CharacterEntity)Entity));
                    if (item.Template.SetId != 0)                    
                        base.Dispatch(WorldMessage.ITEM_SET(item.Template.Set, Items.Where(entry => entry.Template.SetId == item.Template.SetId && entry.IsEquiped)));                    
                    base.CachedBuffer = false;
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

                if (Entity.Type == EntityTypeEnum.TYPE_CHARACTER)
                {
                    if (!item.SatisfyConditions((CharacterEntity)Entity))
                    {
                        base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_CONDITIONS_UNSATISFIED));
                        return;
                    }
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

                AddSet(newItem);

                if (item.IsBoostEquiped)
                    Entity.Statistics.Merge(StatsType.TYPE_BOOST, item.Statistics);
                else
                    Entity.Statistics.Merge(StatsType.TYPE_ITEM, item.Statistics);
               
                // send new stats
                if (Entity.Type == EntityTypeEnum.TYPE_CHARACTER)
                {
                    Entity.MovementHandler.Dispatch(WorldMessage.ENTITY_OBJECT_ACTUALIZE(Entity));

                    base.CachedBuffer = true;
                    Entity.MovementHandler.Dispatch(WorldMessage.ENTITY_OBJECT_ACTUALIZE(Entity));
                    base.Dispatch(WorldMessage.ACCOUNT_STATS((CharacterEntity)Entity));
                    if(item.Template.SetId != 0)                    
                        base.Dispatch(WorldMessage.ITEM_SET(item.Template.Set, Items.Where(entry => entry.Template.SetId == item.Template.SetId && entry.IsEquiped)));
                    if (newItem.Slot == ItemSlotEnum.SLOT_WEAPON)
                    {
                        var character = (CharacterEntity)Entity;
                        character.CharacterJobs.ToolEquipped(item.TemplateId);
                    }
                    base.CachedBuffer = false;
                }
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
        public void SendSets()
        {
            if (Entity.Type == EntityTypeEnum.TYPE_CHARACTER)
            {
                base.CachedBuffer = true;
                foreach (var set in m_equippedSets)
                    if (set.Value > 0)
                        base.Dispatch(WorldMessage.ITEM_SET(ItemSetRepository.Instance.GetSetById(set.Key), Items.Where(entry => entry.Template.SetId == set.Key && entry.IsEquiped)));
                base.CachedBuffer = false;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void AddSet(InventoryItemDAO item)
        {
            if (item.Template.SetId == 0 || item.Template.Set == null)
                return;

            var set = item.Template.Set;
            if (!m_equippedSets.ContainsKey(set.Id))
                m_equippedSets.Add(set.Id, 0);
            var count = ++m_equippedSets[set.Id];
            
            if (count > 0)
            {
                Entity.Statistics.UnMerge(Stats.StatsType.TYPE_ITEM, set.GetStats(count - 1));
                Entity.Statistics.Merge(Stats.StatsType.TYPE_ITEM, set.GetStats(count));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveSet(InventoryItemDAO item)
        {
            if (item.Template.SetId == 0 || item.Template.Set == null)
                return;

            var set = item.Template.Set;
            var count = --m_equippedSets[set.Id];

            if (count > 0)
            {
                Entity.Statistics.Merge(Stats.StatsType.TYPE_ITEM, set.GetStats(count));
                Entity.Statistics.UnMerge(Stats.StatsType.TYPE_ITEM, set.GetStats(count + 1));
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
