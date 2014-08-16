using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Framework.Network;

namespace Codebreak.RPCService
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TServer"></typeparam>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TMessageBuilder"></typeparam>
    public abstract class RPCService<TServer, TClient, TMessageBuilder> : TcpServerBase<TServer, TClient>
        where TServer : RPCService<TServer, TClient, TMessageBuilder>, new()
        where TClient : RPCClient<TClient>, new()
        where TMessageBuilder : RPCMessageBuilder, new ()
    {
        /// <summary>
        /// 
        /// </summary>
        public RPCMessageBuilder MessageBuilder
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, Action<TClient, RPCMessageBase>> _handlers;

        /// <summary>
        /// 
        /// </summary>
        public RPCService()
        {
            _handlers = new Dictionary<int, Action<TClient, RPCMessageBase>>();

            MessageBuilder = new TMessageBuilder();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="handler"></param>
        public void RegisterHandler(int messageId, Action<TClient, RPCMessageBase> handler)
        {
            if (_handlers.ContainsKey(messageId))
                throw new InvalidOperationException(string.Format("RPCService::RegisterHandler already registered handler for messageId = {0}", messageId));
            else
                _handlers.Add(messageId, handler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void HandleMessage(TClient client, RPCMessageBase message)
        {
            if (!_handlers.ContainsKey(message.Id))
                Logger.Debug(string.Format("RPCService::HandlerMessage unregistered handler for messageId={0}", message.Id));
            else
                // execute in context
                AddMessage(() => _handlers[message.Id](client, message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        protected override void OnClientConnected(TClient client)
        {
            client.MessageBuilder = MessageBuilder;
            
            AddMessage(() => OnRpcClientConnected(client));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        protected override void OnClientDisconnected(TClient client)
        {
            // execute in thread context
            AddMessage(() => OnRpcClientDisconnected(client));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        protected override void OnDataReceived(TClient client, byte[] buffer, int offset, int count)
        {
            foreach (var message in client.GetMessages(buffer, offset, count))
            {
                // execute in context
                AddMessage(() => OnMessageReceived(client, message));

                HandleMessage(client, message);  
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        protected abstract void OnRpcClientConnected(TClient client);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        protected abstract void OnRpcClientDisconnected(TClient client);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        protected abstract void OnMessageReceived(TClient client, RPCMessageBase message);
    }
}
