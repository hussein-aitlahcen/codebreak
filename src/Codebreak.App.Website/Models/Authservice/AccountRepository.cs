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
        private Dictionary<long, Account> m_accountById;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, Account> m_accountByName;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, Account> m_accountByPseudo;

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
        {
            m_accountById = new Dictionary<long, Account>();
            m_accountByName = new Dictionary<string, Account>();
            m_accountByPseudo = new Dictionary<string, Account>();
        }

        /// <summary>
        /// 
        /// </summary>
        public Account GetById(long accountId)
        {
            Account account = null;
            m_accountById.TryGetValue(accountId, out account);
            return account;
        }

        /// <summary>
        /// 
        /// </summary>
        public Account GetByName(string accountName)
        {
            Account account = null;
            if (!m_accountByName.TryGetValue(accountName.ToLower(), out account))
                account = base.Load("upper(name)=upper(@name)", new { name = accountName });
            return account;
        }

        /// <summary>
        /// 
        /// </summary>
        public Account GetByPseudo(string pseudo)
        {
            Account account = null;
            if (!m_accountByPseudo.TryGetValue(pseudo.ToLower(), out account))
                account = base.Load("upper(pseudo)=upper(@pseudo)", new { pseudo = pseudo });
            return account;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnObjectAdded(Account account)
        {
            m_accountById.Add(account.Id, account);
            m_accountByName.Add(account.Name.ToLower(), account);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnObjectRemoved(Account account)
        {
            m_accountById.Remove(account.Id);
            m_accountByName.Remove(account.Name.ToLower());
        }
    }
}