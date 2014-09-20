using System.Collections.Generic;
using Codebreak.Framework.IO;
using Codebreak.Framework.Network;

namespace Codebreak.RPC.Service
{
    public abstract class RPCClient<TClient> : TcpClientBase<TClient>
        where TClient : RPCClient<TClient>, new()
    {
        public RPCMessageBuilder MessageBuilder
        {
            get;
            set;
        }

        private int _messageId;
        private int _messageLength;
        private BinaryQueue _messageData;

        protected RPCClient()
        {
            _messageId = -1;
            _messageLength = -1;
            _messageData = new BinaryQueue();
        }

        public IEnumerable<RPCMessageBase> GetMessages(byte[] buffer, int offset, int length)
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
                    yield return MessageBuilder.BuildMessage(_messageId, _messageData.ReadBytes(_messageLength));

                    _messageId = -1;
                    _messageLength = -1;
                }
            }
            while ((_messageLength == -1 || _messageId == -1) && _messageData.Count > 3);
        }

        public void Send(RPCMessageBase message)
        {
            message.Serialize();
            base.Send(message.Data);
        }
    }
}
