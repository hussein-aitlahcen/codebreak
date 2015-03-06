using Codebreak.App.Website.Controllers;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Codebreak.App.Website.Signalr
{
    /// <summary>
    /// 
    /// </summary>
    public class StatisticHub : Hub
    {
        /// <summary>
        /// Lazy loading context because of his high cost
        /// </summary>
        private readonly static Lazy<IHubContext> ctx = new Lazy<IHubContext>(
          () => GlobalHost.ConnectionManager.GetHubContext<StatisticHub>());

        /// <summary>
        /// Static variable representing the amout of visitors
        /// </summary>
        public readonly static HashSet<string> Visitors = new HashSet<string>();

        /// <summary>
        /// Static variable representing connected users
        /// </summary>
        public readonly static ConnectionMapping<AccountTicket> Users = new ConnectionMapping<AccountTicket>(new AccountComparer());

        /// <summary>
        /// Interlocked increment ensure thread safety
        /// </summary>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnConnected()
        {
            Visitors.Add(Context.ConnectionId);

            if(Context.User.Identity.IsAuthenticated)           
                Users.Add(Context.User as AccountTicket, Context.ConnectionId);

            NotifyPlayersConnected(Context.ConnectionId);
            NotifyVisitorCount();
                                        
            return base.OnConnected();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnReconnected()
        {
            Visitors.Add(Context.ConnectionId);

            if (Context.User.Identity.IsAuthenticated)            
                Users.Add(Context.User as AccountTicket, Context.ConnectionId);

            NotifyPlayersConnected(Context.ConnectionId);
            NotifyVisitorCount();

            return base.OnReconnected();
        }

        /// <summary>
        /// Interlocked decrement ensure thread safety
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            Visitors.Remove(Context.ConnectionId);

            if (Context.User.Identity.IsAuthenticated)            
                Users.Remove(Context.User as AccountTicket, Context.ConnectionId);            

            NotifyVisitorCount();
            
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// Notify all connected clients that there is X clients online
        /// </summary>
        public static void NotifyVisitorCount()
        {
            ctx.Value.Clients.All.notifyVisitorCount(Visitors.Count);
        }

        /// <summary>
        /// Notify all connected clients 
        /// </summary>
        public static void NotifyPlayersConnected(string connectionId)
        {
            ctx.Value.Clients.Client(connectionId).notifyPlayersConnected(Users.Count, string.Join(", ", Users.Keys.Select(ticket => ticket.Account.Pseudo)));
        }
    }
}