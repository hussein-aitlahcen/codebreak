using Codebreak.Service.World.Game.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.Effect.Type
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PureLifeStealEffect : AbstractSpellEffect
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(CastInfos castInfos)
        {
            if (castInfos.Target == null)
                return FightActionResultEnum.RESULT_NOTHING;

            var damageJet = castInfos.RandomJet;
            castInfos.EffectType = EffectEnum.DamageBrut;

            // cannot kill the target if hes an ally
            if (castInfos.Caster.Team == castInfos.Target.Team && damageJet > castInfos.Target.Life)
                damageJet = castInfos.Target.Life - 1;

            if (DamageEffect.ApplyDamages(castInfos, castInfos.Target, ref damageJet) == FightActionResultEnum.RESULT_END)
                return FightActionResultEnum.RESULT_END;

            var healJet = damageJet / 2;

            return HealEffect.ApplyHeal(castInfos, castInfos.Caster, ref healJet);
        }
    }
}
