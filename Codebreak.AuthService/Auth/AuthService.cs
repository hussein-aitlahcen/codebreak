using Codebreak.AuthService.Auth.Handler;
using Codebreak.AuthService.Auth.Manager;
using Codebreak.Framework.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.AuthService.Auth
{
    public sealed class AuthService : TcpServerBase<AuthService, AuthClient>
    {
        protected override void OnClientConnected(AuthClient client)
        {
            Logger.Debug("Connected : " + client.Ip);

            AddMessage(() =>
            {
                if (base.Clients.Count() >= AuthConfig.AUTH_MAX_CLIENT)
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

                    AccountManager.Instance.ClientDisconnected(client);
                });
        }

        protected override void OnDataReceived(AuthClient client, byte[] buffer, int offset, int count)
        {
            if(client.FrameManager.IsEmpty)
            {
                Logger.Debug("AuthClient network frame is empty.");
                client.Disconnect();
                return;
            }

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
    }
}
