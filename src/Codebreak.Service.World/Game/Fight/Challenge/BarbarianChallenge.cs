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
    public sealed class BarbarianChallenge : ChallengeBase
    {
        /// <summary>
        /// 
        /// </summary>
        public BarbarianChallenge()
            : base(ChallengeTypeEnum.BARBARIRAN)
        {
            BasicDropBonus = 60;
            BasicXpBonus = 60;

            TeamDropBonus = 75;
            TeamXpBonus = 75;

            ShowTarget = false;
            TargetId = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="castInfos"></param>
        public override void CheckSpell(FighterBase fighter, Effect.CastInfos castInfos)
        {
            base.OnFailed(fighter.Name);
        }
    }
}
