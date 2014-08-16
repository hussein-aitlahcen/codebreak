using Codebreak.AuthService.Auth.Manager;
using Codebreak.Framework.Generic;
using Codebreak.RPCMessage;
using Codebreak.RPCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.AuthService.RPC
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuthServiceRPC : RPCService<AuthServiceRPC, AuthServiceRPCClient, AuthMessageBuilder>
    {
        /// <summary>
        /// 
        /// </summary>
        public AuthServiceRPC()
        {
            base.RegisterHandler((int)MessageId.WORLD_TO_AUTH_CREDENTIAL, HandleAuthentification);
            base.RegisterHandler((int)MessageId.WORLD_TO_AUTH_GAMESTATEUPDATE,  HandleGameStateUpdate);
            base.RegisterHandler((int)MessageId.WORLD_TO_AUTH_GAMEIDUPDATE, HandleGameIdUpdate);
            base.RegisterHandler((int)MessageId.WORLD_TO_AUTH_GAMEACCOUNTDISCONNECTED, HandleGameAccountDisconnected);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        protected override void OnRpcClientConnected(AuthServiceRPCClient client)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        protected override void OnRpcClientDisconnected(AuthServiceRPCClient client)
        {
            if (client.AuthState != AuthState.SUCCESS)
                return;

            if (client.GameId != -1)
                WorldManager.Instance.DeleteWorld(client.GameId);

            Logger.Warn(string.Format("AuthServiceRPC [{0}][{1}] Disconnected", client.Ip, client.GameId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        protected override void OnMessageReceived(AuthServiceRPCClient client, RPCMessageBase message)
        {
            Logger.Debug("AuthServiceRPC " + (MessageId)message.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void HandleAuthentification(AuthServiceRPCClient client, RPCMessageBase message)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void HandleGameIdUpdate(AuthServiceRPCClient client, RPCMessageBase message)
        {
            if (client.AuthState != AuthState.SUCCESS)
                return;

            var gameIdUpdateMessage = (GameIdUpdateMessage)message;

            WorldManager.Instance.RegisterWorld(gameIdUpdateMessage.GameId, client);

            Logger.Info(string.Format("AuthServiceRPC [{0}] GameId updated to [{1}]", client.Ip, gameIdUpdateMessage.GameId));

            client.GameId = gameIdUpdateMessage.GameId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void HandleGameStateUpdate(AuthServiceRPCClient client, RPCMessageBase message)
        {
            if (client.AuthState != AuthState.SUCCESS)
                return;

            var state = ((GameStateUpdateMessage)message).State;

            Logger.Info(string.Format("AuthServiceRPC [{0}][{1}] GameState updated to {2}", client.Ip, client.GameId, state));

            client.GameState = state;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void HandleGameAccountDisconnected(AuthServiceRPCClient client, RPCMessageBase message)
        {
            if (client.AuthState != AuthState.SUCCESS)
                return;

            var accountId = ((GameAccountDisconnected)message).AccountId;
            
            Logger.Info(string.Format("AuthServiceRPC [{0}][{1}] GameAccount disconnected accountId={2}", client.Ip, client.GameId, accountId));

            AuthService.Auth.AuthService.Instance.AddMessage(() =>
                {
                    AccountManager.Instance.GameAccountDisconnect(accountId);
                });
        }
    }
}
