using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Exchange
{
    /// <summary>
    /// TO BE DONE SOON EASY <3
    /// </summary>
    public sealed class NpcExchange : EntityExchange
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="local"></param>
        /// <param name="distant"></param>
        public NpcExchange(CharacterEntity local, NonPlayerCharacterEntity distant)
            : base(ExchangeTypeEnum.EXCHANGE_PLAYER, local, distant)
        {
        }
    }
}
