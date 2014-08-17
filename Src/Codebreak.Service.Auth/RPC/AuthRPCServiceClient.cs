using Codebreak.RPC.Protocol;
using Codebreak.RPC.Service;

namespace Codebreak.Service.Auth.RPC
{

// ReSharper disable once InconsistentNaming
    public sealed class AuthRPCServiceClient : RPCClient<AuthRPCServiceClient>
    {
        public GameState GameState
        {
            get;
            set;
        }

        public AuthState AuthState
        {
            get;
            set;
        }

        public int GameId
        {
            get;
            set;
        }

        public AuthRPCServiceClient()
        {
            GameState = GameState.OFFLINE;
            AuthState = AuthState.NEGOTIATING;
            GameId = -1;
        }
    }
}
