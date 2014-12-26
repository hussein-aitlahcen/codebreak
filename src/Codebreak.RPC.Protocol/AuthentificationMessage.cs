using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
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
