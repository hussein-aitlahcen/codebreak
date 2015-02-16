using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.Challenges
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SurvivorChallenge : ChallengeBase
    {
        /// <summary>
        /// 
        /// </summary>
        public SurvivorChallenge()
            : base(ChallengeTypeEnum.SURVIVOR)
        {
            BasicDropBonus = 10;
            BasicXpBonus = 10;

            TeamDropBonus = 15;
            TeamXpBonus = 15;

            ShowTarget = false;
            TargetId = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public override void EndTurn(FighterBase fighter)
        {
            if(fighter.Team.AliveFighters.Count() != fighter.Team.Fighters.Count)            
                base.OnFailed(fighter.Name);            
        }
    }
}
