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
        public AuthentificationMessage()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        public AuthentificationMessage(string password)
        {
            Password = password;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Deserialize()
        {
            Password = base.ReadString();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Serialize()
        {
            base.WriteString(Password);
        }
    }
}
