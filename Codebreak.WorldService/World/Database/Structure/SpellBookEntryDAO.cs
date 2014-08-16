using Codebreak.Framework.Database;
using Codebreak.WorldService.World.Manager;
using Codebreak.WorldService.World.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("SpellBookEntry")]
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
        [Key]
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
            if (_level == null)
            {
                _level = GetTemplate().GetLevel(Level);
            }
            return _level;
        }
    }
}
