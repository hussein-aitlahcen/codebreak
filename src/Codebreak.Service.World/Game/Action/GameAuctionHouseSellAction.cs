using Codebreak.Service.World.Game.Auction;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Action
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameAuctionHouseSellAction : AbstractGameAuctionHouseAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="npc"></param>
        public GameAuctionHouseSellAction(CharacterEntity character, NonPlayerCharacterEntity npc)
            : base(new AuctionHouseSellExchange(character, npc), character, npc)
        {
        }
    }
}
