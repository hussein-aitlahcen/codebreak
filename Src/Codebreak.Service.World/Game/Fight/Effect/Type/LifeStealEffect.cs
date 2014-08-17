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
    public sealed class LifeStealEffect : EffectBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CastInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(CastInfos CastInfos)
        {
            if (CastInfos.Target == null)
                return FightActionResultEnum.RESULT_NOTHING;

            var damageJet = CastInfos.RandomJet;

            if (DamageEffect.ApplyDamages(CastInfos, CastInfos.Target, ref damageJet) == FightActionResultEnum.RESULT_END)
                return FightActionResultEnum.RESULT_END;

            // On ne se soigne que de 50% des dégats
            var healJet = damageJet / 2;

            if (HealEffect.ApplyHeal(CastInfos, CastInfos.Caster, ref healJet) == FightActionResultEnum.RESULT_END)
                return FightActionResultEnum.RESULT_END;

            return FightActionResultEnum.RESULT_NOTHING;
        }
    }
}
