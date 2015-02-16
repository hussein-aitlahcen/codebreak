using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;
using System.Collections.Generic;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("subareatemplate")]
    public sealed class SubAreaDAO : DataAccessObject<SubAreaDAO>
    {
        [Key]
        public int Id
        {
            get;
            set;
        }

        public int AreaId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
