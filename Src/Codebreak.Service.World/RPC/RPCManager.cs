using Codebreak.Framework.Configuration;
using Codebreak.Framework.Generic;
using Codebreak.RPC.Protocol;
using Codebreak.RPC.Service;
using Codebreak.Service.World.Manager;
using Codebreak.WorldService;

namespace Codebreak.Service.World.RPC
{
    public sealed class RPCManager : TaskProcessor<RPCManager>
    {
        [Configurable("RPCPassword")]
        public static string RPCPassword = "smarken";

        [Configurable("RPCIP")]
        public static string RPCIP = "25.214.133.179";

        [Configurable("RPCPort")]
        public static int RPCPort = 4321;

        private AuthServiceRPCConnection _rpcConnection;
        
        public AuthState AuthState
        {
            get;
            private set;
        }

        public RPCManager()
            : base("RPCManager")
        {
            AuthState = AuthState.NEGOTIATING;

            _rpcConnection = new AuthServiceRPCConnection();
            _rpcConnection.OnConnectedEvent += () => AddMessage(() => OnConnected());
            _rpcConnection.OnDisconnectedEvent += () => AddMessage(() => OnDisconnected());
            _rpcConnection.OnMessageEvent += (message) => AddMessage(() => OnMessage(message));
        }

        public void Initialize()
        {
            Logger.Info("RPCManager initializing...");

            Connect();
        }
        
        public void Send(RPCMessageBase message)
        {
            _rpcConnection.Send(message);
        }

        private void Connect()
        {
            Logger.Info("RPCManager connecting...");

            _rpcConnection.Connect(RPCIP, RPCPort);
        }

        private void OnConnected()
        {
            Logger.Info("RPCManager connected, sending credentials.");

            AuthState = AuthState.NEGOTIATING;

            _rpcConnection.Send(new AuthentificationMessage(RPCPassword));
        }

        private void OnDisconnected()
        {
            Logger.Warn("RPCManager disconnected.");

            AddMessage(() => Connect());
        }

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
