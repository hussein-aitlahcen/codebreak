using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BankInventory : PersistentInventory
    {
        /// <summary>
        /// 
        /// </summary>
        public override long Kamas
        {
            get
            {
                return m_record.Kamas;
            }
            set
            {
                m_record.Kamas = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private BankDAO m_record;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        public BankInventory(BankDAO databaseRecord)
            : base((int)EntityTypeEnum.TYPE_BANK, databaseRecord.Id)
        {
            m_record = databaseRecord;
        }
    }
}
