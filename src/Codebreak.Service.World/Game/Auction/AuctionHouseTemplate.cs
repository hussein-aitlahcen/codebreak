using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Auction
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuctionHouseTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        public AuctionHouseDAO DatabaseRecord
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        public AuctionHouseTemplate(AuctionHouseDAO record)
        {
            DatabaseRecord = record;
        }
    }
}
