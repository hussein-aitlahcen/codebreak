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
    public sealed class ReflectSpellBuff : BuffBase
    {
        public ReflectSpellBuff(CastInfos castInfos, FighterBase target)
            : base(castInfos, target, ActiveType.ACTIVE_ATTACKED_AFTER_JET, DecrementType.TYPE_ENDTURN)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="damageValue"></param>
        /// <param name="damageInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(ref int damageValue, CastInfos damageInfos = null)
        {
            // cant reflect a spell himself
            if (damageInfos.Caster == Target)
                return FightActionResultEnum.RESULT_NOTHING;

            // reflect only spells
            if (damageInfos.SpellId < 1)
                return FightActionResultEnum.RESULT_NOTHING;

            // cannot reflect a spell that is higher in level
            if (damageInfos.SpellLevel > CastInfos.Value2)
                return FightActionResultEnum.RESULT_NOTHING;

            // cant reflect reflected damage or poison or a trap damage
            if (damageInfos.IsReflect || damageInfos.IsPoison || damageInfos.IsTrap)
                return FightActionResultEnum.RESULT_NOTHING;

            // cancel damage
            damageValue = 0;

            // damaged are being reflected
            damageInfos.IsReflect = true;

            // new jet for new target
            var damageJet = damageInfos.RandomJet;

            // apply damage back to the caster
            return DamageEffect.ApplyDamages(damageInfos, damageInfos.Caster, ref damageJet);
        }
    }
}
