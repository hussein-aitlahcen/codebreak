using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("bank")]
    public sealed class BankDAO : DataAccessObject<BankDAO>
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
        public long Kamas
        {
            get;
            set;
        }
    }
}
