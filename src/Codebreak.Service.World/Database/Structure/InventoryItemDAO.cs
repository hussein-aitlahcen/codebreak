using System.Text;
using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Game.Stats;
using PropertyChanged;
using Codebreak.Service.World.Game.Spell;

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

        /// <summary>
        /// 
        /// </summary>
        public long MerchantPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private ItemTemplateDAO m_template;

        /// <summary>
        /// 
        /// </summary>
        public ItemTemplateDAO GetTemplate()
        {
            if (m_template == null)            
                m_template = ItemTemplateRepository.Instance.GetById(TemplateId);            
            return m_template;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private GenericStats m_statistics;

        /// <summary>
        /// 
        /// </summary>
        public GenericStats GetStatistics()
        {
            if (m_statistics == null)            
                m_statistics = GenericStats.Deserialize(Effects);            
            return m_statistics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="value"></param>
        public void SaveStats()
        {
            Effects = GetStatistics().Serialize();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ItemSlotEnum GetSlot()
        {
            return (ItemSlotEnum)SlotId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsEquiped()
        {
            return IsEquipedSlot(GetSlot());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
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
        public override string ToString()
        {
            return  (Id.ToString("x")) + ('~') +
                   (TemplateId.ToString("x")) + ('~') +
                   (Quantity.ToString("x")) +('~') +
                   ((SlotId != (int)ItemSlotEnum.SLOT_INVENTORY ? SlotId.ToString("x") : "")) + ('~') +
                   (StringEffects) + (';'); 
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="quantity"></param>
        /// <param name="statistics"></param>
        /// <returns></returns>
        public static InventoryItemDAO Create(int templateId, long quantity, GenericStats statistics)
        {
            var instance = new InventoryItemDAO();
            instance.OwnerId = -1;
            instance.TemplateId = templateId;
            instance.Quantity = (int)quantity;
            instance.m_statistics = statistics;
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
