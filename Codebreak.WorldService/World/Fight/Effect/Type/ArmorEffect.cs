using Codebreak.WorldService.World.Spell;
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
    public sealed class ArmorEffect : EffectBase
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
                    if (castInfos.Target.Team != castInfos.Caster.Team)
                        return FightActionResultEnum.RESULT_NOTHING;
                    castInfos.Target.Statistics.AddBoost(EffectEnum.AddArmorFire, castInfos.Value1);
                    break;

                case 6:
                    if (castInfos.Target.Team != castInfos.Caster.Team)
                        return FightActionResultEnum.RESULT_NOTHING;
                    castInfos.Target.Statistics.AddBoost(EffectEnum.AddArmorEarth, castInfos.Value1);
                    break;

                case 14:
                    if (castInfos.Target.Team != castInfos.Caster.Team)
                        return FightActionResultEnum.RESULT_NOTHING;
                    castInfos.Target.Statistics.AddBoost(EffectEnum.AddArmorAir, castInfos.Value1);
                    break;

                case 18:
                    if (castInfos.Target.Team != castInfos.Caster.Team)
                        return FightActionResultEnum.RESULT_NOTHING;
                    castInfos.Target.Statistics.AddBoost(EffectEnum.AddArmorWater, castInfos.Value1);
                    break;

                default:
                    castInfos.Target.Statistics.AddBoost(EffectEnum.AddArmor, castInfos.Value1);
                    break;
            }

            // Ajout du buff
            castInfos.Target.BuffManager.AddBuff(new ArmorBuff(castInfos, castInfos.Target));

            return FightActionResultEnum.RESULT_NOTHING;
        }
    }    
}
