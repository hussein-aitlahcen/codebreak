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
    public class GameStorageExchangeAction : GameExchangeActionBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="storage"></param>
        public GameStorageExchangeAction(CharacterEntity character, PersistentInventory storage, ExchangeTypeEnum type = ExchangeTypeEnum.EXCHANGE_STORAGE)
            : base(new StorageExchange(character, storage, type), character, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            Exchange.Create();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Stop(params object[] args)
        {
            base.Stop(args);
            base.Leave(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Abort(params object[] args)
        {
            base.Abort(args);
            base.Leave();
        }
    }
}
