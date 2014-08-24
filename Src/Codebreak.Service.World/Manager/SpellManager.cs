using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Structures;
using Codebreak.Service.World.Game.Database.Repositories;
using Codebreak.Service.World.Game.Spell;
using ProtoBuf;

namespace Codebreak.Service.World.Manager
{
    public sealed class SpellManager : Singleton<SpellManager>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, SpellTemplate> _templateById = new Dictionary<int, SpellTemplate>();

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            using (var stream = File.OpenRead("Resources/data/spells.bin"))
            {
                _templateById = Serializer.Deserialize<Dictionary<int, SpellTemplate>>(stream);
            }

            Logger.Info("SpellManager : " + _templateById.Count + " SpellTemplate loaded.");
        }

        //public void Save()
        //{
        //    var templates = new Dictionary<int, SpellTemplate>();
        //    foreach(var sort in SortsRepository.Instance.GetAll())
        //    {
        //        var newTemplate = new SpellTemplate();
        //        var oldTemplate = GetTemplate(sort.id);

        //        newTemplate.Id = oldTemplate.Id;
        //        newTemplate.Name = oldTemplate.Name;
        //        newTemplate.Description = oldTemplate.Description;
        //        newTemplate.Sprite = sort.sprite;
        //        newTemplate.SpriteInfos = sort.spriteInfos;
        //        newTemplate.Levels = oldTemplate.Levels;
        //        var targets = sort.effectTarget == "" ? 
        //            new int[] { } : 
        //            sort.effectTarget.Contains(",") ? sort.effectTarget.Split(',').Select(x => int.Parse(x)).ToArray() : 
        //            sort.effectTarget.Split(';').Select(x => int.Parse(x)).ToArray();
        //        int lvl = 0;
        //        foreach(var level in newTemplate.Levels)
        //        {
        //            if (level.CriticalEffects == null)
        //                level.CriticalEffects = new List<SpellEffect>();
        //            if (level.Effects == null)
        //                level.Effects = new List<SpellEffect>();
        //            level.SpellId = newTemplate.Id;
        //            if (targets.Length > 0)
        //                level.Targets = targets;
        //            else
        //                level.Targets = new int[] { };
        //            level.Level = ++lvl;
        //            foreach(var effect in level.Effects.Concat(level.CriticalEffects))
        //            {
        //                effect.SpellId = newTemplate.Id;
        //                effect.SpellLevel = level.Level;
        //            }
        //        }
        //        templates.Add(newTemplate.Id, newTemplate);
        //    }
        //    using (var stream = File.Open("Resources/data/spells.bin", FileMode.Create))
        //    {
        //         Serializer.Serialize<Dictionary<int, SpellTemplate>>(stream, templates);
        //    }
        //}

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
            if (_templateById.TryGetValue(spellId, out spell))
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
            _templateById.TryGetValue(spellId, out spell);
            return spell;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public List<SpellBookEntryDAO> GetSpells(long ownerId)
        {
            return SpellBookEntryRepository.Instance.GetSpellEntries(ownerId);
        }
    }
}
