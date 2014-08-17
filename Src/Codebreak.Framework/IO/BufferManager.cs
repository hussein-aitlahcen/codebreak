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
        private byte[] _bufferBlock;
        private int _bufferSize;
        private ConcurrentStack<int> _freeOffset;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bufferSize"></param>
        public BufferManager(int bufferSize, int chunkCount)
        {
            _bufferSize = bufferSize;
            _freeOffset = new ConcurrentStack<int>();
            _bufferBlock = new byte[bufferSize * chunkCount];
            for (int i = 0; i < chunkCount; i++)
            {
                _freeOffset.Push(bufferSize * i);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saea"></param>
        public void SetBuffer(IBufferHandler bufferHandler)
        {
            int offset = -1;
            if (_freeOffset.TryPop(out offset))
            {
                bufferHandler.SetBuffer(_bufferBlock, offset, _bufferSize);
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
            _freeOffset.Push(bufferHandler.Offset);
            bufferHandler.SetBuffer(null, 0, 0);
        }

        public void Dispose()
        {
            _bufferBlock = null;
            _freeOffset.Clear();
        }
    }
}
