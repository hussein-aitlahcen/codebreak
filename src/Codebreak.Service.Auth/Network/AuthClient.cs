using Codebreak.Framework.Network;
using Codebreak.Service.Auth.Database.Structure;

namespace Codebreak.Service.Auth.Network
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuthClient : DofusClient<AuthClient>
    {
        /// <summary>
        /// 
        /// </summary>
        public string AuthKey
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Ticket
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public AccountDAO Account
        {
            get;
            set;
        }
    }
}
