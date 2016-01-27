using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Exchange
{
    public abstract class AuctionHouseExchange : AbstractExchange
    {
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
        public NonPlayerCharacterEntity Npc
        {
            get;
            private set;
        }

         /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="npc"></param>
        public AuctionHouseExchange(ExchangeTypeEnum type, CharacterEntity character, NonPlayerCharacterEntity npc)
            : base(type)
        {
            Character = character;
            Npc = npc;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string SerializeAs_ExchangeCreate()
        {
            return "1,10,100" + ";"
                + String.Join(",", Npc.AuctionHouse.AllowedTypes) + ";"
                + Npc.AuctionHouse.Taxe + ";"
                + Npc.AuctionHouse.ItemMaxLevel + ";"
                + Npc.AuctionHouse.PlayerMaxItem + ";"
                + Npc.Id + ";"
                + Npc.AuctionHouse.Timeout + "|"
                + Npc.Id;
        }
    }
}
