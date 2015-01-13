using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AccountDisconnected : RPCMessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        public override int Id
        {
            get 
            { 
                return (int)MessageIdEnum.WORLD_TO_AUTH_ACCOUNT_DISCONNECTED;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long AccountId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        public AccountDisconnected(long accountId)
        {
            AccountId = accountId;
        }

        /// <summary>
        /// 
        /// </summary>
        public AccountDisconnected()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Deserialize()
        {
            AccountId = base.ReadLong();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Serialize()
        {
            base.WriteLong(AccountId);
        }
    }
}
