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

            ShowTarget = true;
            TargetId = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="team"></param>
        public override void StartFight(FightTeam team)
        {
            if (team.OpponentTeam.HasSomeoneAlive)
            {
                var randomIndex = Util.Next(0, team.OpponentTeam.AliveFighters.Count());
                var target = team.OpponentTeam.AliveFighters.ElementAt(randomIndex);

                Target = target;
                TargetId = target.Id;
                base.FlagCell(target.Cell.Id, TargetId);
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
                if (fighter.Team.AliveFighters.Count() == 0)
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
}
