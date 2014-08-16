using Codebreak.Framework.Database;
using Codebreak.WorldService.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Database.Repository
{
    public sealed class CharacterRepository : Repository<CharacterRepository, CharacterDAO>
    {
        private Dictionary<long, CharacterDAO> _characterById;
        private Dictionary<string, CharacterDAO> _characterByName;
        private Dictionary<long, List<CharacterDAO>> _charactersByAccount;

        public CharacterRepository()
        {
            _characterById = new Dictionary<long, CharacterDAO>();
            _characterByName = new Dictionary<string, CharacterDAO>();
            _charactersByAccount = new Dictionary<long, List<CharacterDAO>>();
        }

        public CharacterDAO GetById(long characterId)
        {
            if (_characterById.ContainsKey(characterId))
                return _characterById[characterId];
            return base.Load("Id=@Id", new { Id = characterId}); ;
        }

        public CharacterDAO GetByName(string name)
        {
            if (_characterByName.ContainsKey(name.ToLower()))
                return _characterByName[name.ToLower()];
            return base.Load("upper(Name)=upper(@Name)", new { Name = name });
        }

        public List<CharacterDAO> GetByAccount(long accountId)
        {
            List<CharacterDAO> characters = new List<CharacterDAO>();
            if (_charactersByAccount.ContainsKey(accountId))
                characters.AddRange(_charactersByAccount[accountId]);
            else
                characters.AddRange(base.LoadMultiple("AccountId=@AccountId", new { AccountId = accountId }));
            return characters;
        }

        public override void OnObjectAdded(CharacterDAO character)
        {
            _characterById.Add(character.Id, character);
            _characterByName.Add(character.Name.ToLower(), character);
            if (!_charactersByAccount.ContainsKey(character.AccountId))
                _charactersByAccount.Add(character.AccountId, new List<CharacterDAO>());
            _charactersByAccount[character.AccountId].Add(character);
        }

        public override void OnObjectRemoved(CharacterDAO character)
        {
            _characterById.Remove(character.Id);
            _characterByName.Remove(character.Name.ToLower()); 
            _charactersByAccount[character.AccountId].Remove(character);
        }
    }
}
