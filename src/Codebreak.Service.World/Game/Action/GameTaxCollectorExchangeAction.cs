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
    public sealed class GameTaxCollectorExchangeAction : AbstractGameExchangeAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="localEntity"></param>
        /// <param name="distantEntity"></param>
        public GameTaxCollectorExchangeAction(CharacterEntity character, TaxCollectorEntity taxCollector)
            : base(new TaxCollectorExchange(character, taxCollector), character, taxCollector)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            Exchange.Create();
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
