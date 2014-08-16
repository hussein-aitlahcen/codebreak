using Codebreak.Framework.Generic;
using Codebreak.WorldService.World.Database.Repository;
using Codebreak.WorldService.World.Database.Structure;
using Codebreak.WorldService.World.Spell;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Manager
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
