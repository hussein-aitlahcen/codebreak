using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
{
    public sealed class AuthentificationResult : RPCMessageBase
    {
        public override int Id
        {
            get { return (int)MessageId.AUTH_TO_WORLD_CREDENTIALRESULT; }
        }

        public AuthResult Result
        {
            get;
            private set;
        }

        public AuthentificationResult()
        {
        }

        public AuthentificationResult(AuthResult result)
        {
            Result = result;
        }

        public override void Deserialize()
        {
            Result = (AuthResult)base.ReadInt();
        }

        public override void Serialize()
        {
            base.WriteInt((int)Result);
        }
    }
}
