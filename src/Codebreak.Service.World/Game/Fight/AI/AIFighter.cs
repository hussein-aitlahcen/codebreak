using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Fight.AI.Brain;

namespace Codebreak.Service.World.Game.Fight.AI
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AIFighter : FighterBase
    {
        /// <summary>
        /// 
        /// </summary>
        public override bool TurnReady
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool TurnPass
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public AIBrain CurrentBrain
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        protected AIFighter(EntityTypeEnum type, long id) 
            : base(type, id)
        {
            CurrentBrain = new DefaultAIBrain(this);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        /// <param name="team"></param>
        public override void JoinFight(FightBase fight, FightTeam team)
        {
            Life = MaxLife;

            base.JoinFight(fight, team);
        }     
    }
}
