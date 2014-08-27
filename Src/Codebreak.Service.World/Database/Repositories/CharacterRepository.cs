using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CharacterRepository : Repository<CharacterRepository, CharacterDAO>
    {
        private Dictionary<long, CharacterDAO> _characterById;
        private Dictionary<string, CharacterDAO> _characterByName;
        private Dictionary<long, List<CharacterDAO>> _charactersByAccount;

        /// <summary>
        /// 
        /// </summary>
        public CharacterRepository()
        {
            _characterById = new Dictionary<long, CharacterDAO>();
            _characterByName = new Dictionary<string, CharacterDAO>();
            _charactersByAccount = new Dictionary<long, List<CharacterDAO>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        public CharacterDAO GetById(long characterId)
        {
            if (_characterById.ContainsKey(characterId))
                return _characterById[characterId];
            return base.Load("Id=@Id", new { Id = characterId}); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CharacterDAO GetByName(string name)
        {
            if (_characterByName.ContainsKey(name.ToLower()))
                return _characterByName[name.ToLower()];
            return base.Load("upper(Name)=upper(@Name)", new { Name = name });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public List<CharacterDAO> GetByAccount(long accountId)
        {
            List<CharacterDAO> characters = new List<CharacterDAO>();
            if (_charactersByAccount.ContainsKey(accountId))
                characters.AddRange(_charactersByAccount[accountId]);
            else
                characters.AddRange(base.LoadMultiple("AccountId=@AccountId", new { AccountId = accountId }));
            return characters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public override void OnObjectAdded(CharacterDAO character)
        {
            _characterById.Add(character.Id, character);
            _characterByName.Add(character.Name.ToLower(), character);
            if (!_charactersByAccount.ContainsKey(character.AccountId))
                _charactersByAccount.Add(character.AccountId, new List<CharacterDAO>());
            _charactersByAccount[character.AccountId].Add(character);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public override void OnObjectRemoved(CharacterDAO character)
        {
            _characterById.Remove(character.Id);
            _characterByName.Remove(character.Name.ToLower()); 
            _charactersByAccount[character.AccountId].Remove(character);
        }
    }
}
