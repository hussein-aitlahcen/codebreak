using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Database.Repository;
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
            using (var stream = File.OpenRead(ResourceManager.SPELLS_BINARY_PATH))            
                _templateById = Serializer.Deserialize<Dictionary<int, SpellTemplate>>(stream);

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
        public IEnumerable<SpellBookEntryDAO> GetSpells(int ownerType, long ownerId)
        {
            return SpellBookEntryRepository.Instance.GetSpellEntries(ownerType, ownerId);
        }
    }
}
