using Codebreak.Framework.Database;
using PropertyChanged;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("characteralignment")]
    [ImplementPropertyChanged]
    public sealed class CharacterAlignmentDAO : DataAccessObject<CharacterAlignmentDAO>
    {
        [Key]
        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int AlignmentId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Level
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Promotion
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Honour
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Dishonour
        {
            get;
            set;
        }
    }
}
