using Codebreak.RPCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.RPCMessage
{
    public sealed class GameAccountDisconnected : RPCMessageBase
    {
        public override int Id
        {
            get { return (int)MessageId.WORLD_TO_AUTH_GAMEACCOUNTDISCONNECTED; }
        }

        public long AccountId
        {
            get;
            private set;
        }

        public GameAccountDisconnected(long accountId)
        {
            AccountId = accountId;
        }

        public GameAccountDisconnected()
        {
        }

        public override void Deserialize()
        {
            AccountId = base.ReadLong();
        }

        public override void Serialize()
        {
            base.WriteLong(AccountId);
        }
    }
}
