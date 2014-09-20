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
    public sealed class ReprieveChallenge : ChallengeBase
    {
        /// <summary>
        /// 
        /// </summary>
        public ReprieveChallenge()
            : base(ChallengeTypeEnum.REPRIEVE)
        {
            BasicDropBonus = 10;
            BasicXpBonus = 10;

            TeamDropBonus = 15;
            TeamXpBonus = 15;

            ShowTarget = false;
            TargetId = 0;
        }
    }
}
