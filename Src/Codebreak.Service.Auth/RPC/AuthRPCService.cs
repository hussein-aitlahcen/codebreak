using Codebreak.Framework.Configuration;
using Codebreak.RPC.Protocol;
using Codebreak.RPC.Service;

namespace Codebreak.Service.Auth.RPC
{
    public sealed class AuthRPCService : RPCService<AuthRPCService, AuthRPCServiceClient, AuthMessageBuilder>
    {
        [Configurable("RPCServiceIP")]
        public static string RPCServiceIP = "localhost";

        [Configurable("RPCServicePort")]
        public static int RPCServicePort = 4321;

        public AuthRPCService()
        {
            base.RegisterHandler((int)MessageId.WORLD_TO_AUTH_CREDENTIAL, HandleAuthentification);
            base.RegisterHandler((int)MessageId.WORLD_TO_AUTH_GAMESTATEUPDATE,  HandleGameStateUpdate);
            base.RegisterHandler((int)MessageId.WORLD_TO_AUTH_GAMEIDUPDATE, HandleGameIdUpdate);
            base.RegisterHandler((int)MessageId.WORLD_TO_AUTH_GAMEACCOUNTDISCONNECTED, HandleGameAccountDisconnected);
        }

        public new void Start()
        {
            base.Start(RPCServiceIP, RPCServicePort);
        }

        protected override void OnRPCClientConnected(AuthRPCServiceClient client)
        {

        }

        protected override void OnRPCClientDisconnected(AuthRPCServiceClient client)
        {
            if (client.AuthState != AuthState.SUCCESS)
                return;

            if (client.GameId != -1)
                AuthService.Instance.DeleteWorld(client.GameId);

            Logger.Warn(string.Format("AuthServiceRPC [{0}][{1}] Disconnected", client.Ip, client.GameId));
        }

        protected override void OnMessageReceived(AuthRPCServiceClient client, RPCMessageBase message)
        {
            Logger.Debug("AuthServiceRPC " + (MessageId)message.Id);
        }

        private void HandleAuthentification(AuthRPCServiceClient client, RPCMessageBase message)
        {
            if (client.AuthState != AuthState.NEGOTIATING)
                return;

            var result = AuthResult.FAILED;
            var authMessage = (AuthentificationMessage)message;

            if (authMessage.Password == "smarken")
            {
                client.AuthState = AuthState.SUCCESS;
                result = AuthResult.SUCCESS;

                Logger.Info(string.Format("AuthServiceRPC [{0}] Authed sucessfully", client.Ip));
            }
            
            client.Send(new AuthentificationResult(result));                       
        }

        private void HandleGameIdUpdate(AuthRPCServiceClient client, RPCMessageBase message)
        {
            if (client.AuthState != AuthState.SUCCESS)
                return;

            var gameIdUpdateMessage = (GameIdUpdateMessage)message;

            AuthService.Instance.RegisterWorld(gameIdUpdateMessage.GameId, client);

            Logger.Info(string.Format("AuthServiceRPC [{0}] GameId updated to [{1}]", client.Ip, gameIdUpdateMessage.GameId));

            client.GameId = gameIdUpdateMessage.GameId;
        }

        private void HandleGameStateUpdate(AuthRPCServiceClient client, RPCMessageBase message)
        {
            if (client.AuthState != AuthState.SUCCESS)
                return;

            var state = ((GameStateUpdateMessage)message).State;

            Logger.Info(string.Format("AuthServiceRPC [{0}][{1}] GameState updated to {2}", client.Ip, client.GameId, state));

            client.GameState = state;

            AuthService.Instance.RefreshWorldList();
        }

        private void HandleGameAccountDisconnected(AuthRPCServiceClient client, RPCMessageBase message)
        {
            if (client.AuthState != AuthState.SUCCESS)
                return;

            var accountId = ((GameAccountDisconnected)message).AccountId;
            
            Logger.Info(string.Format("AuthServiceRPC [{0}][{1}] GameAccount disconnected accountId={2}", client.Ip, client.GameId, accountId));

            AuthService.Instance.AddMessage(() => AuthService.Instance.GameAccountDisconnect(accountId));
        }
    }
}
