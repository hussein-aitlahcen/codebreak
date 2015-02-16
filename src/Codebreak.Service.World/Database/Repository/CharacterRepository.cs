using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Manager;
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
    public sealed class CharacterRepository : Repository<CharacterRepository, CharacterDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        public long NextCharacterId
        {
            get
            {
                lock (m_syncLock)
                    return m_nextCharacterId++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private long m_nextCharacterId;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, CharacterDAO> m_characterById;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, CharacterDAO> m_characterByName;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, List<CharacterDAO>> m_charactersByAccount;

        /// <summary>
        /// 
        /// </summary>
        public CharacterRepository()
        {
            m_characterById = new Dictionary<long, CharacterDAO>();
            m_characterByName = new Dictionary<string, CharacterDAO>();
            m_charactersByAccount = new Dictionary<long, List<CharacterDAO>>();
            m_nextCharacterId = 10000;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        public CharacterDAO GetById(long characterId)
        {
            if (m_characterById.ContainsKey(characterId))
                return m_characterById[characterId];
            return base.Load("Id=@Id", new { Id = characterId}); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CharacterDAO GetByName(string name)
        {
            if (m_characterByName.ContainsKey(name.ToLower()))
                return m_characterByName[name.ToLower()];
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
            if (m_charactersByAccount.ContainsKey(accountId))
                characters.AddRange(m_charactersByAccount[accountId]);
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
            m_characterById.Add(character.Id, character);
            m_characterByName.Add(character.Name.ToLower(), character);

            if (!m_charactersByAccount.ContainsKey(character.AccountId))
                m_charactersByAccount.Add(character.AccountId, new List<CharacterDAO>());
            m_charactersByAccount[character.AccountId].Add(character);

            if (character.Id >= m_nextCharacterId)
                m_nextCharacterId = character.Id + 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public override void OnObjectRemoved(CharacterDAO character)
        {
            m_characterById.Remove(character.Id);
            m_characterByName.Remove(character.Name.ToLower()); 
            m_charactersByAccount[character.AccountId].Remove(character);
        }
    }
}
