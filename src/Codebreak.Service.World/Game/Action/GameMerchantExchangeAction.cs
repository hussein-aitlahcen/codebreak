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
    public sealed class GameMerchantExchangeAction: AbstractGameExchangeAction
    {
        /// <summary>
        /// 
        /// </summary>
        public MerchantEntity Merchant
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity Character
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="merchant"></param>
        public GameMerchantExchangeAction(CharacterEntity character, MerchantEntity merchant)
            : base(new MerchantExchange(character, merchant), character, merchant)
        {
            Merchant = merchant;
            Character = character;
            Merchant.Buyers.Add(Character);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            Accept();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Stop(params object[] args)
        {
            IsFinished = true;
            base.Leave(true);
            Merchant.Buyers.Remove(Character);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Abort(params object[] args)
        {
            IsFinished = true;
            base.Leave(false);
            if (Merchant.Buyers != null)
                Merchant.Buyers.Remove(Character);
        }
    }
}
