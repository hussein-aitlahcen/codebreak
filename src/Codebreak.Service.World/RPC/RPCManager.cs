using Codebreak.Framework.Configuration;
using Codebreak.Framework.Generic;
using Codebreak.RPC.Protocol;
using Codebreak.RPC.Service;
using Codebreak.Service.World.Manager;

namespace Codebreak.Service.World.RPC
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RPCManager : Updatable
    {
        /// <summary>
        /// 
        /// </summary>
        [Configurable("RPCPassword")]
        public static string RPCPassword = "smarken";

        /// <summary>
        /// 
        /// </summary>
        [Configurable("RPCIP")]
        public static string RPCIP = "127.0.0.1";

        /// <summary>
        /// 
        /// </summary>
        [Configurable("RPCPort")]
        public static int RPCPort = 4321;

        /// <summary>
        /// 
        /// </summary>
        private AuthServiceRPCConnection m_rpcConnection;
        
        /// <summary>
        /// 
        /// </summary>
        public AuthStateEnum AuthState
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public static RPCManager Instance
        {
            get
            {
                return Singleton<RPCManager>.Instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public RPCManager()
        {
            AuthState = AuthStateEnum.NEGOTIATING;
            
            m_rpcConnection = new AuthServiceRPCConnection();
            m_rpcConnection.OnConnectedEvent += () =>
                {
                    AddMessage(() => OnConnected());
                };
            m_rpcConnection.OnDisconnectedEvent += () =>
                {
                    AddMessage(() => OnDisconnected());
                };
            m_rpcConnection.OnMessageEvent += (message) =>
                {
                    AddMessage(() => OnMessage(message));
                };
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            Logger.Info("RPCManager initializing...");

            Connect();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Send(RPCMessageBase message)
        {
            AddMessage(() =>
                {
                    m_rpcConnection.Send(message);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        private void Connect()
        {
            AddMessage(() => 
            {
                Logger.Info("RPCManager connecting...");

                m_rpcConnection.Connect(RPCIP, RPCPort);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnConnected()
        {
            Logger.Info("RPCManager connected, sending credentials.");

            AuthState = AuthStateEnum.NEGOTIATING;

            m_rpcConnection.Send(new AuthentificationMessage(RPCPassword));
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDisconnected()
        {
            Logger.Warn("RPCManager disconnected.");

            Connect();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void UpdateState(GameStateEnum state)
        {
            Send(new GameStateUpdateMessage(state));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void AccountDisconnected(long id)
        {
            Send(new GameAccountDisconnected(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void OnMessage(RPCMessageBase message)
        {
            switch(message.Id)
            {
                case (int)MessageIdEnum.AUTH_TO_WORLD_CREDENTIALRESULT:
                    if(((AuthentificationResult)message).Result == AuthResultEnum.SUCCESS)
                    {
                        AuthState = AuthStateEnum.SUCCESS;
                        Logger.Info("RPCManager authentification success.");
                        Send(new GameIdUpdateMessage(WorldConfig.GAME_ID));
                        Send(new GameStateUpdateMessage(GameStateEnum.ONLINE));
                    }
                    else
                    {
                        AuthState = AuthStateEnum.FAILED;
                        Logger.Error("RPCManager authentification failed : wrong credentials.");
                    }
                    break;

                case (int)MessageIdEnum.AUTH_TO_WORLD_GAMETICKET:
                    var ticketMessage = (GameTicketMessage)message;
                    AccountManager.Instance.AddTicket
                        (
                            ticketMessage.AccountId,
                            ticketMessage.Name,
                            ticketMessage.Power,
                            ticketMessage.RemainingSubscription,
                            ticketMessage.LastConnectionDate,
                            ticketMessage.LastConnectionIP,
                            ticketMessage.Ticket
                        );
                    break;
            }
        }       
    }
}
