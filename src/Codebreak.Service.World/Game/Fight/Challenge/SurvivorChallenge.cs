using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.Challenge
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SurvivorChallenge : AbstractChallenge
    {
        /// <summary>
        /// 
        /// </summary>
        public SurvivorChallenge()
            : base(ChallengeTypeEnum.SURVIVOR)
        {
            BasicDropBonus = 30;
            BasicXpBonus = 30;

            TeamDropBonus = 30;
            TeamXpBonus = 30;

            ShowTarget = false;
            TargetId = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public override void EndTurn(AbstractFighter fighter)
        {
            if(fighter.Team.AliveFighters.Count() != fighter.Team.Fighters.Count)            
                base.OnFailed(fighter.Name);            
        }
    }
}
