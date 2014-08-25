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
    public sealed class SelfDamageEffect : EffectBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(CastInfos castInfos)
        {
            var damageJet = castInfos.RandomJet;
            castInfos.EffectType = Spell.EffectEnum.DamageNeutral;

            return DamageEffect.ApplyDamages(castInfos, castInfos.Caster, ref damageJet);
        }
    }
}
