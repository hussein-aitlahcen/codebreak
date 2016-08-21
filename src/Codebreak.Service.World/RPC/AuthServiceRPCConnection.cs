using Codebreak.RPC.Protocol;
using Codebreak.RPC.Service;

namespace Codebreak.Service.World.RPC
{
    public sealed class AuthServiceRPCConnection : AbstractRcpConnection<WorldMessageBuilder>
    {
        protected override void OnMessage(AbstractRcpMessage message)
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
