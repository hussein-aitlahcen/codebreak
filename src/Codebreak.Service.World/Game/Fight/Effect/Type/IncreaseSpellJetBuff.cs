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
    public sealed class IncreaseSpellJetBuff : AbstractSpellBuff
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <param name="target"></param>
        public IncreaseSpellJetBuff(CastInfos castInfos, AbstractFighter target)
            : base(castInfos, target, ActiveType.ACTIVE_ATTACK_BEFORE_JET, DecrementType.TYPE_ENDTURN)
        {
            Duration += 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="damageValue"></param>
        /// <param name="damageInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(ref int damageValue, CastInfos damageInfos = null)
        {
            if (damageInfos.SpellId == CastInfos.SpellId)
                damageValue += CastInfos.Value3;

            return base.ApplyEffect(ref damageValue, damageInfos);
        }
    }
}
