using System.Collections.Generic;
using Codebreak.Framework.Database;
using Codebreak.Service.Auth.Database.Structure;

namespace Codebreak.Service.Auth.Database.Repository
{
    public sealed class AccountRepository : Repository<AccountRepository, AccountDAO>
    {
        private Dictionary<long, AccountDAO> _accountById;
        private Dictionary<string, AccountDAO> _accountByName;

        public AccountRepository()
        {
            _accountById = new Dictionary<long, AccountDAO>();
            _accountByName = new Dictionary<string, AccountDAO>();
        }

        public AccountDAO GetById(long accountId)
        {
            AccountDAO account = null;
            _accountById.TryGetValue(accountId, out account);
            return account;
        }

        public AccountDAO GetByName(string accountName)
        {
            AccountDAO account = null;
            if(!_accountByName.TryGetValue(accountName.ToLower(), out account))            
                account = Load("upper(name)=upper(@name)", new { name = accountName });            
            return account;
        }

        public override void OnObjectAdded(AccountDAO account)
        {
            _accountById.Add(account.Id, account);
            _accountByName.Add(account.Name.ToLower(), account);
        }

        public override void OnObjectRemoved(AccountDAO account)
        {
            _accountById.Remove(account.Id);
            _accountByName.Remove(account.Name.ToLower());
        }
    }
}
