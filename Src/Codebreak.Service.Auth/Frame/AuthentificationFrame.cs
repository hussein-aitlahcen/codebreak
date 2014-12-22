using System;
using Codebreak.Framework.Network;
using Codebreak.Service.Auth.Database.Repositories;
using Codebreak.Service.Auth.Network;

namespace Codebreak.Service.Auth.Frames
{
    public sealed class AuthentificationFrame : FrameBase<AuthentificationFrame, AuthClient, string>
    {
        public override Action<AuthClient, string> GetHandler(string message)
        {
            if(message != "Af")
                return HandleAuthentification;
            return null;
        }

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

                    client.Send(AuthMessage.ACCOUNT_RIGHT(0));
                    client.Send(AuthMessage.ACCOUNT_SECRET_ANSWER());

                    client.FrameManager.AddFrame(WorldSelectionFrame.Instance);
                });
        }
    }
}
