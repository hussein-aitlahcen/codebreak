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
    public sealed class PoolableSocketAsyncEventArgs : SocketAsyncEventArgs, IBufferHandler, IDisposable
    {
        private BufferManager _buffManager;

        public PoolableSocketAsyncEventArgs(BufferManager bufferManager)
        {
            _buffManager = bufferManager;
            _buffManager.SetBuffer(this);
        }

        public new void Dispose()
        {
            _buffManager.FreeBuffer(this);
            _buffManager = null;

            base.Dispose();
        }
    }
}
