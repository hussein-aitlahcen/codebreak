using Codebreak.WorldService.World.Entity;
using Codebreak.WorldService.World.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Action
{
    public sealed class GameShopExchangeAction : GameExchangeActionBase
    {
        public GameShopExchangeAction(EntityBase buyer, EntityBase shop)
            : base(new ShopExchange(buyer, shop), buyer, shop)
        {
            Accept();
        }

        public override void Start()
        {
        }

        public override void Stop(params object[] args)
        {
            IsFinished = true;

            base.Leave(true);
        }

        public override void Abort(params object[] args)
        {
            IsFinished = true;

            base.Leave(false);
        }
    }
}
