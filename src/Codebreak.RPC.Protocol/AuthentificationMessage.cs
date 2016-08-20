using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuthentificationMessage : RPCMessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        public override int Id
        {
            get 
            { 
                return (int)MessageIdEnum.WORLD_TO_AUTH_CREDENTIAL;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string RemoteIp
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public AuthentificationMessage()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        public AuthentificationMessage(string password, string remoteIp)
        {
            Password = password;
            RemoteIp = remoteIp;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Deserialize()
        {
            Password = base.ReadString();
            RemoteIp = base.ReadString();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Serialize()
        {
            base.WriteString(Password);
            base.WriteString(RemoteIp);
        }
    }
}
