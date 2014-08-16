using Codebreak.RPCMessage;
using Codebreak.RPCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.AuthService.RPC
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuthServiceRPCClient : RPCClient<AuthServiceRPCClient>
    {
        /// <summary>
        /// 
        /// </summary>
        public GameState GameState
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public AuthState AuthState
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
        public AuthServiceRPCClient()
        {
            GameState = GameState.OFFLINE;
            AuthState = AuthState.NEGOTIATING;
            GameId = -1;
        }
    }
}
