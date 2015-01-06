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
        /// <param name="stats"></param>
        /// <returns></returns>
        public byte[] Serialize()
        {
            using(var stream = new MemoryStream())
            {
                Serializer.Serialize<GenericStats>(stream, this);

                return stream.ToArray();
            }
        }


        /// <summary>
        /// Effet possible de statistique
        /// </summary>
        [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
        public sealed class GenericEffect
        {
            /// <summary>
            /// 
            /// </summary>
            [ProtoIgnore]
            public int Total
            {
                get
                {
                    return Base + Items + Dons + Boosts;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            [ProtoIgnore]
            public EffectEnum EffectType
            {
                get
                {
                    return (EffectEnum)Type;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public int Type;
            public int Base;
            public int Items;
            public int Dons;
            public int Boosts;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="EffectId"></param>
            /// <param name="baseValue"></param>
            /// <param name="items"></param>
            /// <param name="dons"></param>
            /// <param name="boosts"></param>
            public GenericEffect(EffectEnum EffectId, int baseValue = 0, int items = 0, int dons = 0, int boosts = 0)
            {
                Type = (int)EffectId;
                Base = baseValue;
                Items = items;
                Dons = dons;
                Boosts = boosts;
            }

            /// <summary>
            /// 
            /// </summary>
            public GenericEffect()
            {
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="effect"></param>
            public void Merge(GenericEffect effect)
            {
                Base += effect.Base;
                Items += effect.Items;
                Dons += effect.Dons;
                Boosts += effect.Boosts;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="effect"></param>
            public void UnMerge(GenericEffect effect)
            {
                Base -= effect.Base;
                Items -= effect.Items;
                Dons -= effect.Dons;
                Boosts -= effect.Boosts;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Base + "," + Items + "," + Dons + "," + Boosts;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string ToItemString()
            {
                var serializedStats = new StringBuilder();
                serializedStats.Append(Type.ToString("x"));
                serializedStats.Append("#" + Items.ToString("x") + "#0#0#");
                serializedStats.Append("0d0+0");
                return serializedStats.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
        public sealed class WeaponEffect
        {
            /// <summary>
            /// 
            /// </summary>
            public int EffectId;
            public int Min;
            public int Max;
            public string Args;

            /// <summary>
            /// 
            /// </summary>
            public WeaponEffect()
            {
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <param name="args"></param>
            public WeaponEffect(int effectId, int min, int max, string args)
            {
                EffectId = effectId;
                Min = min;
                Max = max;
                Args = args;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                var serializedStats = new StringBuilder();
                serializedStats.Append(EffectId.ToString("x"));
                serializedStats.Append("#" + Min.ToString("x") + "#" + Max.ToString("x") + "#0#");
                serializedStats.Append("0d0+0");
                return serializedStats.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<EffectEnum, GenericEffect> m_genericEffects = new Dictionary<EffectEnum, GenericEffect>();
        private List<WeaponEffect> m_weaponEffects = new List<WeaponEffect>();
        private Dictionary<EffectEnum, string> m_specialEffects = new Dictionary<EffectEnum, string>();

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
        public GenericStats()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monster"></param>
        public GenericStats(MonsterGradeDAO monster)
        {
            m_genericEffects.Add(EffectEnum.AddAP, new GenericEffect(EffectEnum.AddAP, monster.AP));
            m_genericEffects.Add(EffectEnum.AddMP, new GenericEffect(EffectEnum.AddMP, monster.MP));
            m_genericEffects.Add(EffectEnum.AddInvocationMax, new GenericEffect(EffectEnum.AddInvocationMax, monster.MaxInvocation));
            m_genericEffects.Add(EffectEnum.AddInitiative, new GenericEffect(EffectEnum.AddInitiative, monster.Initiative));
            m_genericEffects.Add(EffectEnum.AddWisdom, new GenericEffect(EffectEnum.AddWisdom, monster.Wisdom));
            m_genericEffects.Add(EffectEnum.AddStrength, new GenericEffect(EffectEnum.AddStrength, monster.Strenght));
            m_genericEffects.Add(EffectEnum.AddIntelligence, new GenericEffect(EffectEnum.AddIntelligence, monster.Intelligence));
            m_genericEffects.Add(EffectEnum.AddAgility, new GenericEffect(EffectEnum.AddAgility, monster.Agility));
            m_genericEffects.Add(EffectEnum.AddChance, new GenericEffect(EffectEnum.AddChance, monster.Chance));

            m_genericEffects.Add(EffectEnum.AddReduceDamagePercentNeutral, new GenericEffect(EffectEnum.AddReduceDamagePercentNeutral, monster.NeutralResistance));
            m_genericEffects.Add(EffectEnum.AddReduceDamagePercentEarth, new GenericEffect(EffectEnum.AddReduceDamagePercentEarth, monster.EarthResistance));
            m_genericEffects.Add(EffectEnum.AddReduceDamagePercentFire, new GenericEffect(EffectEnum.AddReduceDamagePercentFire, monster.FireResistance));
            m_genericEffects.Add(EffectEnum.AddReduceDamagePercentWater, new GenericEffect(EffectEnum.AddReduceDamagePercentWater, monster.WaterResistance));
            m_genericEffects.Add(EffectEnum.AddReduceDamagePercentAir, new GenericEffect(EffectEnum.AddReduceDamagePercentAir, monster.AirResistance));

            m_genericEffects.Add(EffectEnum.AddAPDodge, new GenericEffect(EffectEnum.AddAPDodge, monster.APDodgePercent));
            m_genericEffects.Add(EffectEnum.AddMPDodge, new GenericEffect(EffectEnum.AddMPDodge, monster.MPDodgePercent));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guild"></param>
        public GenericStats(GuildDAO guild) 
        {
            m_genericEffects.Add(EffectEnum.AddAP, new GenericEffect(EffectEnum.AddAP, 6));
            m_genericEffects.Add(EffectEnum.AddMP, new GenericEffect(EffectEnum.AddMP, 5));   
            m_genericEffects.Add(EffectEnum.AddProspection, new GenericEffect(EffectEnum.AddProspection, 100));
            m_genericEffects.Add(EffectEnum.AddPods, new GenericEffect(EffectEnum.AddPods, 1000));
            m_genericEffects.Add(EffectEnum.AddInitiative, new GenericEffect(EffectEnum.AddInitiative, 100));
            m_genericEffects.Add(EffectEnum.AddVitality, new GenericEffect(EffectEnum.AddVitality, 100 * guild.Level));
            m_genericEffects.Add(EffectEnum.AddWisdom, new GenericEffect(EffectEnum.AddWisdom, guild.Level * 4));
            m_genericEffects.Add(EffectEnum.AddStrength, new GenericEffect(EffectEnum.AddStrength, guild.Level));
            m_genericEffects.Add(EffectEnum.AddIntelligence, new GenericEffect(EffectEnum.AddIntelligence, guild.Level));
            m_genericEffects.Add(EffectEnum.AddAgility, new GenericEffect(EffectEnum.AddAgility, guild.Level));
            m_genericEffects.Add(EffectEnum.AddChance, new GenericEffect(EffectEnum.AddChance, guild.Level));
            m_genericEffects.Add(EffectEnum.AddDamage, new GenericEffect(EffectEnum.AddDamage, guild.Level));
            m_genericEffects.Add(EffectEnum.AddReduceDamagePercentAir, new GenericEffect(EffectEnum.AddReduceDamagePercentAir, guild.Level / 2));
            m_genericEffects.Add(EffectEnum.AddReduceDamagePercentWater, new GenericEffect(EffectEnum.AddReduceDamagePercentWater, guild.Level / 2));
            m_genericEffects.Add(EffectEnum.AddReduceDamagePercentFire, new GenericEffect(EffectEnum.AddReduceDamagePercentFire, guild.Level / 2));
            m_genericEffects.Add(EffectEnum.AddReduceDamagePercentEarth, new GenericEffect(EffectEnum.AddReduceDamagePercentEarth, guild.Level / 2));
            m_genericEffects.Add(EffectEnum.AddReduceDamagePercentNeutral, new GenericEffect(EffectEnum.AddReduceDamagePercentNeutral, guild.Level / 2));  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public GenericStats(CharacterDAO character)
        {
            m_genericEffects.Add(EffectEnum.AddAP, new GenericEffect(EffectEnum.AddAP, character.Ap));
            m_genericEffects.Add(EffectEnum.AddMP, new GenericEffect(EffectEnum.AddMP, character.Mp));
            m_genericEffects.Add(EffectEnum.AddProspection, new GenericEffect(EffectEnum.AddProspection, ((CharacterBreedEnum)character.Breed == CharacterBreedEnum.BREED_ENUTROF ? 120 : 100)));
            m_genericEffects.Add(EffectEnum.AddPods, new GenericEffect(EffectEnum.AddPods, 1000));
            m_genericEffects.Add(EffectEnum.AddInvocationMax, new GenericEffect(EffectEnum.AddInvocationMax, 1));
            m_genericEffects.Add(EffectEnum.AddInitiative, new GenericEffect(EffectEnum.AddInitiative, 100));
            m_genericEffects.Add(EffectEnum.AddVitality, new GenericEffect(EffectEnum.AddVitality, character.Vitality));
            m_genericEffects.Add(EffectEnum.AddWisdom, new GenericEffect(EffectEnum.AddWisdom, character.Wisdom));
            m_genericEffects.Add(EffectEnum.AddStrength, new GenericEffect(EffectEnum.AddStrength, character.Strength));
            m_genericEffects.Add(EffectEnum.AddIntelligence, new GenericEffect(EffectEnum.AddIntelligence, character.Intelligence));
            m_genericEffects.Add(EffectEnum.AddAgility, new GenericEffect(EffectEnum.AddAgility, character.Agility));
            m_genericEffects.Add(EffectEnum.AddChance, new GenericEffect(EffectEnum.AddChance, character.Chance));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<EffectEnum, GenericEffect> GetEffects()
        {
            return m_genericEffects;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<WeaponEffect> GetWeaponEffects()
        {
            return m_weaponEffects;
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
                    if (m_genericEffects.ContainsKey(OppositeEffect))
                    {
                        totalBase -= m_genericEffects[OppositeEffect].Base;
                        totalBoosts -= m_genericEffects[OppositeEffect].Boosts;
                        totalDons -= m_genericEffects[OppositeEffect].Dons;
                        totalItems -= m_genericEffects[OppositeEffect].Items;
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

            if (m_genericEffects.ContainsKey(effectType))
            {
                total += m_genericEffects[effectType].Total;
            }

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
            {
                foreach (EffectEnum OppositeEffect in OppositeStats[effectType])
                {
                    if (m_genericEffects.ContainsKey(OppositeEffect))
                    {
                        total -= m_genericEffects[OppositeEffect].Total;
                    }
                }
            }

            return total;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="EffectType"></param>
        /// <returns></returns>
        public GenericEffect GetEffect(EffectEnum EffectType)
        {
            if (!m_genericEffects.ContainsKey(EffectType))
                m_genericEffects.Add(EffectType, new GenericEffect(EffectType));
            return m_genericEffects[EffectType];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EffectType"></param>
        /// <returns></returns>
        public IEnumerable<WeaponEffect> GetWeaponEffect(EffectEnum EffectType)
        {
            return m_weaponEffects.Where(effect => effect.EffectId == (int)EffectType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EffectType"></param>
        /// <returns></returns>
        public string GetSpecialEffect(EffectEnum EffectType)
        {
            if (!m_specialEffects.ContainsKey(EffectType))
                return null;
            return m_specialEffects[EffectType];
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearBoosts()
        {
            foreach (var effect in m_genericEffects)            
                effect.Value.Boosts = 0;            
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="EffectType"></param>
        /// <param name="Value"></param>
        public void AddBase(EffectEnum effectType, int Value)
        {
            if (!m_genericEffects.ContainsKey(effectType))
                m_genericEffects.Add(effectType, new GenericEffect(effectType));
            m_genericEffects[effectType].Base += Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EffectType"></param>
        /// <param name="Value"></param>
        public void AddBoost(EffectEnum effectType, int value)
        {
            if (!m_genericEffects.ContainsKey(effectType))
                m_genericEffects.Add(effectType, new GenericEffect(effectType));
            m_genericEffects[effectType].Boosts += value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effectType"></param>
        /// <param name="value"></param>
        public void AddItem(EffectEnum effectType, int value)
        {
            if (!m_genericEffects.ContainsKey(effectType))
                m_genericEffects.Add(effectType, new GenericEffect(effectType));
            m_genericEffects[effectType].Items += value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EffectType"></param>
        /// <param name="Value"></param>
        public void AddDon(EffectEnum effectType, int Value)
        {
            if (!m_genericEffects.ContainsKey(effectType))
                m_genericEffects.Add(effectType, new GenericEffect(effectType));
            m_genericEffects[effectType].Dons += Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effectType"></param>
        /// <param name="minJet"></param>
        /// <param name="maxJet"></param>
        public void AddWeaponEffect(EffectEnum effectType, int minJet, int maxJet, string args)
        {
            m_weaponEffects.Add(new WeaponEffect((int)effectType, minJet, maxJet, args));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="EffectType"></param>
        /// <param name="Args"></param>
        public void AddSpecialEffect(EffectEnum EffectType, string Args)
        {
            m_specialEffects.Add(EffectType, Args);
        }

        /// <summary>
        /// 
        /// </summary>
        public string ToItemStats()
        {
            return string.Join(",", m_weaponEffects.Select(x => x.ToString())) + (m_genericEffects.Count > 0 ? "," : "") +  string.Join(",", m_genericEffects.Select(x => x.Value.ToItemString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Stats"></param>
        public void Merge(GenericStats Stats)
        {
            foreach (var Effect in Stats.GetEffects())
            {
                if (!m_genericEffects.ContainsKey(Effect.Key))
                    m_genericEffects.Add(Effect.Key, new GenericEffect(Effect.Key));
                m_genericEffects[Effect.Key].Merge(Effect.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Stats"></param>
        public void UnMerge(GenericStats Stats)
        {
            foreach (var Effect in Stats.GetEffects())
            {
                if (!m_genericEffects.ContainsKey(Effect.Key))
                    m_genericEffects.Add(Effect.Key, new GenericEffect(Effect.Key));
                m_genericEffects[Effect.Key].UnMerge(Effect.Value);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            m_genericEffects.Clear();
            m_genericEffects = null;

            m_specialEffects.Clear();
            m_specialEffects = null;

            m_weaponEffects.Clear();
            m_weaponEffects = null;
        }

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
    }
}
