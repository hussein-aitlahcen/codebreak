using Codebreak.Framework.Generic;
using Codebreak.RPCMessage;
using Codebreak.RPCService;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.RPC
{
    public sealed class AuthServiceRPCConnection : RPCConnectionBase<WorldMessageBuilder>
    {
        protected override void OnMessage(RPCMessageBase message)
        {
        }

        protected override void OnDisconnected()
        {
        }

        protected override void OnConnected()
        {
        }
    }
}
