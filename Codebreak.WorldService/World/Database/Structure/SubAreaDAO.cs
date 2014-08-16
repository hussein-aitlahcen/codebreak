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
    [Table("SubAreaTemplate")]
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
