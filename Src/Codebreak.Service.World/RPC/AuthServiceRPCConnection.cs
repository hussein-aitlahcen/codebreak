using Codebreak.RPC.Protocol;
using Codebreak.RPC.Service;

namespace Codebreak.Service.World.RPC
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
