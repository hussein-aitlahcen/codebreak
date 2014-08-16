using Codebreak.Framework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.RPCService
{
    public abstract class RPCMessageBuilder
    {
        private Dictionary<int, Func<RPCMessageBase>> _messageById;

        public RPCMessageBuilder()
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
