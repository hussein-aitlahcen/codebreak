using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Action
{
    public abstract class GameExchangeActionBase : GameActionBase
    {
        public ExchangeBase Exchange
        {
            get;
            private set;
        }

        public EntityBase DistantEntity
        {
            get;
            private set;
        }

        public override bool CanAbort
        {
            get
            { 
                return true;
            }
        }

        public GameExchangeActionBase(ExchangeBase exchange, EntityBase localEntity, EntityBase distantEntity)
            : base(GameActionTypeEnum.EXCHANGE, localEntity)
        {
            DistantEntity = distantEntity;
            Exchange = exchange;
            Exchange.AddHandler(Entity.Dispatch);
            Exchange.AddHandler(DistantEntity.Dispatch);
        }
        
        public void Accept()
        {
            Exchange.Create();
        }

        public void Leave(bool success = false)
        {
            Exchange.Leave(success);
            Exchange.RemoveHandler(Entity.Dispatch);
            Exchange.RemoveHandler(DistantEntity.Dispatch);
        }
    }
}
