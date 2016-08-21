using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuthMessageBuilder : RpcMessageBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        public AuthMessageBuilder()
        {
            base.Register<AuthentificationMessage>((int)MessageIdEnum.WORLD_TO_AUTH_CREDENTIAL);
            base.Register<IdUpdateMessage>((int)MessageIdEnum.WORLD_TO_AUTH_ID_UPDATE);
            base.Register<StateUpdateMessage>((int)MessageIdEnum.WORLD_TO_AUTH_STATE_UPDATE);
            base.Register<AccountDisconnected>((int)MessageIdEnum.WORLD_TO_AUTH_ACCOUNT_DISCONNECTED);
            base.Register<AccountConnectedList>((int)MessageIdEnum.WORLD_TO_AUTH_ACCOUNT_CONNECTED_LIST);
        }
    }
}
