using Codebreak.Framework.Generic;
using Codebreak.RPCMessage;
using Codebreak.WorldService.RPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AccountTicket
    {
        public long Id;
        public string Name;
        public int Power;
        public DateTime RemainingSubscription;
        public DateTime LastConnectionTime;
        public string LastConnectionIP;
        public string Ticket;
        public long Time;
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class AccountManager : Singleton<AccountManager>
    {

        private Dictionary<long, WorldClient> _clientByAccount;
        private Dictionary<string, AccountTicket> _accountByTicket;
        public const int TICKET_TIMEOUT = 5000; // 5 sec

        /// <summary>
        /// 
        /// </summary>
        public AccountManager()
        {
            _accountByTicket = new Dictionary<string, AccountTicket>();
            _clientByAccount = new Dictionary<long, WorldClient>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            WorldService.Instance.AddTimer(1000, UpdateTickets);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="name"></param>
        /// <param name="power"></param>
        /// <param name="remainingSub"></param>
        /// <param name="lastConnection"></param>
        /// <param name="lastIp"></param>
        /// <param name="ticket"></param>
        public void AddTicket(long accountId, string name, int power, long remainingSub, long lastConnection, string lastIp, string ticket)
        {
            Logger.Info("GameTicket : account=" + name + " ticket=" + ticket);
            WorldService.Instance.AddMessage(() => _accountByTicket.Add(ticket, new AccountTicket()
            { 
                Id = accountId,
                Name = name,
                RemainingSubscription = new DateTime(remainingSub),
                Power = power,
                LastConnectionTime = new DateTime(lastConnection),
                LastConnectionIP = lastIp,
                Ticket = ticket, 
                Time = WorldService.Instance.LastUpdate
            }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public AccountTicket GetAccountTicket(string ticket)
        {
            AccountTicket account = null;
            if(_accountByTicket.ContainsKey(ticket))
            {
                account = _accountByTicket[ticket];
                _accountByTicket.Remove(ticket);
            }
            return account;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void ClientAuthentified(WorldClient client)
        {
            _clientByAccount.Add(client.Account.Id, client);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void ClientDisconnected(WorldClient client)
        {
            if(client.Account != null)
            {
                _clientByAccount.Remove(client.Account.Id);

                RPCManager.Instance.AddMessage(() =>
                {
                    RPCManager.Instance.Send(new GameAccountDisconnected(client.Account.Id));
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateTickets()
        {
            if (_accountByTicket.Count > 0)
            {
                for (int i = _accountByTicket.Count - 1; i > -1; i--)
                {
                    var value = _accountByTicket.ElementAt(i).Value;
                    var elapsedTime = WorldService.Instance.LastUpdate - value.Time;

                    if (elapsedTime >= TICKET_TIMEOUT)
                    {
                        Logger.Debug("Ticket timed out : " + value.Ticket);
                        _accountByTicket.Remove(value.Ticket);
                        Logger.Debug("Ticket count : " + _accountByTicket.Count);

                        RPCManager.Instance.AddMessage(() =>
                        {
                            RPCManager.Instance.Send(new GameAccountDisconnected(value.Id));
                        });
                    }
                }
            }
        }
    }
}
