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
    public sealed class SightLostChallenge : ChallengeBase
    {
        /// <summary>
        /// 
        /// </summary>
        public SightLostChallenge()
            : base(ChallengeTypeEnum.LOST_SIGHT)
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
        /// <param name="castInfos"></param>
        public override void CheckSpell(FighterBase fighter, Effect.CastInfos castInfos)
        {
            if((castInfos.EffectType == Spell.EffectEnum.SubPO ||
                castInfos.EffectType == Spell.EffectEnum.POSteal) &&
                castInfos.Target != null &&
                castInfos.Target.Team != fighter.Team)
            {
                base.OnFailed(fighter.Name);
            }
        }
    }
}
