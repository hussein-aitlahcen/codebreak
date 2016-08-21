using Codebreak.Service.World.Game.Map;
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
    public sealed class AnachoriteChallenge : AbstractChallenge
    {
        /// <summary>
        /// 
        /// </summary>
        public AnachoriteChallenge()
            : base(ChallengeTypeEnum.ANACHORITE)
        {
            BasicDropBonus = 20;
            BasicXpBonus = 20;

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
            var aroundFighters = Pathfinding.GetFightersNear(fighter.Fight, fighter.Cell.Id);
            if(aroundFighters.Where(f => f.Team == fighter.Team).Count() > 0)            
                base.OnFailed(fighter.Name);            
        }
    }
}
