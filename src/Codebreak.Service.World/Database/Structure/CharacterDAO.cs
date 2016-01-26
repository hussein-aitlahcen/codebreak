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
        BREED_SADIDAS = 10,
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
        public int SavedCellId
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

        /// <summary>
        /// 
        /// </summary>
        public int TitleId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string TitleParams
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int EmoteCapacity
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int DeathType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int EquippedMount
        {
            get;
            set;
        }

        #region Unmapped

        private CharacterAlignmentDAO m_alignment;
        private CharacterGuildDAO m_guild;

        [Write(false)]
        [DoNotNotify]
        public string HexColor1
        {
            get
            {
                if (Color1 == -1)
                    return "-1";
                return Color1.ToString("x");
            }
        }

        [Write(false)]
        [DoNotNotify]
        public string HexColor2
        {
            get
            {
                if (Color2 == -1)
                    return "-1";
                return Color2.ToString("x");
            }
        }

        [Write(false)]
        [DoNotNotify]
        public string HexColor3
        {
            get
            {
                if (Color3 == -1)
                    return "-1";
                return Color3.ToString("x");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        [DoNotNotify]
        public CharacterAlignmentDAO Alignment
        {
            get
            {
                if (m_alignment == null)
                    m_alignment = CharacterAlignmentRepository.Instance.GetById(Id);
                return m_alignment;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        [DoNotNotify]
        public CharacterGuildDAO Guild
        {
            get
            {
                if (m_guild == null)
                    m_guild = CharacterGuildRepository.Instance.GetById(Id);
                return m_guild;
            }
        }
           
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SerializeAs_ActorLookMessage(StringBuilder message)
        {
            var items = new List<ItemDAO>(InventoryItemRepository.Instance.GetByOwner((int)EntityTypeEnum.TYPE_CHARACTER, Id));
            var weapon = items.Find(entry => entry.Slot == ItemSlotEnum.SLOT_WEAPON);
            var hat = items.Find(entry => entry.Slot == ItemSlotEnum.SLOT_HAT);
            var cape = items.Find(entry => entry.Slot == ItemSlotEnum.SLOT_CAPE);
            var pet = items.Find(entry => entry.Slot == ItemSlotEnum.SLOT_PET);
            var shield = items.Find(entry => entry.Slot == ItemSlotEnum.SLOT_SHIELD);
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
