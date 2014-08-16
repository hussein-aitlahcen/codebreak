using Codebreak.Framework.Network;
using Codebreak.WorldService.World.Action;
using Codebreak.WorldService.World.Handler;
using Codebreak.WorldService.World.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World
{
    public sealed class WorldService : TcpServerBase<WorldService, WorldClient>
    {
        public MessageDispatcher Dispatcher
        {
            get;
            private set;
        }

        public WorldService()
        {
            Dispatcher = new MessageDispatcher();

            AddUpdatable(Dispatcher);
        }

        protected override void OnClientConnected(WorldClient client)
        {
            AddMessage(() =>
            {
                client.FrameManager.AddFrame(AuthentificationFrame.Instance);
                client.Send(WorldMessage.HELLO_GAME());
            });
        }

        protected override void OnClientDisconnected(WorldClient client)
        {
            AddMessage(() =>
                {
                    if(client.CurrentCharacter != null)
                    {
                        EntityManager.Instance.RemoveCharacter(client.CurrentCharacter);

                        client.Characters = null;
                        client.CurrentCharacter = null;
                    }
                    AccountManager.Instance.ClientDisconnected(client);
                });
        }

        protected override void OnDataReceived(WorldClient client, byte[] buffer, int offset, int count)
        {
            if ((client.FrameManager.IsEmpty && client.CurrentCharacter == null) || (client.CurrentCharacter != null && client.CurrentCharacter.FrameManager.IsEmpty))
            {
                Logger.Debug("WorldClient network fram is empty.");
                client.Disconnect();
                return;
            }

            foreach (var message in client.Receive(buffer, offset, count))
            {
                AddMessage(() =>
                    {
                        Logger.Debug("Client : " + message);

                        if (client.CurrentCharacter != null)
                        {
                            client.CurrentCharacter.FrameManager.ProcessMessage(message);
                        }
                        else
                        {
                            client.FrameManager.ProcessMessage(message);
                        }
                    });
            }        
        }

        public void SendToAll(string message)
        {
            base.SendToAll(Encoding.Default.GetBytes(message + (char)0x00));
        }
    }
}
