using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebreak.App.Website.Models.Authservice
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AccountRepository : Repository<AccountRepository, Account>
    {        
        /// <summary>
        /// 
        /// </summary>
        private long m_nextAccountId;

        /// <summary>
        /// 
        /// </summary>
        public long NextAccountId
        {
            get
            {
                lock (m_syncLock)
                    return m_nextAccountId++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AccountRepository()
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

            m_nextAccountId = SqlMgr.QuerySingle<long>("select MAX(Id) from account") + 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public Account GetById(long accountId)
        {
            return base.Load("id=@Id", new { Id = accountId });
        }

        /// <summary>
        /// 
        /// </summary>
        public Account GetByName(string accountName)
        {
            return base.Load("upper(name)=upper(@name)", new { name = accountName });
        }

        /// <summary>
        /// 
        /// </summary>
        public Account GetByPseudo(string pseudo)
        {
            return base.Load("upper(pseudo)=upper(@pseudo)", new { pseudo = pseudo });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pseudo"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Account Create(string name, string pseudo, string password, string email, string question, string answer)
        {
            var account = new Account()
            {
                Id = NextAccountId,
                Name = name,
                Pseudo = pseudo,
                Password = password,
                Email = email,
                LastConnectionDate = DateTime.Now,
                LastConnectionIP = "0.0.0.0",
                CreationDate = DateTime.Now,
                Power = 0,
                RemainingSubscription = DateTime.Now,
                Question = question,
                Response = answer,
                Banned = false,
            };

            if (base.Insert(account))            
                return account;
            
            return null;
        }
    }
}