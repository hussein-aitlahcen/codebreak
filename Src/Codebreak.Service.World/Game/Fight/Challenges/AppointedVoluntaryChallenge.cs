using Codebreak.Service.World.Network;
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
    public sealed class AppointedVoluntaryChallenge : ChallengeBase
    {
        /// <summary>
        /// 
        /// </summary>
        public AppointedVoluntaryChallenge()
            : base(ChallengeTypeEnum.APPOINTED_VOLUNTARY)
        {

            BasicDropBonus = 10;
            BasicXpBonus = 10;

            TeamDropBonus = 15;
            TeamXpBonus = 15;

            ShowTarget = true;
            TargetId = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public override void BeginTurn(FighterBase fighter)
        {
            if(TargetId == 0)
            {
                var randomIndex = Util.Next(0, fighter.Team.OpponentTeam.AliveFighters.Count());
                var target = fighter.Team.OpponentTeam.AliveFighters.ElementAt(randomIndex);

                TargetId = target.Id;
                base.FlagCell(target.Cell.Id);                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public override void CheckDeath(FighterBase fighter)
        {
            if (fighter.Id == TargetId)
            {
                base.OnSuccess();
            }
            else
            {
                base.OnFailed();
            }
        }
    }
}
