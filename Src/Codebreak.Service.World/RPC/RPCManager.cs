using Codebreak.Framework.Configuration;
using Codebreak.Framework.Generic;
using Codebreak.RPC.Protocol;
using Codebreak.RPC.Service;
using Codebreak.Service.World.Manager;
using Codebreak.WorldService;

namespace Codebreak.Service.World.RPC
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RPCManager : TaskProcessor<RPCManager>
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
        private AuthServiceRPCConnection _rpcConnection;
        
        /// <summary>
        /// 
        /// </summary>
        public AuthState AuthState
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public RPCManager()
            : base("RPCManager")
        {
            AuthState = AuthState.NEGOTIATING;

            _rpcConnection = new AuthServiceRPCConnection();
            _rpcConnection.OnConnectedEvent += () => AddMessage(() => OnConnected());
            _rpcConnection.OnDisconnectedEvent += () => AddMessage(() => OnDisconnected());
            _rpcConnection.OnMessageEvent += (message) => AddMessage(() => OnMessage(message));
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
                    _rpcConnection.Send(message);
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

                    _rpcConnection.Connect(RPCIP, RPCPort);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnConnected()
        {
            Logger.Info("RPCManager connected, sending credentials.");

            AuthState = AuthState.NEGOTIATING;

            _rpcConnection.Send(new AuthentificationMessage(RPCPassword));
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
        public void UpdateState(GameState state)
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
                case (int)MessageId.AUTH_TO_WORLD_CREDENTIALRESULT:
                    if(((AuthentificationResult)message).Result == AuthResult.SUCCESS)
                    {
                        AuthState = AuthState.SUCCESS;

                        Logger.Info("RPCManager authentification success.");

                        _rpcConnection.Send(new GameIdUpdateMessage(WorldConfig.GAME_ID));
                        _rpcConnection.Send(new GameStateUpdateMessage(GameState.ONLINE));
                    }
                    else
                    {
                        AuthState = AuthState.FAILED;

                        Logger.Error("RPCManager authentification failed : wrong credentials.");
                    }
                    break;

                case (int)MessageId.AUTH_TO_WORLD_GAMETICKET:
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
