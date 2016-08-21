using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Frame
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuthentificationFrame : AbstractNetworkFrame<AuthentificationFrame, WorldClient, string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Action<WorldClient, string> GetHandler(string message)
        {
            //if (message.StartsWith("Ak"))
            //    return HandleKey;
            return HandleTicket;
        }
        
        //private void HandleKey(WorldClient client, string message)
        //{
        //    client.FrameManager.RemoveFrame(AuthentificationFrame.Instance);
        //    client.FrameManager.AddFrame(CharacterSelectionFrame.Instance);
        //    client.Cypher = true;
        //    client.Send(WorldMessage.BASIC_NO_OPERATION());
        //}

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void HandleTicket(WorldClient client, string message)
        {
            var ticket = message.Substring(2);

            client.FrameManager.RemoveFrame(AuthentificationFrame.Instance);

            WorldService.Instance.AddMessage(() =>
                {
                    var account = ClientManager.Instance.GetAccountTicket(ticket);
                    if (account == null)
                    {
                        client.Send(WorldMessage.ACCOUNT_TICKET_ERROR());
                        return;
                    }

                    WorldService.Instance.AddMessage(() =>
                        {
                            client.FrameManager.AddFrame(CharacterSelectionFrame.Instance);
                            client.Account = account;
                            ClientManager.Instance.ClientAuthentified(client);                            
                            client.Send(WorldMessage.ACCOUNT_TICKET_SUCCESS());
                        });
                });
        }
    }
}
