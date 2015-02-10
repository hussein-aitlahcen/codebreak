using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;
using System.Collections.Generic;

namespace Codebreak.Service.World.Database.Structure
{
    [Table("superareatemplate")]
    public sealed class SuperAreaDAO : DataAccessObject<SuperAreaDAO>
    {
        [Key]
        public int Id
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
