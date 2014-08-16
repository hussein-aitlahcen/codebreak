using Codebreak.RPCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.RPCMessage
{
    public sealed class AuthentificationMessage : RPCMessageBase
    {

        public override int Id
        {
            get 
            { 
                return (int)MessageId.WORLD_TO_AUTH_CREDENTIAL;
            }
        }

        public string Password
        {
            get;
            private set;
        }

        public AuthentificationMessage()
        {
        }

        public AuthentificationMessage(string password)
        {
            Password = password;
        }

        public override void Deserialize()
        {
            Password = base.ReadString();
        }

        public override void Serialize()
        {
            base.WriteString(Password);
        }
    }
}
