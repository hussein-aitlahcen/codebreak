using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Database.Structure
{
    [Table("SuperAreaTemplate")]
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
