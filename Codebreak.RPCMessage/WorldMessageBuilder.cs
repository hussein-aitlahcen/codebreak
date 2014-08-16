using Codebreak.RPCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.RPCMessage
{
    public sealed class WorldMessageBuilder : RPCMessageBuilder
    {
        public WorldMessageBuilder()
        {
            base.Register<AuthentificationResult>((int)MessageId.AUTH_TO_WORLD_CREDENTIALRESULT);
            base.Register<GameTicketMessage>((int)MessageId.AUTH_TO_WORLD_GAMETICKET);
        }
    }
}
