using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Frames
{
    public sealed class AuthentificationFrame : FrameBase<AuthentificationFrame, WorldClient, string>
    {
        public override Action<WorldClient, string> GetHandler(string message)
        {
            return HandleTicket;
        }

        private void HandleTicket(WorldClient client, string message)
        {
            client.FrameManager.RemoveFrame(AuthentificationFrame.Instance);

            var ticket = message.Substring(2);

            var account = AccountManager.Instance.GetAccountTicket(ticket);
            if (account == null)
            {
                client.Send(WorldMessage.ACCOUNT_TICKET_ERROR());
                return;
            }

            WorldService.Instance.AddMessage(() =>
                {
                    client.Account = account;

                    AccountManager.Instance.ClientAuthentified(client);
                    
                    client.FrameManager.AddFrame(CharacterSelectionFrame.Instance);

                    client.Send(WorldMessage.ACCOUNT_TICKET_SUCCESS());
                });
        }
    }
}
