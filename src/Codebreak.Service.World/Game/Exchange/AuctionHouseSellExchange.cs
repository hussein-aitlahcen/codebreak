using Codebreak.Service.World.Game.Auction;
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
    public sealed class AuctionHouseSellExchange : AuctionHouseExchange
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="npc"></param>
        public AuctionHouseSellExchange(CharacterEntity character, NonPlayerCharacterEntity npc)
            : base(ExchangeTypeEnum.EXCHANGE_AUCTION_HOUSE_SELL, character, npc)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Create()
        {
            base.Create();

            Npc.AuctionHouse.SendAuctionOwnerList(Character);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="guid"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        public override void AddItem(EntityBase actor, long guid, int quantity, long price = -1)
        {
            switch(Npc.AuctionHouse.TryAdd(Character, guid, quantity, price))
            {
                case AuctionAddResultEnum.INVALID_PRICE:
                    Character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_INVALID_PRICE));
                    break;

                case AuctionAddResultEnum.ERROR:
                case AuctionAddResultEnum.INVALID_FLOOR:
                case AuctionAddResultEnum.INVALID_ITEM:
                case AuctionAddResultEnum.INVALID_QUANTITY:
                case AuctionAddResultEnum.INVALID_TYPE:
                case AuctionAddResultEnum.TOO_HIGH_LEVEL:
                    Character.Dispatch(WorldMessage.OBJECT_MOVE_ERROR());
                    break;

                case AuctionAddResultEnum.TOO_MANY_ENTRIES:
                    Character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_AUCTION_HOUSE_TOO_MANY_ITEMS));
                    break;

                case AuctionAddResultEnum.NOT_ENOUGH_KAMAS_FOR_TAXE:
                    Character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_NOT_ENOUGH_KAMAS_FOR_TAXE));
                    break;                    
            }
        }
    }
}
