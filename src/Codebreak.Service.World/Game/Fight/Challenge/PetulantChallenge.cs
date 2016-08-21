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
    public sealed class PetulantChallenge : AbstractChallenge
    {
        /// <summary>
        /// 
        /// </summary>
        public PetulantChallenge()
             : base(ChallengeTypeEnum.PETULANT)
        {
            BasicDropBonus = 10;
            BasicXpBonus = 10;

            TeamDropBonus = 10;
            TeamXpBonus = 10;

            ShowTarget = false;
            TargetId = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public override void EndTurn(AbstractFighter fighter)
        {
            if (fighter.AP > 0)
                base.OnFailed(fighter.Name);
        }
    }
}
