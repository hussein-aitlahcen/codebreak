using System.Collections.Generic;
using Codebreak.Framework.Database;
using Codebreak.Service.Auth.Database.Structure;

namespace Codebreak.Service.Auth.Database.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AccountRepository : Repository<AccountRepository, AccountDAO>
    {        
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, AccountDAO> m_accountById;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, AccountDAO> m_accountByName;
        
        /// <summary>
        /// 
        /// </summary>
        public AccountRepository()
        {
            m_accountById = new Dictionary<long, AccountDAO>();
            m_accountByName = new Dictionary<string, AccountDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        public AccountDAO GetById(long accountId)
        {
            AccountDAO account = null;
            m_accountById.TryGetValue(accountId, out account);
            return account;
        }

        /// <summary>
        /// 
        /// </summary>
        public AccountDAO GetByName(string accountName)
        {
            AccountDAO account = null;
            if(!m_accountByName.TryGetValue(accountName.ToLower(), out account))            
                account = Load("upper(name)=upper(@name)", new { name = accountName });            
            return account;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnObjectAdded(AccountDAO account)
        {
            m_accountById.Add(account.Id, account);
            m_accountByName.Add(account.Name.ToLower(), account);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnObjectRemoved(AccountDAO account)
        {
            m_accountById.Remove(account.Id);
            m_accountByName.Remove(account.Name.ToLower());
        }
    }
}
