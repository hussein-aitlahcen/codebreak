using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Fight.Effect.Type
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SkinChangeBuff : BuffBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <param name="target"></param>
        public SkinChangeBuff(CastInfos castInfos, FighterBase target)
            : base(castInfos, target, ActiveType.ACTIVE_STATS, DecrementType.TYPE_ENDTURN)
        {
            var damageValue = 0;

            ApplyEffect(ref damageValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DamageValue"></param>
        /// <param name="DamageInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(ref int DamageValue, CastInfos DamageInfos = null)
        {
            SkinChangeEffect.ApplySkinChange(CastInfos);

            return base.ApplyEffect(ref DamageValue, DamageInfos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override FightActionResultEnum RemoveEffect()
        {
            Duration = 0;

            SkinChangeEffect.ApplySkinChange(CastInfos);

            return base.RemoveEffect();
        }
    }
}
