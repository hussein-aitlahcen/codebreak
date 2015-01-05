using System;
using Codebreak.Framework.Network;
using Codebreak.Service.Auth.Database.Repository;
using Codebreak.Service.Auth.Network;

namespace Codebreak.Service.Auth.Frames
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuthentificationFrame : FrameBase<AuthentificationFrame, AuthClient, string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Action<AuthClient, string> GetHandler(string message)
        {
            if(message != "Af")
                return HandleAuthentification;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void HandleAuthentification(AuthClient client, string message)
        {
            client.FrameManager.RemoveFrame(AuthentificationFrame.Instance);

            var credentials = message.Split('#');

            if(credentials.Length != 2)
            {
                client.Send(AuthMessage.AUTH_FAILED_CREDENTIALS());
                return;
            }

            var account = credentials[0];
            var password = credentials[1].Substring(1);

            AuthService.Instance.AddMessage(() => ProcessAuthentification(client, account, password));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="accountName"></param>
        /// <param name="password"></param>
        private void ProcessAuthentification(AuthClient client, string accountName, string password)
        {
            var account = AccountRepository.Instance.GetByName(accountName);

            if (account == null || Util.CryptPassword(client.AuthKey, account.Password) != password)
            {
                client.Send(AuthMessage.AUTH_FAILED_CREDENTIALS());
                return;
            }

            if(account.Banned)
            {
                client.Send(AuthMessage.AUTH_FAILED_BANNED());
                return;
            }

            if(AuthService.Instance.IsConnected(account.Id))
            {
                client.Send(AuthMessage.AUTH_FAILED_ALREADY_CONNECTED());
                return;
            }
                        
            AuthService.Instance.AddMessage(() =>
                {
                    client.Account = account;

                    AuthService.Instance.ClientAuthentified(client);

                    client.Send(AuthMessage.ACCOUNT_PSEUDO(account.Pseudo));
                    client.Send(AuthMessage.UNKNOW_AC0());

                    AuthService.Instance.SendWorldList(client);

                    client.Send(AuthMessage.ACCOUNT_RIGHT(client.Account.Power));
                    client.Send(AuthMessage.ACCOUNT_SECRET_ANSWER());

                    client.FrameManager.AddFrame(WorldSelectionFrame.Instance);
                });
        }
    }
}
