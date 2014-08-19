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

            var carrierInfos = new CastInfos(castInfos.EffectType, castInfos.SpellId, 0, 0, 0, (int)FighterStateEnum.STATE_CARRIER, 0, int.MaxValue - 1, castInfos.Caster, null);
            var carriedInfos = new CastInfos(castInfos.EffectType, castInfos.SpellId, 0, 0, 0, (int)FighterStateEnum.STATE_CARRIED, 0, int.MaxValue - 1, castInfos.Caster, null);
                        
            castInfos.Caster.BuffManager.AddBuff(new PandaCarrierBuff(carrierInfos, castInfos.Target));
            castInfos.Target.BuffManager.AddBuff(new PandaCarriedBuff(carriedInfos, castInfos.Target));

            return FightActionResultEnum.RESULT_NOTHING;
        }
    }
}
