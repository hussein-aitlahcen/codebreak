using Codebreak.Framework.Database;

namespace Codebreak.Service.World.Database.Structures
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
