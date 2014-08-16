using Codebreak.AuthService.Auth.Database.Repository;
using Codebreak.AuthService.Auth.Manager;
using Codebreak.Framework.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.AuthService.Auth.Handler
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
                client.Disconnect();
                return;
            }

            var account = credentials[0];
            var password = credentials[1].TrimStart('1');

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

            if(AccountManager.Instance.IsConnected(account.Id))
            {
                client.Send(AuthMessage.AUTH_FAILED_ALREADY_CONNECTED());
                return;
            }
                        
            AuthService.Instance.AddMessage(() =>
                {
                    client.Account = account;

                    AccountManager.Instance.ClientAuthentified(client);

                    client.Send(AuthMessage.ACCOUNT_PSEUDO(account.Pseudo));
                    client.Send(AuthMessage.UNKNOW_AC0());

                    WorldManager.Instance.SendWorldList(client);

                    client.Send(AuthMessage.ACCOUNT_RIGHT(0));
                    client.Send(AuthMessage.ACCOUNT_SECRET_ANSWER());

                    client.FrameManager.AddFrame(WorldSelectionFrame.Instance);
                });
        }
    }
}
