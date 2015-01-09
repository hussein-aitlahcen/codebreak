using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Auction;
using Codebreak.Service.World.Game.Exchange;
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
    public sealed class NonPlayerCharacterEntity : EntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        public override string Name
        {
            get
            {
                return "Npc_" + m_npcRecord.Template.Id + "_" + Id;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int MapId
        {
            get
            {
                return m_npcRecord.MapId;
            }
            set
            {
                m_npcRecord.MapId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int CellId
        {
            get
            {
                return m_npcRecord.CellId;
            }
            set
            {
                m_npcRecord.CellId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int Level
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override int RealLife
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override int BaseLife
        {
            get 
            { 
                return 0; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int Restriction
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string HexColor1
        {
            get
            {
                if (m_npcRecord.Template.Color3 == -1)
                    return "-1";
                return m_npcRecord.Template.Color3.ToString("x");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string HexColor2
        {
            get
            {
                if (m_npcRecord.Template.Color3 == -1)
                    return "-1";
                return m_npcRecord.Template.Color3.ToString("x");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string HexColor3
        {
            get
            {
                if (m_npcRecord.Template.Color3 == -1)
                    return "-1";
                return m_npcRecord.Template.Color3.ToString("x");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public NpcQuestionDAO InitialQuestion
        {
            get
            {
                if (m_initialQuestion == null && m_npcRecord.QuestionId != -1)
                    m_initialQuestion = NpcQuestionRepository.Instance.GetById(m_npcRecord.QuestionId);
                return m_initialQuestion;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AuctionHouseInstance AuctionHouse
        {
            get
            {
                return m_auctionInstance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TemplateId
        {
            get
            {
                return m_npcRecord.TemplateId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<ItemTemplateDAO> ShopItems
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public List<RewardEntry> Rewards
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        private AuctionHouseInstance m_auctionInstance;
        private NpcInstanceDAO m_npcRecord;
        private NpcQuestionDAO m_initialQuestion;
        private StringBuilder m_cachedEntityMapInformations, m_cachedShopListInformations;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="npcDAO"></param>
        /// <param name="id"></param>
        public NonPlayerCharacterEntity(NpcInstanceDAO npcDAO, long id)
            : base(EntityTypeEnum.TYPE_NPC, id)
        {
            m_npcRecord = npcDAO;

            Orientation = m_npcRecord.Orientation;

            Rewards = new List<RewardEntry>();
            Rewards.AddRange(npcDAO.Template.Rewards);

            ShopItems = new List<ItemTemplateDAO>();
            ShopItems.AddRange(npcDAO.Template.ShopList);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        public void SetAuctionHouse(AuctionHouseInstance instance)
        {
            m_auctionInstance = instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeType"></param>
        /// <returns></returns>
        public override bool CanBeExchanged(ExchangeTypeEnum exchangeType)
        {
            switch(exchangeType)
            {
                case ExchangeTypeEnum.EXCHANGE_NPC:
                    return Rewards.Count > 0;

                case ExchangeTypeEnum.EXCHANGE_SHOP:
                    return ShopItems.Count > 0;

                case ExchangeTypeEnum.EXCHANGE_AUCTION_HOUSE_BUY:
                case ExchangeTypeEnum.EXCHANGE_AUCTION_HOUSE_SELL:
                    return AuctionHouse != null;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="message"></param>
        public override void SerializeAs_GameMapInformations(OperatorEnum operation, StringBuilder message)
        {
            if (m_cachedEntityMapInformations == null)
            {
                m_cachedEntityMapInformations = new StringBuilder();
                m_cachedEntityMapInformations.Append(CellId).Append(';');
                m_cachedEntityMapInformations.Append(Orientation).Append(';');
                m_cachedEntityMapInformations.Append('0').Append(';'); // Unknow
                m_cachedEntityMapInformations.Append(Id).Append(';');
                m_cachedEntityMapInformations.Append(m_npcRecord.TemplateId).Append(';');
                m_cachedEntityMapInformations.Append((int)EntityTypeEnum.TYPE_NPC).Append(';');
                m_cachedEntityMapInformations.Append(m_npcRecord.Template.GfxID).Append('^');
                m_cachedEntityMapInformations.Append(m_npcRecord.Template.ScaleX).Append(';'); // size
                m_cachedEntityMapInformations.Append(m_npcRecord.Template.Sex).Append(';');
                m_cachedEntityMapInformations.Append(HexColor1 + ';' + HexColor2 + ';' + HexColor3).Append(';');
                m_cachedEntityMapInformations.Append(m_npcRecord.Template.EntityLook);
                m_cachedEntityMapInformations.Append(';');
                m_cachedEntityMapInformations.Append("").Append(';'); // ExtraClip
                m_cachedEntityMapInformations.Append(m_npcRecord.Template.CustomArtwork);
            }

            message.Append(m_cachedEntityMapInformations);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void SerializeAs_ShopItemsListInformations(StringBuilder message)
        {
            if (m_cachedShopListInformations == null)
            {
                m_cachedShopListInformations = new StringBuilder();
                foreach(var template in ShopItems)
                {
                    m_cachedShopListInformations.Append(template.Id).Append(';');
                    m_cachedShopListInformations.Append(template.Effects).Append('|');
                }
            }
            message.Append(m_cachedShopListInformations);
        }
    }
}
