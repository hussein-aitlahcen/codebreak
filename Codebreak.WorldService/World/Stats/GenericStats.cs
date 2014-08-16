using Codebreak.WorldService.World.Database.Structure;
using Codebreak.WorldService.World.Entity;
using Codebreak.WorldService.World.Spell;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Stats
{
    /// <summary>
    /// Classe generique pour les statistiques
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public sealed class GenericStats
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
                var SerializedStats = new StringBuilder();

                if (ItemTemplateDAO.IsWeaponEffect(EffectType))
                {
                    // TODO JET DES WEAPONS 1dx+x
                }
                else
                {
                    SerializedStats.Append(Type.ToString("x"));
                    SerializedStats.Append("#" + Items.ToString("x") + "#0#0#");
                    SerializedStats.Append("0d0+0");
                }

                return SerializedStats.ToString();
            }
        }

        /// <summary>
        /// Effet possible d'une arme
        /// </summary>
        [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
        public sealed class WeaponEffect
        {
            /// <summary>
            /// 
            /// </summary>
            public int Min;
            public int Max;
            public string Args;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <param name="args"></param>
            public WeaponEffect(int min, int max, string args)
            {
                Min = min;
                Max = max;
                Args = args;
            }

            /// <summary>
            /// 
            /// </summary>
            public WeaponEffect()
            {
            }
        }

        /* Dictionnaire des effets */
        private Dictionary<EffectEnum, GenericEffect> _genericEffects = new Dictionary<EffectEnum, GenericEffect>();
        private Dictionary<Tuple<EffectEnum, int>, WeaponEffect> _weaponEffects = new Dictionary<Tuple<EffectEnum, int>, WeaponEffect>();
        private Dictionary<EffectEnum, string> _specialEffects = new Dictionary<EffectEnum, string>();

        /* Effets opposés */
        private static Dictionary<EffectEnum, List<EffectEnum>> OppositeStats = new Dictionary<EffectEnum, List<EffectEnum>>()
        {
            /* Statistiques de base */
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

            /* Statistiques avancés */
            {EffectEnum.AddDamage, new List<EffectEnum>() { EffectEnum.SubDamage }},
            {EffectEnum.AddDamagePercent, new List<EffectEnum>() { EffectEnum.SubDamagePercent }},
            {EffectEnum.AddDamageCritic, new List<EffectEnum>() { EffectEnum.SubDamageCritic }},
            {EffectEnum.AddDamageMagic, new List<EffectEnum>() { EffectEnum.SubDamageMagic }},
            {EffectEnum.AddDamagePhysic, new List<EffectEnum>() { EffectEnum.SubDamagePhysic }},
            {EffectEnum.AddAPDodge, new List<EffectEnum>() { EffectEnum.SubAPDodgeable }},
            {EffectEnum.AddMPDodge, new List<EffectEnum>() { EffectEnum.SubMPDodgeable }},

            /* Resistances / Malus */
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

        public GenericStats()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public GenericStats(CharacterDAO character)
        {
            _genericEffects.Add(EffectEnum.AddAP, new GenericEffect(EffectEnum.AddAP, character.Ap));
            _genericEffects.Add(EffectEnum.AddMP, new GenericEffect(EffectEnum.AddMP, character.Mp));
            _genericEffects.Add(EffectEnum.AddProspection, new GenericEffect(EffectEnum.AddProspection, ((CharacterBreedEnum)character.Breed == CharacterBreedEnum.BREED_ENUTROF ? 120 : 100)));
            _genericEffects.Add(EffectEnum.AddPods, new GenericEffect(EffectEnum.AddPods, 1000));
            _genericEffects.Add(EffectEnum.AddInvocationMax, new GenericEffect(EffectEnum.AddInvocationMax, 1));
            _genericEffects.Add(EffectEnum.AddInitiative, new GenericEffect(EffectEnum.AddInitiative, 100));

            _genericEffects.Add(EffectEnum.AddVitality, new GenericEffect(EffectEnum.AddVitality, character.Vitality));
            _genericEffects.Add(EffectEnum.AddWisdom, new GenericEffect(EffectEnum.AddWisdom, character.Wisdom));
            _genericEffects.Add(EffectEnum.AddStrength, new GenericEffect(EffectEnum.AddStrength, character.Strength));
            _genericEffects.Add(EffectEnum.AddIntelligence, new GenericEffect(EffectEnum.AddIntelligence, character.Intelligence));
            _genericEffects.Add(EffectEnum.AddAgility, new GenericEffect(EffectEnum.AddAgility, character.Agility));
            _genericEffects.Add(EffectEnum.AddChance, new GenericEffect(EffectEnum.AddChance, character.Chance));
        }

        /// <summary>
        /// Retourne tous les effets
        /// </summary>
        /// <returns></returns>
        public Dictionary<EffectEnum, GenericEffect> GetEffects()
        {
            return _genericEffects;
        }

        /// <summary>
        /// Retoure les effets d'arme
        /// </summary>
        /// <returns></returns>
        public Dictionary<Tuple<EffectEnum, int>, WeaponEffect> GetWeaponEffects()
        {
            return _weaponEffects;
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

            // malus ?
            if (OppositeStats.ContainsKey(effectType))
            {
                foreach (EffectEnum OppositeEffect in OppositeStats[effectType])
                {
                    if (_genericEffects.ContainsKey(OppositeEffect))
                    {
                        totalBase -= _genericEffects[OppositeEffect].Base;
                        totalBoosts -= _genericEffects[OppositeEffect].Boosts;
                        totalDons -= _genericEffects[OppositeEffect].Dons;
                        totalItems -= _genericEffects[OppositeEffect].Items;
                    }
                }
            }

            return new GenericEffect(effectType, totalBase, totalItems, totalDons, totalBoosts);
        }

        /// <summary>
        /// Retourne le total d'un effet
        /// </summary>
        /// <param name="effectType"></param>
        /// <returns></returns>
        public int GetTotal(EffectEnum effectType)
        {
            int total = 0;

            // existant ?
            if (_genericEffects.ContainsKey(effectType))
            {
                total += _genericEffects[effectType].Total;
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

            // malus ?
            if (OppositeStats.ContainsKey(effectType))
            {
                foreach (EffectEnum OppositeEffect in OppositeStats[effectType])
                {
                    if (_genericEffects.ContainsKey(OppositeEffect))
                    {
                        total -= _genericEffects[OppositeEffect].Total;
                    }
                }
            }

            return total;
        }

        /// <summary>
        /// Retourne l'effet désiré
        /// </summary>
        /// <param name="EffectType"></param>
        /// <returns></returns>
        public GenericEffect GetEffect(EffectEnum EffectType)
        {
            if (!_genericEffects.ContainsKey(EffectType))
                _genericEffects.Add(EffectType, new GenericEffect(EffectType));

            return _genericEffects[EffectType];
        }

        /// <summary>
        /// Retourne l'effet désiré
        /// </summary>
        /// <param name="EffectType"></param>
        /// <returns></returns>
        public IEnumerable<WeaponEffect> GetWeaponEffect(EffectEnum EffectType)
        {
            return _weaponEffects.Where(entry => entry.Key.Item1 == EffectType).Select(entry => entry.Value);
        }

        /// <summary>
        /// Retourne l'effet désiré
        /// </summary>
        /// <param name="EffectType"></param>
        /// <returns></returns>
        public string GetSpecialEffect(EffectEnum EffectType)
        {
            if (!_specialEffects.ContainsKey(EffectType))
                return null;

            return _specialEffects[EffectType];
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearBoosts()
        {
            foreach (var effect in _genericEffects)
            {
                effect.Value.Boosts = 0;
            }
        }

        /// <summary>
        /// Stats de base
        /// </summary>
        /// <param name="EffectType"></param>
        /// <param name="Value"></param>
        public void AddBase(EffectEnum EffectType, int Value)
        {
            if (!_genericEffects.ContainsKey(EffectType))
                _genericEffects.Add(EffectType, new GenericEffect(EffectType, baseValue: Value));
            else
                _genericEffects[EffectType].Base += Value;
        }

        /// <summary>
        /// Boosts (buffs etc)
        /// </summary>
        /// <param name="EffectType"></param>
        /// <param name="Value"></param>
        public void AddBoost(EffectEnum EffectType, int Value)
        {
            if (!_genericEffects.ContainsKey(EffectType))
                _genericEffects.Add(EffectType, new GenericEffect(EffectType, boosts: Value));
            else
                _genericEffects[EffectType].Boosts += Value;
        }

        /// <summary>
        /// Effets d'items
        /// </summary>
        /// <param name="EffectType"></param>
        /// <param name="Value"></param>
        public void AddItem(EffectEnum EffectType, int Value)
        {
            if (!_genericEffects.ContainsKey(EffectType))
                _genericEffects.Add(EffectType, new GenericEffect(EffectType, items: Value));
            else
                _genericEffects[EffectType].Items += Value;
        }

        /// <summary>
        /// Effets d'items
        /// </summary>
        /// <param name="EffectType"></param>
        /// <param name="Value"></param>
        public void AddDon(EffectEnum EffectType, int Value)
        {
            if (!_genericEffects.ContainsKey(EffectType))
                _genericEffects.Add(EffectType, new GenericEffect(EffectType, dons: Value));
            else
                _genericEffects[EffectType].Dons += Value;
        }

        /// <summary>
        /// Ajout d'un effet d'arme
        /// </summary>
        /// <param name="EffectType"></param>
        /// <param name="MinJet"></param>
        /// <param name="MaxJet"></param>
        public void AddWeaponEffect(EffectEnum EffectType, int MinJet, int MaxJet, string Args)
        {
            _weaponEffects.Add(new Tuple<EffectEnum, int>(EffectType, _weaponEffects.Count), new WeaponEffect(MinJet, MaxJet, Args));
        }

        /// <summary>
        /// Ajout d'un effet texte genre traque etc ...
        /// </summary>
        /// <param name="EffectType"></param>
        /// <param name="Args"></param>
        public void AddSpecialEffect(EffectEnum EffectType, string Args)
        {
            _specialEffects.Add(EffectType, Args);
        }

        /// <summary>
        /// Retourne les stats en format de stats d'item
        /// </summary>
        public string ToItemStats()
        {
            return string.Join(",", _genericEffects.Select(x => x.Value.ToItemString())) + (_weaponEffects.Count > 0 ? "," : "") + string.Join(",", _weaponEffects.Select(x => x.Value.ToString()));
        }

        /// <summary>
        /// Fusionne les statistiques
        /// </summary>
        /// <param name="Stats"></param>
        public void Merge(GenericStats Stats)
        {
            foreach (var Effect in Stats.GetEffects())
            {
                if (!_genericEffects.ContainsKey(Effect.Key))
                    _genericEffects.Add(Effect.Key, new GenericEffect(Effect.Key));

                _genericEffects[Effect.Key].Merge(Effect.Value);
            }
        }

        /// <summary>
        /// Defusionne les statistiques
        /// </summary>
        /// <param name="Stats"></param>
        public void UnMerge(GenericStats Stats)
        {
            foreach (var Effect in Stats.GetEffects())
            {
                if (!_genericEffects.ContainsKey(Effect.Key))
                    _genericEffects.Add(Effect.Key, new GenericEffect(Effect.Key));

                _genericEffects[Effect.Key].UnMerge(Effect.Value);
            }
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
