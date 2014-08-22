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
    public sealed class DropLifeEffect : EffectBase
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

            // damage himself
            if (castInfos.Target == castInfos.Caster)
            {
                var damageInfos = new CastInfos(EffectEnum.DamageBrut, castInfos.SpellId, castInfos.CellId, castInfos.Value1, castInfos.Value2, castInfos.Value3, castInfos.Chance, 0, castInfos.Caster, castInfos.Target);
                var damageValue = (int)(((double)castInfos.Caster.Life / 100) * castInfos.RandomJet);

                return DamageEffect.ApplyDamages(damageInfos, castInfos.Caster,  ref damageValue);
            }

            var healInfos = new CastInfos(EffectEnum.DamageBrut, -1, -1, -1, -1, -1, -1, -1, castInfos.Caster, castInfos.Target);
            var healValue = (int)(((double)castInfos.Caster.Life / 100) * castInfos.RandomJet);

            return HealEffect.ApplyHeal(healInfos, castInfos.Target, ref healValue);
        }
    }
}
