using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuctionHouseEntryRepository : Repository<AuctionHouseEntryRepository, AuctionHouseEntryDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="houseId"></param>
        /// <param name="ownerId"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public AuctionHouseEntryDAO Create(long itemId, int houseId, long ownerId, long price, long time)
        {
            var entry = new AuctionHouseEntryDAO()
            {
                ItemId = itemId,
                AuctionHouseId = houseId,
                OwnerId = ownerId,
                Price = price,
                ExpireDate = DateTime.Now.AddHours((double)time),
            };
            base.Created(entry);
            return entry;
        }
    }
}
