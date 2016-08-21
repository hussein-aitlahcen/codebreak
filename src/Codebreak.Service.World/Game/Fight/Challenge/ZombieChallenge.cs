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
    public sealed class ZombieChallenge : AbstractChallenge
    {
        /// <summary>
        /// 
        /// </summary>
        private bool m_hasMoved = false;

        /// <summary>
        /// 
        /// </summary>
        public ZombieChallenge()
            : base(ChallengeTypeEnum.ZOMBIE)
        {
            BasicDropBonus = 30;
            BasicXpBonus = 30;

            TeamDropBonus = 50;
            TeamXpBonus = 50;

            ShowTarget = false;
            TargetId = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginCell"></param>
        /// <param name="endCell"></param>
        /// <param name="length"></param>
        public override void CheckMovement(AbstractFighter fighter, int beginCell, int endCell, int length)
        {
            if(length != 1 || m_hasMoved)            
                base.OnFailed(fighter.Name);            
            else            
                m_hasMoved = true;            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public override void EndTurn(AbstractFighter fighter)
        {
            if (!m_hasMoved)
                OnFailed(fighter.Name);
            m_hasMoved = false;
        }
    }
}
