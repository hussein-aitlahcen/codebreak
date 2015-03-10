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
                base.Removed(m_spellBookEntriesByOwner[ownerType][ownerId].ToArray());
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
            m_spellBookEntriesByOwner[ownerType].Add(ownerId, new List<SpellBookEntryDAO>());
            base.LoadMultiple("OwnerType = @OwnerType AND OwnerId = @OwnerId", new { OwnerType = ownerType, OwnerId = ownerId });
            return m_spellBookEntriesByOwner[ownerType][ownerId];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerType"></param>
        /// <param name="ownerId"></param>
        /// <param name="spellId"></param>
        /// <param name="level"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public SpellBookEntryDAO Create(int ownerType, long ownerId, int spellId, int level, int position)
        {
            var instance = new SpellBookEntryDAO()
            {
                OwnerType = ownerType,
                OwnerId = ownerId,
                SpellId = spellId,
                Level = level,
                Position = position
            };
            base.Created(instance);
            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="breed"></param>
        public void GenerateForBreed(long ownerId, CharacterBreedEnum breed)
        {
            switch (breed)
            {
                case CharacterBreedEnum.BREED_FECA:
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 3, 1, 1);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 6, 1, 2);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 17, 1, 3);
                    break;

                case CharacterBreedEnum.BREED_OSAMODAS:
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 34, 1, 1);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 21, 1, 2);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 23, 1, 3);
                    break;

                case CharacterBreedEnum.BREED_ENUTROF:
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 51, 1, 1);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 43, 1, 2);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 41, 1, 3);
                    break;

                case CharacterBreedEnum.BREED_SRAM:
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 61, 1, 1);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 72, 1, 2);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 65, 1, 3);
                    break;

                case CharacterBreedEnum.BREED_XELOR:
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 82, 1, 1);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 81, 1, 2);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 83, 1, 3);
                    break;

                case CharacterBreedEnum.BREED_ECAFLIP:
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 102, 1, 1);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 103, 1, 2);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 105, 1, 3);
                    break;

                case CharacterBreedEnum.BREED_ENIRIPSA:
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 125, 1, 1);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 128, 1, 2);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 121, 1, 3);
                    break;

                case CharacterBreedEnum.BREED_IOP:
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 143, 1, 1);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 141, 1, 2);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 142, 1, 3);
                    break;

                case CharacterBreedEnum.BREED_CRA:
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 161, 1, 1);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 169, 1, 2);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 164, 1, 3);
                    break;

                case CharacterBreedEnum.BREED_SADIDAS:
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 183, 1, 1);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 200, 1, 2);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 193, 1, 3);
                    break;

                case CharacterBreedEnum.BREED_SACRIEUR:
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 432, 1, 1);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 431, 1, 2);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 434, 1, 3);
                    break;

                case CharacterBreedEnum.BREED_PANDAWA:
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 686, 1, 1);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 692, 1, 2);
                    Create((int)EntityTypeEnum.TYPE_CHARACTER, ownerId, 687, 1, 3);
                    break;
            }
        }

    }
}
