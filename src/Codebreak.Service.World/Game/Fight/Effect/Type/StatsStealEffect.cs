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
    public sealed class StatsStealEffect : AbstractSpellEffect
    {
        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<EffectEnum, EffectEnum> _targetMalus = new Dictionary<EffectEnum, EffectEnum>()
        {
            { EffectEnum.StrengthSteal          , EffectEnum.SubStrength         },
            { EffectEnum.StealFire              , EffectEnum.SubIntelligence  },
            { EffectEnum.AgilitySteal           , EffectEnum.SubAgility       },
            { EffectEnum.WisdomSteal            , EffectEnum.SubWisdom       },
            { EffectEnum.ChanceSteal            , EffectEnum.SubChance        },
            { EffectEnum.APSteal                , EffectEnum.SubAP            },
            { EffectEnum.MPSteal                , EffectEnum.SubMP            },
            { EffectEnum.POSteal                , EffectEnum.SubPO            },
        };

        private static Dictionary<EffectEnum, EffectEnum> _casterBonus = new Dictionary<EffectEnum, EffectEnum>()
        {
            { EffectEnum.StrengthSteal          , EffectEnum.AddStrength         },
            { EffectEnum.StealFire              , EffectEnum.AddIntelligence  },
            { EffectEnum.AgilitySteal           , EffectEnum.AddAgility       },
            { EffectEnum.WisdomSteal            , EffectEnum.AddWisdom       },
            { EffectEnum.ChanceSteal            , EffectEnum.AddChance        },
            { EffectEnum.APSteal                , EffectEnum.AddAP            },
            { EffectEnum.MPSteal                , EffectEnum.AddMP            },
            { EffectEnum.POSteal                , EffectEnum.AddPO            },
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CastInfos"></param>
        /// <returns></returns>
        public override FightActionResultEnum ApplyEffect(CastInfos CastInfos)
        {
            if (CastInfos.Target == null)
                return FightActionResultEnum.RESULT_NOTHING;

            var malusType = _targetMalus[CastInfos.EffectType];
            var bonusType = _casterBonus[CastInfos.EffectType];

            var malusInfos = new CastInfos(malusType, CastInfos.SpellId, CastInfos.CellId, CastInfos.Value1, CastInfos.Value2, CastInfos.Value3, CastInfos.Chance, CastInfos.Duration, CastInfos.Caster, CastInfos.Target);
            var bonusInfos = new CastInfos(bonusType, CastInfos.SpellId, CastInfos.CellId, CastInfos.Value1, CastInfos.Value2, CastInfos.Value3, CastInfos.Chance, CastInfos.Duration - 1, CastInfos.Caster, CastInfos.Target);
            var damageValue = 0;

            if (CastInfos.Target == CastInfos.Caster)
                return FightActionResultEnum.RESULT_NOTHING;

            // Malus a la cible
            var BuffStats = new StatsBuff(malusInfos, CastInfos.Target);
            if (BuffStats.ApplyEffect(ref damageValue) == FightActionResultEnum.RESULT_END)
                return FightActionResultEnum.RESULT_END;

            CastInfos.Target.BuffManager.AddBuff(BuffStats);

            // Bonus au lanceur
            BuffStats = new StatsBuff(bonusInfos, CastInfos.Caster);
            CastInfos.Caster.BuffManager.AddBuff(BuffStats);

            return BuffStats.ApplyEffect(ref damageValue);
        }
    }
}
