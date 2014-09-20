using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repositories;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structures
{
    /// <summary>
    /// 
    /// </summary>
    [Table("characterguild")]
    [ImplementPropertyChanged]
    public sealed class CharacterGuildDAO : DataAccessObject<CharacterGuildDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long GuildId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Rank
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Power
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int XPSharePercent
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long XPGiven
        {
            get;
            set;
        }
    }
}
