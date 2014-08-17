using System;
using Codebreak.Framework.IO;
using Codebreak.Framework.Network;

namespace Codebreak.RPC.Service
{
    public abstract class RPCConnectionBase<TMessageBuilder> : SocketClientBase
        where TMessageBuilder : RPCMessageBuilder, new()
    {
        public event Action<RPCMessageBase> OnMessageEvent;
        public RPCMessageBuilder MessageBuilder
        {
            get;
            private set;
        }

        private int _messageId;
        private int _messageLength;
        private BinaryQueue _messageData;

        protected RPCConnectionBase()
        {
            MessageBuilder = new TMessageBuilder();

            _messageId = -1;
            _messageLength = -1;
            _messageData = new BinaryQueue();

            OnMessageEvent += OnMessage;
        }

        ~RPCConnectionBase()
        {
            OnMessageEvent = null;
        }

        public void Send(RPCMessageBase message)
        {
            message.Serialize();
            base.Send(message.Data);
        }

        protected override void OnBytesRead(byte[] buffer, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
            {
                _messageData.WriteByte(buffer[i]);
            }

            do
            {
                if (_messageLength == -1 && _messageData.Count > 3)
                {
                    _messageLength = _messageData.ReadInt();
                }
                if (_messageLength != -1 && _messageId == -1 && _messageData.Count > 3)
                {
                    _messageId = _messageData.ReadInt();
                }
                if (_messageLength != -1 && _messageId != -1 && _messageData.Count >= _messageLength)
                {
                    var message = MessageBuilder.BuildMessage(_messageId, _messageData.ReadBytes(_messageLength));

                    if (OnMessageEvent != null)
                        OnMessageEvent(message);
                    
                    _messageId = -1;
                    _messageLength = -1;
                }
            }
            while ((_messageLength == -1 || _messageId == -1) && _messageData.Count > 3);
        }

        protected override abstract void OnConnected();
        protected override abstract void OnDisconnected();
        protected abstract void OnMessage(RPCMessageBase message);
    }
}
