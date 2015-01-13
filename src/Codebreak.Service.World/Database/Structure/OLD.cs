using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{
    [Table("monsters")]
    public class monsters : DataAccessObject<monsters>
    {
        public int Id
        {
            get;
            set;
        }

        public string Spells
        {
            get;
            set;
        }
    }

    [Table("sorts")]
    public class sorts : DataAccessObject<sorts>
    {
        public int id
        {
            get;
            set;
        }
        public string nom
        {
            get;
            set;
        }
        public int sprite
        {
            get;
            set;
        }
        public string spriteInfos
        {
            get;
            set;
        }
        public string lvl1
        {
            get;
            set;
        }
        public string lvl2
        {
            get;
            set;
        }
        public string lvl3
        {
            get;
            set;
        }
        public string lvl4
        {
            get;
            set;
        }
        public string lvl5
        {
            get;
            set;
        }
        public string lvl6
        {
            get;
            set;
        }
        public string effectTarget
        {
            get;
            set;
        }
    }
}
