using Codebreak.Framework.Generic;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Codebreak.Framework.Network
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TServer"></typeparam>
    /// <typeparam name="TClient"></typeparam>
    public abstract class AbstractTcpServer<TServer, TClient> : TaskProcessor<TServer>, IServer<TClient>
        where TServer : AbstractTcpServer<TServer, TClient>, new()
        where TClient : AbstractTcpClient<TClient>, new()
    {
        public const int MAX_CLIENT = 10000;

        /// <summary>
        /// 
        /// </summary>
        private readonly Socket m_socket;
        private readonly ObjectPool<SocketAsyncEventArgs> m_sendPool;
        private readonly ObjectPool<SocketAsyncEventArgs> m_recvPool;
        private readonly BufferManager m_bufferManager;
        private readonly ConcurrentStack<int> m_freeId;
        private readonly ConcurrentDictionary<int, TClient> m_clients;

        /// <summary>
        /// 
        /// </summary>
        public string Host
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Port
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int BackLog
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<TClient> Clients => m_clients.Values;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxClient"></param>
        protected AbstractTcpServer(int maxClient = MAX_CLIENT)
            : base(typeof(TServer).Name)
        {
            m_bufferManager = new BufferManager(1024, 20000);
            m_sendPool = new ObjectPool<SocketAsyncEventArgs>(CreateSendSaea, 10000);
            m_recvPool = new ObjectPool<SocketAsyncEventArgs>(CreateRecvSaea, 10000);
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_clients = new ConcurrentDictionary<int, TClient>();
            m_freeId = new ConcurrentStack<int>();
            for (var i = maxClient; i > 0; i--)
                m_freeId.Push(i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SocketAsyncEventArgs CreateSendSaea()
        {
            var saea = new SocketAsyncEventArgs();
            saea.Completed += IOCompleted;
            return saea;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private PoolableSocketAsyncEventArgs CreateRecvSaea()
        {
            var saea = new PoolableSocketAsyncEventArgs(m_bufferManager);
            saea.Completed += IOCompleted;
            return saea;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="backLog"></param>
        protected void Start(string host, int port, int backLog = 100)
        {
            Host = host;
            Port = port;
            BackLog = backLog;

            m_socket.NoDelay = true;
            m_socket.Bind(new IPEndPoint(IPAddress.Parse(Host), Port));
            m_socket.Listen(BackLog);

            for(int i = 0; i < BackLog; i++)
            {
                StartAccept(null);
            }

            Logger.Info(GetType().Name + " listening on " + host + ":" + port);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <param name="arg"></param>
        private void AsyncSafe(Func<SocketAsyncEventArgs, bool> func, SocketAsyncEventArgs arg)
        {
            if (!func(arg))
                IOCompleted(this, arg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="saea"></param>
        private void IOCompleted(object sender, SocketAsyncEventArgs saea)
        {
            switch (saea.LastOperation)
            {
                case SocketAsyncOperation.Accept: ProcessAccepted(saea); break;
                case SocketAsyncOperation.Receive: ProcessReceived(saea); break;
                case SocketAsyncOperation.Send: ProcessSent(saea); break;
                case SocketAsyncOperation.Disconnect: ProcessDisconnected(saea); break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saea"></param>
        private void ProcessDisconnected(SocketAsyncEventArgs saea)
        {
            m_recvPool.Push(saea);
            Disconnect((TClient)saea.UserToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saea"></param>
        private void StartAccept(SocketAsyncEventArgs saea)
        {
            if(saea == null)
            {
                saea = new SocketAsyncEventArgs();
                saea.Completed += IOCompleted;
            }
            else
            {
                saea.AcceptSocket = null;
            }

            AsyncSafe(m_socket.AcceptAsync, saea);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saea"></param>
        /// <param name="client"></param>
        private void StartReceive(SocketAsyncEventArgs saea, TClient client)
        {
            if (saea == null)
            {
                saea = m_recvPool.Pop();
                saea.UserToken = client;
                saea.AcceptSocket = client.Socket;
            }

            AsyncSafe(client.Socket.ReceiveAsync, saea);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private bool AddClient(TClient client)
        {
            int clientId = -1;
            if (!m_freeId.TryPop(out clientId))
                return false;
            client.Id = clientId;
            return m_clients.AddOrUpdate(clientId, client, (id, cl) => client) == client;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saea"></param>
        private void ProcessAccepted(SocketAsyncEventArgs saea)
        {
            // get connected socket
            var socket = saea.AcceptSocket;

            StartAccept(saea);

            // create new client
            var client = new TClient
            {
                Socket = socket,
                Ip = ((IPEndPoint) socket.RemoteEndPoint).Address.ToString(),
                Server = this
            };

            // add the client
            if (AddClient(client))
            {
                // start receiving data
                StartReceive(null, client);

                // raise client connection
                OnClientConnected(client);
            }
            else
            {
                // server busy
                client.Socket.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saea"></param>
        private void ProcessReceived(SocketAsyncEventArgs saea)
        {
            // client disconnected
            if(saea.BytesTransferred == 0)
            {
                saea.Completed -= IOCompleted;
                Disconnect(saea);
                return;
            }

            var client = (TClient)saea.UserToken;

            // raise event
            OnDataReceived(client, saea.Buffer, saea.Offset, saea.BytesTransferred);
            
            StartReceive(saea, client);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saea"></param>
        private void ProcessSent(SocketAsyncEventArgs saea)
        {
            saea.SetBuffer(null, 0, 0);
            m_sendPool.Push(saea);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saea"></param>
        public void Disconnect(SocketAsyncEventArgs saea)
        {
            var client = (TClient) saea.UserToken;
            m_recvPool.Push(saea);
            if (client == null)
                return;
            Disconnect(client);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="data"></param>
        public void Send(TClient client, byte[] data)
        {
            if (client == null)
                return;
            
            var saea = m_sendPool.Pop();
            saea.SetBuffer(data, 0, data.Length);
            AsyncSafe(client.Socket.SendAsync, saea);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void Disconnect(TClient client)
        {
            var socket = client.Socket;
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Dispose();
            }
            catch { }

            if (client.Id != -1)
            {
                m_clients.TryRemove(client.Id, out client);
                if (client != null)
                {
                    m_freeId.Push(client.Id);
                    OnClientDisconnected(client);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void SendToAll(byte[] data)
        {
            foreach (var client in m_clients.Values)
                Send(client, data);
        }

        protected abstract void OnClientConnected(TClient client);
        protected abstract void OnClientDisconnected(TClient client);
        protected abstract void OnDataReceived(TClient client, byte[] buffer, int offset, int count);
    }
}
