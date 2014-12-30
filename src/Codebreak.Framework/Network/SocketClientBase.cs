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
        /// <summary>
        /// 
        /// </summary>
        public event Action OnConnectedEvent;
        public event Action OnDisconnectedEvent;

        /// <summary>
        /// 
        /// </summary>
        private const int BUFF_SIZE = 1024;
        private Socket _socket;
        private SocketAsyncEventArgs m_connectSaea;
        private BufferManager m_bufferManager;
        private ObjectPool<SocketAsyncEventArgs> m_saeaSendPool;
        private ObjectPool<PoolableSocketAsyncEventArgs> m_saeaRecvPool;

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
            m_bufferManager = new BufferManager(1024, 1000);
            m_saeaRecvPool = new ObjectPool<PoolableSocketAsyncEventArgs>(() => new PoolableSocketAsyncEventArgs(m_bufferManager));
            m_saeaSendPool = new ObjectPool<SocketAsyncEventArgs>(() => new SocketAsyncEventArgs());
            m_connectSaea = new SocketAsyncEventArgs();
            m_connectSaea.Completed += IOCompleted;

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
			_socket.Blocking = false;

            m_connectSaea = new SocketAsyncEventArgs();
            m_connectSaea.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
            m_connectSaea.Completed += IOCompleted;

            if (!_socket.ConnectAsync(m_connectSaea))
                ProcessConnected();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void Send(byte[] data)
        {
            if (!_socket.Connected)
                return;

            SocketAsyncEventArgs saea = m_saeaSendPool.Pop();
            saea.Completed += IOCompleted;
            saea.SetBuffer(data, 0, data.Length);

            if (!_socket.SendAsync(saea))
                ProcessSent(saea);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="saea"></param>
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

        /// <summary>
        /// 
        /// </summary>
        private void ProcessConnected()
        {
            m_connectSaea.Completed -= IOCompleted;
            if (m_connectSaea.SocketError == SocketError.Success && _socket.Connected)
            {
                SocketAsyncEventArgs saea = m_saeaRecvPool.Pop();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saea"></param>
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
                m_saeaRecvPool.Push((PoolableSocketAsyncEventArgs)saea);

                ProcessDisconnect(saea);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saea"></param>
        private void ProcessDisconnect(SocketAsyncEventArgs saea)
        {
            saea.Completed -= IOCompleted;

			try
			{
            	_socket.Shutdown(SocketShutdown.Both);
			}
			catch(Exception) 
			{
			}

            if (_socket.Connected)
                _socket.Disconnect(false);

            _socket.Close();

            OnDisconnectedEvent();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Disconnect()
        {
            if (!_socket.Connected)
                return;

            var saea = m_saeaSendPool.Pop();
            saea.Completed += IOCompleted;

            if (!_socket.DisconnectAsync(saea))
                ProcessDisconnect(saea);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saea"></param>
        private void ProcessSent(SocketAsyncEventArgs saea)
        {
            saea.Completed -= IOCompleted;
            saea.SetBuffer(null, 0, 0);
            m_saeaSendPool.Push(saea);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        protected abstract void OnBytesRead(byte[] buffer, int offset, int length);
        
        /// <summary>
        /// 
        /// </summary>
        protected abstract void OnDisconnected();
        
        /// <summary>
        /// 
        /// </summary>
        protected abstract void OnConnected();
    }
}
