using System.Text;
using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Game.Stats;
using PropertyChanged;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("inventoryitem")]
    [ImplementPropertyChanged]
    public sealed class InventoryItemDAO : DataAccessObject<InventoryItemDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int OwnerType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long OwnerId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int TemplateId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int SlotId
        {
            get;
            set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int Quantity
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Effects
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string StringEffects
        {
            get;
            set;
        }

        private ItemTemplateDAO _template;
        /// <summary>
        /// 
        /// </summary>
        public ItemTemplateDAO GetTemplate()
        {
            if (_template == null)
            {
                _template = ItemTemplateRepository.Instance.GetTemplate(TemplateId);
            }
            return _template;
        }
        
        private GenericStats _statistics;
        /// <summary>
        /// 
        /// </summary>
        public GenericStats GetStatistics()
        {
            if (_statistics == null)            
                _statistics = GenericStats.Deserialize(Effects);            
            return _statistics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ItemSlotEnum GetSlot()
        {
            return (ItemSlotEnum)SlotId;
        }

        public bool IsEquiped()
        {
            return IsEquipedSlot(GetSlot());
        }

        public static bool IsEquipedSlot(ItemSlotEnum slot)
        {
            return slot > ItemSlotEnum.SLOT_INVENTORY && slot <= ItemSlotEnum.SLOT_SHIELD;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SerializeAs_BagContent(StringBuilder message)
        {
            message
                .Append(Id.ToString("x")).Append('~')
                .Append(TemplateId.ToString("x")).Append('~')
                .Append(Quantity.ToString("x")).Append('~')
                .Append((SlotId != (int)ItemSlotEnum.SLOT_INVENTORY ? SlotId.ToString("x") : "")).Append('~')
                .Append(StringEffects).Append(';');        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public InventoryItemDAO Clone(int quantity)
        {
            var instance = new InventoryItemDAO();
            instance.OwnerId = -1;
            instance.TemplateId = TemplateId;
            instance.Quantity = quantity;
            instance.Effects = GetStatistics().Serialize();
            instance.StringEffects = GetStatistics().ToItemStats();
            instance.SlotId = (int)ItemSlotEnum.SLOT_INVENTORY;

            if (InventoryItemRepository.Instance.Insert(instance))
                return instance;
            else
                InventoryItemRepository.Logger.Debug("InventoryItemDAO::Clone error while inserting in database");
            return null;
        }

        public static InventoryItemDAO Create(int templateId, int quantity, GenericStats statistics)
        {
            var instance = new InventoryItemDAO();
            instance.OwnerId = -1;
            instance.TemplateId = templateId;
            instance.Quantity = quantity;
            instance._statistics = statistics;
            instance.Effects = statistics.Serialize();
            instance.StringEffects = statistics.ToItemStats();
            instance.SlotId = (int)ItemSlotEnum.SLOT_INVENTORY;

            if (InventoryItemRepository.Instance.Insert(instance))
                return instance;
            else
                InventoryItemRepository.Logger.Debug("InventoryItemDAO::Create error while inserting in database");
            return null;
        }
    }
}
