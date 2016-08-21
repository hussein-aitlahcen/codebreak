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
    public sealed class ArmorEffect : AbstractSpellEffect
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

            // Les buffs ne fonctionnent que sur les équipiés je crois
            switch (castInfos.SpellId)
            {
                case 1:
                    castInfos.Target.Statistics.AddDon(EffectEnum.AddArmorFire, castInfos.Value1);
                    break;

                case 6:
                    castInfos.Target.Statistics.AddDon(EffectEnum.AddArmorEarth, castInfos.Value1);
                    break;

                case 14:
                    castInfos.Target.Statistics.AddDon(EffectEnum.AddArmorAir, castInfos.Value1);
                    break;

                case 18:
                    castInfos.Target.Statistics.AddDon(EffectEnum.AddArmorWater, castInfos.Value1);
                    break;

                default:
                    castInfos.Target.Statistics.AddDon(EffectEnum.AddArmor, castInfos.Value1);
                    break;
            }

            // Ajout du buff
            castInfos.Target.BuffManager.AddBuff(new ArmorBuff(castInfos, castInfos.Target));

            return FightActionResultEnum.RESULT_NOTHING;
        }
    }    
}
