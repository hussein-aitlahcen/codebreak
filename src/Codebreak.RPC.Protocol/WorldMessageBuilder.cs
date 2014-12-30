using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WorldMessageBuilder : RPCMessageBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        public WorldMessageBuilder()
        {
            base.Register<AuthentificationResult>((int)MessageIdEnum.AUTH_TO_WORLD_CREDENTIALRESULT);
            base.Register<GameTicketMessage>((int)MessageIdEnum.AUTH_TO_WORLD_GAMETICKET);
        }
    }
}
