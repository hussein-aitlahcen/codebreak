using Codebreak.RPC.Protocol;
using Codebreak.RPC.Service;

namespace Codebreak.Service.Auth.RPC
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuthRPCServiceClient : RPCClientBase<AuthRPCServiceClient>
    {
        /// <summary>
        /// 
        /// </summary>
        public GameStateEnum GameState
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public AuthStateEnum AuthState
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int GameId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public AuthRPCServiceClient()
        {
            GameState = GameStateEnum.OFFLINE;
            AuthState = AuthStateEnum.NEGOTIATING;
            GameId = -1;
        }
    }
}
