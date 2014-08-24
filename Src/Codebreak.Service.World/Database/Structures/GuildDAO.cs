using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structures
{
    /// <summary>
    /// 
    /// </summary>
    [Table("Guild")]
    public sealed class GuildDAO : DataAccessObject<GuildDAO>
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
        public int SymbolId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int SymbolColor
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int BackgroundId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int BackgroundColor
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
        public long Experience
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        //public byte[] Statistics
        //{
        //    get;
        //    set;
        //}
    }
}
