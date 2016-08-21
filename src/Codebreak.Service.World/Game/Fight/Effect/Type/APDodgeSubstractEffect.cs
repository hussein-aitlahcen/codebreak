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
    public sealed class APDodgeSubstractEffect : AbstractSpellEffect
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

            var damageValue = 0;

            if (CastInfos.Duration > 1)
            {
                var subInfos = new CastInfos(EffectEnum.SubAPDodgeable, CastInfos.SpellId, 0, CastInfos.Value1, 0, 0, 0, CastInfos.Duration, CastInfos.Caster, null);
                var buff = new APDodgeSubstractBuff(subInfos, CastInfos.Target);

                buff.ApplyEffect(ref damageValue);
                CastInfos.Target.BuffManager.AddBuff(buff);
            }
            else
            {
                var subInfos = new CastInfos(EffectEnum.SubAPDodgeable, CastInfos.SpellId, 0, CastInfos.Value1, 0, 0, 0, 0, CastInfos.Caster, null);
                var buff = new APDodgeSubstractBuff(subInfos, CastInfos.Target);
                
                buff.ApplyEffect(ref damageValue);
                CastInfos.Target.BuffManager.AddBuff(buff);
            }

            return FightActionResultEnum.RESULT_NOTHING;
        }
    }
}
