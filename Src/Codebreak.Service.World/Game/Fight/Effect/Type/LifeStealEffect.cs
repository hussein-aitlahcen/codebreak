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
    public sealed class LifeStealEffect : EffectBase
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

            if (DamageEffect.ApplyDamages(castInfos, castInfos.Target, ref damageJet) == FightActionResultEnum.RESULT_END)
                return FightActionResultEnum.RESULT_END;

            castInfos.EffectType = Spell.EffectEnum.DamageBrut;

            // On ne se soigne que de 50% des dégats
            var healJet = damageJet / 2;

            return HealEffect.ApplyHeal(castInfos, castInfos.Caster, ref healJet);
        }
    }
}
