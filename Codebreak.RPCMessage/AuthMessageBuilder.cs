using Codebreak.RPCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.RPCMessage
{
    public sealed class AuthMessageBuilder : RPCMessageBuilder
    {
        public AuthMessageBuilder()
        {
            base.Register<AuthentificationMessage>((int)MessageId.WORLD_TO_AUTH_CREDENTIAL);
            base.Register<GameIdUpdateMessage>((int)MessageId.WORLD_TO_AUTH_GAMEIDUPDATE);
            base.Register<GameStateUpdateMessage>((int)MessageId.WORLD_TO_AUTH_GAMESTATEUPDATE);
            base.Register<GameAccountDisconnected>((int)MessageId.WORLD_TO_AUTH_GAMEACCOUNTDISCONNECTED);
        }
    }
}
