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
    public sealed class HealBuff : AbstractSpellBuff
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <param name="target"></param>
        public HealBuff(CastInfos castInfos, AbstractFighter target)
            : base(castInfos, target, ActiveType.ACTIVE_BEGINTURN, DecrementType.TYPE_ENDTURN)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="healValue"></param>
        /// <param name="healInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(ref int healValue, CastInfos healInfos = null)
        {
            var heal = CastInfos.RandomJet;

            return HealEffect.ApplyHeal(CastInfos, Target, ref heal);
        }
    }
}
