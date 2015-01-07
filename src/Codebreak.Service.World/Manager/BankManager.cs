using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BankManager : Singleton<BankManager>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, BankInventory> m_bankByAccountId;

        /// <summary>
        /// 
        /// </summary>
        public BankManager()
        {
            m_bankByAccountId = new Dictionary<long, BankInventory>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public BankInventory GetBankByAccountId(long accountId)
        {
            if(!m_bankByAccountId.ContainsKey(accountId))
            {
                var bank = new BankInventory(BankRepository.Instance.GetByAccountId(accountId));
                m_bankByAccountId.Add(accountId, bank);
                return bank;
            }
            return m_bankByAccountId[accountId];
        }
    }
}
