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
    public sealed class ArmorBuff : BuffBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <param name="target"></param>
        public ArmorBuff(CastInfos castInfos, FighterBase target)
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
            switch (this.CastInfos.SpellId)
            {
                case 1:
                    Target.Statistics.GetEffect(EffectEnum.AddArmorFire).Dons -= this.CastInfos.Value1;
                    break;

                case 6:
                    Target.Statistics.GetEffect(EffectEnum.AddArmorNeutral).Dons -= this.CastInfos.Value1;
                    Target.Statistics.GetEffect(EffectEnum.AddArmorEarth).Dons -= this.CastInfos.Value1;
                    break;

                case 14:
                    Target.Statistics.GetEffect(EffectEnum.AddArmorAir).Dons -= this.CastInfos.Value1;
                    break;

                case 18:
                    Target.Statistics.GetEffect(EffectEnum.AddArmorWater).Dons -= this.CastInfos.Value1;
                    break;

                default:
                    Target.Statistics.GetEffect(EffectEnum.AddArmor).Dons -= this.CastInfos.Value1;
                    break;
            }

            return base.RemoveEffect();
        }
    }
}
