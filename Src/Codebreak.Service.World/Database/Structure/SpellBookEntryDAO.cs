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

        private SpellTemplate _template;
        private SpellLevel _level;

        /// <summary>
        /// 
        /// </summary>
        public SpellTemplate GetTemplate()
        {
            if (_template == null)
            {
                _template = SpellManager.Instance.GetTemplate(SpellId);
            }
            return _template;
        }

        /// <summary>
        /// 
        /// </summary>
        public SpellLevel GetSpellLevel()
        {
            if (_level == null || Level != _level.Level)
            {
                _level = GetTemplate().GetLevel(Level);
            }
            return _level;
        }
    }
}
