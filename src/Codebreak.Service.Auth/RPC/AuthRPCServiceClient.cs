using Codebreak.RPC.Protocol;
using Codebreak.RPC.Service;
using System.Collections.Generic;

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

        public List<long> Players
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
            Players = new List<long>();
            GameId = -1;
        }
    }
}
