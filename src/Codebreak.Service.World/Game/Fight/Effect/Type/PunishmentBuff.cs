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
    public sealed class PunishmentBuff : BuffBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CastInfos"></param>
        /// <param name="Target"></param>
        public PunishmentBuff(CastInfos CastInfos, FighterBase Target)
            : base(CastInfos, Target, ActiveType.ACTIVE_ATTACKED_AFTER_JET, DecrementType.TYPE_ENDTURN)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="damageValue"></param>
        /// <param name="damageInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(ref int damageValue, CastInfos damageInfos = null)
        {
            var buffValue = damageValue / 2; // Divise par deux les stats a boost car c'est un personnage.
            var statsType = (EffectEnum)CastInfos.Value1;
            var maxValue = CastInfos.Value2;
            var duration = CastInfos.Value3;

            if (Target.Fight.CurrentFighter.Id == CastInfos.FakeValue)
            {
                if (CastInfos.DamageValue < maxValue)
                {
                    if (CastInfos.DamageValue + buffValue > maxValue)
                    {
                        buffValue = maxValue - CastInfos.DamageValue;
                    }
                }
                else
                {
                    buffValue = 0;
                }
            }
            else
            {
                CastInfos.DamageValue = 0;
                CastInfos.FakeValue = (int)Target.Fight.CurrentFighter.Id;

                if (CastInfos.DamageValue + buffValue > maxValue)
                {
                    buffValue = maxValue;
                }
            }

            if (buffValue != 0)
            {
                CastInfos.DamageValue += buffValue;

                switch(statsType)
                {
                    case EffectEnum.Heal:
                        HealEffect.ApplyHeal(new CastInfos(statsType, CastInfos.SpellId, CastInfos.SpellId, buffValue, 0, 0, 0, duration, CastInfos.Caster, null), Target, ref buffValue);
                        break;
                    
                    default:
                        var BuffStats = new StatsBuff(new CastInfos(statsType, CastInfos.SpellId, CastInfos.SpellId, buffValue, 0, 0, 0, duration, CastInfos.Caster, null), Target);
                        BuffStats.ApplyEffect(ref buffValue);
                        Target.BuffManager.AddBuff(BuffStats);
                        break;
                }
            }

            return base.ApplyEffect(ref damageValue, damageInfos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override FightActionResultEnum RemoveEffect()
        {
            return base.RemoveEffect();
        }
    }
}
