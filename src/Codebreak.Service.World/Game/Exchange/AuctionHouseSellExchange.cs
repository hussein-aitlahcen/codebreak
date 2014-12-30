using Codebreak.Service.World.Game.Auction;
using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Exchange
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuctionHouseSellExchange : ExchangeBase
    {
        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity Character
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public AuctionHouseInstance AuctionHouse
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="npc"></param>
        public AuctionHouseSellExchange(CharacterEntity character, AuctionHouseInstance auctionHouse)
            : base(ExchangeTypeEnum.EXCHANGE_BIGSTORE_SELL)
        {
            Character = character;
            AuctionHouse = auctionHouse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="templateId"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        public void BuyItem(EntityBase actor, int itemId, int quantity, long price)
        {

        }
    }
}
