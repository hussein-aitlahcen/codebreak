using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuthMessageBuilder : RPCMessageBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        public AuthMessageBuilder()
        {
            base.Register<AuthentificationMessage>((int)MessageIdEnum.WORLD_TO_AUTH_CREDENTIAL);
            base.Register<GameIdUpdateMessage>((int)MessageIdEnum.WORLD_TO_AUTH_GAMEIDUPDATE);
            base.Register<GameStateUpdateMessage>((int)MessageIdEnum.WORLD_TO_AUTH_GAMESTATEUPDATE);
            base.Register<GameAccountDisconnected>((int)MessageIdEnum.WORLD_TO_AUTH_GAMEACCOUNTDISCONNECTED);
        }
    }
}
