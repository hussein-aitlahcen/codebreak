using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Repository;
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
        private Dictionary<int, AuctionHouseInstance> m_auctionHouses;

        /// <summary>
        /// 
        /// </summary>
        public AuctionHouseManager()
        {
            m_auctionHouses = new Dictionary<int, AuctionHouseInstance>();       
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            foreach (var auctionHouseDao in AuctionHouseRepository.Instance.GetAll())
            {
                var auctionHouse = new AuctionHouseInstance(auctionHouseDao);
                m_auctionHouses.Add(auctionHouseDao.Id, auctionHouse);
                EntityManager.Instance.GetNpcById(auctionHouseDao.NpcId).SetAuctionHouse(auctionHouse);
            }

            foreach (var auctionHouseAllowedTypeDao in AuctionHouseAllowedTypeRepository.Instance.GetAll())
                m_auctionHouses[auctionHouseAllowedTypeDao.AuctionHouseId].AddAllowedType(auctionHouseAllowedTypeDao.TemplateId);

            foreach (var auctionHouseEntry in AuctionHouseEntryRepository.Instance.GetAll())
                m_auctionHouses[auctionHouseEntry.AuctionHouseId].Add(new AuctionEntry(auctionHouseEntry));     
        }
    }
}
