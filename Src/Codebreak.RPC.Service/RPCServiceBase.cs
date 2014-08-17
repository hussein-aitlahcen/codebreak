using System;
using System.Collections.Generic;
using Codebreak.Framework.Network;

namespace Codebreak.RPC.Service
{
    public abstract class RPCService<TServer, TClient, TMessageBuilder> : TcpServerBase<TServer, TClient>
        where TServer : RPCService<TServer, TClient, TMessageBuilder>, new()
        where TClient : RPCClient<TClient>, new()
        where TMessageBuilder : RPCMessageBuilder, new ()
    {
        public RPCMessageBuilder MessageBuilder
        {
            get;
            private set;
        }

        private Dictionary<int, Action<TClient, RPCMessageBase>> _handlers;

        protected RPCService()
        {
            _handlers = new Dictionary<int, Action<TClient, RPCMessageBase>>();

            MessageBuilder = new TMessageBuilder();
        }


        public void RegisterHandler(int messageId, Action<TClient, RPCMessageBase> handler)
        {
            if (_handlers.ContainsKey(messageId))
                throw new InvalidOperationException(string.Format("RPCService::RegisterHandler already registered handler for messageId = {0}", messageId));
            else
                _handlers.Add(messageId, handler);
        }

        private void HandleMessage(TClient client, RPCMessageBase message)
        {
            if (!_handlers.ContainsKey(message.Id))
                Logger.Debug(string.Format("RPCService::HandlerMessage unregistered handler for messageId={0}", message.Id));
            else
                // execute in context
                AddMessage(() => _handlers[message.Id](client, message));
        }

        protected override void OnClientConnected(TClient client)
        {
            client.MessageBuilder = MessageBuilder;

            AddMessage(() => OnRPCClientConnected(client));
        }

        protected override void OnClientDisconnected(TClient client)
        {
            // execute in thread context
            AddMessage(() => OnRPCClientDisconnected(client));
        }

        protected override void OnDataReceived(TClient client, byte[] buffer, int offset, int count)
        {
            foreach (var message in client.GetMessages(buffer, offset, count))
            {
                // execute in context
                AddMessage(() => OnMessageReceived(client, message));

                HandleMessage(client, message);  
            }
        }

// ReSharper disable once InconsistentNaming
        protected abstract void OnRPCClientConnected(TClient client);

// ReSharper disable once InconsistentNaming
        protected abstract void OnRPCClientDisconnected(TClient client);
        
        protected abstract void OnMessageReceived(TClient client, RPCMessageBase message);
    }
}
