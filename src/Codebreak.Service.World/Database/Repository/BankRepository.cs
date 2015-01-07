using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BankRepository : Repository<BankRepository, BankDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, BankDAO> m_bankByAccountId;

        /// <summary>
        /// 
        /// </summary>
        public BankRepository()
        {
            m_bankByAccountId = new Dictionary<long, BankDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public override void OnObjectAdded(BankDAO bank)
        {
            m_bankByAccountId.Add(bank.Id, bank);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public BankDAO GetByAccountId(long accountId)
        {
            if (m_bankByAccountId.ContainsKey(accountId))
                return m_bankByAccountId[accountId];
            var bank = new BankDAO() { Id = accountId };
            base.InsertWithKey(bank);
            return bank;
        }
    }
}
