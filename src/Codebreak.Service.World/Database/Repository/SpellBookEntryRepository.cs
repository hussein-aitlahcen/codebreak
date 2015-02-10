using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
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
        private Dictionary<int, Dictionary<long, List<SpellBookEntryDAO>>> m_spellBookEntriesByOwner;

        /// <summary>
        /// 
        /// </summary>
        public SpellBookEntryRepository()
        {
            m_spellBookEntriesByOwner = new Dictionary<int, Dictionary<long, List<SpellBookEntryDAO>>>();
            m_spellBookEntriesByOwner.Add((int)EntityTypeEnum.TYPE_CHARACTER, new Dictionary<long, List<SpellBookEntryDAO>>());
            m_spellBookEntriesByOwner.Add((int)EntityTypeEnum.TYPE_MONSTER_FIGHTER, new Dictionary<long, List<SpellBookEntryDAO>>());
            m_spellBookEntriesByOwner.Add((int)EntityTypeEnum.TYPE_TAX_COLLECTOR, new Dictionary<long, List<SpellBookEntryDAO>>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spellBookEntry"></param>
        public override void OnObjectAdded(SpellBookEntryDAO spellBookEntry)
        {
            if (!m_spellBookEntriesByOwner[spellBookEntry.OwnerType].ContainsKey(spellBookEntry.OwnerId))
                m_spellBookEntriesByOwner[spellBookEntry.OwnerType].Add(spellBookEntry.OwnerId, new List<SpellBookEntryDAO>());
            m_spellBookEntriesByOwner[spellBookEntry.OwnerType][spellBookEntry.OwnerId].Add(spellBookEntry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spellBookEntry"></param>
        public override void OnObjectRemoved(SpellBookEntryDAO spellBookEntry)
        {
            m_spellBookEntriesByOwner[spellBookEntry.OwnerType][spellBookEntry.OwnerId].Remove(spellBookEntry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerType"></param>
        /// <param name="ownerId"></param>
        public void RemoveAll(int ownerType, long ownerId)
        {
            if (m_spellBookEntriesByOwner[ownerType].ContainsKey(ownerId))
                m_spellBookEntriesByOwner[ownerType].Remove(ownerId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerType"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public IEnumerable<SpellBookEntryDAO> GetSpellEntries(int ownerType, long ownerId)
        {
            if (m_spellBookEntriesByOwner[ownerType].ContainsKey(ownerId))
                return m_spellBookEntriesByOwner[ownerType][ownerId];
            return base.LoadMultiple("OwnerType = @OwnerType AND OwnerId = @OwnerId", new { OwnerType = ownerType, OwnerId = ownerId });
        }
    }
}
