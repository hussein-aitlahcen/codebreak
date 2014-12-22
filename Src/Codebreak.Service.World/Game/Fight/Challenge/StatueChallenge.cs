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
    public sealed class StatueChallenge : ChallengeBase
    {
        private int _cellId;

        /// <summary>
        /// 
        /// </summary>
        public StatueChallenge()
            : base(ChallengeTypeEnum.STATUE)
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
        public override void BeginTurn(FighterBase fighter)
        {
            _cellId = fighter.Cell.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public override void EndTurn(FighterBase fighter)
        {
            if (fighter.Cell.Id != _cellId)
                base.OnFailed();
        }
    }
}
