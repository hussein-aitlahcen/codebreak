using System.Collections.Generic;
using Codebreak.Framework.IO;
using Codebreak.Framework.Network;

namespace Codebreak.RPC.Service
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    public abstract class RPCClientBase<TClient> : TcpClientBase<TClient>
        where TClient : RPCClientBase<TClient>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public RPCMessageBuilder MessageBuilder
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private int m_messageId;
        private int m_messageLength;
        private BinaryQueue m_messageData;

        /// <summary>
        /// 
        /// </summary>
        protected RPCClientBase()
        {
            m_messageId = -1;
            m_messageLength = -1;
            m_messageData = new BinaryQueue();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public IEnumerable<RPCMessageBase> GetMessages(byte[] buffer, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
            {
                m_messageData.WriteByte(buffer[i]);
            }

            do
            {
                if (m_messageLength == -1 && m_messageData.Count > 3)
                {
                    m_messageLength = m_messageData.ReadInt();
                }
                if (m_messageLength != -1 && m_messageId == -1 && m_messageData.Count > 3)
                {
                    m_messageId = m_messageData.ReadInt();
                }
                if (m_messageLength != -1 && m_messageId != -1 && m_messageData.Count >= m_messageLength)
                {
                    yield return MessageBuilder.BuildMessage(m_messageId, m_messageData.ReadBytes(m_messageLength));

                    m_messageId = -1;
                    m_messageLength = -1;
                }
            }
            while ((m_messageLength == -1 || m_messageId == -1) && m_messageData.Count > 3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Send(RPCMessageBase message)
        {
            message.Serialize();
            base.Send(message.Data);
        }
    }
}
