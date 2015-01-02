using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public long MiddlePrice
        {
            get
            {
                long total = 0;
                long count = 0;
                var auctions = All();
                foreach (var auction in auctions)
                {
                    total += auction.Price;
                    count += auction.Item.Quantity;
                }
                return total / Math.Max(1, count);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<AuctionCategoryFloorEnum, List<AuctionEntry>> m_auctionsByFloor;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        public AuctionCategory(int itemType, int templateId, int id)
        {
            Id = id;
            ItemType = itemType;
            TemplateId = templateId;
            m_auctionsByFloor = new Dictionary<AuctionCategoryFloorEnum, List<AuctionEntry>>()
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
        public IEnumerable<AuctionEntry> All()
        {
            foreach (var entries in m_auctionsByFloor.Values)
                foreach (var entry in entries)
                    yield return entry;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AuctionEntry FirstOrDefault()
        {
            AuctionEntry auction = null;
            foreach (var entries in m_auctionsByFloor.Values)
                if (auction == null)
                    auction = entries.FirstOrDefault();
            return auction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AuctionEntry FirstOrDefault(AuctionCategoryFloorEnum floor)
        {
            return m_auctionsByFloor[floor].FirstOrDefault();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsValidForThisCategory(InventoryItemDAO item)
        {
            AuctionEntry auction = FirstOrDefault();
            if (auction == null)
                throw new InvalidOperationException("AuctionCategory::IsValidForThisCategory empty category, should not happend.");

            return auction.Item.StringEffects == item.StringEffects;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        public AuctionBuyResultEnum Buy(CharacterEntity character, AuctionCategoryFloorEnum floor, long price)
        {
            var auction = FirstOrDefault(floor);
            if (auction == null || auction.Price != price)
                return AuctionBuyResultEnum.ALREADY_SOLD;

            character.Inventory.SubKamas(price);
            character.Inventory.AddItem(auction.Item);

            auction.Remove();
            m_auctionsByFloor[floor].Remove(auction);

            return AuctionBuyResultEnum.SUCCES;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auction"></param>
        public bool Remove(AuctionEntry auction)
        {
            return m_auctionsByFloor[GetFloorByQuantity(auction.Item.Quantity)].Remove(auction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floor"></param>
        /// <param name="auction"></param>
        public void Add(AuctionEntry auction)
        {
            var floor = GetFloorByQuantity(auction.Item.Quantity);
            if (floor == AuctionCategoryFloorEnum.INVALID)
                throw new InvalidOperationException("AuctionCategory::Add invalid floor for quantity=" + auction.Item.Quantity);
            m_auctionsByFloor[floor].Add(auction);
            m_auctionsByFloor[floor].Sort();
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
                case 10: return AuctionCategoryFloorEnum.FLOOR_TEN;
                case 100: return AuctionCategoryFloorEnum.FLOOR_HUNDRED;
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
