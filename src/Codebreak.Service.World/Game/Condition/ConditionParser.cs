using Codebreak.Framework.Generic;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Spell;
using ExpressionEvaluator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Condition
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ConditionParser : Singleton<ConditionParser>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, Func<CharacterEntity, bool>> m_compiledExpressions;

        /// <summary>
        /// 
        /// </summary>
        public ConditionParser()
        {
            m_compiledExpressions = new Dictionary<string, Func<CharacterEntity, bool>>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool Check(string conditions, CharacterEntity character)
        {
            if (conditions == string.Empty)
                return true;

            Func<CharacterEntity, bool> method;
            lock(m_compiledExpressions)
            {
                if (m_compiledExpressions.ContainsKey(conditions))
                {
                    method = m_compiledExpressions[conditions];
                }
                else
                {
                    Regex hasTemplateRegex = new Regex(@"PO==(?<HasTemplate>[0-9]*)", RegexOptions.None);
                    Regex notHasTemplateRegex = new Regex(@"PO!=(?<NotHasTemplate>[0-9]*)", RegexOptions.None);

                    var subCond = hasTemplateRegex.Replace(conditions, @"character.Inventory.HasTemplate(${HasTemplate})");
                    subCond = notHasTemplateRegex.Replace(subCond, @"character.Inventory.NotHasTemplate(${NotHasTemplate})");
                    
                    // Nouvelle
                    StringBuilder realConditions = new StringBuilder(subCond);

                    // Stats tot
                    realConditions.Replace("CI", "character.Statistics.GetTotal(EffectEnum.AddIntelligence)");
                    realConditions.Replace("CV", "character.Statistics.GetTotal(EffectEnum.AddVitality)");
                    realConditions.Replace("CA", "character.Statistics.GetTotal(EffectEnum.AddAgility)");
                    realConditions.Replace("CW", "character.Statistics.GetTotal(EffectEnum.AddWisdom)");
                    realConditions.Replace("CC", "character.Statistics.GetTotal(EffectEnum.AddChance)");
                    realConditions.Replace("CS", "character.Statistics.GetTotal(EffectEnum.AddStrength)");

                    // Stats base
                    realConditions.Replace("Ci", "character.DatabaseRecord.Intelligence");
                    realConditions.Replace("Cs", "character.DatabaseRecord.Strength");
                    realConditions.Replace("Cv", "character.DatabaseRecord.Vitality");
                    realConditions.Replace("Ca", "character.DatabaseRecord.Agility");
                    realConditions.Replace("Cw", "character.DatabaseRecord.Wisdom");
                    realConditions.Replace("Cc", "character.DatabaseRecord.Chance");

                    // Perso
                    realConditions.Replace("Ps", "character.AlignmentId");
                    realConditions.Replace("Pa", "character.AlignmentLevel");
                    realConditions.Replace("PP", "character.AlignmentPromotion");
                    realConditions.Replace("PL", "character.Level");
                    realConditions.Replace("PK", "character.Inventory.Kamas");
                    realConditions.Replace("PG", "(int)character.Breed");
                    realConditions.Replace("PS", "character.Sex");
                    realConditions.Replace("PZ", "1"); // Abonné
                    realConditions.Replace("PN", "character.Name");
                    realConditions.Replace("Pg", "0"); // Don
                    realConditions.Replace("PR", "0"); // Married 
                    if (character.Account != null)
                        realConditions.Replace("PX", "character.Account.Power"); // Admin level
                    realConditions.Replace("PW", "10000"); // MaxWeight
                    if (character.Map != null)
                        realConditions.Replace("PB", "character.Map.SubAreaId");

                    realConditions.Replace("SI", "character.MapId");
                    realConditions.Replace("MiS", "character.Id");
                    realConditions.Replace("BI", "0"); // ?

                    var registry = new TypeRegistry();
                    registry.RegisterSymbol("character", character);
                    method = new CompiledExpression<bool>(realConditions.ToString()) { TypeRegistry = registry }.ScopeCompile<CharacterEntity>();                 
                    m_compiledExpressions.Add(conditions, method);
                }
            }
            return method(character);
        }
    }
}
