using System;
using System.Collections.Generic;

namespace Codebreak.RPC.Service
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RpcMessageBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<int, Func<AbstractRcpMessage>> m_messageById;

        /// <summary>
        /// 
        /// </summary>
        protected RpcMessageBuilder()
        {
            m_messageById = new Dictionary<int, Func<AbstractRcpMessage>>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageId"></param>
        public void Register<T>(int messageId)
            where T : AbstractRcpMessage, new()
        {
            m_messageById.Add(messageId, () => new T());
        }
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public AbstractRcpMessage BuildMessage(int messageId, byte[] data)
        {
            if(!m_messageById.ContainsKey(messageId))            
                throw new NotImplementedException(string.Format("RPCMessageBuilder::BuildMessage unknow messageId : {0}", messageId));
            
            var message = m_messageById[messageId]();
            message.SetData(data);
            message.Deserialize();
            return message;
        }
    }
}
