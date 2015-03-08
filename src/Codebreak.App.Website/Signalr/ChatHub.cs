using Codebreak.App.Website.Controllers;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebreak.App.Website.Signalr
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public sealed class ChatHub : Hub
    {
        /// <summary>
        /// Lazy loading context because of his high cost
        /// </summary>
        private readonly static Lazy<IHubContext> ctx = new Lazy<IHubContext>(
          () => GlobalHost.ConnectionManager.GetHubContext<ChatHub>());
        
        /// <summary>
        /// Static variable representing connected users
        /// </summary>
        public readonly static ConnectionMapping<AccountTicket> Users = new ConnectionMapping<AccountTicket>(new AccountComparer());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnConnected()
        {
            var ticket = Context.User as AccountTicket;
            Users.Add(ticket, Context.ConnectionId);

            
            return base.OnConnected();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnReconnected()
        {
            var ticket = Context.User as AccountTicket;
            Users.Add(ticket, Context.ConnectionId);
                                    
            return base.OnReconnected();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var ticket = Context.User as AccountTicket;
            Users.Remove(ticket, Context.ConnectionId);

            if (!Users.Keys.Contains(ticket))
                NotifyPlayerDisconnected(ticket);

            return base.OnDisconnected(stopCalled);
        }

        public static void NotifyPlayersConnected(string connectionId)
        {
            ctx.Value.Clients.Client(connectionId).notifyPlayersConnected(
                    Users.Keys.Select(ticket => new { Id = ticket.Account.Id, Power = ticket.Account.Power, Pseudo = ticket.Account.Pseudo })
                );
        }

        public static void NotifyPlayerConnected(AccountTicket ticket)
        {
            ctx.Value.Clients.All.notifyPlayerConnected(ticket.Account.Id, ticket.Account.Power, ticket.Account.Pseudo);
        }

        public static void NotifyPlayerDisconnected(AccountTicket ticket)
        {
            ctx.Value.Clients.All.notifyPlayerDisconnected(ticket.Account.Id);
        }
    }
}