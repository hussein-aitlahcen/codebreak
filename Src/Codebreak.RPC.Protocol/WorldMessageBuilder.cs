using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
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
