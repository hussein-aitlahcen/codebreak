using Codebreak.Framework.Network;
using Codebreak.WorldService.World.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Handler
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
