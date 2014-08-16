using Codebreak.WorldService.World.Action;
using Codebreak.WorldService.World.Database.Repository;
using Codebreak.WorldService.World.Database.Structure;
using Codebreak.WorldService.World.Exchange;
using Codebreak.WorldService.World.Fight;
using Codebreak.WorldService.World.Spell;
using Codebreak.WorldService.World.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Entity
{
    public sealed class CharacterEntity : FighterBase
    {
        /// <summary>
        /// 
        /// </summary>
        public override string Name
        {
            get
            {
                return _characterRecord.Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int MapId
        {
            get
            {
                return _characterRecord.MapId;
            }
            set
            {
                _characterRecord.MapId = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public override int CellId
        {
            get
            {
                return _characterRecord.CellId;
            }
            set
            {
                _characterRecord.CellId = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public override int Level
        {
            get
            {
                return _characterRecord.Level;
            }
            set
            {
                _characterRecord.Level = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int Restriction
        {
            get
            {
                return _characterRecord.Restriction;
            }
            set
            {
                _characterRecord.Restriction = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AlignmentId
        {
            get
            {
                return _alignmentRecord.AlignmentId;
            }
            set
            {
                _alignmentRecord.AlignmentId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AlignmentLevel
        {
            get
            {
                return _alignmentRecord.Level;
            }
            set
            {
                _alignmentRecord.Level = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<InventoryItemDAO> Items
        {
            get
            {
                return _characterRecord.GetItems();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AlignmentPromotion
        {
            get
            {
                return _alignmentRecord.Promotion;
            }
            set
            {
                _alignmentRecord.Promotion = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AlignmentHonour
        {
            get
            {
                return _alignmentRecord.Honour;
            }
            set
            {
                _alignmentRecord.Honour = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AlignmentDishonour
        {
            get
            {
                return _alignmentRecord.Dishonour;
            }
            set
            {
                _alignmentRecord.Dishonour = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long Kamas
        {
            get
            {
                return _characterRecord.Kamas;
            }
            set
            {
                _characterRecord.Kamas = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CaractPoints
        {
            get
            {
                return _characterRecord.CaracPoint;
            }
            set
            {
                _characterRecord.CaracPoint = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SpellPoints
        {
            get
            {
                return _characterRecord.SpellPoint;
            }
            set
            {
                _characterRecord.SpellPoint = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long Experience
        {
            get
            {
                return _characterRecord.Experience;
            }
            set
            {
                _characterRecord.Experience = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long ExperienceFloorCurrent
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long ExperienceFloorNext
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int RealLife
        {
            get
            {
                return _characterRecord.Life;
            }
            set
            {
                _characterRecord.Life = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Energy
        {
            get
            {
                return _characterRecord.Energy;
            }
            set
            {
                _characterRecord.Energy = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int BaseLife
        {
            get
            {
                return 50 + (Level * 5);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string HexColor1
        {
            get
            {
                return _characterRecord.GetHexColor1();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string HexColor2
        {
            get
            {
                return _characterRecord.GetHexColor2();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string HexColor3
        {
            get
            {
                return _characterRecord.GetHexColor3();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int Skin
        {
            get
            {
                return _characterRecord.Skin;
            }
            set
            {
                _characterRecord.SkinSize = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int SkinSize
        {
            get
            {
                return _characterRecord.SkinSize;
            }
            set
            {
                _characterRecord.SkinSize = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CharacterBreedEnum Breed
        {
            get
            {
                return (CharacterBreedEnum)_characterRecord.Breed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Sex
        {
            get
            {
                return _characterRecord.Sex ? 1 : 0;
            }
            set
            {
                _characterRecord.Sex = value == 1 ? true : false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Dead
        {
            get
            {
                return _characterRecord.Dead;
            }
            set
            {
                _characterRecord.Dead = value;
            }
        }

        public int SpellPoint
        {
            get
            {
                return _characterRecord.SpellPoint;
            }
            set
            {
                _characterRecord.SpellPoint = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Aura
        {
            get
            {
                if (Level > 199)
                    return 2;
                else if (Level > 100)
                    return 1;
                return 0;
            }
        }
        
        public override bool TurnReady
        {
            get;
            set;
        }

        public override bool TurnPass
        {
            get;
            set;
        }

        public override int SkinBase
        {
            get
            {
                return Skin;
            }
        }

        private CharacterDAO _characterRecord;
        private CharacterAlignmentDAO _alignmentRecord;

        public CharacterEntity(CharacterDAO characterDAO)
            : base(EntityTypEnum.TYPE_CHARACTER, characterDAO.Id)
        {
            _characterRecord = characterDAO;
            _alignmentRecord = characterDAO.GetAlignment();

            Statistics = new GenericStats(_characterRecord);
            Inventory = new CharacterInventory(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void ExchangePlayer(CharacterEntity player)
        {
            CurrentAction = new GamePlayerExchangeAction(this, player);
            player.CurrentAction = CurrentAction;

            StartAction(GameActionTypeEnum.EXCHANGE);
            player.StartAction(GameActionTypeEnum.EXCHANGE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defender"></param>
        public void ChallengePlayer(CharacterEntity player)
        {
            CurrentAction = new GameChallengeRequestAction(this, player);
            player.CurrentAction = CurrentAction;

            StartAction(GameActionTypeEnum.CHALLENGE_REQUEST);
            player.StartAction(GameActionTypeEnum.CHALLENGE_REQUEST);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void ExchangeShop(EntityBase entity)
        {
            CurrentAction = new GameShopExchangeAction(this, entity);

            StartAction(GameActionTypeEnum.EXCHANGE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeType"></param>
        /// <returns></returns>
        public override bool CanBeExchanged(ExchangeTypeEnum exchangeType)
        {
            return base.CanBeExchanged(exchangeType) && exchangeType == ExchangeTypeEnum.EXCHANGE_PLAYER;
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
                        message.Append(Orientation).Append(';'); ;
                        message.Append((int)Type).Append(';');
                        message.Append(Id).Append(';');
                        message.Append(Name).Append(';');
                        message.Append((int)Breed).Append(';');
                        message.Append(Skin).Append('^');
                        message.Append(SkinSize).Append(';');
                        message.Append(Sex).Append(';');
                        message.Append("0,0,0,0"); // AlignmentInfos
                        message.Append(';');
                        message.Append(HexColor1).Append(';');
                        message.Append(HexColor2).Append(';');
                        message.Append(HexColor3).Append(';');
                        Inventory.SerializeAs_ActorLookMessage(message);
                        message.Append(';');
                        message.Append(Aura).Append(';');
                        message.Append("").Append(';'); // DisplayEmotes
                        message.Append("").Append(';'); // EmotesTimer
                        //if (this.HasGuild())
                        //{
                        //    message.Append(this.GetGuild().Name).Append(';');
                        //    message.Append(this.GetGuild().Emblem).Append(';');
                        //}
                        //else
                        //{
                        message.Append("").Append(';'); // GuildInfos
                        message.Append("").Append(';');
                        //}
                        message.Append(Util.EncodeBase36(EntityRestriction))
                            .Append(';');
                        message.Append("")
                            .Append(';'); // MountLightInfos
                    }
                    else if (HasGameAction(GameActionTypeEnum.FIGHT))
                    {
                        message.Append(Cell.Id).Append(';');
                        message.Append(Orientation).Append(';'); // Direction
                        message.Append((int)Type).Append(';');
                        message.Append(Id).Append(';');
                        message.Append(Name).Append(';');
                        message.Append((int)Breed).Append(';');
                        message.Append(Skin).Append('^');
                        message.Append(SkinSize).Append(';');
                        message.Append(Sex).Append(';');
                        message.Append(Level).Append(';');
                        message.Append("0,0,0,0").Append(';'); // Alignmentnfos
                        message.Append(HexColor1).Append(';');
                        message.Append(HexColor2).Append(';');
                        message.Append(HexColor3).Append(';');
                        Inventory.SerializeAs_ActorLookMessage(message);
                        message.Append(';');
                        message.Append(Life).Append(';');
                        message.Append(AP).Append(';');
                        message.Append(MP).Append(';');
                        switch (Fight.Type)
                        {
                            case FightTypeEnum.TYPE_CHALLENGE:
                            case FightTypeEnum.TYPE_AGGRESSION:
                                message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentNeutral) + Statistics.GetTotal(EffectEnum.AddReduceDamagePercentPvPNeutral)).Append(';');
                                message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentEarth) + Statistics.GetTotal(EffectEnum.AddReduceDamagePercentPvPEarth)).Append(';');
                                message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentFire) + Statistics.GetTotal(EffectEnum.AddReduceDamagePercentPvPFire)).Append(';');
                                message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentWater) + Statistics.GetTotal(EffectEnum.AddReduceDamagePercentPvPWater)).Append(';');
                                message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentAir) + Statistics.GetTotal(EffectEnum.AddReduceDamagePercentPvPAir)).Append(';');
                                break;

                            default:
                                message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentNeutral)).Append(';');
                                message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentEarth)).Append(';');
                                message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentFire)).Append(';');
                                message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentWater)).Append(';');
                                message.Append(Statistics.GetTotal(EffectEnum.AddReduceDamagePercentAir)).Append(';');
                                break;
                        }
                        message.Append(Statistics.GetTotal(EffectEnum.AddAPDodge)).Append(';');
                        message.Append(Statistics.GetTotal(EffectEnum.AddMPDodge)).Append(';');
                        message.Append(Team.Id).Append(';');
                        message.Append("").Append(';'); // TODO Display Paddock
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
    }
}
