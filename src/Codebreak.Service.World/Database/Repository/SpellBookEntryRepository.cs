using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SpellBookEntryRepository : Repository<SpellBookEntryRepository, SpellBookEntryDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, List<SpellBookEntryDAO>> m_spellBookEntriesByOwner;

        /// <summary>
        /// 
        /// </summary>
        public SpellBookEntryRepository()
        {
            m_spellBookEntriesByOwner = new Dictionary<long, List<SpellBookEntryDAO>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spellBookEntry"></param>
        public override void OnObjectAdded(SpellBookEntryDAO spellBookEntry)
        {
            if (!m_spellBookEntriesByOwner.ContainsKey(spellBookEntry.OwnerId))
                m_spellBookEntriesByOwner.Add(spellBookEntry.OwnerId, new List<SpellBookEntryDAO>());
            m_spellBookEntriesByOwner[spellBookEntry.OwnerId].Add(spellBookEntry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spellBookEntry"></param>
        public override void OnObjectRemoved(SpellBookEntryDAO spellBookEntry)
        {
            m_spellBookEntriesByOwner[spellBookEntry.OwnerId].Remove(spellBookEntry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerType"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public IEnumerable<SpellBookEntryDAO> GetSpellEntries(int ownerType, long ownerId)
        {
            if (m_spellBookEntriesByOwner.ContainsKey(ownerId))
                return m_spellBookEntriesByOwner[ownerId];
            return base.LoadMultiple("OwnerType = @OwnerType AND OwnerId = @OwnerId", new { OwnerType = ownerType, OwnerId = ownerId });
        }
    }
}
