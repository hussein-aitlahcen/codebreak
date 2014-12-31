using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
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
    public enum AuctionCategoryFloorEnum
    {
        FLOOR_ONE = 1,
        FLOOR_TEN = 2,
        FLOOR_HUNDRED = 3,
        INVALID = -1,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum AuctionBuyResultEnum
    {
        NOT_ENOUGH_KAMAS,
        ALREADY_SOLD,
        SUCCES,
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class AuctionCategory
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int TemplateId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int ItemType
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return FirstOrDefault() == null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<AuctionCategoryFloorEnum, List<AuctionEntry>> m_entriesByFloor;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        public AuctionCategory(int itemType, int templateId, int id)
        {
            Id = id;
            ItemType = itemType;
            TemplateId = templateId;
            m_entriesByFloor = new Dictionary<AuctionCategoryFloorEnum, List<AuctionEntry>>()
            {
                { AuctionCategoryFloorEnum.FLOOR_ONE, new List<AuctionEntry>() },
                { AuctionCategoryFloorEnum.FLOOR_TEN, new List<AuctionEntry>() },
                { AuctionCategoryFloorEnum.FLOOR_HUNDRED, new List<AuctionEntry>() },
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AuctionEntry FirstOrDefault()
        {
            AuctionEntry entry = null;
            foreach (var entries in m_entriesByFloor.Values)
                if (entry == null)
                    entry = entries.FirstOrDefault();
            return entry;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AuctionEntry FirstOrDefault(AuctionCategoryFloorEnum floor)
        {
            return m_entriesByFloor[floor].FirstOrDefault();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsValidForThisCategory(InventoryItemDAO item)
        {
            AuctionEntry entry = FirstOrDefault();
            if (entry == null)
                throw new InvalidOperationException("AuctionCategory::IsValidForThisCategory empty category, should not happend.");

            return entry.Item.StringEffects == item.StringEffects;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        public AuctionBuyResultEnum Buy(CharacterEntity character, int quantity, long price)
        {
            AuctionCategoryFloorEnum floor = GetFloorByQuantity(quantity);
            var auctionEntry = FirstOrDefault(floor);
            if (auctionEntry == null || auctionEntry.Price != price)
                return AuctionBuyResultEnum.ALREADY_SOLD;

            // TODO : BANQUE, pas sur le personnage
            WorldService.Instance.AddMessage(() =>
                {
                    var seller = EntityManager.Instance.GetCharacterByAccount(auctionEntry.Owner.AccountId);
                    if(seller != null)
                    {
                        seller.AddMessage(() =>
                            {
                                seller.Inventory.AddKamas(price);
                                seller.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_AUCTION_BANK_CREDITED, price, auctionEntry.Item.TemplateId));
                            });
                    }
                    else
                    {
                        auctionEntry.Owner.Kamas += price;
                    }
                });

            character.Inventory.SubKamas(price);
            character.Inventory.AddItem(auctionEntry.Item);

            auctionEntry.Remove();
            m_entriesByFloor[floor].Remove(auctionEntry);

            return AuctionBuyResultEnum.SUCCES;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floor"></param>
        /// <param name="entry"></param>
        public void Add(AuctionEntry entry)
        {
            var floor = GetFloorByQuantity(entry.Item.Quantity);
            if (floor == AuctionCategoryFloorEnum.INVALID)
                throw new InvalidOperationException("AuctionCategory::Add invalid floor for quantity=" + entry.Item.Quantity);
            m_entriesByFloor[floor].Add(entry);
            m_entriesByFloor[floor].Sort();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public AuctionCategoryFloorEnum GetFloorByQuantity(int quantity)
        {
            switch(quantity)
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
        /// <param name="quantity"></param>
        /// <returns></returns>
        public int GetQuantityByFloor(AuctionCategoryFloorEnum floor)
        {
            switch (floor)
            {
                case AuctionCategoryFloorEnum.FLOOR_ONE: return 1;
                case AuctionCategoryFloorEnum.FLOOR_TEN: return 10;
                case AuctionCategoryFloorEnum.FLOOR_HUNDRED: return 100;
                default: throw new InvalidOperationException("AuctionCategory::GetQuantityByFloor invalid floor.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floor"></param>
        /// <returns></returns>
        public string GetLowerPrice(AuctionCategoryFloorEnum floor)
        {
            AuctionEntry entry = FirstOrDefault(floor);
            if (entry == null)
                return "";
            return entry.Price.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SerializeAs_BuyExchange()
        {
            return "|" + Id + ";"
                + FirstOrDefault().Item.StringEffects + ";"
                + GetLowerPrice(AuctionCategoryFloorEnum.FLOOR_ONE) + ";"
                + GetLowerPrice(AuctionCategoryFloorEnum.FLOOR_TEN) + ";"
                + GetLowerPrice(AuctionCategoryFloorEnum.FLOOR_HUNDRED) + ";";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="message"></param>
        public void SerializeAs_CategoryMovement(OperatorEnum op, StringBuilder message)
        {
            switch(op)
            {
                case OperatorEnum.OPERATOR_ADD:
                case OperatorEnum.OPERATOR_REFRESH:
                    message.Append(Id).Append('|');
                    message.Append(TemplateId).Append('|');
                    message.Append(FirstOrDefault().Item.StringEffects).Append('|');
                    message.Append(GetLowerPrice(AuctionCategoryFloorEnum.FLOOR_ONE)).Append('|');
                    message.Append(GetLowerPrice(AuctionCategoryFloorEnum.FLOOR_TEN)).Append('|');
                    message.Append(GetLowerPrice(AuctionCategoryFloorEnum.FLOOR_HUNDRED)).Append('|');
                    break;

                case OperatorEnum.OPERATOR_REMOVE:
                    message.Append(Id).Append('|');
                    break;
            }
        }
    }
}
