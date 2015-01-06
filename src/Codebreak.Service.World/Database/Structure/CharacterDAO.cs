using System.Collections.Generic;
using System.Text;
using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;
using PropertyChanged;
using Codebreak.Service.World.Game.Entity;

namespace Codebreak.Service.World.Database.Structure
{ 
    /// <summary>
    /// 
    /// </summary>
    public enum CharacterBreedEnum : byte
    {
        BREED_FECA = 1,
        BREED_OSAMODAS = 2,
        BREED_ENUTROF = 3,
        BREED_SRAM = 4,
        BREED_XELOR = 5,
        BREED_ECAFLIP = 6,
        BREED_ENIRIPSA = 7,
        BREED_IOP = 8,
        BREED_CRA = 9,
        BREED_SADIDA = 10,
        BREED_SACRIEUR = 11,
        BREED_PANDAWA = 12,
    }

    /// <summary>
    /// 
    /// </summary>    
    [Table("characterinstance")]
    [ImplementPropertyChanged]
    public sealed class CharacterDAO : DataAccessObject<CharacterDAO>
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
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Breed
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Color1
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Color2
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Color3
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Skin
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int SkinSize
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Vitality
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Wisdom
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Strength
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Intelligence
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Agility
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Chance
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Ap
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Mp
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Life
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Energy
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int SpellPoint
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int CaracPoint
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int MapId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int CellId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Restriction
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long Experience
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long AccountId
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        public bool Dead
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        public int MaxLevel
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        public int DeathCount
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Level
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Sex
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long Kamas
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int SavedMapId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Merchant
        {
            get;
            set;
        }

        #region Unmapped
        public string GetHexColor1()
        {
            if (Color1 == -1)
                return "-1";
            return Color1.ToString("x");
        }
        public string GetHexColor2()
        {
            if (Color2 == -1)
                return "-1";
            return Color2.ToString("x");
        }

        public string GetHexColor3()
        {
            if (Color3 == -1)
                return "-1";
            return Color3.ToString("x");
        }

        private CharacterAlignmentDAO _alignment;
        public CharacterAlignmentDAO GetCharacterAlignment()
        {
            if (_alignment == null)
                _alignment = CharacterAlignmentRepository.Instance.GetById(Id);
            return _alignment;
        }

        private CharacterGuildDAO _guild;
        public CharacterGuildDAO GetCharacterGuild()
        {
            if (_guild == null)
                _guild = CharacterGuildRepository.Instance.GetById(Id);
            return _guild;
        }

        private List<InventoryItemDAO> _items;
        public List<InventoryItemDAO> GetItems()
        {
            if (_items == null)                      
                _items = new List<InventoryItemDAO>(InventoryItemRepository.Instance.GetByOwner((int)EntityTypeEnum.TYPE_CHARACTER, Id));            
            return _items;
        }

        public InventoryItemDAO GetItemInSlot(ItemSlotEnum slot)
        {
            if (slot == ItemSlotEnum.SLOT_INVENTORY)
                return null;
            foreach (var item in GetItems())
                if (item.SlotId == (int)slot)
                    return item;
            return null;
        }

        public void SerializeAs_ActorLookMessage(StringBuilder message)
        {
            var items = GetItems();
            var weapon = items.Find(entry => entry.GetSlot() == ItemSlotEnum.SLOT_WEAPON);
            var hat = items.Find(entry => entry.GetSlot() == ItemSlotEnum.SLOT_HAT);
            var cape = items.Find(entry => entry.GetSlot() == ItemSlotEnum.SLOT_CAPE);
            var pet = items.Find(entry => entry.GetSlot() == ItemSlotEnum.SLOT_PET);
            var shield = items.Find(entry => entry.GetSlot() == ItemSlotEnum.SLOT_SHIELD);

            if (weapon != null)
                message.Append(weapon.TemplateId.ToString("x"));
            message.Append(',');
            if (hat != null)
                message.Append(hat.TemplateId.ToString("x"));
            message.Append(',');
            if (cape != null)
                message.Append(cape.TemplateId.ToString("x"));
            message.Append(',');
            if (pet != null)
                message.Append(pet.TemplateId.ToString("x"));
            message.Append(',');
            if (shield != null)
                message.Append(shield.TemplateId.ToString("x"));
            message.Append(',');
        }
        #endregion
    }
}
