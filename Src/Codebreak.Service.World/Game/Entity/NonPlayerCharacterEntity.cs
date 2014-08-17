using Codebreak.Service.World.Database.Structures;
using Codebreak.Service.World.Game.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Entity
{
    public sealed class NonPlayerCharacterEntity : EntityBase
    {
        public override string Name
        {
            get
            {
                return "Npc_" + _npcRecord.GetTemplate().Id + "_" + Id;
            }
        }

        public override int MapId
        {
            get
            {
                return _npcRecord.MapId;
            }
            set
            {
                _npcRecord.MapId = value;
            }
        }

        public override int CellId
        {
            get
            {
                return _npcRecord.CellId;
            }
            set
            {
                _npcRecord.CellId = value;
            }
        }

        public override int Level
        {
            get;
            set;
        }

        public override int RealLife
        {
            get;
            set;
        }

        public override int BaseLife
        {
            get 
            { 
                return 0; 
            }
        }

        public override int Restriction
        {
            get;
            set;
        }

        public string HexColor1
        {
            get
            {
                if (_npcRecord.GetTemplate().Color3 == -1)
                    return "-1";
                return _npcRecord.GetTemplate().Color3.ToString("x");
            }
        }
        public string HexColor2
        {
            get
            {
                if (_npcRecord.GetTemplate().Color3 == -1)
                    return "-1";
                return _npcRecord.GetTemplate().Color3.ToString("x");
            }
        }
        public string HexColor3
        {
            get
            {
                if (_npcRecord.GetTemplate().Color3 == -1)
                    return "-1";
                return _npcRecord.GetTemplate().Color3.ToString("x");
            }
        }
        private NpcInstanceDAO _npcRecord;
        private StringBuilder _cachedEntityMapInformations, _cachedShopListInformations;

        public NonPlayerCharacterEntity(NpcInstanceDAO npcDAO, long id)
            : base(EntityTypEnum.TYPE_NPC, id)
        {
            _npcRecord = npcDAO;

            Orientation = _npcRecord.Orientation;
            ShopItems.AddRange(npcDAO.GetTemplate().GetShopList());
        }

        public override bool CanBeExchanged(ExchangeTypeEnum exchangeType)
        {
            return base.CanBeExchanged(exchangeType) && exchangeType == ExchangeTypeEnum.EXCHANGE_SHOP && ShopItems.Count > 0;
        }

        public override void SerializeAs_GameMapInformations(OperatorEnum operation, StringBuilder message)
        {
            if (_cachedEntityMapInformations == null)
            {
                _cachedEntityMapInformations = new StringBuilder();
                _cachedEntityMapInformations.Append(CellId).Append(';');
                _cachedEntityMapInformations.Append(Orientation).Append(';');
                _cachedEntityMapInformations.Append('0').Append(';'); // Unknow
                _cachedEntityMapInformations.Append(Id).Append(';');
                _cachedEntityMapInformations.Append(_npcRecord.TemplateId).Append(';');
                _cachedEntityMapInformations.Append((int)EntityTypEnum.TYPE_NPC).Append(';');
                _cachedEntityMapInformations.Append(_npcRecord.GetTemplate().GfxID).Append('^');
                _cachedEntityMapInformations.Append(_npcRecord.GetTemplate().ScaleX).Append(';'); // size
                _cachedEntityMapInformations.Append(_npcRecord.GetTemplate().Sex).Append(';');
                _cachedEntityMapInformations.Append(HexColor1 + ';' + HexColor2 + ';' + HexColor3).Append(';');
                _cachedEntityMapInformations.Append(_npcRecord.GetTemplate().EntityLook);
                _cachedEntityMapInformations.Append(';');
                _cachedEntityMapInformations.Append("").Append(';'); // ExtraClip
                _cachedEntityMapInformations.Append(_npcRecord.GetTemplate().CustomArtwork);
            }

            message.Append(_cachedEntityMapInformations);
        }

        public override void SerializeAs_ShopItemsListInformations(StringBuilder message)
        {
            if (_cachedShopListInformations == null)
            {
                _cachedShopListInformations = new StringBuilder();
                foreach(var template in ShopItems)
                {
                    _cachedShopListInformations.Append(template.Id).Append(';');
                    _cachedShopListInformations.Append(template.Effects).Append('|');
                }
            }
            message.Append(_cachedShopListInformations);
        }
    }
}
