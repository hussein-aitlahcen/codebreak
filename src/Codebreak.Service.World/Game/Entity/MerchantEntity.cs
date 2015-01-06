using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Exchange;
using Codebreak.Service.World.Game.Guild;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MerchantEntity : CharacterEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CharacterEntity> Buyers
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="power"></param>
        /// <param name="characterDAO"></param>
        public MerchantEntity(CharacterDAO characterDAO)
            : base(0, characterDAO, EntityTypeEnum.TYPE_MERCHANT)
        {
            Buyers = new List<CharacterEntity>();    
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeType"></param>
        /// <returns></returns>
        public override bool CanBeExchanged(ExchangeTypeEnum exchangeType)
        {
            return exchangeType == ExchangeTypeEnum.EXCHANGE_MERCHANT;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="message"></param>
        public override void SerializeAs_GameMapInformations(OperatorEnum operation, StringBuilder message)
        {
            switch (operation)
            {
                case OperatorEnum.OPERATOR_REMOVE:
                    message.Append(Id);
                    break;

                case OperatorEnum.OPERATOR_ADD:
                case OperatorEnum.OPERATOR_REFRESH:
                    if (HasGameAction(GameActionTypeEnum.MAP))
                    {
                        message.Append(CellId).Append(';');
                        message.Append(Orientation).Append(';');
                        message.Append(0).Append(';'); // ???
                        message.Append(Id).Append(';');
                        message.Append(Name).Append(';');
                        message.Append((int)Type).Append(';');
                        message.Append(DatabaseRecord.Skin).Append('^');
                        message.Append(DatabaseRecord.SkinSize).Append(';');
                        message.Append(HexColor1).Append(';');
                        message.Append(HexColor2).Append(';');
                        message.Append(HexColor3).Append(';');
                        Inventory.SerializeAs_ActorLookMessage(message);
                        message.Append(';');
                        if (m_guildDisplayInfos != null)
                        {
                            message.Append(m_guildDisplayInfos).Append(';');
                        }
                        else
                        {
                            message.Append("").Append(';');
                            message.Append("").Append(';');
                        }
                        message.Append("0") // OffLineType
                            .Append(';');
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void SerializeAs_ShopItemsListInformations(StringBuilder message)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            Buyers.Clear();
            Buyers = null;

            base.Dispose();
        }
    }
}
