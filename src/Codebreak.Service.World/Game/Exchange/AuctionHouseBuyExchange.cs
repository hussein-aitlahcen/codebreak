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
        public override void Create()
        {
            Npc.AuctionHouse.AddHandler(Character.Dispatch);

            base.Create();
        }

        public override void Leave(bool success = false)
        {
            Npc.AuctionHouse.RemoveHandler(Character.Dispatch);

            base.Leave(success);
        }
    }
}
