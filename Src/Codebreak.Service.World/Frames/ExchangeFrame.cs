using System;
using System.Linq;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Exchange;

namespace Codebreak.Service.World.Frames
{
    public sealed class ExchangeFrame : FrameBase<ExchangeFrame, EntityBase, string>
    {
        public override Action<EntityBase, string> GetHandler(string message)
        {
            if (message.Length < 2)
                return null;

            switch (message[0])
            {
                case 'E':
                    switch (message[1])
                    {
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

                        case 'M': // move
                            if (message.Length < 3)
                                return null;

                            switch (message[2])
                            {
                                case 'G': // gold
                                    return ExchangeMoveGold;

                                case 'O': // object
                                    return ExchangeMoveObject;

                                default:
                                    return null;
                            }

                        default:
                            return null;
                    }

                default:
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void ExchangeRequest(EntityBase entity, string message)
        {
            var exchangeData = message.Substring(2).Split('|');
            var exchangeTypeId = int.Parse(exchangeData[0]);
            var exchangeActorId = int.Parse(exchangeData[1]);

            if (!Enum.IsDefined(typeof(ExchangeTypeEnum), exchangeTypeId))
            {
                Logger.Debug("ExchangeFrame::Request unknow exchangeType : " + exchangeTypeId + " " + entity.Name);
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            entity.AddMessage(() =>
            {
                var exchangeType = (ExchangeTypeEnum)exchangeTypeId;

                if (!entity.CanGameAction(GameActionTypeEnum.EXCHANGE))
                {
                    Logger.Debug("ExchangeFrame::Request entity cant start an exchange : " + entity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                var distantEntity = entity.Map.GetEntity(exchangeActorId);
                if (distantEntity == null)
                {
                    Logger.Debug("ExchangeFrame::Request unknow distant entity id : " + entity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (!distantEntity.CanBeExchanged(exchangeType))
                {
                    Logger.Debug("ExchangeFrame::Request distant entity cannot start an exchange : " + distantEntity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }
                switch (distantEntity.Type)
                {
                    case EntityTypEnum.TYPE_CHARACTER:
                        ((CharacterEntity)entity).ExchangePlayer((CharacterEntity)distantEntity);
                        break;

                    case EntityTypEnum.TYPE_NPC:
                        ((CharacterEntity)entity).ExchangeShop(distantEntity);
                        break;
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void ExchangeAccept(EntityBase entity, string message)
        {            
            entity.AddMessage(() =>
            {
                if (!entity.HasGameAction(GameActionTypeEnum.EXCHANGE))
                {
                    Logger.Debug("ExchangeFrame:Accept entity not in an exchange request : " + entity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                var action = entity.CurrentAction;
                if (!(action is GamePlayerExchangeAction))
                {
                    Logger.Debug("ExchangeFrame::Accept entity is not in a player exchange : " + entity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                var playerExchangeAction = (GamePlayerExchangeAction)action;
                if (entity.Id != playerExchangeAction.DistantEntity.Id)
                {
                    Logger.Debug("ExchangeFrame::Accept player cannot accept an exchange is he is the one who requested it : " + entity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                playerExchangeAction.Accept();
            });
        }

        private void ExchangeLeave(EntityBase entity, string message)
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
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void ExchangeValidate(EntityBase entity, string message)
        {      
            entity.AddMessage(() =>
                {
                    if (!entity.HasGameAction(GameActionTypeEnum.EXCHANGE))
                    {
                        Logger.Debug("ExchangeFrame::Validate entity is not in a exchange : " + entity.Name);
                        entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    var action = entity.CurrentAction as GameExchangeActionBase;

                    if (action.Exchange.Type != ExchangeTypeEnum.EXCHANGE_PLAYER)
                    {
                        Logger.Debug("ExchangeFrame::Validate entity is not in a player exchange : " + entity.Name);
                        entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    var exchange = (PlayerExchange)action.Exchange;
                    if (!exchange.Validate(entity))
                    {
                        return;
                    }

                    entity.StopAction(GameActionTypeEnum.EXCHANGE, entity.Id);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void ExchangeSell(EntityBase entity, string message)
        {
            if (!message.Contains('|'))
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var data = message.Substring(2).Split('|');

            if(data.Length != 2)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            long itemId = -1;
            if(!long.TryParse(data[0], out itemId))
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            int quantity = -1;
            if(!int.TryParse(data[1], out quantity))
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
            
            entity.AddMessage(() =>
            {
                if (!entity.HasGameAction(GameActionTypeEnum.EXCHANGE))
                {
                    Logger.Debug("ExchangeFrame::Sell entity is not in an exchange : " + entity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                ((GameExchangeActionBase)entity.CurrentAction).Exchange.SellItem(entity, itemId, quantity);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void ExchangeBuy(EntityBase entity, string message)
        {
            if(!message.Contains('|'))
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var data = message.Substring(2).Split('|');

            if(data.Length != 2)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            int templateId = -1;
            if (!int.TryParse(data[0], out templateId))
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            int quantity = -1;
            if(!int.TryParse(data[1], out quantity))
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            entity.AddMessage(() =>
            {
                if (!entity.HasGameAction(GameActionTypeEnum.EXCHANGE))
                {
                    Logger.Debug("ExchangeFrame::Buy entity not in an exchange : " + entity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                ((GameExchangeActionBase)entity.CurrentAction).Exchange.BuyItem(entity, templateId, quantity);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void ExchangeMoveGold(EntityBase entity, string message)
        {
            long kamas = -1;
            if(!long.TryParse(message.Substring(3), out kamas))
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
            
            entity.AddMessage(() =>
            {
                if (!entity.HasGameAction(GameActionTypeEnum.EXCHANGE))
                {
                    Logger.Debug("ExchangeFrame::MoveGold entity not in an exchange : " + entity.Name);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                ((GameExchangeActionBase)entity.CurrentAction).Exchange.MoveKamas(entity, kamas);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void ExchangeMoveObject(EntityBase entity, string message)
        {
            if (!message.Contains('|'))
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var data = message.Substring(3).Split('|');

            if (data.Length != 2)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var add = data[0][0] == '+';
            var itemId = long.Parse(data[0].Substring(1));
            var quantity = int.Parse(data[1]);

            entity.AddMessage(() =>
            {
                if (!entity.HasGameAction(GameActionTypeEnum.EXCHANGE))
                {
                    Logger.Debug("ExchangeFrame::MoveObject entity not in an exchange : " + entity.Name);     
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }
                
                if (add)
                {
                    ((GameExchangeActionBase)entity.CurrentAction).Exchange.AddItem(entity, itemId, quantity);
                }
                else
                {
                    ((GameExchangeActionBase)entity.CurrentAction).Exchange.RemoveItem(entity, itemId, quantity);
                }
            });
        }
    }
}
