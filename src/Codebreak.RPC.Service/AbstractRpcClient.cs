using System.Collections.Generic;
using Codebreak.Framework.IO;
using Codebreak.Framework.Network;

namespace Codebreak.RPC.Service
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    public abstract class AbstractRpcClient<TClient> : AbstractTcpClient<TClient>
        where TClient : AbstractRpcClient<TClient>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public RpcMessageBuilder MessageBuilder
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
        protected AbstractRpcClient()
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
        public IEnumerable<AbstractRcpMessage> GetMessages(byte[] buffer, int offset, int length)
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
        public void Send(AbstractRcpMessage message)
        {
            message.Serialize();
            base.Send(message.Data);
        }
    }
}
