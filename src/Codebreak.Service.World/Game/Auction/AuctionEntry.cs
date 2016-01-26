using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Manager;
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
        public DateTime ExpireDate
        {
            get
            {
                return m_databaseRecord.ExpireDate;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int HoursLeft
        {
            get
            {
                return (int)Math.Floor(ExpireDate.Subtract(DateTime.Now).TotalHours);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ItemDAO Item
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
        public BankInventory OwnerBank
        {
            get
            {
                if (m_owner == null)
                    m_owner = BankManager.Instance.GetBankByAccountId(OwnerId);
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
        private ItemDAO m_item;

        /// <summary>
        /// 
        /// </summary>
        private BankInventory m_owner;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        public AuctionEntry(AuctionHouseEntryDAO record, ItemDAO item = null)
        {
            m_databaseRecord = record;
            m_item = item;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Remove()
        {
            AuctionHouseEntryRepository.Instance.Removed(m_databaseRecord);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(AuctionEntry other)
        {
            if (Price < other.Price)
                return -1;
            if (Price > other.Price)
                return 1;
            return 0;
        }
    }
}
