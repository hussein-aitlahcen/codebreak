using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("auctionhouseentry")]
    [ImplementPropertyChanged]
    public sealed class AuctionHouseEntryDAO : DataAccessObject<AuctionHouseEntryDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long ItemId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int AuctionHouseId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long OwnerId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long Price
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ExpireDate
        {
            get;
            set;
        }        
    }
}
