using System;
using System.Collections.Generic;

namespace Codebreak.RPC.Service
{
    public abstract class RPCMessageBuilder
    {
        private Dictionary<int, Func<RPCMessageBase>> _messageById;

        protected RPCMessageBuilder()
        {
            _messageById = new Dictionary<int, Func<RPCMessageBase>>();
        }
        
        public void Register<T>(int messageId)
            where T : RPCMessageBase, new()
        {
            _messageById.Add(messageId, () => new T());
        }
                
        public RPCMessageBase BuildMessage(int messageId, byte[] data)
        {
            if(!_messageById.ContainsKey(messageId))
            {
                throw new NotImplementedException(string.Format("RPCMessageBuilder::BuildMessage unknow messageId : {0}", messageId));
            }

            var message = _messageById[messageId]();
            message.SetData(data);
            message.Deserialize();
            return message;
        }
    }
}
