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
    public sealed class AuthentificationFrame : FrameBase<AuthentificationFrame, WorldClient, string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Action<WorldClient, string> GetHandler(string message)
        {
            return HandleTicket;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void HandleTicket(WorldClient client, string message)
        {
            client.FrameManager.RemoveFrame(AuthentificationFrame.Instance);

            var ticket = message.Substring(2);

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
                            client.Account = account;

                            ClientManager.Instance.ClientAuthentified(client);

                            client.FrameManager.AddFrame(CharacterSelectionFrame.Instance);

                            client.Send(WorldMessage.ACCOUNT_TICKET_SUCCESS());
                        });
                });
        }
    }
}
