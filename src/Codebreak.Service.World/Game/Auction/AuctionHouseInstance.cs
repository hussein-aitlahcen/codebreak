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
        INVALID_TYPE,
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
        public IEnumerable<int> AllowedTypes
        {
            get
            {
                return m_allowedTypes;
            }
        }
    
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, List<AuctionCategory>> m_categoriesByTemplate;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, List<int>> m_templatesByType;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, int> m_characterItemCount;

        /// <summary>
        /// 
        /// </summary>
        private List<int> m_allowedTypes;

        /// <summary>
        /// 
        /// </summary>
        private List<AuctionEntry> m_entries;

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
            m_allowedTypes = new List<int>();
            m_entries = new List<AuctionEntry>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void AddAllowedType(int id)
        {
            m_allowedTypes.Add(id);
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
        public AuctionAddResultEnum TryAdd(CharacterEntity character, long itemId, int quantity, long price)
        {
            Logger.Debug("AuctionHouse::TryAdd itemId=" + itemId + " quantity=" + quantity + " price=" + price);

            var item = character.Inventory.GetItem(itemId);
            if (item == null)
                return AuctionAddResultEnum.INVALID_ITEM;

            if (!m_allowedTypes.Contains(item.GetTemplate().Type))
                return AuctionAddResultEnum.INVALID_TYPE;

            switch(quantity)
            {
                case 1:
                case 10:
                case 100:
                    break;
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

            var record = AuctionHouseEntryDAO.Create(item.Id, Id, character.Id, price, Timeout);
            if (record == null)
                return AuctionAddResultEnum.ERROR;

            var entry = new AuctionEntry(record, item);

            var newItem = character.Inventory.RemoveItem(item.Id, quantity);
            newItem.OwnerType = (int)EntityTypeEnum.TYPE_AUCTION_HOUSE;
            newItem.OwnerId = Id;
                       
            Add(entry);

            character.Inventory.SubKamas(taxe);
            SendAuctionOwnerList(character);

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

            m_entries.Add(entry);
            category.Add(entry);

            if (!m_characterItemCount.ContainsKey(entry.OwnerId))
                m_characterItemCount.Add(entry.OwnerId, 0);
            m_characterItemCount[entry.OwnerId]++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void SendAuctionOwnerList(CharacterEntity character)
        {
            character.Dispatch(WorldMessage.AUCTION_HOUSE_AUCTION_OWNER_LIST(m_entries.Where(entry => entry.Owner.AccountId == character.AccountId)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void SendTemplatesByTypeList(CharacterEntity character, int type)
        {
            var templates = new List<int>();
            if(m_templatesByType.ContainsKey(type))
                templates.AddRange(m_templatesByType[type]);
            character.Dispatch(WorldMessage.AUCTION_HOUSE_TEMPLATE_LIST(type, templates));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="templateId"></param>
        public void SendCategoriesByTemplate(CharacterEntity character, int templateId)
        {
            var categories = new List<AuctionCategory>();
            if (m_categoriesByTemplate.ContainsKey(templateId))
                categories.AddRange(m_categoriesByTemplate[templateId]);
            character.Dispatch(WorldMessage.AUCTION_HOUSE_AUCTION_LIST(templateId, categories));
        }
    }
}
