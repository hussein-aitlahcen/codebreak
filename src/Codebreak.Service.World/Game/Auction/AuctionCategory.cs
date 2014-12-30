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
    public enum AuctionCategoryFloorEnum
    {
        FLOOR_ONE = 1,
        FLOOR_TEN = 2,
        FLOOR_HUNDRED = 3,
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class AuctionCategory
    {
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
        private Dictionary<AuctionCategoryFloorEnum, SortedDictionary<long, AuctionEntry>> m_entriesByFloor;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        public AuctionCategory(int templateId)
        {
            TemplateId = templateId;
            m_entriesByFloor = new Dictionary<AuctionCategoryFloorEnum, SortedDictionary<long, AuctionEntry>>()
            {
                { AuctionCategoryFloorEnum.FLOOR_ONE, new SortedDictionary<long, AuctionEntry>(PriceComparer.Instance) },
                { AuctionCategoryFloorEnum.FLOOR_TEN, new SortedDictionary<long, AuctionEntry>(PriceComparer.Instance) },
                { AuctionCategoryFloorEnum.FLOOR_HUNDRED, new SortedDictionary<long, AuctionEntry>(PriceComparer.Instance) },
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsValidForThisCategory(InventoryItemDAO item)
        {
            AuctionEntry entry = null;
            foreach(var entries in m_entriesByFloor.Values)            
                if(entry == null)                
                    entry = entries.Values.FirstOrDefault();

            if (entry == null)
                throw new InvalidOperationException("AuctionCategory::IsValidForThisCategory empty category, should not happend.");

            return entry.Item.StringEffects == item.StringEffects;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floor"></param>
        /// <param name="entry"></param>
        public void Add(AuctionEntry entry)
        {
            switch(entry.Item.Quantity)
            {
                case 1:
                    m_entriesByFloor[AuctionCategoryFloorEnum.FLOOR_ONE].Add(entry.Price, entry);
                    break;

                case 10:
                    m_entriesByFloor[AuctionCategoryFloorEnum.FLOOR_TEN].Add(entry.Price, entry);
                    break;

                case 100:                    
                    m_entriesByFloor[AuctionCategoryFloorEnum.FLOOR_HUNDRED].Add(entry.Price, entry);
                    break;
            }
        }
    }
}
