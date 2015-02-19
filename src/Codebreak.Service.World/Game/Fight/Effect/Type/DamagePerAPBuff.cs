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
    public sealed class DamagePerAPBuff : BuffBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="infos"></param>
        public DamagePerAPBuff(CastInfos infos, FighterBase target)
            : base(infos, target, ActiveType.ACTIVE_ENDTURN, DecrementType.TYPE_ENDTURN)
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
            var usedAp = Target.UsedAP;
            if (usedAp < 1)
                return FightActionResultEnum.RESULT_NOTHING;

            damageInfos = new CastInfos(EffectEnum.DamageNeutral, -1, -1, -1, -1, -1, -1, -1, CastInfos.Caster, CastInfos.Target);
            var damageJet = (usedAp / CastInfos.Value1) * CastInfos.Value2;

            return DamageEffect.ApplyDamages(damageInfos, CastInfos.Target, ref damageJet);
        }
    }
}
