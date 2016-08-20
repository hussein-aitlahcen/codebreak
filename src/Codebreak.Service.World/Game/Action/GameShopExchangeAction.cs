using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Action
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameShopExchangeAction : AbstractGameExchangeAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buyer"></param>
        /// <param name="shop"></param>
        public GameShopExchangeAction(CharacterEntity character, NonPlayerCharacterEntity npc)
            : base(new ShopExchange(character, npc), character, npc)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            base.Accept();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Stop(params object[] args)
        {
            base.Leave(true);
            base.Stop(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Abort(params object[] args)
        {
            base.Leave();
            base.Abort(args);
        }
    }
}
