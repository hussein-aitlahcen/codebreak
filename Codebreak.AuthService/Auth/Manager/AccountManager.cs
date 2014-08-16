using Codebreak.Framework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.AuthService.Auth.Manager
{
    public sealed class AccountManager : Singleton<AccountManager>
    {
        private Dictionary<long, AuthClient> _clientByAccount;
        private List<long> _clientConnected;

        public AccountManager()
        {
            _clientByAccount = new Dictionary<long, AuthClient>();
            _clientConnected = new List<long>();
        }

        public bool IsConnected(long accountId)
        {
            return _clientByAccount.ContainsKey(accountId) || _clientConnected.Contains(accountId);
        }

        public void ClientAuthentified(AuthClient client)
        {
            _clientByAccount.Add(client.Account.Id, client);
            _clientConnected.Add(client.Account.Id);
        }

        public void GameAccountDisconnect(long accountId)
        {
            _clientConnected.Remove(accountId);
        }

        public void ClientDisconnected(AuthClient client)
        {
            if (client.Account != null)
            {
                if (client.Ticket == null)
                {
                    _clientConnected.Remove(client.Account.Id);
                }
                _clientByAccount.Remove(client.Account.Id);
            }
        }
    }
}
