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
        private Dictionary<int, AuctionHouseInstance> m_auctionHousesById;
        private Dictionary<int, AuctionHouseInstance> m_auctionHouseByNpcId;

        /// <summary>
        /// 
        /// </summary>
        public AuctionHouseManager()
        {
            m_auctionHousesById = new Dictionary<int, AuctionHouseInstance>();
            m_auctionHouseByNpcId = new Dictionary<int, AuctionHouseInstance>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            foreach (var auctionHouseDao in AuctionHouseRepository.Instance.GetAll())
            {
                var auctionHouse = new AuctionHouseInstance(auctionHouseDao);
                m_auctionHousesById.Add(auctionHouseDao.Id, auctionHouse);
                m_auctionHouseByNpcId.Add(auctionHouseDao.NpcId, auctionHouse);
            }

            foreach (var auctionHouseAllowedTypeDao in AuctionHouseAllowedTypeRepository.Instance.GetAll())
                m_auctionHousesById[auctionHouseAllowedTypeDao.AuctionHouseId].AddAllowedType(auctionHouseAllowedTypeDao.TemplateId);

            foreach (var auctionHouseEntry in AuctionHouseEntryRepository.Instance.GetAll())
                m_auctionHousesById[auctionHouseEntry.AuctionHouseId].Add(new AuctionEntry(auctionHouseEntry));     
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="npcId"></param>
        /// <returns></returns>
        public AuctionHouseInstance GetByNpcId(int npcId)
        {
            if (m_auctionHouseByNpcId.ContainsKey(npcId))
                return m_auctionHouseByNpcId[npcId];
            return null;
        }
    }
}
