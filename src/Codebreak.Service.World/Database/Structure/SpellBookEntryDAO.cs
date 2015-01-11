using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Manager;
using PropertyChanged;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("spellbookentry")]
    [ImplementPropertyChanged]
    public sealed class SpellBookEntryDAO : DataAccessObject<SpellBookEntryDAO>
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
        public int SpellId
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
        public int Position
        {
            get;
            set;
        }

        private SpellTemplate m_template;
        private SpellLevel m_level;

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public SpellTemplate Template
        {
            get
            {
                if (m_template == null)                
                    m_template = SpellManager.Instance.GetTemplate(SpellId);                
                return m_template;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public SpellLevel SpellLevel
        {
            get
            {
                if (m_level == null || Level != m_level.Level)
                    m_level = Template.GetLevel(Level);
                return m_level;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerType"></param>
        /// <param name="ownerId"></param>
        /// <param name="spellId"></param>
        /// <param name="level"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static SpellBookEntryDAO Create(int ownerType, long ownerId, int spellId, int level, int position)
        {
            var instance = new SpellBookEntryDAO()
            {
                OwnerType = ownerType,
                OwnerId = ownerId,
                SpellId = spellId,
                Level = level,
                Position = position
            };
            instance.IsDirty = false;
            if(SpellBookEntryRepository.Instance.Insert(instance))
                return instance;
            return null;
        }
    }
}
