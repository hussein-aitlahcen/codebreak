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
    public sealed class MPDodgeSubstractEffect : EffectBase
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

            if (castInfos.Duration > 1)
            {
                var subInfos = new CastInfos(EffectEnum.SubMPDodgeable, castInfos.SpellId, 0, castInfos.Value1, 0, 0, 0, castInfos.Duration, castInfos.Caster, null);
                var buff = new MPDodgeSubstractBuff(subInfos, castInfos.Target);

                castInfos.Target.BuffManager.AddBuff(buff);
            }
            else
            {
                var damageValue = 0;
                var subInfos = new CastInfos(EffectEnum.SubMPDodgeable, castInfos.SpellId, 0, castInfos.Value1, 0, 0, 0, 0, castInfos.Caster, null);
                var buff = new MPDodgeSubstractBuff(subInfos, castInfos.Target);
                
                buff.ApplyEffect(ref damageValue);
                castInfos.Target.BuffManager.AddBuff(buff);
            }

            return FightActionResultEnum.RESULT_NOTHING;
        }
    }
}
