using Codebreak.Framework.Database;

namespace Codebreak.Service.World.Database.Structures
{
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
