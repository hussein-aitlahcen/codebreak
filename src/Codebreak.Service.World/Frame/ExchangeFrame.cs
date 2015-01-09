using System;
using System.Linq;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Exchange;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.Manager;

namespace Codebreak.Service.World.Frame
{
    public sealed class ExchangeFrame : FrameBase<ExchangeFrame, CharacterEntity, string>
    {
        public override Action<CharacterEntity, string> GetHandler(string message)
        {
            if (message.Length < 2)
                return null;

            switch (message[0])
            {                
                case 'E':
                    switch (message[1])
                    {
                        case 'Q':
                            return MerchantModeProcess;

                        case 'q': // merchant mode : taxes
                            return MerchantModeTaxe;

                        case 'H':
                            if (message.Length < 3)
                                return null;

                            switch (message[2])
                            {
                                case 'T':
                                    return AuctionHouseGetTemplatesList;

                                case 'l':
                                    return AuctionHouseGetItemsList;

                                case 'B':
                                    return AuctionHouseBuyItem;

                                    // SEARCH
                                case 'S':
                                    return null;

                                case 'P':
                                    return AuctionHouseMiddlePrice;
                            }
                            break;

                        case 'A':
                            return ExchangeAccept;

                        case 'R':
                            return ExchangeRequest;

                        case 'V':
                            return ExchangeLeave;

                        case 'K':
                            return ExchangeValidate;

                        case 'B':
                            return ExchangeBuy;

                        case 'S':
                            return ExchangeSell;

                        case 'M': 
                            if (message.Length < 3)
                                return null;

                            switch (message[2])
                            {
                                case 'G':
                                    return ExchangeMoveGold;

                                case 'O':
                                    return ExchangeMoveObject;

                                default:
                                    return null;
                            }
                    }
                    break;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void MerchantModeProcess(CharacterEntity character, string message)
        {
            character.AddMessage(() =>
                {
                    if(character.Inventory.Kamas < character.MerchantTaxe)
                    {
                        character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_NOT_ENOUGH_KAMAS_TO_PAY_MERCHANT_MODE_TAXE));
                        return;
                    }

                    if (character.Map.Entities.OfType<MerchantEntity>().Count() >= WorldConfig.MAX_MERCHANT_PER_MAP)
                    {
                        character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_TOO_MANY_MERCHANT_ON_MAP, WorldConfig.MAX_MERCHANT_PER_MAP));
                        return;
                    }

                    if (character.PersonalShop.Items.Count == 0)
                    {
                        character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_NOT_ENOUGH_ITEMS_TO_BE_MERCHANT));
                        return;
                    }

                    character.Inventory.SubKamas(character.MerchantTaxe);
                    character.Merchant = true;
                    character.SafeKick();
                });            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void MerchantModeTaxe(CharacterEntity character, string message)
        {
            character.AddMessage(() =>
                {
                    if(character.Map.Entities.OfType<MerchantEntity>().Count() >= WorldConfig.MAX_MERCHANT_PER_MAP)
                    {
                        character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_TOO_MANY_MERCHANT_ON_MAP, WorldConfig.MAX_MERCHANT_PER_MAP));
                        return;
                    }

                    if(character.PersonalShop.Items.Count == 0)
                    {
                        character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_NOT_ENOUGH_ITEMS_TO_BE_MERCHANT));
                        return;
                    }

                    character.Dispatch(WorldMessage.MERCHANT_MODE_TAXE(character.MerchantTaxe));
                });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void AuctionHouseMiddlePrice(CharacterEntity character, string message)
        {
            int templateId = -1;
            if (!int.TryParse(message.Substring(3), out templateId))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.AddMessage(() =>
            {
                if (!character.HasGameAction(GameActionTypeEnum.EXCHANGE))
                {
                    Logger.Debug("ExchangeFrame::Leave entity is not in an exchange : " + character.Name);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                var exchangeAction = character.CurrentAction as GameAuctionHouseActionBase;
                if (exchangeAction == null)
                {
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                var middlePrice = ((GameAuctionHouseActionBase)exchangeAction).AuctionExchange.Npc.AuctionHouse.GetMiddlePrice(templateId);

                character.Dispatch(WorldMessage.AUCTION_HOUSE_MIDDLE_PRICE(templateId, middlePrice));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void AuctionHouseBuyItem(CharacterEntity character, string message)
        {
            var data = message.Substring(3).Split('|');
            int categoryId = -1;
            if (!int.TryParse(data[0], out categoryId))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            int quantity = -1;
            if (!int.TryParse(data[1], out quantity))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            long price = -1;
            if (!long.TryParse(data[2], out price))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.AddMessage(() =>
            {
                if (!character.HasGameAction(GameActionTypeEnum.EXCHANGE))
                {
                    Logger.Debug("ExchangeFrame::Leave entity is not in an exchange : " + character.Name);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                var exchangeAction = character.CurrentAction as GameAuctionHouseActionBase;
                if (exchangeAction == null)
                {
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                exchangeAction.AuctionExchange.Npc.AuctionHouse.TryBuy(character, categoryId, quantity, price);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void AuctionHouseGetItemsList(CharacterEntity character, string message)
        {
            int templateId = -1;
            if (!int.TryParse(message.Substring(3), out templateId))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.AddMessage(() =>
            {
                if (!character.HasGameAction(GameActionTypeEnum.EXCHANGE))
                {
                    Logger.Debug("ExchangeFrame::Leave entity is not in an exchange : " + character.Name);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                var exchangeAction = character.CurrentAction as GameAuctionHouseActionBase;
                if (exchangeAction == null)
                {
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                exchangeAction.AuctionExchange.Npc.AuctionHouse.SendCategoriesByTemplate(character, templateId);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void AuctionHouseGetTemplatesList(CharacterEntity character, string message)
        {
            int type = -1;
            if(!int.TryParse(message.Substring(3), out type))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.AddMessage(() =>
                {
                    if (!character.HasGameAction(GameActionTypeEnum.EXCHANGE))
                    {
                        Logger.Debug("ExchangeFrame::Leave entity is not in an exchange, cheat ? : " + character.Name);
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }
                    
                    var exchangeAction = character.CurrentAction as GameAuctionHouseActionBase;
                    if (exchangeAction == null)
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    exchangeAction.AuctionExchange.Npc.AuctionHouse.SendTemplatesByTypeList(character, type);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void ExchangeRequest(CharacterEntity character, string message)
        {
            var exchangeData = message.Substring(2).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            var exchangeTypeId = int.Parse(exchangeData[0]);
            var exchangeActorId = -1;
            if(exchangeData.Length > 1)
                exchangeActorId = int.Parse(exchangeData[1]);

            if (!Enum.IsDefined(typeof(ExchangeTypeEnum), exchangeTypeId))
            {
                Logger.Debug("ExchangeFrame::Request unknow exchangeType : " + exchangeTypeId + " " + character.Name);
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.AddMessage(() =>
            {
                var exchangeType = (ExchangeTypeEnum)exchangeTypeId;
                if (!character.CanGameAction(GameActionTypeEnum.EXCHANGE))
                {
                    Logger.Debug("ExchangeFrame::Request character cant start an exchange, cheat ? : " + character.Name);
                    character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_YOU_ARE_AWAY));
                    return;
                }

                var distantEntity = character.Map.GetEntity(exchangeActorId);
                if (exchangeType == ExchangeTypeEnum.EXCHANGE_PERSONAL_SHOP_EDIT)
                    distantEntity = character;
                if (distantEntity == null)
                {
                    Logger.Debug("ExchangeFrame::Request unknow distant entity id, cheat ? : " + character.Name);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (!distantEntity.CanGameAction(GameActionTypeEnum.EXCHANGE) || !distantEntity.CanBeExchanged(exchangeType))
                {
                    if(distantEntity.Type == EntityTypeEnum.TYPE_CHARACTER)
                        character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_PLAYER_AWAY_NOT_INVITABLE));
                    else
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                switch (distantEntity.Type)
                {
                    case EntityTypeEnum.TYPE_TAX_COLLECTOR:
                        var taxCollector = (TaxCollectorEntity)distantEntity;
                        if (character.GuildMember == null || taxCollector.Guild.Id != character.GuildMember.GuildId)
                        {
                            character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                            return;
                        }
                        character.ExchangeTaxCollector((TaxCollectorEntity)distantEntity);
                        break;

                    case EntityTypeEnum.TYPE_MERCHANT:
                        character.ExchangeMerchant((MerchantEntity)distantEntity);
                        break;

                    case EntityTypeEnum.TYPE_CHARACTER:
                        if (exchangeType == ExchangeTypeEnum.EXCHANGE_PERSONAL_SHOP_EDIT && character.Id == distantEntity.Id)                        
                            character.ExchangePersonalShop();                        
                        else                        
                            character.ExchangePlayer((CharacterEntity)distantEntity);                        
                        break;

                    case EntityTypeEnum.TYPE_NPC:
                        var npc = (NonPlayerCharacterEntity)distantEntity;
                        switch(exchangeType)
                        {
                            case ExchangeTypeEnum.EXCHANGE_NPC:
                                character.ExchangeNpc(npc);                                
                                break;

                            case ExchangeTypeEnum.EXCHANGE_SHOP:
                                character.ExchangeShop(npc);
                                break;

                            case ExchangeTypeEnum.EXCHANGE_AUCTION_HOUSE_BUY:
                                character.ExchangeAuctionHouseBuy(npc);
                                break;

                            case ExchangeTypeEnum.EXCHANGE_AUCTION_HOUSE_SELL:
                                character.ExchangeAuctionHouseSell(npc);
                                break;
                        }
                        break;
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void ExchangeAccept(CharacterEntity character, string message)
        {            
            character.AddMessage(() =>
            {
                if (!character.HasGameAction(GameActionTypeEnum.EXCHANGE))
                {
                    Logger.Debug("ExchangeFrame:Accept entity not in an exchange request : " + character.Name);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                var action = character.CurrentAction;
                if (!(action is GamePlayerExchangeAction))
                {
                    Logger.Debug("ExchangeFrame::Accept entity is not in a player exchange : " + character.Name);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                var playerExchangeAction = (GamePlayerExchangeAction)action;
                if (character.Id != playerExchangeAction.DistantEntity.Id)
                {
                    Logger.Debug("ExchangeFrame::Accept player cannot accept an exchange is he is the one who requested it : " + character.Name);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                playerExchangeAction.Accept();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void ExchangeLeave(CharacterEntity entity, string message)
        {
            entity.AddMessage(() =>
            {
                if (!entity.HasGameAction(GameActionTypeEnum.EXCHANGE))
                {
                    Logger.Debug("ExchangeFrame::Leave entity is not in an exchange : " + entity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                entity.AbortAction(GameActionTypeEnum.EXCHANGE, entity.Id);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void ExchangeValidate(CharacterEntity character, string message)
        {      
            character.AddMessage(() =>
                {
                    if (!character.HasGameAction(GameActionTypeEnum.EXCHANGE))
                    {
                        Logger.Debug("ExchangeFrame::Validate entity is not in a exchange : " + character.Name);
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    var action = character.CurrentAction as GameExchangeActionBase;
                    var exchange = action.Exchange as EntityExchange;
                    if(exchange == null)
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    if (!exchange.Validate(character))
                    {
                        return;
                    }

                    character.StopAction(GameActionTypeEnum.EXCHANGE);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void ExchangeSell(CharacterEntity character, string message)
        {
            if (!message.Contains('|'))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var data = message.Substring(2).Split('|');

            if(data.Length != 2)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            long itemId = -1;
            if(!long.TryParse(data[0], out itemId))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            int quantity = -1;
            if(!int.TryParse(data[1], out quantity))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
            
            character.AddMessage(() =>
            {
                if (!character.HasGameAction(GameActionTypeEnum.EXCHANGE))
                {
                    Logger.Debug("ExchangeFrame::Sell entity is not in an exchange : " + character.Name);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                ((GameExchangeActionBase)character.CurrentAction).Exchange.SellItem(character, itemId, quantity);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void ExchangeBuy(CharacterEntity character, string message)
        {
            if(!message.Contains('|'))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var data = message.Substring(2).Split('|');

            if(data.Length != 2)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            int templateId = -1;
            if (!int.TryParse(data[0], out templateId))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            int quantity = -1;
            if(!int.TryParse(data[1], out quantity))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.AddMessage(() =>
            {
                if (!character.HasGameAction(GameActionTypeEnum.EXCHANGE))
                {
                    Logger.Debug("ExchangeFrame::Buy entity not in an exchange : " + character.Name);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                ((GameExchangeActionBase)character.CurrentAction).Exchange.BuyItem(character, templateId, quantity);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void ExchangeMoveGold(CharacterEntity character, string message)
        {
            long kamas = -1;
            if(!long.TryParse(message.Substring(3), out kamas))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
            
            character.AddMessage(() =>
            {
                if (!character.HasGameAction(GameActionTypeEnum.EXCHANGE))
                {
                    Logger.Debug("ExchangeFrame::MoveGold entity not in an exchange : " + character.Name);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                ((GameExchangeActionBase)character.CurrentAction).Exchange.MoveKamas(character, kamas);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void ExchangeMoveObject(CharacterEntity character, string message)
        {
            if (!message.Contains('|'))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var data = message.Substring(3).Split('|');

            if (data.Length < 2)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var add = data[0][0] == '+';
            var itemId = long.Parse(data[0].Substring(1));
            var quantity = int.Parse(data[1]);
            long price = -1;
            if (data.Length > 2)
                price = long.Parse(data[2]);

            character.AddMessage(() =>
            {
                if (!character.HasGameAction(GameActionTypeEnum.EXCHANGE))
                {
                    Logger.Debug("ExchangeFrame::MoveObject entity not in an exchange : " + character.Name);     
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }
                
                if (add)                
                    ((GameExchangeActionBase)character.CurrentAction).Exchange.AddItem(character, itemId, quantity, price);                
                else                
                    ((GameExchangeActionBase)character.CurrentAction).Exchange.RemoveItem(character, itemId, quantity);                
            });
        }
    }
}
