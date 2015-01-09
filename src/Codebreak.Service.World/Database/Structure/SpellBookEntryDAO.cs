using Codebreak.Framework.Database;
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
    }
}
