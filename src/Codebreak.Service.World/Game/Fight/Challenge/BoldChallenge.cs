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
    public sealed class BoldChallenge : AbstractChallenge
    {
        /// <summary>
        /// 
        /// </summary>
        public BoldChallenge()
            : base(ChallengeTypeEnum.BOLD)
        {
            BasicDropBonus = 25;
            BasicXpBonus = 25;

            TeamDropBonus = 25;
            TeamXpBonus = 25;

            ShowTarget = false;
            TargetId = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public override void EndTurn(AbstractFighter fighter)
        {
            var nearestEnnemis = Pathfinding.GetEnnemiesNear(fighter.Fight, fighter.Team, fighter.Cell.Id);
            if(nearestEnnemis.Count() == 0)            
                base.OnFailed(fighter.Name);
        }
    }
}
