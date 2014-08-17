using System;
using System.Net.Sockets;
using Codebreak.Framework.Configuration;
using Codebreak.Framework.Network;
using Codebreak.RPC.Protocol;
using Codebreak.Service.Auth.Network;

namespace Codebreak.Service.Auth.Frames
{
    public sealed class WorldSelectionFrame : FrameBase<WorldSelectionFrame, AuthClient, string>
    {
        [Configurable("WorldGamePort")]
        public static int WorldGamePort = 5555;

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
            AuthService.Instance.SendWorldCharacterList(client);
        }

        private void WorldSelection(AuthClient client, string message)
        {
            var worldId = int.Parse(message.Substring(2));

            var world = AuthService.Instance.GetById(worldId);

            if(world == null || world.GameState != GameState.ONLINE)
            {
                client.Send(AuthMessage.WORLD_SELECTION_FAILED());
                return;
            }

            client.FrameManager.RemoveFrame(WorldSelectionFrame.Instance);

            var ticket = Util.GenerateString(10);

            client.Ticket = ticket;

            world.Send(new GameTicketMessage(client.Account.Id, client.Account.Name, client.Account.Power, client.Account.RemainingSubscription.ToBinary(), client.Account.LastConnectionDate.ToBinary(), client.Account.LastConnectionIP, ticket));

            AuthService.Instance.AddMessage(() => client.Send(AuthMessage.WORLD_SELECTION_SUCCESS(world.Ip, WorldGamePort, client.Ticket)));
        }
    }
}
