using System.Collections.Generic;
using System.Text;
using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repositories;
using PropertyChanged;

namespace Codebreak.Service.World.Database.Structures
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
    [Table("Character")]
    [ImplementPropertyChanged]
    public sealed class CharacterDAO : DataAccessObject<CharacterDAO>
    {
        [Key]
        public long Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public byte Breed
        {
            get;
            set;
        }
        public int Color1
        {
            get;
            set;
        }
        public int Color2
        {
            get;
            set;
        }
        public int Color3
        {
            get;
            set;
        }
        public int Skin
        {
            get;
            set;
        }
        public int SkinSize
        {
            get;
            set;
        }
        public int Vitality
        {
            get;
            set;
        }
        public int Wisdom
        {
            get;
            set;
        }
        public int Strength
        {
            get;
            set;
        }
        public int Intelligence
        {
            get;
            set;
        }
        public int Agility
        {
            get;
            set;
        }
        public int Chance
        {
            get;
            set;
        }
        public int Ap
        {
            get;
            set;
        }
        public int Mp
        {
            get;
            set;
        }
        public int Life
        {
            get;
            set;
        }
        public int Energy
        {
            get;
            set;
        }
        public int SpellPoint
        {
            get;
            set;
        }
        public int CaracPoint
        {
            get;
            set;
        }
        public int MapId
        {
            get;
            set;
        }
        public int CellId
        {
            get;
            set;
        }
        public int Restriction
        {
            get;
            set;
        }
        public long Experience
        {
            get;
            set;
        }
        public long AccountId
        {
            get;
            set;
        }
        public bool Dead
        {
            get;
            set;
        }
        public int MaxLevel
        {
            get;
            set;
        }
        public int DeathCount
        {
            get;
            set;
        }
        public int Level
        {
            get;
            set;
        }
        public bool Sex
        {
            get;
            set;
        }
        public long Kamas
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
                _items = InventoryItemRepository.Instance.GetByOwner(Id);
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
