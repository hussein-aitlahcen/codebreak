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
    public sealed class AuctionHouseBuyExchange : AuctionHouseExchange
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="npc"></param>
        public AuctionHouseBuyExchange(CharacterEntity character, NonPlayerCharacterEntity npc)
            : base(ExchangeTypeEnum.EXCHANGE_AUCTION_HOUSE_BUY, character, npc)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="templateId"></param>
        /// <param name="quantity"></param>
        public override void BuyItem(EntityBase actor, int templateId, int quantity)
        {
            base.BuyItem(actor, templateId, quantity);
        }
    }
}
