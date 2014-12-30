using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Repository;
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
    /// <typeparam name="TKey"></typeparam>
    public class PriceComparer : Singleton<PriceComparer>, IComparer<long>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(long x, long y)
        {
            if (x < y)
                return 1;
            return -1;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class AuctionEntry : IComparable<AuctionEntry>
    {
        /// <summary>
        /// 
        /// </summary>
        public long ItemId
        {
            get
            {
                return m_databaseRecord.ItemId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AuctionHouseId
        {
            get
            {
                return m_databaseRecord.AuctionHouseId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long OwnerId
        {
            get
            {
                return m_databaseRecord.OwnerId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long Price
        {
            get
            {
                return m_databaseRecord.Price;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime InitialDate
        {
            get
            {
                return m_databaseRecord.InitialDate;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public InventoryItemDAO Item
        {
            get
            {
                if (m_item == null)
                    m_item = InventoryItemRepository.Instance.GetById(ItemId);
                return m_item;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CharacterDAO Owner
        {
            get
            {
                if (m_owner == null)
                    m_owner = CharacterRepository.Instance.GetById(OwnerId);
                return m_owner;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private AuctionHouseEntryDAO m_databaseRecord;

        /// <summary>
        /// 
        /// </summary>
        private InventoryItemDAO m_item;

        /// <summary>
        /// 
        /// </summary>
        private CharacterDAO m_owner;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        public AuctionEntry(AuctionHouseEntryDAO record, InventoryItemDAO item = null)
        {
            m_databaseRecord = record;
            m_item = item;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(AuctionEntry other)
        {
            if (Price < other.Price)
                return 1;
            return -1;
        }
    }
}
