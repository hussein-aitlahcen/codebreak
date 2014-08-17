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
    public sealed class PandaCarrierEffect : EffectBase
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

            var PorteurInfos = new CastInfos(castInfos.EffectType, castInfos.SpellId, 0, 0, 0, (int)FighterStateEnum.STATE_CARRIER, 0, 0, castInfos.Caster, null);
            var PorterInfos = new CastInfos(castInfos.EffectType, castInfos.SpellId, 0, 0, 0, (int)FighterStateEnum.STATE_CARRIED, 0, 0, castInfos.Caster, null);

            castInfos.Caster.BuffManager.AddBuff(new PandaCarrierBuff(PorteurInfos, castInfos.Target));
            castInfos.Target.BuffManager.AddBuff(new PandaCarriedBuff(PorterInfos, castInfos.Target));

            return FightActionResultEnum.RESULT_NOTHING;
        }
    }
}
