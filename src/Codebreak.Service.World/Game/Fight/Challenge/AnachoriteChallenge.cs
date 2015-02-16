using Codebreak.Service.World.Game.Map;
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
    public sealed class AnachoriteChallenge : ChallengeBase
    {
        /// <summary>
        /// 
        /// </summary>
        public AnachoriteChallenge()
            : base(ChallengeTypeEnum.ANACHORITE)
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
            var aroundFighters = Pathfinding.GetFightersNear(fighter.Fight, fighter.Cell.Id);
            if(aroundFighters.Where(f => f.Team == fighter.Team).Count() > 0)            
                base.OnFailed(fighter.Name);            
        }
    }
}
