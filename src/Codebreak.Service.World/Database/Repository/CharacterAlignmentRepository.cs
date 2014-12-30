using System.Collections.Generic;
using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;

namespace Codebreak.Service.World.Database.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CharacterAlignmentRepository : Repository<CharacterAlignmentRepository, CharacterAlignmentDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, CharacterAlignmentDAO> m_characterAlignmentById;

        /// <summary>
        /// 
        /// </summary>
        public CharacterAlignmentRepository()
        {
            m_characterAlignmentById = new Dictionary<long, CharacterAlignmentDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterAlignment"></param>
        public override void OnObjectAdded(CharacterAlignmentDAO characterAlignment)
        {
            m_characterAlignmentById.Add(characterAlignment.Id, characterAlignment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterAlignment"></param>
        public override void OnObjectRemoved(CharacterAlignmentDAO characterAlignment)
        {
            m_characterAlignmentById.Remove(characterAlignment.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        public CharacterAlignmentDAO GetById(long characterId)
        {
            if (m_characterAlignmentById.ContainsKey(characterId))
                return m_characterAlignmentById[characterId];
            return base.Load("Id=@CharacterId", new { CharacterId = characterId });
        }
    }
}
