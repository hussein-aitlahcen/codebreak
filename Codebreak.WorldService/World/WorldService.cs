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
    /// <summary>
    /// 
    /// </summary>
    public sealed class WorldService : TcpServerBase<WorldService, WorldClient>
    {
        /// <summary>
        /// 
        /// </summary>
        public MessageDispatcher Dispatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public WorldService()
        {
            Dispatcher = new MessageDispatcher();

            AddUpdatable(Dispatcher);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        protected override void OnClientConnected(WorldClient client)
        {
            AddMessage(() =>
            {
                client.FrameManager.AddFrame(AuthentificationFrame.Instance);
                client.Send(WorldMessage.HELLO_GAME());
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        protected override void OnDataReceived(WorldClient client, byte[] buffer, int offset, int count)
        {
            foreach (var message in client.Receive(buffer, offset, count))
            {
                AddMessage(() =>
                    {
                        Logger.Debug("Client : " + message);

                        if (client.CurrentCharacter != null)
                        {
                            client.CurrentCharacter.AddMessage(() =>
                                {
                                    client.CurrentCharacter.FrameManager.ProcessMessage(message);
                                });
                        }
                        else
                        {
                            client.FrameManager.ProcessMessage(message);
                        }
                    });
            }        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SendToAll(string message)
        {
            base.SendToAll(Encoding.Default.GetBytes(message + (char)0x00));
        }
    }
}
