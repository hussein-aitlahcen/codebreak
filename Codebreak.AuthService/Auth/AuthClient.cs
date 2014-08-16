using Codebreak.AuthService.Auth.Database.Structure;
using Codebreak.Framework.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.AuthService.Auth
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
