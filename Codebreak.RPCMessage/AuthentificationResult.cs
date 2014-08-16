using Codebreak.RPCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.RPCMessage
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
