using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Game.Spell;
using ProtoBuf;
using System.Xml.Serialization;

namespace Codebreak.Service.World.Manager
{
    public sealed class SpellManager : Singleton<SpellManager>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, SpellTemplate> m_templateById = new Dictionary<int, SpellTemplate>();

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            using (var stream = File.Open(ResourceManager.SPELLS_XML_PATH, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(List<SpellTemplate>));
                foreach (var template in (List<SpellTemplate>)serializer.Deserialize(stream))
                    m_templateById.Add(template.Id, template);          
            }
                      
            Logger.Info("SpellManager : " + m_templateById.Count + " SpellTemplate loaded.");
        }

        public void Save()
        {
            var templates = new Dictionary<int, SpellTemplate>();
            foreach (var sort in SortsRepository.Instance.All)
            {
                var newTemplate = new SpellTemplate();
                var oldTemplate = GetTemplate(sort.id);

                newTemplate.Id = oldTemplate.Id;
                newTemplate.Name = oldTemplate.Name;
                newTemplate.Description = oldTemplate.Description;
                newTemplate.Sprite = sort.sprite;
                newTemplate.SpriteInfos = sort.spriteInfos;
                newTemplate.Levels = new List<SpellLevel>();
                newTemplate.Levels.AddRange(oldTemplate.Levels);
                var targets = sort.effectTarget == "" ?
                    new int[] { } :
                    sort.effectTarget.Contains(",") ? sort.effectTarget.Split(',').Select(x => int.Parse(x)).ToArray() :
                    sort.effectTarget.Split(';').Select(x => int.Parse(x)).ToArray();
                int lvl = 1;
                newTemplate.Targets = new List<int>(targets);
                foreach (var level in newTemplate.Levels)
                {
                    string oldLevel = "";
                    switch (lvl)
                    {
                        case 1: oldLevel = sort.lvl1; break;
                        case 2: oldLevel = sort.lvl2; break;
                        case 3: oldLevel = sort.lvl3; break;
                        case 4: oldLevel = sort.lvl4; break;
                        case 5: oldLevel = sort.lvl5; break;
                        case 6: oldLevel = sort.lvl6; break;
                    }

                    if (oldLevel != "-1" && oldLevel != "")
                    {
                        var data = oldLevel.Split(',');
                        int minPo = int.Parse(data[3]);
                        int maxPo = int.Parse(data[4]);
                        int CSRate = int.Parse(data[5]);
                        int CFRate = int.Parse(data[6]);
                        bool inLine = data[7].Trim() == "true";
                        bool LOS = data[8].Trim() == "true";
                        bool EmptyCell = data[9].Trim() == "true";
                        bool PoBoost = data[10].Trim() == "true";
                        int maxPerTurn = int.Parse(data[12]);
                        int maxPerPlayer = int.Parse(data[13]);
                        bool cfEndTurn = data[19].Trim() == "true";

                        level.MinPO = minPo;
                        level.MaxPO = maxPo;
                        level.CSRate = CSRate;
                        level.ECSRate = CFRate;
                        level.InLine = inLine;
                        level.LOS = LOS;
                        level.EmptyCell = EmptyCell;
                        level.AllowPOBoost = PoBoost;
                        if (PoBoost)
                        {

                        }
                        level.MaxLaunchPerTurn = maxPerTurn;
                        level.MaxLaunchPerTarget = maxPerPlayer;
                    }

                    if (level.CriticalEffects == null)
                        level.CriticalEffects = new List<SpellEffect>();
                    if (level.Effects == null)
                        level.Effects = new List<SpellEffect>();
                    level.SpellId = newTemplate.Id;
                    level.Level = lvl++;
                    foreach (var effect in level.Effects.Concat(level.CriticalEffects))
                    {
                        effect.SpellId = newTemplate.Id;
                        effect.SpellLevel = level.Level;
                    }
                }
                templates.Add(newTemplate.Id, newTemplate);
            }

            using (var stream = File.Open("spells.bin", FileMode.Create))
                Serializer.Serialize<Dictionary<int, SpellTemplate>>(stream, templates);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spellId"></param>
        /// <param name="spellLevel"></param>
        /// <returns></returns>
        public SpellLevel GetSpellLevel(int spellId, int spellLevel)
        {
            SpellTemplate spell = null;
            SpellLevel level = null;
            if (m_templateById.TryGetValue(spellId, out spell))
            {
                level = spell.GetLevel(spellLevel);
            }
            return level;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spellId"></param>
        /// <returns></returns>
        public SpellTemplate GetTemplate(int spellId)
        {
            SpellTemplate spell = null;
            m_templateById.TryGetValue(spellId, out spell);
            return spell;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public IEnumerable<SpellBookEntryDAO> GetSpells(int ownerType, long ownerId)
        {
            return SpellBookEntryRepository.Instance.GetSpellEntries(ownerType, ownerId);
        }
    }
}
