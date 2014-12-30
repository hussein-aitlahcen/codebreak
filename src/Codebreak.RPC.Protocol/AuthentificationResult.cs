using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuthentificationResult : RPCMessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        public override int Id
        {
            get 
            { 
                return (int)MessageIdEnum.AUTH_TO_WORLD_CREDENTIALRESULT;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AuthResultEnum Result
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public AuthentificationResult()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public AuthentificationResult(AuthResultEnum result)
        {
            Result = result;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Deserialize()
        {
            Result = (AuthResultEnum)base.ReadInt();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Serialize()
        {
            base.WriteInt((int)Result);
        }
    }
}
