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
    public sealed class EcaflipChanceBuff : BuffBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <param name="target"></param>
        public EcaflipChanceBuff(CastInfos castInfos, FighterBase target)
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
            var damageCoef = CastInfos.Value1;
            var healCoef = CastInfos.Value2;
            var chance = CastInfos.Value3;
            var chanceJet = Util.Next(0, 100);

            if (chanceJet < chance)
            {
                var HealValue = damageValue * healCoef;

                if (HealEffect.ApplyHeal(CastInfos, Target, ref HealValue) == FightActionResultEnum.RESULT_END)
                    return FightActionResultEnum.RESULT_END;

                damageValue = 0;
            }
            else // Multiplication des dommages
            {
                damageValue *= damageCoef;
            }

            return base.ApplyEffect(ref damageValue, damageInfos);
        }
    }
}
