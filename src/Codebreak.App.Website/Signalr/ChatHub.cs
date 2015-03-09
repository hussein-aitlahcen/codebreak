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
        public const int CACHE_MESSAGE_COUNT = 15;

        /// <summary>
        /// 
        /// </summary>
        public class ChatMessage {
            public long PlayerId;
            public int Power;
            public string Pseudo;
            public string Content;
            public string Time;
        }

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
        /// Message stack
        /// </summary>
        public readonly static List<ChatMessage> ChatMessages = new List<ChatMessage>();

        /// <summary>
        /// Called by the javascript client
        /// </summary>
        /// <param name="content"></param>
        public void SendMessage(string content)
        {
            NotifyChatMessage(Context.User as AccountTicket, content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnConnected()
        {
            var ticket = Context.User as AccountTicket;
            Users.Add(ticket, Context.ConnectionId);

            if (Users.GetConnections(ticket).Count() == 1)
                NotifyPlayerConnected(Context.ConnectionId, ticket);
            
            NotifyPlayersConnected(Context.ConnectionId);
            NotifyChatMessages(Context.ConnectionId);
                        
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

            if (Users.GetConnections(ticket).Count() == 1)
                NotifyPlayerConnected(Context.ConnectionId, ticket);

            NotifyPlayersConnected(Context.ConnectionId);
            NotifyChatMessages(Context.ConnectionId);
                                    
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

            if (Users.GetConnections(ticket).Count() == 0)
                NotifyPlayerDisconnected(Context.ConnectionId, ticket);

            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionId"></param>
        public static void NotifyPlayersConnected(string connectionId)
        {
            ctx.Value.Clients.Client(connectionId).notifyPlayersConnected(
                    Users.Keys.Select(ticket => new { Id = ticket.Account.Id, Power = ticket.Account.Power, Pseudo = ticket.Account.Pseudo })
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        public static void NotifyPlayerConnected(string exceptConnectionId, AccountTicket ticket)
        {
            ctx.Value.Clients.AllExcept(exceptConnectionId).notifyPlayerConnected(new { Id = ticket.Account.Id, Power = ticket.Account.Power, Pseudo = ticket.Account.Pseudo });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        public static void NotifyPlayerDisconnected(string exceptConnectionId, AccountTicket ticket)
        {
            ctx.Value.Clients.AllExcept(exceptConnectionId).notifyPlayerDisconnected(ticket.Account.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionId"></param>
        public static void NotifyChatMessages(string connectionId)
        {
            lock(ChatMessages)
                ctx.Value.Clients.Client(connectionId).notifyChatMessages(ChatMessages.Take(CACHE_MESSAGE_COUNT));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="content"></param>
        public static void NotifyChatMessage(AccountTicket ticket, string content)
        {
            var message = new ChatMessage() 
            { 
                PlayerId = ticket.Account.Id,
                Pseudo = ticket.Account.Pseudo,
                Power = ticket.Account.Power,
                Content = content, 
                Time = DateTime.Now.ToString("HH:mm:ss") 
            };

            lock (ChatMessages)
            {
                if (ChatMessages.Count > CACHE_MESSAGE_COUNT)
                    ChatMessages.RemoveAt(0);
                ChatMessages.Add(message);
            }
            ctx.Value.Clients.All.notifyChatMessage(message);
        }
    }
}