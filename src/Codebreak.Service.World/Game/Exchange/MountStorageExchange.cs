using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Exchange
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MountStorageExchange : AbstractExchange
    {
        /// <summary>
        /// 
        /// </summary>
        public MountStorageExchange()
            : base(ExchangeTypeEnum.EXCHANGE_MOUNT_STORAGE)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string SerializeAs_ExchangeCreate()
        {
            //TODO: "etables~park"
            return base.SerializeAs_ExchangeCreate();
        }
    }
}
