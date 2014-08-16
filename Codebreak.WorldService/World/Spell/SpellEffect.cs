using Codebreak.WorldService.World.Manager;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Spell
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum EffectEnum
    {
        WEAPON_EFFECT = StealEarth | StealFire | StealWater | StealAir | StealNeutral | DamageEarth | DamageNeutral
            | DamageFire | DamageWater | DamageAir | SubAPDodgeable,

        DAMAGE_EFFECT = StealEarth | StealFire | StealWater | StealAir | StealNeutral | DamageEarth | DamageNeutral
            | DamageFire | DamageWater | DamageAir,

        None = -1,

        // Armures
        AddArmorNeutral,
        AddArmorEarth,
        AddArmorFire,
        AddArmorWater,
        AddArmorAir,

        Teleport = 4,
        PushBack = 5,
        PushFront = 6,
        Transpose = 8,
        Evasion = 9,
        DamageBrut,

        TurnPass = 140,

        MPSteal = 77,
        MPBonus = 78,
        ChanceEcaflip = 79,
        LifeSteal = 82,
        APSteal = 84,
        ChanceSteal = 266,
        VitalitySteal = 267,
        AgilitySteal = 268,
        IntelligenceSteal = 269,
        WisdomSteal = 270,
        StrengthSteal = 271,

        DamageLifeWater = 85,
        DamageLifeEarth = 86,
        DamageLifeAir = 87,
        DamageLifeFire = 88,
        DamageLifeNeutral = 89,
        DamageDropLife = 90,
        StealWater = 91,
        StealEarth = 92,
        StealAir = 93,
        StealFire = 94,
        StealNeutral = 95,
        DamageWater = 96,
        DamageEarth = 97,
        DamageAir = 98,
        DamageFire = 99,
        DamageNeutral = 100,
        AddArmor = 105,
        AddArmorBis = 265,

        AddReflectDamageItem = 220,
        ReflectSpell = 106,
        AddReflectDamage = 107,
        Heal = 108,
        SelfDamage = 109,
        AddLife = 110,
        AddAP = 111,
        AddDamage = 112,
        MultiplyDamage = 114,

        AddAPBis = 120,
        AddAgility = 119,
        AddChance = 123,
        AddDamagePercent = 138,
        SubDamagePercent = 186,
        AddDamageCritic = 115,
        AddDamagePiege = 225,
        //AddDamagePiegePercent = 225,
        AddDamagePhysic = 142,
        AddDamageMagic = 143,
        AddEchecCritic = 122,
        AddAPDodge = 160,
        AddMPDodge = 161,
        AddStrength = 118,
        AddInitiative = 174,
        AddIntelligence = 126,
        AddInvocationMax = 182,
        AddMP = 128,
        AddPO = 117,
        AddPods = 158,
        AddProspection = 176,
        AddWisdom = 124,
        AddHealCare = 178,
        AddVitality = 125,
        SubAgility = 154,

        DamagePerAP = 131,
        IncreaseSpellDamage = 293,
        Mastery = 165,
        StealPO = 320,
        Punition = 672,
        Sacrifice = 765,

        SubChance = 152,
        SubDamage = 164,
        SubDamageCritic = 171,
        SubDamageMagic = 172,
        SubDamagePhysic = 173,
        SubAPDodge = 162,
        SubMPDodge = 163,
        SubStrength = 157,
        SubInitiative = 175,
        SubIntelligence = 155,
        SubAPDodgeable = 101,
        SubMPDodgeable = 127,
        SubAP = 168,
        SubMP = 169,
        SubPO = 116,
        SubPods = 159,
        SubProspection = 177,
        SubWisdom = 156,
        SubHealCare = 179,
        SubVitality = 153,

        InvocDouble = 180,
        Invocation = 181,

        AddReduceDamagePhysic = 183,
        AddReduceDamageMagic = 184,

        AddReduceDamagePercentWater = 211,
        AddReduceDamagePercentEarth = 210,
        AddReduceDamagePercentAir = 212,
        AddReduceDamagePercentFire = 213,
        AddReduceDamagePercentNeutral = 214,
        AddReduceDamagePercentPvPWater = 251,
        AddReduceDamagePercentPvPEarth = 250,
        AddReduceDamagePercentPvPAir = 252,
        AddReduceDamagePercentPvPFire = 253,
        AddReduceDamagePercentPvPNeutral = 254,

        AddReduceDamageWater = 241,
        AddReduceDamageEarth = 240,
        AddReduceDamageAir = 242,
        AddReduceDamageFire = 243,
        AddReduceDamageNeutral = 244,
        AddReduceDamagePvPWater = 261,
        AddReduceDamagePvPEarth = 260,
        AddReduceDamagePvPAir = 262,
        AddReduceDamagePvPFire = 263,
        AddReduceDamagePvPNeutral = 264,

        SubReduceDamagePercentWater = 216,
        SubReduceDamagePercentEarth = 215,
        SubReduceDamagePercentAir = 217,
        SubReduceDamagePercentFire = 218,
        SubReduceDamagePercentNeutral = 219,
        SubReduceDamagePercentPvPWater = 255,
        SubReduceDamagePercentPvPEarth = 256,
        SubReduceDamagePercentPvPAir = 257,
        SubReduceDamagePercentPvPFire = 258,
        SubReduceDamagePercentPvpNeutral = 259,
        SubReduceDamageWater = 246,
        SubReduceDamageEarth = 245,
        SubReduceDamageAir = 247,
        SubReduceDamageFire = 248,
        SubReduceDamageNeutral = 249,

        PandaCarry = 50,
        PandaLaunch = 51,
        Perception = 202,
        ChangeSkin = 149,
        SpellBoost = 293,
        UseTrap = 400,
        UseGlyph = 401,
        DoNothing = 666,
        PushFear = 783,
        AddChatiment = 788,
        AddState = 950,
        RemoveState = 951,
        Stealth = 150,
        DeleteAllBonus = 132,

        /* Parchemins */
        AddSpell = 604,
        AddCaractStrength = 607,
        AddCaractWisdom = 678,
        AddCaractChance = 608,
        AddCaractAgility = 609,
        AddCaractVitality = 610,
        AddCaractIntelligence = 611,
        AddCaractPoint = 612,
        AddSpellPoint = 613,

        InvocationInformations = 628,

        SoulStoneStats = 705,
        MountCaptureProba = 706,

        SoulCaptureBonus = 750,
        MountExpBonus = 751,

        LastEat = 808,

        AlignmentId = 960,
        AlignmentGrade = 961,
        TargetLevel = 962,
        CreateTime = 963,
        TargetName = 964,

        MountOwner = 995,

        LivingGfxId = 970,
        LivingMood = 971,
        LivingSkin = 972,
        LivingType = 973,
        LivingXp = 974,

        CanBeExchange = 983,
    }

    /// <summary>
    /// 
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public sealed class SpellEffect
    {
        public int SpellId;
        public int SpellLevel;
        public int Type;
        public int Value1;
        public int Value2;
        public int Value3;
        public int Duration;
        public int Chance;
        private SpellLevel _level;

        /// <summary>
        /// 
        /// </summary>
        [ProtoIgnore]
        public EffectEnum TypeEnum
        {
            get
            {
                return (EffectEnum)Type;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ProtoIgnore]
        public SpellLevel Level
        {
            get
            {
                if(_level == null)
                    _level = SpellManager.Instance.GetSpellLevel(SpellId, SpellLevel);
                return _level;
            }
        }
    }
}
