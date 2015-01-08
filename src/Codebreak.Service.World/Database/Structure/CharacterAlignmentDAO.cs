using Codebreak.Framework.Database;
using PropertyChanged;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    public enum AlignmentTypeEnum
    {
        ALIGNMENT_NEUTRAL = 0,
        ALIGNMENT_BONTARIEN = 1,
        ALIGNMENT_BRAKMARIEN = 2,
        ALIGNMENT_MERCENARY = 3,
    }

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
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled
        {
            get;
            set;
        }
    }
}
