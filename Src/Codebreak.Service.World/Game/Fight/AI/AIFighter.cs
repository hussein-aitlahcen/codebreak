using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Action;

namespace Codebreak.Service.World.Game.Fight.AI
{
    public abstract class AIFighter : FighterBase
    {
        public override bool TurnReady
        {
            get;
            set;
        }

        public override bool TurnPass
        {
            get;
            set;
        }

        protected AIFighter(EntityTypEnum type, long id) : base(type, id)
        {

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
