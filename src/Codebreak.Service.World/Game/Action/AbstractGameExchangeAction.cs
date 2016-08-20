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
    public abstract class AbstractGameExchangeAction : AbstractGameAction
    {
        /// <summary>
        /// 
        /// </summary>
        public AbstractExchange Exchange
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public AbstractEntity DistantEntity
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool CanAbort
        {
            get
            { 
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="localEntity"></param>
        /// <param name="distantEntity"></param>
        public AbstractGameExchangeAction(AbstractExchange exchange, AbstractEntity localEntity, AbstractEntity distantEntity = null)
            : base(GameActionTypeEnum.EXCHANGE, localEntity)
        {
            DistantEntity = distantEntity;
            Exchange = exchange;
            Exchange.AddHandler(Entity.Dispatch);
            if(DistantEntity != null)
                Exchange.AddHandler(DistantEntity.Dispatch);
            Entity.AddUpdatable(Exchange);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Accept()
        {
            Exchange.Create();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        public void Leave(bool success = false)
        {
            Exchange.Leave(success);
            Exchange.RemoveHandler(Entity.Dispatch);
            if(DistantEntity != null)
                Exchange.RemoveHandler(DistantEntity.Dispatch);
            Entity.RemoveUpdatable(Exchange);
        }
    }
}
