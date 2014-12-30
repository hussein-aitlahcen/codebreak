using Codebreak.Framework.Database;
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
    [Table("auctionhouse")]
    public sealed class AuctionHouseDAO : DataAccessObject<AuctionHouseDAO>
    {
        [Key]
        public int Id
        {
            get;
            set;
        }

        public int NpcId
        {
            get;
            set;
        }

        public int ItemMaxLevel
        {
            get;
            set;
        }

        public int PlayerMaxItem
        {
            get;
            set;
        }

        public long Timeout
        {
            get;
            set;
        }

        public int Taxe
        {
            get;
            set;
        }
    }
}
