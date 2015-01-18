using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Exchange;
using Codebreak.Service.World.Manager;
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
        private Dictionary<int, AuctionCategory> m_categoryById;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, List<int>> m_templatesByType;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, List<AuctionEntry>> m_auctionsByAccount;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, long> m_templateMiddlePrice;

        /// <summary>
        /// 
        /// </summary>
        private List<int> m_allowedTypes;
        
        /// <summary>
        /// 
        /// </summary>
        private AuctionHouseDAO m_databaseRecord;

        /// <summary>
        /// 
        /// </summary>
        private int m_nextCategoryId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        public AuctionHouseInstance(AuctionHouseDAO record)
        {
            m_databaseRecord = record;
            m_categoriesByTemplate = new Dictionary<int, List<AuctionCategory>>();
            m_categoryById = new Dictionary<int, AuctionCategory>();
            m_templatesByType = new Dictionary<int, List<int>>();
            m_auctionsByAccount = new Dictionary<long, List<AuctionEntry>>();
            m_allowedTypes = new List<int>();
            m_templateMiddlePrice = new Dictionary<int, long>();
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
        /// <param name="id"></param>
        /// <returns></returns>
        public AuctionCategoryFloorEnum GetFloorById(int id)
        {
            switch(id)
            {
                case 1: return AuctionCategoryFloorEnum.FLOOR_ONE;
                case 2: return AuctionCategoryFloorEnum.FLOOR_TEN;
                case 3: return AuctionCategoryFloorEnum.FLOOR_HUNDRED;
                default: return AuctionCategoryFloorEnum.INVALID;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="itemId"></param>
        /// <param name="floorId"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public void TryBuy(CharacterEntity character, int categoryId, int floorId, long price)
        {
            Logger.Debug("AuctionHouse::TryBuy categoryId=" + categoryId + " floorId=" + floorId + " price=" + price);

            if(price < 1)
            {
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if(character.Inventory.Kamas < price)
            {
                character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_NOT_ENOUGH_KAMAS));
                return;
            }

            if(!m_categoryById.ContainsKey(categoryId))
            {
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var category = m_categoryById[categoryId];
            var floor = GetFloorById(floorId);
            if(floor == AuctionCategoryFloorEnum.INVALID)
            {
                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
            var auction = category.FirstOrDefault(floor);

            switch (category.Buy(character, floor, price))
            {
                case AuctionBuyResultEnum.ALREADY_SOLD:
                    character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_ITEM_ALREADY_SOLD));
                    SendCategoriesByTemplate(character, category.TemplateId);
                    break;

                case AuctionBuyResultEnum.NOT_ENOUGH_KAMAS:
                    character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_NOT_ENOUGH_KAMAS));
                    break;

                case AuctionBuyResultEnum.SUCCES:
                    m_auctionsByAccount[auction.Owner.AccountId].Remove(auction);
                    if (!CheckEmptyCategory(category))
                    {
                        SendCategoriesByTemplate(character, category.TemplateId);
                    }

                    UpdateMiddlePrice(category.TemplateId);

                    character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_AUCTION_LOT_BOUGHT));

                    WorldService.Instance.AddMessage(() =>
                    {
                        var seller = EntityManager.Instance.GetCharacterByAccount(auction.Owner.AccountId);
                        if (seller != null)
                        {
                            seller.AddMessage(() =>
                            {
                                seller.Bank.AddKamas(price);
                                seller.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_AUCTION_BANK_CREDITED, price, auction.Item.TemplateId));
                                if (seller.HasGameAction(GameActionTypeEnum.EXCHANGE))
                                {
                                    var action = seller.CurrentAction as GameAuctionHouseSellAction;
                                    if (action != null)
                                    {
                                        if (action.AuctionExchange.Npc.AuctionHouse.Id == Id)
                                        {
                                            action.AuctionExchange.Npc.AuctionHouse.SendAuctionOwnerList(seller);
                                        }
                                    }
                                }
                            });
                        }
                        else
                        {
                            BankManager.Instance.GetBankByAccountId(auction.Owner.AccountId).AddKamas(price);
                        }
                    });
                    break;

                default:
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool CheckEmptyCategory(AuctionCategory category)
        {
            if (category.IsEmpty)
            {
                m_categoryById.Remove(category.Id);
                m_categoriesByTemplate[category.TemplateId].Remove(category);
                if (m_categoriesByTemplate[category.TemplateId].Count == 0)
                {
                    m_templatesByType[category.ItemType].Remove(category.TemplateId);
                    m_categoriesByTemplate.Remove(category.TemplateId);
                    base.Dispatch(WorldMessage.AUCTION_HOUSE_TEMPLATE_MOVEMENT(OperatorEnum.OPERATOR_REMOVE, category.TemplateId));
                }
                base.Dispatch(WorldMessage.AUCTION_HOUSE_CATEGORY_MOVEMENT(OperatorEnum.OPERATOR_REMOVE, category));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        public void UpdateMiddlePrice(int templateId)
        {
            if (!m_templateMiddlePrice.ContainsKey(templateId))
                m_templateMiddlePrice.Add(templateId, 0);
            if (!m_categoriesByTemplate.ContainsKey(templateId))
                return;
            long total = 0;
            foreach (var category in m_categoriesByTemplate[templateId])
                total += category.MiddlePrice;
            m_templateMiddlePrice[templateId] = total / Math.Max(1, m_categoriesByTemplate[templateId].Count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public long GetMiddlePrice(int templateId)
        {
            if (!m_templateMiddlePrice.ContainsKey(templateId))
                m_templateMiddlePrice.Add(templateId, 0);
            return m_templateMiddlePrice[templateId];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="itemId"></param>
        public void TryRemove(CharacterEntity character, long itemId)
        {
            Logger.Debug("AuctionHouse::TryRemove itemId=" + itemId);

            if(!m_auctionsByAccount.ContainsKey(character.AccountId))
            {
                character.Dispatch(WorldMessage.OBJECT_MOVE_ERROR());
                return;
            }

            var auction = m_auctionsByAccount[character.AccountId].Find(entry => entry.ItemId == itemId);
            if(auction == null)
            {
                character.Dispatch(WorldMessage.OBJECT_MOVE_ERROR());
                return;
            }

            m_auctionsByAccount[character.AccountId].Remove(auction);
            AuctionCategory category = null;
            int i = 0;
            var categories = m_categoriesByTemplate[auction.Item.TemplateId];
            while(i < categories.Count && category == null)
            {
                var current = categories[i];
                if(current.Remove(auction))
                    category = current;
            }

            CheckEmptyCategory(category);
            UpdateMiddlePrice(category.TemplateId);

            auction.Remove();
            character.Inventory.AddItem(auction.Item);

            SendAuctionOwnerList(character);
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

            if (!m_allowedTypes.Contains(item.Template.Type))
                return AuctionAddResultEnum.INVALID_TYPE;

            switch(quantity)
            {
                case 1: break;
                case 2: quantity = 10; break;
                case 3: quantity = 100; break;
                default: return AuctionAddResultEnum.INVALID_FLOOR;
            }

            if(item.Quantity < quantity)
                return AuctionAddResultEnum.INVALID_QUANTITY;

            if (price < 1)
                return AuctionAddResultEnum.INVALID_PRICE;

            var taxe = 1 + ((price / 100) * Taxe);
            if (character.Inventory.Kamas < taxe)
                return AuctionAddResultEnum.NOT_ENOUGH_KAMAS_FOR_TAXE;

            if (m_auctionsByAccount.ContainsKey(character.AccountId) && m_auctionsByAccount[character.AccountId].Count >= PlayerMaxItem)
                return AuctionAddResultEnum.TOO_MANY_ENTRIES;

            if (item.Template.Level > ItemMaxLevel)
                return AuctionAddResultEnum.TOO_HIGH_LEVEL;
                                    
            var newItem = character.Inventory.RemoveItem(item.Id, quantity);
            newItem.OwnerType = (int)EntityTypeEnum.TYPE_AUCTION_HOUSE;
            newItem.OwnerId = Id;

            var record = AuctionHouseEntryRepository.Instance.Create(newItem.Id, Id, character.Id, price, Timeout);
            if (record == null)
            {
                character.Inventory.AddItem(newItem);
                return AuctionAddResultEnum.ERROR;
            }

            Add(new AuctionEntry(record, newItem));

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
            var itemType = entry.Item.Template.Type;

            AuctionCategory category = null;

            if (!m_categoriesByTemplate.ContainsKey(templateId))
                m_categoriesByTemplate.Add(templateId, new List<AuctionCategory>());
            
            category = m_categoriesByTemplate[templateId].Find(categ => categ.IsValidForThisCategory(entry.Item));

            if (category == null)
            {
                category = new AuctionCategory(itemType, templateId, m_nextCategoryId++);

                var type = entry.Item.Template.Type;
                if (!m_templatesByType.ContainsKey(type))
                    m_templatesByType.Add(type, new List<int>());

                m_categoriesByTemplate[templateId].Add(category);
                m_categoryById.Add(category.Id, category);
                m_templatesByType[type].Add(templateId);

                category.Add(entry);

                base.Dispatch(WorldMessage.AUCTION_HOUSE_CATEGORY_MOVEMENT(OperatorEnum.OPERATOR_ADD, category));
            }
            else
            {
                category.Add(entry);
            }

            if (!m_auctionsByAccount.ContainsKey(entry.Owner.AccountId))
                m_auctionsByAccount.Add(entry.Owner.AccountId, new List<AuctionEntry>());
            m_auctionsByAccount[entry.Owner.AccountId].Add(entry);
            
            UpdateMiddlePrice(category.TemplateId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void SendAuctionOwnerList(CharacterEntity character)
        {
            if (m_auctionsByAccount.ContainsKey(character.AccountId))
                character.Dispatch(WorldMessage.AUCTION_HOUSE_AUCTION_OWNER_LIST(m_auctionsByAccount[character.AccountId]));
            else
                character.Dispatch(WorldMessage.AUCTION_HOUSE_AUCTION_OWNER_LIST(new List<AuctionEntry>()));
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
