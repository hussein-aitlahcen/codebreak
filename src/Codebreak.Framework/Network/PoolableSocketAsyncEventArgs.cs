using Codebreak.Framework.Generic;
using Codebreak.Framework.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Network
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PoolableSocketAsyncEventArgs : SocketAsyncEventArgs, IBufferHandler, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private BufferManager m_buffManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bufferManager"></param>
        public PoolableSocketAsyncEventArgs(BufferManager bufferManager)
        {
            m_buffManager = bufferManager;
            m_buffManager.SetBuffer(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public new void Dispose()
        {
            m_buffManager.FreeBuffer(this);
            m_buffManager = null;

            base.Dispose();
        }
    }
}
