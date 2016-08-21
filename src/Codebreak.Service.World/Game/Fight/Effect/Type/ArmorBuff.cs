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
    public sealed class ArmorBuff : AbstractSpellBuff
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <param name="target"></param>
        public ArmorBuff(CastInfos castInfos, AbstractFighter target)
            : base(castInfos, target, ActiveType.ACTIVE_ATTACKED_AFTER_JET, DecrementType.TYPE_ENDTURN)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override FightActionResultEnum RemoveEffect()
        {
            // On supprime le boost stats
            switch (CastInfos.SpellId)
            {
                case 1:
                    Target.Statistics.GetEffect(EffectEnum.AddArmorFire).Dons -= CastInfos.Value1;
                    break;

                case 6:
                    Target.Statistics.GetEffect(EffectEnum.AddArmorEarth).Dons -= CastInfos.Value1;
                    break;

                case 14:
                    Target.Statistics.GetEffect(EffectEnum.AddArmorAir).Dons -= CastInfos.Value1;
                    break;

                case 18:
                    Target.Statistics.GetEffect(EffectEnum.AddArmorWater).Dons -= CastInfos.Value1;
                    break;

                default:
                    Target.Statistics.GetEffect(EffectEnum.AddArmor).Dons -= CastInfos.Value1;
                    break;
            }

            return base.RemoveEffect();
        }
    }
}
