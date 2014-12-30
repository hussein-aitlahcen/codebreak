using Codebreak.Framework.Generic;
using Codebreak.Service.World.Game.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuctionHouseManager : Singleton<AuctionHouseManager>
    {
        /// <summary>
        /// 
        /// </summary>
        private List<AuctionHouseInstance> m_auctionHouses;

        /// <summary>
        /// 
        /// </summary>
        public AuctionHouseManager()
        {
            m_auctionHouses = new List<AuctionHouseInstance>();
        }
    }
}
