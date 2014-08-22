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
    public sealed class DamageLifePercentEffect : EffectBase
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

            var damageInfos = new CastInfos(EffectEnum.DamageBrut, -1, -1, -1, -1, -1, -1, -1, castInfos.Caster, castInfos.Target);
            var damageJet = (castInfos.Target.Life / 100) * castInfos.RandomJet;

            return DamageEffect.ApplyDamages(damageInfos, damageInfos.Target, ref damageJet);
        }
    }
}
