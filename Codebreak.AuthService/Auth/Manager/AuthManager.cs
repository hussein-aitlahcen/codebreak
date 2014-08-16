using Codebreak.AuthService.Auth.Database;
using Codebreak.AuthService.RPC;
using Codebreak.Framework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.AuthService.Auth.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuthManager : Singleton<AuthManager>
    {
        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            AuthDbMgr.Instance.Initialize();

            AuthServiceRPC.Instance.Start(AuthConfig.RPC_BIND_IP, AuthConfig.RPC_BIND_PORT);

            AuthService.Instance.Start(AuthConfig.AUTH_BIND_IP, AuthConfig.AUTH_BIND_PORT);
        }
    }
}
