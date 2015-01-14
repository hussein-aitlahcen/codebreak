using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codebreak.Framework.Configuration;
using Codebreak.Framework.Configuration.Providers;
using Codebreak.Framework.Network;
using Codebreak.Service.Auth.Database;
using Codebreak.Service.Auth.Frames;
using Codebreak.Service.Auth.Network;
using Codebreak.Service.Auth.RPC;
using System;

namespace Codebreak.Service.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuthService : TcpServerBase<AuthService, AuthClient>
    {
        /// <summary>
        /// 
        /// </summary>
        [Configurable("AuthServiceIP")]
        public static string AuthServiceIP = "127.0.0.1";

        /// <summary>
        /// 
        /// </summary>
        [Configurable("AuthServicePort")]
        public static int AuthServicePort = 443;

        /// <summary>
        /// 
        /// </summary>
        [Configurable("AuthMaxClient")]
        public static int AuthMaxClient = 500;

        /// <summary>
        /// 
        /// </summary>
        public ConfigurationManager ConfigurationManager
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configPath"></param>
        public void Start(string configPath)
        {
            ConfigurationManager = new ConfigurationManager();
            ConfigurationManager.RegisterAttributes();
            ConfigurationManager.Add(new JsonConfigurationProvider(configPath), true);
            ConfigurationManager.Load();

            AuthDbMgr.Instance.Initialize();
            AuthRPCService.Instance.Start();

            base.AddTimer(60000, PersistDatabase);

            base.Start(AuthServiceIP, AuthServicePort);
        }

        /// <summary>
        /// 
        /// </summary>
        public void PersistDatabase()
        {
            AuthDbMgr.Instance.UpdateAll();
        }

        #region Network

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        protected override void OnClientConnected(AuthClient client)
        {
            Logger.Debug("Connected : " + client.Ip);

            AddMessage(() =>
            {
                if (base.Clients.Count() >= AuthMaxClient)
                {
                    client.Send(AuthMessage.SERVER_BUSY());
                    client.Disconnect();
                }
                else
                {
                    client.FrameManager.AddFrame(VersionFrame.Instance);
                    client.AuthKey = Util.AuthKeyPool.Pop();
                    client.Send(AuthMessage.HELLO_CONNECT(client.AuthKey));
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        protected override void OnClientDisconnected(AuthClient client)
        {
            AddMessage(() =>
                {
                    Logger.Debug("Disconnected : " + client.Ip);

                    if(client.AuthKey != null)
                    {
                        Util.AuthKeyPool.Push(client.AuthKey);
                    }

                    AuthService.Instance.ClientDisconnected(client);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        protected override void OnDataReceived(AuthClient client, byte[] buffer, int offset, int count)
        {
            foreach (var message in client.Receive(buffer, offset, count))
            {
                AddMessage(() =>
                    {       
                        Logger.Debug("Client : " + message);

                        client.FrameManager.ProcessMessage(message);
                    });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SendToAll(string message)
        {
            base.SendToAll(Encoding.UTF8.GetBytes(message + (char)0x00));
        }

        #endregion

        #region Authentication

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, AuthClient>  m_clientByAccount = new Dictionary<long, AuthClient>();
        
        /// <summary>
        /// 
        /// </summary>
        private List<long> m_clientConnected = new List<long>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public bool IsConnected(long accountId)
        {
            return m_clientByAccount.ContainsKey(accountId) || m_clientConnected.Contains(accountId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void ClientAuthentified(AuthClient client)
        {
            m_clientByAccount.Add(client.Account.Id, client);
            m_clientConnected.Add(client.Account.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        public void GameAccountDisconnect(long accountId)
        {
            m_clientConnected.Remove(accountId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        public void GameAccountConnected(List<long> accounts)
        {
            m_clientConnected.AddRange(accounts);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void ClientDisconnected(AuthClient client)
        {
            if (client.Account != null)
            {
                if (client.Ticket == null)
                {
                    m_clientConnected.Remove(client.Account.Id);
                }
                m_clientByAccount.Remove(client.Account.Id);
            }
        }

        #endregion

        #region World Management

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, AuthRPCServiceClient> m_worldById = new Dictionary<int, AuthRPCServiceClient>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldId"></param>
        /// <returns></returns>
        public AuthRPCServiceClient GetById(int worldId)
        {
            if (m_worldById.ContainsKey(worldId))
                return m_worldById[worldId];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldId"></param>
        /// <param name="client"></param>
        public void RegisterWorld(int worldId, AuthRPCServiceClient client)
        {
            AddMessage(() =>
            {
                if (!m_worldById.ContainsKey(worldId))
                    m_worldById.Add(worldId, client);
            });

            RefreshWorldList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldId"></param>
        public void DeleteWorld(int worldId)
        {
            AddMessage(() =>
            {
                if (m_worldById.ContainsKey(worldId))
                    m_worldById.Remove(worldId);
            });

            RefreshWorldList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void SendWorldList(AuthClient client)
        {
            AddMessage(() => client.Send(AuthMessage.WORLD_HOST_LIST(m_worldById.Values)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void SendWorldCharacterList(AuthClient client)
        {
            AddMessage(() => client.Send(AuthMessage.WORLD_CHARACTER_LIST(m_worldById.Values)));
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshWorldList()
        {
            AddMessage(() => AuthService.Instance.SendToAll(AuthMessage.WORLD_HOST_LIST(m_worldById.Values)));
        }
        #endregion
    }
}
