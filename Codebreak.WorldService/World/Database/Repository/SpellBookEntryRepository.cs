using Codebreak.Framework.Database;
using Codebreak.WorldService.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Database.Repository
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
            if (!_spellBookEntriesByOwner.ContainsKey(spellBookEntry.Id))
                _spellBookEntriesByOwner.Add(spellBookEntry.Id, new List<SpellBookEntryDAO>());
            _spellBookEntriesByOwner[spellBookEntry.Id].Add(spellBookEntry);
        }

        public override void OnObjectRemoved(SpellBookEntryDAO spellBookEntry)
        {
            _spellBookEntriesByOwner[spellBookEntry.Id].Remove(spellBookEntry);
        }

        public List<SpellBookEntryDAO> GetSpellEntries(long ownerId)
        {
            if (_spellBookEntriesByOwner.ContainsKey(ownerId))
                return _spellBookEntriesByOwner[ownerId];
            return base.LoadMultiple("Id = @OwnerId", new { OwnerId = ownerId });
        }
    }
}
