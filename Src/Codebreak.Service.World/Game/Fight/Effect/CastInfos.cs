using Codebreak.Service.World.Game.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.Game.Map;

namespace Codebreak.Service.World.Game.Fight.Effect
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CastInfos
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="effectType"></param>
        /// <returns></returns>
        public static bool IsMalusEffect(EffectEnum effectType)
        {
            return !IsDamageEffect(effectType) && !IsBonusEffect(effectType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effectType"></param>
        /// <returns></returns>
        public static bool IsDamageEffect(EffectEnum effectType)
        {
            switch (effectType)
            {
                case EffectEnum.StealEarth:
                case EffectEnum.StealFire:
                case EffectEnum.StealWater:
                case EffectEnum.StealAir:
                case EffectEnum.StealNeutral:
                case EffectEnum.DamageEarth:
                case EffectEnum.DamageNeutral:
                case EffectEnum.DamageFire:
                case EffectEnum.DamageWater:
                case EffectEnum.DamageAir:
                case EffectEnum.DamageBrut:
                case EffectEnum.DamageLifeAir:
                case EffectEnum.DamageLifeEarth:
                case EffectEnum.DamageLifeFire:
                case EffectEnum.DamageLifeNeutral:
                case EffectEnum.DamageLifeWater:
                case EffectEnum.DamagePerAP:
                case EffectEnum.LifeSteal:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effectType"></param>
        /// <returns></returns>
        public static bool IsBonusEffect(EffectEnum effectType)
        {
            switch (effectType)
            {
                case EffectEnum.Heal:
                case EffectEnum.AddAgility:
                case EffectEnum.AddAP:
                case EffectEnum.AddAPBis:
                case EffectEnum.AddAPDodge:
                case EffectEnum.AddArmor:
                case EffectEnum.AddArmorAir:
                case EffectEnum.AddArmorBis:
                case EffectEnum.AddArmorEarth:
                case EffectEnum.AddArmorFire:
                case EffectEnum.AddArmorNeutral:
                case EffectEnum.AddArmorWater:
                case EffectEnum.AddCaractAgility:
                case EffectEnum.AddCaractIntelligence:
                case EffectEnum.AddCaractPoint:
                case EffectEnum.AddCaractStrength:
                case EffectEnum.AddCaractVitality:
                case EffectEnum.AddCaractWisdom:
                case EffectEnum.AddChance:
                case EffectEnum.AddChatiment:
                case EffectEnum.AddDamage:
                case EffectEnum.AddDamageCritic:
                case EffectEnum.AddDamageMagic:
                case EffectEnum.AddDamagePercent:
                case EffectEnum.AddDamagePhysic:
                case EffectEnum.AddDamagePiege:
                case EffectEnum.AddHealCare:
                case EffectEnum.AddInitiative:
                case EffectEnum.AddIntelligence:
                case EffectEnum.AddInvocationMax:
                case EffectEnum.AddLife:
                case EffectEnum.AddMP:
                case EffectEnum.AddMPDodge:
                case EffectEnum.AddPO:
                case EffectEnum.AddPods:
                case EffectEnum.AddProspection:
                case EffectEnum.AddReduceDamageAir:
                case EffectEnum.AddReduceDamageEarth:
                case EffectEnum.AddReduceDamageFire:
                case EffectEnum.AddReduceDamageMagic:
                case EffectEnum.AddReduceDamageNeutral:
                case EffectEnum.AddReduceDamagePercentAir:
                case EffectEnum.AddReduceDamagePercentEarth:
                case EffectEnum.AddReduceDamagePercentFire:
                case EffectEnum.AddReduceDamagePercentNeutral:
                case EffectEnum.AddReduceDamagePercentWater:
                case EffectEnum.AddReduceDamagePhysic:
                case EffectEnum.AddReduceDamageWater:
                case EffectEnum.AddReflectDamage:
                case EffectEnum.AddState:
                case EffectEnum.AddStrength:
                case EffectEnum.AddVitality:
                case EffectEnum.AddWisdom:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public EffectEnum EffectType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public EffectEnum SubEffect
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int SpellId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int CellId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReflect
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPoison
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReturnedDamages
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMelee
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsTrap
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int RandomJet
        {
            get
            {
                return (Value2 < Value1 ? Value1 : Util.Next(Value1, Value2 + 1));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Value1
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Value2
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Value3
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int FakeValue
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int DamageValue
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Chance
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Duration
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int SpellLevel
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string RangeType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FighterBase Caster
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FighterBase Target
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public MapInstance Map
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FightBase Fight
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int TargetKnownCellId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effectType"></param>
        /// <param name="spellId"></param>
        /// <param name="cellId"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <param name="chance"></param>
        /// <param name="duration"></param>
        /// <param name="caster"></param>
        /// <param name="target"></param>
        /// <param name="rangeType"></param>
        /// <param name="targetKnownCellId"></param>
        /// <param name="spellLevel"></param>
        /// <param name="isMelee"></param>
        /// <param name="isTrap"></param>
        /// <param name="subEffect"></param>
        /// <param name="damageValue"></param>
        /// <param name="fakeValue"></param>
        public CastInfos(EffectEnum effectType,
            int spellId,
            int cellId,
            int value1,
            int value2,
            int value3,
            int chance,
            int duration,
            FighterBase caster,
            FighterBase target,
            string rangeType = "",
            int targetKnownCellId = 0,
            int spellLevel = -1,
            bool isMelee = false,
            bool isTrap = false,
            EffectEnum subEffect = EffectEnum.None,
            int damageValue = 0, int fakeValue = 0)
        {
            Fight = caster.Fight;
            Map = caster.Fight.Map;
            SpellLevel = spellLevel;
            TargetKnownCellId = targetKnownCellId;
            FakeValue = fakeValue;
            RangeType = rangeType;
            EffectType = effectType;
            SpellId = spellId;
            CellId = cellId;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            Chance = chance;
            IsTrap = isTrap;
            Duration = duration;
            Caster = caster;
            Target = target;
            if (subEffect == EffectEnum.None)
            {
                SubEffect = effectType;
            }
            else
            {
                SubEffect = subEffect;
            }
            DamageValue = damageValue;
            IsMelee = isMelee;
        }
    }    
}
