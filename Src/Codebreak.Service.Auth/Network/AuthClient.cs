using Codebreak.Framework.Network;
using Codebreak.Service.Auth.Database.Structures;

namespace Codebreak.Service.Auth.Network
{
    public sealed class AuthClient : DofusClient<AuthClient>
    {
        public string AuthKey
        {
            get;
            set;
        }

        public string Ticket
        {
            get;
            set;
        }

        public AccountDAO Account
        {
            get;
            set;
        }
    }
}
