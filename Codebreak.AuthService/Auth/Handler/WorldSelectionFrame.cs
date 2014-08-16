using Codebreak.AuthService.Auth.Manager;
using Codebreak.Framework.Network;
using Codebreak.RPCMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.AuthService.Auth.Handler
{
    public sealed class WorldSelectionFrame : FrameBase<WorldSelectionFrame, AuthClient, string>
    {
        public override Action<AuthClient, string> GetHandler(string message)
        {
            if (message.Length < 2)
                return null;

            switch (message[0])
            {
                case 'A':
                    if (message[1] == 'x')
                        return WorldCharacterList;
                    else if (message[1] == 'X')
                        return WorldSelection;
                    break;
            }

            return null;
        }

        private void WorldCharacterList(AuthClient client, string message)
        {
            WorldManager.Instance.SendWorldCharacterList(client);
        }

        private void WorldSelection(AuthClient client, string message)
        {
            var worldId = int.Parse(message.Substring(2));

            var world = WorldManager.Instance.GetById(worldId);

            if(world == null || world.GameState != RPCMessage.GameState.ONLINE)
            {
                client.Send(AuthMessage.WORLD_SELECTION_FAILED());
                return;
            }

            client.FrameManager.RemoveFrame(WorldSelectionFrame.Instance);

            var ticket = Util.GenerateString(10);

            client.Ticket = ticket;

            world.Send(new GameTicketMessage(client.Account.Id, client.Account.Name, client.Account.Power, client.Account.RemainingSubscription.ToBinary(), client.Account.LastConnectionDate.ToBinary(), client.Account.LastConnectionIP, ticket));

            AuthService.Instance.AddMessage(() =>
            {
                client.Send(AuthMessage.WORLD_SELECTION_SUCCESS(world.Ip, AuthConfig.WORLD_GAME_PORT, client.Ticket));
            });
        }
    }
}
