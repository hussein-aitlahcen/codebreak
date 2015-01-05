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
    public sealed class GameShopExchangeAction : GameExchangeActionBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buyer"></param>
        /// <param name="shop"></param>
        public GameShopExchangeAction(EntityBase buyer, NonPlayerCharacterEntity shop)
            : base(new ShopExchange(buyer, shop), buyer, shop)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            Accept();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Stop(params object[] args)
        {
            IsFinished = true;
            base.Leave(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Abort(params object[] args)
        {
            IsFinished = true;
            base.Leave(false);
        }
    }
}
