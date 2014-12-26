using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Action
{
    public sealed class GamePlayerExchangeAction : GameExchangeActionBase
    {
        public GamePlayerExchangeAction(CharacterEntity localEntity, CharacterEntity distantEntity)
            : base(new PlayerExchange(localEntity, distantEntity), localEntity, distantEntity)
        {
        }

        public override void Start()
        {
            Exchange.Dispatch(WorldMessage.EXCHANGE_REQUEST(Entity.Id, DistantEntity.Id));
        }

        public override void Stop(params object[] args)
        {
            IsFinished = true;

            if (int.Parse(args[0].ToString()) == Entity.Id)
                DistantEntity.StopAction(GameActionTypeEnum.EXCHANGE);
            else
                Entity.StopAction(GameActionTypeEnum.EXCHANGE);

            base.Leave(true);
        }

        public override void Abort(params object[] args)
        {
            IsFinished = true;

            if (int.Parse(args[0].ToString()) == Entity.Id)
                DistantEntity.AbortAction(GameActionTypeEnum.EXCHANGE);
            else
                Entity.AbortAction(GameActionTypeEnum.EXCHANGE);

            base.Leave();
        }
    }
}
