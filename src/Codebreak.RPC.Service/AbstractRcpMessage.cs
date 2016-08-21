using System;
using Codebreak.Framework.IO;

namespace Codebreak.RPC.Service
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractRcpMessage : BinaryQueue
    {
        /// <summary>
        /// 
        /// </summary>
        private byte[] m_cache;
               
        /// <summary>
        /// 
        /// </summary>
        public byte[] Data
        {
            get
            {
                if (m_cache == null)
                {
                    var count = base.Count;
                    var data = base.ReadBytes(count);
                    var idBytes = BitConverter.GetBytes((int)Id);
                    var lengthBytes = BitConverter.GetBytes(count);
                    m_cache = new byte[8 + count];
                    Buffer.BlockCopy(lengthBytes, 0, m_cache, 0, 4);
                    Buffer.BlockCopy(idBytes, 0, m_cache, 4, 4);
                    Buffer.BlockCopy(data, 0, m_cache, 8, count);
                }
                return m_cache;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract int Id
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        protected AbstractRcpMessage()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void SetData(byte[] data)
        {
            base.WriteBytes(data);
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract void Deserialize();

        /// <summary>
        /// 
        /// </summary>
        public abstract void Serialize();
    }
}
