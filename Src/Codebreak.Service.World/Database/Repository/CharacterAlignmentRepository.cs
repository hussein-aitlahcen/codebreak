using System.Collections.Generic;
using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;

namespace Codebreak.Service.World.Database.Repository
{
    public sealed class CharacterAlignmentRepository : Repository<CharacterAlignmentRepository, CharacterAlignmentDAO>
    {
        private Dictionary<long, CharacterAlignmentDAO> _characterAlignmentById;

        public CharacterAlignmentRepository()
        {
            _characterAlignmentById = new Dictionary<long, CharacterAlignmentDAO>();
        }

        public override void OnObjectAdded(CharacterAlignmentDAO characterAlignment)
        {
            _characterAlignmentById.Add(characterAlignment.Id, characterAlignment);
        }

        public override void OnObjectRemoved(CharacterAlignmentDAO characterAlignment)
        {
            _characterAlignmentById.Remove(characterAlignment.Id);
        }

        public CharacterAlignmentDAO GetById(long characterId)
        {
            if (_characterAlignmentById.ContainsKey(characterId))
                return _characterAlignmentById[characterId];
            return base.Load("Id=@CharacterId", new { CharacterId = characterId });
        }
    }
}
