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
    public sealed class ZombieChallenge : ChallengeBase
    {
        /// <summary>
        /// 
        /// </summary>
        private bool _hasMoved = false;

        /// <summary>
        /// 
        /// </summary>
        public ZombieChallenge()
            : base(ChallengeTypeEnum.ZOMBIE)
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
        /// <param name="beginCell"></param>
        /// <param name="endCell"></param>
        /// <param name="length"></param>
        public override void CheckMovement(FighterBase fighter, int beginCell, int endCell, int length)
        {
            if(length != 1 || _hasMoved)
            {
                base.OnFailed(fighter.Name);
            }
            else
            {
                _hasMoved = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public override void EndTurn(FighterBase fighter)
        {
            if (!_hasMoved)
                OnFailed(fighter.Name);
            _hasMoved = false;
        }
    }
}
