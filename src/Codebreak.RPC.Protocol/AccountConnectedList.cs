using Codebreak.RPC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.RPC.Protocol
{
    public sealed class AccountConnectedList : RPCMessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        public override int Id
        {
            get 
            { 
                return (int)MessageIdEnum.WORLD_TO_AUTH_ACCOUNT_CONNECTED_LIST;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<long> ConnectedAccounts
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        public AccountConnectedList(IEnumerable<long> connectedAccounts)
        {
            ConnectedAccounts = new List<long>(connectedAccounts);
        }

        /// <summary>
        /// 
        /// </summary>
        public AccountConnectedList()
        {
            ConnectedAccounts = new List<long>();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Deserialize()
        {
            var length = base.ReadLong();
            for(int i = 0; i < length; i++)
                ConnectedAccounts.Add(base.ReadLong());
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Serialize()
        {
            base.WriteLong(ConnectedAccounts.Count);
            foreach(var connectedAccount in ConnectedAccounts)
                base.WriteLong(connectedAccount);
        }
    }
}
