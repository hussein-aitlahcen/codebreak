using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Exchange;
using Codebreak.Service.World.Game.Interactive.Type;
using Codebreak.Service.World.Game.Job;
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
    public sealed class GameCraftPlanExchangeAction : GameExchangeActionBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public GameCraftPlanExchangeAction(CharacterEntity character, CraftPlan plan, JobSkill skill)
            : base(new CraftPlanExchange(character, plan, skill), character)
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
