using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Database.Repository
{
    public sealed class SpellBookEntryRepository : Repository<SpellBookEntryRepository, SpellBookEntryDAO>
    {
        private Dictionary<long, List<SpellBookEntryDAO>> _spellBookEntriesByOwner;

        public SpellBookEntryRepository()
        {
            _spellBookEntriesByOwner = new Dictionary<long, List<SpellBookEntryDAO>>();
        }

        public override void OnObjectAdded(SpellBookEntryDAO spellBookEntry)
        {
            if (!_spellBookEntriesByOwner.ContainsKey(spellBookEntry.CharacterId))
                _spellBookEntriesByOwner.Add(spellBookEntry.CharacterId, new List<SpellBookEntryDAO>());
            _spellBookEntriesByOwner[spellBookEntry.CharacterId].Add(spellBookEntry);
        }

        public override void OnObjectRemoved(SpellBookEntryDAO spellBookEntry)
        {
            _spellBookEntriesByOwner[spellBookEntry.CharacterId].Remove(spellBookEntry);
        }

        public List<SpellBookEntryDAO> GetSpellEntries(long ownerId)
        {
            if (_spellBookEntriesByOwner.ContainsKey(ownerId))
                return _spellBookEntriesByOwner[ownerId];
            return base.LoadMultiple("CharacterId = @OwnerId", new { OwnerId = ownerId });
        }
    }
}
