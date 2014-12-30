using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace Codebreak.Framework.Network
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BufferManager : IDisposable
    {
        private byte[] m_bufferBlock;
        private int m_bufferSize;
        private ConcurrentStack<int> m_freeOffset;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bufferSize"></param>
        public BufferManager(int bufferSize, int chunkCount)
        {
            m_bufferSize = bufferSize;
            m_freeOffset = new ConcurrentStack<int>();
            m_bufferBlock = new byte[bufferSize * chunkCount];
            for (int i = 0; i < chunkCount; i++)            
                m_freeOffset.Push(bufferSize * i);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saea"></param>
        public void SetBuffer(IBufferHandler bufferHandler)
        {
            int offset = -1;
            if (m_freeOffset.TryPop(out offset))
            {
                bufferHandler.SetBuffer(m_bufferBlock, offset, m_bufferSize);
            }
            else
            {
                throw new InvalidOperationException("No more free offset on this BufferManager.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bufferHandler"></param>
        public void FreeBuffer(IBufferHandler bufferHandler)
        {
            m_freeOffset.Push(bufferHandler.Offset);
            bufferHandler.SetBuffer(null, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            m_bufferBlock = null;
            m_freeOffset.Clear();
        }
    }
}
