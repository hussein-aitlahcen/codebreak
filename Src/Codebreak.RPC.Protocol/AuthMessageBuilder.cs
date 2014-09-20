using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
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
