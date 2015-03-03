using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebreak.App.Website.Models.Worldservice
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CharacterRepository : Repository<CharacterRepository, Character>
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
        public CharacterRepository()
            : base(true)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlMgr"></param>
        public override void Initialize(SqlManager sqlMgr)
        {
            base.Initialize(sqlMgr);

            m_nextCharacterId = Math.Max(10000, SqlMgr.QuerySingle<long>("select MAX(Id) from characterinstance") + 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        public Character GetById(long characterId)
        {
            return base.Load("Id=@Id", new { Id = characterId }); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Character GetByName(string name)
        {
            return base.Load("upper(Name)=upper(@Name)", new { Name = name });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IEnumerable<Character> GetByAccount(long accountId)
        {
            return base.LoadMultiple("AccountId=@AccountId", new { AccountId = accountId });
        }
    }
}