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
    public enum ChallengeTypeEnum
    {
        ZOMBIE = 1,
        STATUE = 2, 
        APPOINTED_VOLUNTARY = 3, // designé volontaire
        REPRIEVE = 4, // sursis
        PEELER = 5, // econome
        VERSATILE = 6, 
        BARBARIRAN = 9, // barbare
        CIRCULATE = 21,
        LOST_SIGHT = 23, // perdu de vu
        SURVIVOR = 33, 
        BOLRD = 36, // hardi
        TIGHTS = 37, // collant
        ANACHORITE = 39, // anachorite
        PETULANT = 41,
        ABNEGATION = 43,
    }

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
        public override void CheckMovement(int beginCell, int endCell, int length)
        {
            if(length != 1 || _hasMoved)
            {
                OnFailed();
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
                OnFailed();
            _hasMoved = false;
        }
    }
}
