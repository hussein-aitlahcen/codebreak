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
    public abstract class AbstractGameAuctionHouseAction : AbstractGameExchangeAction
    {
        /// <summary>
        /// 
        /// </summary>
        public AuctionHouseExchange AuctionExchange
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="character"></param>
        /// <param name="npc"></param>
        public AbstractGameAuctionHouseAction(AuctionHouseExchange exchange, CharacterEntity character, NonPlayerCharacterEntity npc)
            : base(exchange, character, npc)
        {
            AuctionExchange = exchange;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            base.Accept();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Stop(params object[] args)
        {
            base.Leave(true);
            base.Stop(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Abort(params object[] args)
        {
            base.Leave();
            base.Abort(args);
        }
    }
}
