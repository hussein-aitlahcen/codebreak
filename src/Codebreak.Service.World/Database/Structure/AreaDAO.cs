using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;
using System.Collections.Generic;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("areatemplate")]
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
