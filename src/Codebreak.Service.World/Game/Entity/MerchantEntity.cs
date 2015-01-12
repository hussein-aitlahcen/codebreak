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
        private StringBuilder m_serializedMapInformations;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="power"></param>
        /// <param name="characterDAO"></param>
        public MerchantEntity(CharacterDAO characterDAO)
            : base(null, characterDAO, EntityTypeEnum.TYPE_MERCHANT)
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
                        if (m_serializedMapInformations == null)
                        {
                            m_serializedMapInformations = new StringBuilder();
                            m_serializedMapInformations.Append(CellId).Append(';');
                            m_serializedMapInformations.Append(Orientation).Append(';');
                            m_serializedMapInformations.Append(0).Append(';'); // ???
                            m_serializedMapInformations.Append(Id).Append(';');
                            m_serializedMapInformations.Append(Name).Append(';');
                            m_serializedMapInformations.Append((int)Type).Append(';');
                            m_serializedMapInformations.Append(DatabaseRecord.Skin).Append('^');
                            m_serializedMapInformations.Append(DatabaseRecord.SkinSize).Append(';');
                            m_serializedMapInformations.Append(HexColor1).Append(';');
                            m_serializedMapInformations.Append(HexColor2).Append(';');
                            m_serializedMapInformations.Append(HexColor3).Append(';');
                            Inventory.SerializeAs_ActorLookMessage(m_serializedMapInformations);
                            m_serializedMapInformations.Append(';');
                            if (m_guildDisplayInfos != null && GuildMember.Guild.IsActive)
                            {
                                m_serializedMapInformations.Append(m_guildDisplayInfos).Append(';');
                            }
                            else
                            {
                                m_serializedMapInformations.Append("").Append(';');
                                m_serializedMapInformations.Append("").Append(';');
                            }
                            m_serializedMapInformations.Append("0") // OffLineType
                                .Append(';');
                        }
                        message.Append(m_serializedMapInformations);
                    }
                    break;
            }
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
