using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Spell;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Stats
{
    /// <summary>
    /// 
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public sealed class GenericStats : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<EffectEnum, List<EffectEnum>> OppositeStats = new Dictionary<EffectEnum, List<EffectEnum>>()
        {
            {EffectEnum.AddInitiative, new List<EffectEnum>() { EffectEnum.SubInitiative }},
            {EffectEnum.AddAP, new List<EffectEnum>() { EffectEnum.SubAP,  EffectEnum.SubAPDodgeable }},
            {EffectEnum.AddMP, new List<EffectEnum>() { EffectEnum.SubMP, EffectEnum.SubMPDodgeable }},
            {EffectEnum.AddPO, new List<EffectEnum>() { EffectEnum.SubPO }},
            {EffectEnum.AddHealCare, new List<EffectEnum>() { EffectEnum.SubHealCare }},
            {EffectEnum.AddProspection, new List<EffectEnum>() { EffectEnum.SubProspection }},
            {EffectEnum.AddPods, new List<EffectEnum>() { EffectEnum.SubPods }},
            {EffectEnum.AddVitality, new List<EffectEnum>() { EffectEnum.SubVitality }},
            {EffectEnum.AddWisdom, new List<EffectEnum>() { EffectEnum.SubWisdom }},
            {EffectEnum.AddStrength, new List<EffectEnum>() { EffectEnum.SubStrength }},
            {EffectEnum.AddIntelligence, new List<EffectEnum>() { EffectEnum.SubIntelligence }},
            {EffectEnum.AddAgility, new List<EffectEnum>() { EffectEnum.SubAgility }},
            {EffectEnum.AddChance, new List<EffectEnum>() { EffectEnum.SubChance }},

            {EffectEnum.AddDamage, new List<EffectEnum>() { EffectEnum.SubDamage }},
            {EffectEnum.AddDamagePercent, new List<EffectEnum>() { EffectEnum.SubDamagePercent }},
            {EffectEnum.AddDamageCritic, new List<EffectEnum>() { EffectEnum.SubDamageCritic }},
            {EffectEnum.AddDamageMagic, new List<EffectEnum>() { EffectEnum.SubDamageMagic }},
            {EffectEnum.AddDamagePhysic, new List<EffectEnum>() { EffectEnum.SubDamagePhysic }},
            {EffectEnum.AddAPDodge, new List<EffectEnum>() { EffectEnum.SubAPDodgeable }},
            {EffectEnum.AddMPDodge, new List<EffectEnum>() { EffectEnum.SubMPDodgeable }},

            {EffectEnum.AddReduceDamageAir, new List<EffectEnum>() { EffectEnum.SubReduceDamageAir }},
            {EffectEnum.AddReduceDamageWater, new List<EffectEnum>() { EffectEnum.SubReduceDamageWater }},
            {EffectEnum.AddReduceDamageFire, new List<EffectEnum>() { EffectEnum.SubReduceDamageFire }},
            {EffectEnum.AddReduceDamageNeutral, new List<EffectEnum>() { EffectEnum.SubReduceDamageNeutral }},
            {EffectEnum.AddReduceDamageEarth, new List<EffectEnum>() { EffectEnum.SubReduceDamageEarth }},

            {EffectEnum.AddReduceDamagePercentAir, new List<EffectEnum>() { EffectEnum.SubReduceDamagePercentAir }},
            {EffectEnum.AddReduceDamagePercentWater, new List<EffectEnum>() { EffectEnum.SubReduceDamagePercentWater }},
            {EffectEnum.AddReduceDamagePercentFire, new List<EffectEnum>() { EffectEnum.SubReduceDamagePercentFire }},
            {EffectEnum.AddReduceDamagePercentNeutral, new List<EffectEnum>() { EffectEnum.SubReduceDamagePercentNeutral }},
            {EffectEnum.AddReduceDamagePercentEarth, new List<EffectEnum>() { EffectEnum.SubReduceDamagePercentEarth }},

            {EffectEnum.AddReduceDamagePercentPvPAir, new List<EffectEnum>() { EffectEnum.SubReduceDamagePercentPvPAir }},
            {EffectEnum.AddReduceDamagePercentPvPWater, new List<EffectEnum>() { EffectEnum.SubReduceDamagePercentPvPWater }},
            {EffectEnum.AddReduceDamagePercentPvPFire, new List<EffectEnum>() { EffectEnum.SubReduceDamagePercentPvPFire }},
            {EffectEnum.AddReduceDamagePercentPvPNeutral, new List<EffectEnum>() { EffectEnum.SubReduceDamagePercentPvpNeutral }},
            {EffectEnum.AddReduceDamagePercentPvPEarth, new List<EffectEnum>() { EffectEnum.SubReduceDamagePercentPvPEarth }},
        };


        /// <summary>
        /// 
        /// </summary>
        /// <param name="breed"></param>
        /// <param name="statId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetRequiredStatsPoint(CharacterBreedEnum breed, int statId, int value)
        {
            switch (statId)
            {
                case 11://Vita
                    return 1;
                case 12://Sage
                    return 3;
                case 10://Strength
                    switch (breed)
                    {
                        case CharacterBreedEnum.BREED_SACRIEUR:
                            return 3;

                        case CharacterBreedEnum.BREED_FECA:
                            if (value < 50)
                                return 2;
                            if (value < 150)
                                return 3;
                            if (value < 250)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_XELOR:
                            if (value < 50)
                                return 2;
                            if (value < 150)
                                return 3;
                            if (value < 250)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_SRAM:
                            if (value < 100)
                                return 1;
                            if (value < 200)
                                return 2;
                            if (value < 300)
                                return 3;
                            if (value < 400)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_OSAMODAS:
                            if (value < 50)
                                return 2;
                            if (value < 150)
                                return 3;
                            if (value < 250)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_ENIRIPSA:
                            if (value < 50)
                                return 2;
                            if (value < 150)
                                return 3;
                            if (value < 250)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_PANDAWA:
                            if (value < 50)
                                return 1;
                            if (value < 200)
                                return 2;
                            return 3;

                        case CharacterBreedEnum.BREED_SADIDA:
                            if (value < 50)
                                return 1;
                            if (value < 250)
                                return 2;
                            if (value < 300)
                                return 3;
                            if (value < 400)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_CRA:
                            if (value < 50)
                                return 1;
                            if (value < 150)
                                return 2;
                            if (value < 250)
                                return 3;
                            if (value < 350)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_ENUTROF:
                            if (value < 50)
                                return 1;
                            if (value < 150)
                                return 2;
                            if (value < 250)
                                return 3;
                            if (value < 350)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_ECAFLIP:
                            if (value < 100)
                                return 1;
                            if (value < 200)
                                return 2;
                            if (value < 300)
                                return 3;
                            if (value < 400)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_IOP:
                            if (value < 100)
                                return 1;
                            if (value < 200)
                                return 2;
                            if (value < 300)
                                return 3;
                            if (value < 400)
                                return 4;
                            return 5;

                    }
                    break;
                case 13://Chance
                    switch (breed)
                    {
                        case CharacterBreedEnum.BREED_FECA:
                            if (value < 20)
                                return 1;
                            if (value < 40)
                                return 2;
                            if (value < 60)
                                return 3;
                            if (value < 80)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_XELOR:
                            if (value < 20)
                                return 1;
                            if (value < 40)
                                return 2;
                            if (value < 60)
                                return 3;
                            if (value < 80)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_SACRIEUR:
                            return 3;

                        case CharacterBreedEnum.BREED_SRAM:
                            if (value < 20)
                                return 1;
                            if (value < 40)
                                return 2;
                            if (value < 60)
                                return 3;
                            if (value < 80)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_SADIDA:
                            if (value < 100)
                                return 1;
                            if (value < 200)
                                return 2;
                            if (value < 300)
                                return 3;
                            if (value < 400)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_PANDAWA:
                            if (value < 50)
                                return 1;
                            if (value < 200)
                                return 2;
                            return 3;

                        case CharacterBreedEnum.BREED_IOP:
                            if (value < 20)
                                return 1;
                            if (value < 40)
                                return 2;
                            if (value < 60)
                                return 3;
                            if (value < 80)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_ENUTROF:
                            if (value < 100)
                                return 1;
                            if (value < 150)
                                return 2;
                            if (value < 230)
                                return 3;
                            if (value < 330)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_OSAMODAS:
                            if (value < 100)
                                return 1;
                            if (value < 200)
                                return 2;
                            if (value < 300)
                                return 3;
                            if (value < 400)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_ECAFLIP:
                            if (value < 20)
                                return 1;
                            if (value < 40)
                                return 2;
                            if (value < 60)
                                return 3;
                            if (value < 80)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_ENIRIPSA:
                            if (value < 20)
                                return 1;
                            if (value < 40)
                                return 2;
                            if (value < 60)
                                return 3;
                            if (value < 80)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_CRA:
                            if (value < 20)
                                return 1;
                            if (value < 40)
                                return 2;
                            if (value < 60)
                                return 3;
                            if (value < 80)
                                return 4;
                            return 5;
                    }
                    break;
                case 14://Agilit�
                    switch (breed)
                    {
                        case CharacterBreedEnum.BREED_FECA:
                            if (value < 20)
                                return 1;
                            if (value < 40)
                                return 2;
                            if (value < 60)
                                return 3;
                            if (value < 80)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_XELOR:
                            if (value < 20)
                                return 1;
                            if (value < 40)
                                return 2;
                            if (value < 60)
                                return 3;
                            if (value < 80)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_SACRIEUR:
                            return 3;

                        case CharacterBreedEnum.BREED_SRAM:
                            if (value < 100)
                                return 1;
                            if (value < 200)
                                return 2;
                            if (value < 300)
                                return 3;
                            if (value < 400)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_SADIDA:
                            if (value < 20)
                                return 1;
                            if (value < 40)
                                return 2;
                            if (value < 60)
                                return 3;
                            if (value < 80)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_PANDAWA:
                            if (value < 50)
                                return 1;
                            if (value < 200)
                                return 2;
                            return 3;

                        case CharacterBreedEnum.BREED_ENIRIPSA:
                            if (value < 20)
                                return 1;
                            if (value < 40)
                                return 2;
                            if (value < 60)
                                return 3;
                            if (value < 80)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_IOP:
                            if (value < 20)
                                return 1;
                            if (value < 40)
                                return 2;
                            if (value < 60)
                                return 3;
                            if (value < 80)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_ENUTROF:
                            if (value < 20)
                                return 1;
                            if (value < 40)
                                return 2;
                            if (value < 60)
                                return 3;
                            if (value < 80)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_ECAFLIP:
                            if (value < 50)
                                return 1;
                            if (value < 100)
                                return 2;
                            if (value < 150)
                                return 3;
                            if (value < 200)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_CRA:
                            if (value < 50)
                                return 1;
                            if (value < 100)
                                return 2;
                            if (value < 150)
                                return 3;
                            if (value < 200)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_OSAMODAS:
                            if (value < 20)
                                return 1;
                            if (value < 40)
                                return 2;
                            if (value < 60)
                                return 3;
                            if (value < 80)
                                return 4;
                            return 5;
                    }
                    break;
                case 15://Intelligence
                    switch (breed)
                    {
                        case CharacterBreedEnum.BREED_XELOR:
                            if (value < 100)
                                return 1;
                            if (value < 200)
                                return 2;
                            if (value < 300)
                                return 3;
                            if (value < 400)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_FECA:
                            if (value < 100)
                                return 1;
                            if (value < 200)
                                return 2;
                            if (value < 300)
                                return 3;
                            if (value < 400)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_SACRIEUR:
                            return 3;

                        case CharacterBreedEnum.BREED_SRAM:
                            if (value < 50)
                                return 2;
                            if (value < 150)
                                return 3;
                            if (value < 250)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_SADIDA:
                            if (value < 100)
                                return 1;
                            if (value < 200)
                                return 2;
                            if (value < 300)
                                return 3;
                            if (value < 400)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_ENUTROF:
                            if (value < 20)
                                return 1;
                            if (value < 60)
                                return 2;
                            if (value < 100)
                                return 3;
                            if (value < 140)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_PANDAWA:
                            if (value < 50)
                                return 1;
                            if (value < 200)
                                return 2;
                            return 3;

                        case CharacterBreedEnum.BREED_IOP:
                            if (value < 20)
                                return 1;
                            if (value < 40)
                                return 2;
                            if (value < 60)
                                return 3;
                            if (value < 80)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_ENIRIPSA:
                            if (value < 100)
                                return 1;
                            if (value < 200)
                                return 2;
                            if (value < 300)
                                return 3;
                            if (value < 400)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_CRA:
                            if (value < 50)
                                return 1;
                            if (value < 150)
                                return 2;
                            if (value < 250)
                                return 3;
                            if (value < 350)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_OSAMODAS:
                            if (value < 100)
                                return 1;
                            if (value < 200)
                                return 2;
                            if (value < 300)
                                return 3;
                            if (value < 400)
                                return 4;
                            return 5;

                        case CharacterBreedEnum.BREED_ECAFLIP:
                            if (value < 20)
                                return 1;
                            if (value < 40)
                                return 2;
                            if (value < 60)
                                return 3;
                            if (value < 80)
                                return 4;
                            return 5;
                    }
                    break;
            }
            return 5;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static GenericStats Deserialize(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return Serializer.Deserialize<GenericStats>(stream);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ProtoIgnore]
        public Dictionary<EffectEnum, GenericEffect> Effects
        {
            get
            {
                return m_effects;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ProtoIgnore]
        public IEnumerable<KeyValuePair<EffectEnum, GenericEffect>> WeaponEffects
        {
            get
            {
                return m_effects.Where(x => ItemTemplateDAO.IsWeaponEffect(x.Key));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<EffectEnum, GenericEffect> m_effects = new Dictionary<EffectEnum, GenericEffect>();               

        /// <summary>
        /// 
        /// </summary>
        public GenericStats()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monster"></param>
        public GenericStats(MonsterGradeDAO monster)
        {
            m_effects.Add(EffectEnum.AddAP, new GenericEffect(EffectEnum.AddAP, monster.AP));
            m_effects.Add(EffectEnum.AddMP, new GenericEffect(EffectEnum.AddMP, monster.MP));
            m_effects.Add(EffectEnum.AddInvocationMax, new GenericEffect(EffectEnum.AddInvocationMax, monster.MaxInvocation));
            m_effects.Add(EffectEnum.AddInitiative, new GenericEffect(EffectEnum.AddInitiative, monster.Initiative));
            m_effects.Add(EffectEnum.AddWisdom, new GenericEffect(EffectEnum.AddWisdom, monster.Wisdom));
            m_effects.Add(EffectEnum.AddStrength, new GenericEffect(EffectEnum.AddStrength, monster.Strenght));
            m_effects.Add(EffectEnum.AddIntelligence, new GenericEffect(EffectEnum.AddIntelligence, monster.Intelligence));
            m_effects.Add(EffectEnum.AddAgility, new GenericEffect(EffectEnum.AddAgility, monster.Agility));
            m_effects.Add(EffectEnum.AddChance, new GenericEffect(EffectEnum.AddChance, monster.Chance));

            m_effects.Add(EffectEnum.AddReduceDamagePercentNeutral, new GenericEffect(EffectEnum.AddReduceDamagePercentNeutral, monster.NeutralResistance));
            m_effects.Add(EffectEnum.AddReduceDamagePercentEarth, new GenericEffect(EffectEnum.AddReduceDamagePercentEarth, monster.EarthResistance));
            m_effects.Add(EffectEnum.AddReduceDamagePercentFire, new GenericEffect(EffectEnum.AddReduceDamagePercentFire, monster.FireResistance));
            m_effects.Add(EffectEnum.AddReduceDamagePercentWater, new GenericEffect(EffectEnum.AddReduceDamagePercentWater, monster.WaterResistance));
            m_effects.Add(EffectEnum.AddReduceDamagePercentAir, new GenericEffect(EffectEnum.AddReduceDamagePercentAir, monster.AirResistance));

            m_effects.Add(EffectEnum.AddAPDodge, new GenericEffect(EffectEnum.AddAPDodge, monster.APDodgePercent));
            m_effects.Add(EffectEnum.AddMPDodge, new GenericEffect(EffectEnum.AddMPDodge, monster.MPDodgePercent));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guild"></param>
        public GenericStats(GuildDAO guild) 
        {
            m_effects.Add(EffectEnum.AddAP, new GenericEffect(EffectEnum.AddAP, 6));
            m_effects.Add(EffectEnum.AddMP, new GenericEffect(EffectEnum.AddMP, 5));   
            m_effects.Add(EffectEnum.AddProspection, new GenericEffect(EffectEnum.AddProspection, 100));
            m_effects.Add(EffectEnum.AddPods, new GenericEffect(EffectEnum.AddPods, 1000));
            m_effects.Add(EffectEnum.AddInitiative, new GenericEffect(EffectEnum.AddInitiative, 100));
            m_effects.Add(EffectEnum.AddVitality, new GenericEffect(EffectEnum.AddVitality, 100 * guild.Level));
            m_effects.Add(EffectEnum.AddWisdom, new GenericEffect(EffectEnum.AddWisdom, guild.Level * 4));
            m_effects.Add(EffectEnum.AddStrength, new GenericEffect(EffectEnum.AddStrength, guild.Level));
            m_effects.Add(EffectEnum.AddIntelligence, new GenericEffect(EffectEnum.AddIntelligence, guild.Level));
            m_effects.Add(EffectEnum.AddAgility, new GenericEffect(EffectEnum.AddAgility, guild.Level));
            m_effects.Add(EffectEnum.AddChance, new GenericEffect(EffectEnum.AddChance, guild.Level));
            m_effects.Add(EffectEnum.AddDamage, new GenericEffect(EffectEnum.AddDamage, guild.Level));
            m_effects.Add(EffectEnum.AddReduceDamagePercentAir, new GenericEffect(EffectEnum.AddReduceDamagePercentAir, guild.Level / 2));
            m_effects.Add(EffectEnum.AddReduceDamagePercentWater, new GenericEffect(EffectEnum.AddReduceDamagePercentWater, guild.Level / 2));
            m_effects.Add(EffectEnum.AddReduceDamagePercentFire, new GenericEffect(EffectEnum.AddReduceDamagePercentFire, guild.Level / 2));
            m_effects.Add(EffectEnum.AddReduceDamagePercentEarth, new GenericEffect(EffectEnum.AddReduceDamagePercentEarth, guild.Level / 2));
            m_effects.Add(EffectEnum.AddReduceDamagePercentNeutral, new GenericEffect(EffectEnum.AddReduceDamagePercentNeutral, guild.Level / 2));  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public GenericStats(CharacterDAO character)
        {
            m_effects.Add(EffectEnum.AddAP, new GenericEffect(EffectEnum.AddAP, character.Ap));
            m_effects.Add(EffectEnum.AddMP, new GenericEffect(EffectEnum.AddMP, character.Mp));
            m_effects.Add(EffectEnum.AddProspection, new GenericEffect(EffectEnum.AddProspection, ((CharacterBreedEnum)character.Breed == CharacterBreedEnum.BREED_ENUTROF ? 120 : 100)));
            m_effects.Add(EffectEnum.AddPods, new GenericEffect(EffectEnum.AddPods, 1000));
            m_effects.Add(EffectEnum.AddInvocationMax, new GenericEffect(EffectEnum.AddInvocationMax, 1));
            m_effects.Add(EffectEnum.AddInitiative, new GenericEffect(EffectEnum.AddInitiative, 100));
            m_effects.Add(EffectEnum.AddVitality, new GenericEffect(EffectEnum.AddVitality, character.Vitality));
            m_effects.Add(EffectEnum.AddWisdom, new GenericEffect(EffectEnum.AddWisdom, character.Wisdom));
            m_effects.Add(EffectEnum.AddStrength, new GenericEffect(EffectEnum.AddStrength, character.Strength));
            m_effects.Add(EffectEnum.AddIntelligence, new GenericEffect(EffectEnum.AddIntelligence, character.Intelligence));
            m_effects.Add(EffectEnum.AddAgility, new GenericEffect(EffectEnum.AddAgility, character.Agility));
            m_effects.Add(EffectEnum.AddChance, new GenericEffect(EffectEnum.AddChance, character.Chance));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stats"></param>
        /// <returns></returns>
        public byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize<GenericStats>(stream, this);

                return stream.ToArray();
            }
        }   
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        public GenericEffect GetTotalEffect(EffectEnum effectType)
        {
            var totalBase = 0;
            var totalItems = 0;
            var totalDons = 0;
            var totalBoosts = 0;
            var effect = GetEffect(effectType);

            totalBase = effect.Base;
            totalItems = effect.Items;
            totalDons = effect.Dons;
            totalBoosts = effect.Boosts;

            switch (effectType)
            {
                case EffectEnum.AddAPDodge:
                case EffectEnum.AddMPDodge:
                    totalBase += GetTotal(EffectEnum.AddWisdom) / 4;
                    break;
                case EffectEnum.AddAP:
                    totalItems += GetTotal(EffectEnum.AddAPBis);
                    break;
                case EffectEnum.AddMP:
                    totalItems += GetTotal(EffectEnum.MPBonus);
                    break;
                case EffectEnum.AddReflectDamage:
                    totalItems += GetTotal(EffectEnum.AddReflectDamageItem);
                    break;
            }

            if (OppositeStats.ContainsKey(effectType))
            {
                foreach (EffectEnum OppositeEffect in OppositeStats[effectType])
                {
                    if (m_effects.ContainsKey(OppositeEffect))
                    {
                        totalBase -= m_effects[OppositeEffect].Base;
                        totalBoosts -= m_effects[OppositeEffect].Boosts;
                        totalDons -= m_effects[OppositeEffect].Dons;
                        totalItems -= m_effects[OppositeEffect].Items;
                    }
                }
            }

            return new GenericEffect(effectType, totalBase, totalItems, totalDons, totalBoosts);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="effectType"></param>
        /// <returns></returns>
        public int GetTotal(EffectEnum effectType)
        {
            int total = 0;

            if (m_effects.ContainsKey(effectType))            
                total += m_effects[effectType].Total;            

            switch (effectType)
            {
                case EffectEnum.AddAPDodge:
                case EffectEnum.AddMPDodge:
                    total += GetTotal(EffectEnum.AddWisdom) / 4;
                    break;
                case EffectEnum.AddAP:
                    total += GetTotal(EffectEnum.AddAPBis);
                    break;
                case EffectEnum.AddMP:
                    total += GetTotal(EffectEnum.MPBonus);
                    break;
                case EffectEnum.AddReflectDamage:
                    total += GetTotal(EffectEnum.AddReflectDamageItem);
                    break;
            }

            if (OppositeStats.ContainsKey(effectType))            
                foreach (EffectEnum OppositeEffect in OppositeStats[effectType])                
                    if (m_effects.ContainsKey(OppositeEffect))                    
                        total -= m_effects[OppositeEffect].Total;

            return total;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public void AddEffect(EffectEnum id, int min, int max = 0, string args = "0")
        {
            if (!m_effects.ContainsKey(id))
                m_effects.Add((EffectEnum)id, new GenericEffect(id, min, max, args));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GenericEffect GetEffect(EffectEnum id)
        {
            if (!m_effects.ContainsKey(id))
                m_effects.Add(id, new GenericEffect(id));
            return m_effects[id];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="EffectType"></param>
        /// <param name="value"></param>
        public void AddBase(EffectEnum id, int value)
        {
            if (!m_effects.ContainsKey(id))
                m_effects.Add(id, new GenericEffect(id));
            m_effects[id].Base += value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EffectType"></param>
        /// <param name="Value"></param>
        public void AddBoost(EffectEnum effectType, int value)
        {
            if (!m_effects.ContainsKey(effectType))
                m_effects.Add(effectType, new GenericEffect(effectType));
            m_effects[effectType].Boosts += value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effectType"></param>
        /// <param name="value"></param>
        public void AddItem(EffectEnum effectType, int value)
        {
            if (!m_effects.ContainsKey(effectType))
                m_effects.Add(effectType, new GenericEffect(effectType));
            m_effects[effectType].Items += value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EffectType"></param>
        /// <param name="Value"></param>
        public void AddDon(EffectEnum effectType, int Value)
        {
            if (!m_effects.ContainsKey(effectType))
                m_effects.Add(effectType, new GenericEffect(effectType));
            m_effects[effectType].Dons += Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Stats"></param>
        public void Merge(GenericStats Stats)
        {
            foreach (var effect in Stats.Effects)
            {
                if (!m_effects.ContainsKey(effect.Key))
                    m_effects.Add(effect.Key, new GenericEffect(effect.Key));
                m_effects[effect.Key].Merge(effect.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Stats"></param>
        public void UnMerge(GenericStats Stats)
        {
            foreach (var effect in Stats.Effects)
            {
                if (!m_effects.ContainsKey(effect.Key))
                    m_effects.Add(effect.Key, new GenericEffect(effect.Key));
                m_effects[effect.Key].UnMerge(effect.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearBoosts()
        {
            foreach (var effect in m_effects)
                effect.Value.Boosts = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        [ProtoIgnore]
        private string m_serialized;

        /// <summary>
        /// 
        /// </summary>
        public string ToItemStats()
        {
            if (m_serialized == null)
            {
                var serialized = new StringBuilder();
                if (Effects.Count > 0)
                {
                    foreach (var effect in m_effects)
                        serialized.Append(effect.Value.ToItemString()).Append(',');
                    serialized.Remove(serialized.Length - 1, 1);
                }
                m_serialized = serialized.ToString();
            }
            return m_serialized;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            m_effects.Clear();
            m_effects = null;
        }
    }
}
