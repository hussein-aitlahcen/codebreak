using System;
using System.Collections.Generic;

namespace Codebreak.RPC.Service
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RPCMessageBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, Func<RPCMessageBase>> m_messageById;

        /// <summary>
        /// 
        /// </summary>
        protected RPCMessageBuilder()
        {
            m_messageById = new Dictionary<int, Func<RPCMessageBase>>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageId"></param>
        public void Register<T>(int messageId)
            where T : RPCMessageBase, new()
        {
            m_messageById.Add(messageId, () => new T());
        }
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public RPCMessageBase BuildMessage(int messageId, byte[] data)
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
