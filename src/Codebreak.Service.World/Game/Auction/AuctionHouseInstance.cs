using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Network;
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
    public enum AuctionAddResultEnum
    {
        INVALID_ITEM,
        INVALID_FLOOR,
        INVALID_PRICE,
        INVALID_QUANTITY,
        NOT_ENOUGH_KAMAS_FOR_TAXE,
        TOO_MANY_ENTRIES,
        TOO_HIGH_LEVEL,
        ERROR,
        SUCCESS,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum AuctionBuyResultEnum
    {
        INVALID_ID,
        NOT_ENOUGH_KAMAS,
        ERROR,
        SUCCESS,
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class AuctionHouseInstance : MessageDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get
            {
                return m_databaseRecord.Id;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MapId
        {
            get
            {
                return m_databaseRecord.MapId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int NpcId
        {
            get
            {
                return m_databaseRecord.NpcId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ItemMaxLevel
        {
            get
            {
                return m_databaseRecord.ItemMaxLevel;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PlayerMaxItem
        {
            get
            {
                return m_databaseRecord.PlayerMaxItem;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public long Timeout
        {
            get
            {
                return m_databaseRecord.Timeout;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Taxe
        {
            get
            {
                return m_databaseRecord.Taxe;
            }
        }
    
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, List<AuctionCategory>> m_categoriesByTemplate;
        private Dictionary<int, List<int>> m_templatesByType;
        private Dictionary<long, int> m_characterItemCount;

        /// <summary>
        /// 
        /// </summary>
        private AuctionHouseDAO m_databaseRecord;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        public AuctionHouseInstance(AuctionHouseDAO record)
        {
            m_databaseRecord = record;
            m_categoriesByTemplate = new Dictionary<int, List<AuctionCategory>>();
            m_templatesByType = new Dictionary<int, List<int>>();
            m_characterItemCount = new Dictionary<long, int>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="itemId"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public AuctionBuyResultEnum TryBuy(CharacterEntity character, long itemId, int quantity, long price)
        {
            Logger.Debug("AuctionHouse::TryBuy itemId=" + itemId + " quantity=" + quantity + " price=" + price);
            return AuctionBuyResultEnum.SUCCESS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="item"></param>
        public AuctionAddResultEnum TryAdd(CharacterEntity character, long itemId, int floor, long price)
        {
            Logger.Debug("AuctionHouse::TryAdd itemId=" + itemId + " floor=" + floor + " price=" + price);

            var item = character.Inventory.GetItem(itemId);
            if (item == null)
                return AuctionAddResultEnum.INVALID_ITEM;

            int quantity = 0;
            switch((AuctionCategoryFloorEnum)floor)
            {
                case AuctionCategoryFloorEnum.FLOOR_ONE: quantity = 1; break;
                case AuctionCategoryFloorEnum.FLOOR_TEN: quantity = 10; break;
                case AuctionCategoryFloorEnum.FLOOR_HUNDRED: quantity = 100; break;
                default: return AuctionAddResultEnum.INVALID_FLOOR;
            }

            if(item.Quantity < quantity)
                return AuctionAddResultEnum.INVALID_QUANTITY;

            if (price < 1)
                return AuctionAddResultEnum.INVALID_PRICE;

            var taxe = 1 + ((price / 100) * Taxe);
            if (character.Inventory.Kamas < taxe)
                return AuctionAddResultEnum.NOT_ENOUGH_KAMAS_FOR_TAXE;

            if (m_characterItemCount.ContainsKey(character.Id) && m_characterItemCount[character.Id] >= PlayerMaxItem)
                return AuctionAddResultEnum.TOO_MANY_ENTRIES;

            if (item.GetTemplate().Level > ItemMaxLevel)
                return AuctionAddResultEnum.TOO_HIGH_LEVEL;

            var entry = new AuctionEntry(AuctionHouseEntryDAO.Create(item.Id, Id, character.Id, price), item);
            if (entry == null)
                return AuctionAddResultEnum.ERROR;

            var newItem = character.Inventory.RemoveItem(item.Id, quantity);
            newItem.OwnerType = (int)EntityTypeEnum.TYPE_AUCTION_HOUSE;
            newItem.OwnerId = Id;

            character.Inventory.SubKamas(taxe);
            Add(entry);

            return AuctionAddResultEnum.SUCCESS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        public void Add(AuctionEntry entry)
        {            
            var templateId = entry.Item.TemplateId;

            AuctionCategory category = null;

            if (!m_categoriesByTemplate.ContainsKey(templateId))
                m_categoriesByTemplate.Add(templateId, new List<AuctionCategory>());
            
            category = m_categoriesByTemplate[templateId].Find(categ => categ.IsValidForThisCategory(entry.Item));

            if (category == null)
            {
                category = new AuctionCategory(templateId);

                var type = entry.Item.GetTemplate().Type;
                if (!m_templatesByType.ContainsKey(type))
                    m_templatesByType.Add(type, new List<int>());

                m_categoriesByTemplate[templateId].Add(category);
                m_templatesByType[type].Add(templateId);
            }

            category.Add(entry);

            if (!m_characterItemCount.ContainsKey(entry.OwnerId))
                m_characterItemCount.Add(entry.OwnerId, 0);
            m_characterItemCount[entry.OwnerId]++;

        }
    }
}
