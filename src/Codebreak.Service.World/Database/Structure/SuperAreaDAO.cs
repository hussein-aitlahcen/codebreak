using Codebreak.Framework.Database;

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
