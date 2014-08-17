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

namespace Codebreak.Service.Auth
{
    public sealed class AuthService : TcpServerBase<AuthService, AuthClient>
    {
        [Configurable("AuthServiceIP")]
        public static string AuthServiceIP = "127.0.0.1";

        [Configurable("AuthServicePort")]
        public static int AuthServicePort = 443;

        [Configurable("AuthMaxClient")]
        public static int AuthMaxClient = 500;

        public ConfigurationManager ConfigurationManager
        {
            get;
            private set;
        }

        public void Start(string configPath)
        {
            ConfigurationManager = new ConfigurationManager();
            ConfigurationManager.RegisterAttributes();
            ConfigurationManager.Add(new JsonConfigurationProvider(configPath), true);

            AuthDbMgr.Instance.Initialize();
            AuthRPCService.Instance.Start();

            base.Start(AuthServiceIP, AuthServicePort);

        }

        #region Network

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

        public void SendToAll(string message)
        {
            base.SendToAll(Encoding.Default.GetBytes(message + (char)0x00));
        }

        #endregion

        #region Authentication

        private Dictionary<long, AuthClient>  _clientByAccount = new Dictionary<long, AuthClient>();
        private List<long> _clientConnected = new List<long>();

        public bool IsConnected(long accountId)
        {
            return _clientByAccount.ContainsKey(accountId) || _clientConnected.Contains(accountId);
        }

        public void ClientAuthentified(AuthClient client)
        {
            _clientByAccount.Add(client.Account.Id, client);
            _clientConnected.Add(client.Account.Id);
        }

        public void GameAccountDisconnect(long accountId)
        {
            _clientConnected.Remove(accountId);
        }

        public void ClientDisconnected(AuthClient client)
        {
            if (client.Account != null)
            {
                if (client.Ticket == null)
                {
                    _clientConnected.Remove(client.Account.Id);
                }
                _clientByAccount.Remove(client.Account.Id);
            }
        }

        #endregion

        #region World Management

        private Dictionary<int, AuthRPCServiceClient> _worldById = new Dictionary<int, AuthRPCServiceClient>();

        public AuthRPCServiceClient GetById(int worldId)
        {
            if (_worldById.ContainsKey(worldId))
                return _worldById[worldId];
            return null;
        }

        public void RegisterWorld(int worldId, AuthRPCServiceClient client)
        {
            Instance.AddMessage(() =>
            {
                if (!_worldById.ContainsKey(worldId))
                    _worldById.Add(worldId, client);
            });

            RefreshWorldList();
        }

        public void DeleteWorld(int worldId)
        {
            Instance.AddMessage(() =>
            {
                if (_worldById.ContainsKey(worldId))
                    _worldById.Remove(worldId);
            });

            RefreshWorldList();
        }

        public void SendWorldList(AuthClient client)
        {
            AddMessage(() => client.Send(AuthMessage.WORLD_HOST_LIST(_worldById.Values)));
        }

        public void SendWorldCharacterList(AuthClient client)
        {
            AddMessage(() => client.Send(AuthMessage.WORLD_CHARACTER_LIST(_worldById.Values)));
        }

        public void RefreshWorldList()
        {
            AddMessage(() => AuthService.Instance.SendToAll(AuthMessage.WORLD_HOST_LIST(_worldById.Values)));
        }
        #endregion
    }
}
