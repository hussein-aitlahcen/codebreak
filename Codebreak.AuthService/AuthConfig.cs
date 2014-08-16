using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.AuthService
{
    public static class AuthConfig
    {
        public const int AUTH_KEY_LENGTH = 32;
        public const int AUTH_MAX_CLIENT = 500;
        public const int WORLD_GAME_PORT = 5555;
        public const string CLIENT_VERSION = "1.29.1";
        public const string AUTH_BIND_IP = "25.214.133.179";
        public const int AUTH_BIND_PORT = 444;
        public const string RPC_BIND_IP = "25.214.133.179";
        public const int RPC_BIND_PORT = 4321;
        public const string DB_CONNECTION = "Data Source=SMARKEN;Initial Catalog=codebreak_auth;Integrated Security=True;Pooling=False";
    }
}
