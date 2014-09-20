using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
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
