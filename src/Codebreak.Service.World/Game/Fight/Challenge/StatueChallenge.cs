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
        private int m_cellId;

        /// <summary>
        /// 
        /// </summary>
        public StatueChallenge()
            : base(ChallengeTypeEnum.STATUE)
        {
            BasicDropBonus = 25;
            BasicXpBonus = 25;

            TeamDropBonus = 55;
            TeamXpBonus = 55;

            ShowTarget = false;
            TargetId = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public override void BeginTurn(FighterBase fighter)
        {
            m_cellId = fighter.Cell.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public override void EndTurn(FighterBase fighter)
        {
            if (fighter.Cell.Id != m_cellId)
                base.OnFailed(fighter.Name);
        }
    }
}
