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
    [Table("AreaTemplate")]
    public sealed class AreaDAO : DataAccessObject<AreaDAO>
    {
        [Key]
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int SuperAreaId
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
    }
}
