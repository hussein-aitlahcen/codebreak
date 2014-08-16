using Codebreak.Framework.Database;
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
    [Table("CharacterAlignment")]
    public sealed class CharacterAlignmentDAO : DataAccessObject<CharacterAlignmentDAO>
    {
        [Key]
        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int AlignmentId
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
        public int Promotion
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Honour
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Dishonour
        {
            get;
            set;
        }
    }
}
