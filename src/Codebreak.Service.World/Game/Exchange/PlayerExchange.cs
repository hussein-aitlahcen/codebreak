using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Network;
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
    public sealed class PlayerExchange : AbstractEntityExchange
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="local"></param>
        /// <param name="distant"></param>
        public PlayerExchange(CharacterEntity local, CharacterEntity distant)
            : base(ExchangeTypeEnum.EXCHANGE_PLAYER, local, distant)
        {
        }
    }
}
