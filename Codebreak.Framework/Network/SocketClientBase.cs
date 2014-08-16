using Codebreak.Framework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Network
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SocketClientBase
    {
        public event Action OnConnectedEvent;
        public event Action OnDisconnectedEvent;

        private const int BUFF_SIZE = 1024;
        private Socket _socket;
        private SocketAsyncEventArgs _connectSaea;
        private BufferManager _bufferManager;
        private ObjectPool<SocketAsyncEventArgs> _saeaSendPool;
        private ObjectPool<PoolableSocketAsyncEventArgs> _saeaRecvPool;

        /// <summary>
        /// 
        /// </summary>
        public bool Connected
        {
            get
            {
                return _socket != null && _socket.Connected;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SocketClientBase()
        {
            _bufferManager = new BufferManager(1024, 1000);
            _saeaRecvPool = new ObjectPool<PoolableSocketAsyncEventArgs>(() => new PoolableSocketAsyncEventArgs(_bufferManager));
            _saeaSendPool = new ObjectPool<SocketAsyncEventArgs>(() => new SocketAsyncEventArgs());
            _connectSaea = new SocketAsyncEventArgs();
            _connectSaea.Completed += IOCompleted;

            OnConnectedEvent += OnConnected;
            OnDisconnectedEvent += OnDisconnected;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void Connect(String host, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.NoDelay = true;

            _connectSaea = new SocketAsyncEventArgs();
            _connectSaea.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
            _connectSaea.Completed += IOCompleted;

            if (!_socket.ConnectAsync(_connectSaea))
                ProcessConnected();
        }

        public void Send(byte[] data)
        {
            if (!_socket.Connected)
                return;

            SocketAsyncEventArgs saea = _saeaSendPool.Pop();
            saea.Completed += IOCompleted;
            saea.SetBuffer(data, 0, data.Length);

            if (!_socket.SendAsync(saea))
                ProcessSent(saea);
        }

        private void IOCompleted(object sender, SocketAsyncEventArgs saea)
        {
            switch(saea.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    ProcessConnected();
                    break;

                case SocketAsyncOperation.Receive:
                    ProcessReceived(saea);
                    break;

                case SocketAsyncOperation.Send:
                    ProcessSent(saea);
                    break;

                case SocketAsyncOperation.Disconnect:
                    ProcessDisconnect(saea);
                    break;
            }
        }

        private void ProcessConnected()
        {
            _connectSaea.Completed -= IOCompleted;
            if (_connectSaea.SocketError == SocketError.Success)
            {
                SocketAsyncEventArgs saea = _saeaRecvPool.Pop();
                saea.Completed += IOCompleted;
                if (!_socket.ReceiveAsync(saea))
                    ProcessReceived(saea);

                OnConnectedEvent();
            }
            else
            {
                OnDisconnectedEvent();
            }
        }

        private void ProcessReceived(SocketAsyncEventArgs saea)
        {
            int bytesRead = saea.BytesTransferred;
            if(bytesRead > 0)
            {
                if (!_socket.ReceiveAsync(saea))
                    ProcessReceived(saea);

                OnBytesRead(saea.Buffer, saea.Offset, bytesRead);
            }
            else
            {
                _saeaRecvPool.Push((PoolableSocketAsyncEventArgs)saea);

                ProcessDisconnect(saea);
            }
        }

        private void ProcessDisconnect(SocketAsyncEventArgs saea)
        {
            saea.Completed -= IOCompleted;

            _socket.Shutdown(SocketShutdown.Both);

            if (_socket.Connected)
                _socket.Disconnect(false);

            _socket.Close();

            OnDisconnectedEvent();
        }

        public void Disconnect()
        {
            if (!_socket.Connected)
                return;

            var saea = _saeaSendPool.Pop();
            saea.Completed += IOCompleted;

            if (!_socket.DisconnectAsync(saea))
                ProcessDisconnect(saea);
        }

        private void ProcessSent(SocketAsyncEventArgs saea)
        {
            saea.Completed -= IOCompleted;
            saea.SetBuffer(null, 0, 0);
            _saeaSendPool.Push(saea);
        }

        protected abstract void OnBytesRead(byte[] buffer, int offset, int length);
        protected abstract void OnDisconnected();
        protected abstract void OnConnected();
    }
}
