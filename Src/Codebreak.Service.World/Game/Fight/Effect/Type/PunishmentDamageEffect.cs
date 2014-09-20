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
    public sealed class PunishmentDamageEffect : EffectBase
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

            castInfos.EffectType = EffectEnum.DamageBrut;
            
            // official formulas
            var damageCoef = (double)castInfos.RandomJet / 100;
            var maxLife = castInfos.Caster.MaxLife;
            var percentLife = (double)castInfos.Caster.Life / castInfos.Caster.MaxLife;
            var rad = (2 * Math.PI * (percentLife - 0.5));
            var cos = Math.Cos(rad);
            var rate = Math.Pow(cos + 1, 2) / 4;
            var maxDamages = damageCoef * maxLife;
            var realDamages = (int)(rate * maxDamages);
            
            return DamageEffect.ApplyDamages(castInfos, castInfos.Target, ref realDamages);
        }
    }
}
