using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repositories
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
            if (!_spellBookEntriesByOwner.ContainsKey(spellBookEntry.OwnerId))
                _spellBookEntriesByOwner.Add(spellBookEntry.OwnerId, new List<SpellBookEntryDAO>());
            _spellBookEntriesByOwner[spellBookEntry.OwnerId].Add(spellBookEntry);
        }

        public override void OnObjectRemoved(SpellBookEntryDAO spellBookEntry)
        {
            _spellBookEntriesByOwner[spellBookEntry.OwnerId].Remove(spellBookEntry);
        }

        public List<SpellBookEntryDAO> GetSpellEntries(int ownerType, long ownerId)
        {
            if (_spellBookEntriesByOwner.ContainsKey(ownerId))
                return _spellBookEntriesByOwner[ownerId];
            return base.LoadMultiple("OwnerType = @OwnerType AND OwnerId = @OwnerId", new { OwnerType = ownerType, OwnerId = ownerId });
        }
    }
}
