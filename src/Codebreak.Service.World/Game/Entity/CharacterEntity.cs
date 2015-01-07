using Codebreak.Framework.Network;
using Codebreak.Service.World.Command;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Frame;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Exchange;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Guild;
using Codebreak.Service.World.Game.Interactive.Type;
using Codebreak.Service.World.Game.Job;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Game.Stats;
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
    public class CharacterEntity : FighterBase, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public delegate void OnKick();

        /// <summary>
        /// 
        /// </summary>
        public event OnKick KickEvent;

        /// <summary>
        /// 
        /// </summary>
        public FrameManager<CharacterEntity, string> FrameManager
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Name
        {
            get
            {
                return DatabaseRecord.Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SavedMapId
        {
            get
            {
                return DatabaseRecord.SavedMapId;
            }
            set
            {
                DatabaseRecord.SavedMapId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int MapId
        {
            get
            {
                return DatabaseRecord.MapId;
            }
            set
            {
                DatabaseRecord.MapId = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public override int CellId
        {
            get
            {
                return DatabaseRecord.CellId;
            }
            set
            {
                DatabaseRecord.CellId = value;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override int Level
        {
            get
            {
                return DatabaseRecord.Level;
            }
            set
            {
                DatabaseRecord.Level = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int Restriction
        {
            get
            {
                return DatabaseRecord.Restriction;
            }
            set
            {
                DatabaseRecord.Restriction = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override long Kamas
        {
            get
            {
                return DatabaseRecord.Kamas;
            }
            set
            {
                DatabaseRecord.Kamas = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CaractPoint
        {
            get
            {
                return DatabaseRecord.CaracPoint;
            }
            set
            {
                DatabaseRecord.CaracPoint = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SpellPoint
        {
            get
            {
                return DatabaseRecord.SpellPoint;
            }
            set
            {
                DatabaseRecord.SpellPoint = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long Experience
        {
            get
            {
                return DatabaseRecord.Experience;
            }
            set
            {
                DatabaseRecord.Experience = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long ExperienceFloorCurrent
        {
            get
            {
                return ExperienceManager.Instance.GetFloor(Level, ExperienceTypeEnum.CHARACTER);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long ExperienceFloorNext
        {
            get
            {
                return ExperienceManager.Instance.GetFloor(Level + 1, ExperienceTypeEnum.CHARACTER); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int RealLife
        {
            get
            {
                return DatabaseRecord.Life;
            }
            set
            {
                DatabaseRecord.Life = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Energy
        {
            get
            {
                return DatabaseRecord.Energy;
            }
            set
            {
                DatabaseRecord.Energy = value;
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
                return DatabaseRecord.GetHexColor1();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string HexColor2
        {
            get
            {
                return DatabaseRecord.GetHexColor2();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string HexColor3
        {
            get
            {
                return DatabaseRecord.GetHexColor3();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int SkinBase
        {
            get
            {
                return DatabaseRecord.Skin;
            }
            set
            {
                DatabaseRecord.Skin = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int SkinSizeBase
        {
            get
            {
                return DatabaseRecord.SkinSize;
            }
            set
            {
                DatabaseRecord.SkinSize = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CharacterBreedEnum Breed
        {
            get
            {
                return (CharacterBreedEnum)DatabaseRecord.Breed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Sex
        {
            get
            {
                return DatabaseRecord.Sex ? 1 : 0;
            }
            set
            {
                DatabaseRecord.Sex = value == 1 ? true : false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Dead
        {
            get
            {
                return DatabaseRecord.Dead;
            }
            set
            {
                DatabaseRecord.Dead = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CharacterDAO DatabaseRecord
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long AccountId
        {
            get
            {
                return DatabaseRecord.AccountId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public GuildMember CharacterGuild
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CharacterAlignmentDAO CharacterAlignment
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public JobBook CharacterJobs
        {
            get;
            private set;
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
        
        /// <summary>
        /// 
        /// </summary>
        public override bool TurnReady
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool TurnPass
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long PartyId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long PartyInvitedPlayerId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long PartyInviterPlayerId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long GuildInvitedPlayerId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long GuildInviterPlayerId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Power
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CharacterWaypointDAO> Waypoints
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public PersistentInventory PersonalShop
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public BankInventory Bank
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long MerchantTaxe
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Merchant
        {
            get
            {
                return DatabaseRecord.Merchant;
            }
            set
            {
                DatabaseRecord.Merchant = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TitleId
        {
            get
            {
                return DatabaseRecord.TitleId;
            }
            set
            {
                DatabaseRecord.TitleId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TitleParams
        {
            get
            {
                return DatabaseRecord.TitleParams;
            }
            set
            {
                DatabaseRecord.TitleParams = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string m_guildDisplayInfos;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="power"></param>
        /// <param name="characterDAO"></param>
        public CharacterEntity(int power, CharacterDAO characterDAO, EntityTypeEnum type = EntityTypeEnum.TYPE_CHARACTER)
            : base(type, characterDAO.Id)
        {            
            CharacterAlignment = characterDAO.GetCharacterAlignment();
                     
            Power = power;
            PartyId = -1;
            PartyInvitedPlayerId = -1;
            PartyInviterPlayerId = -1;
            GuildInvitedPlayerId = -1;
            GuildInviterPlayerId = -1;
            DatabaseRecord = characterDAO;
            
            CharacterJobs = new JobBook();
            Statistics = new GenericStats(characterDAO);
            Spells = SpellBookFactory.Instance.Create(this);
            Waypoints = CharacterWaypointRepository.Instance.GetByCharacterId(Id);
            FrameManager = new FrameManager<CharacterEntity, string>(this);
            Inventory = new CharacterInventory(this);
            Bank = BankManager.Instance.GetBankByAccountId(AccountId);
            PersonalShop = new PersistentInventory((int)EntityTypeEnum.TYPE_MERCHANT, Id);
            RefreshPersonalShopTaxe();
            
            var guildMember = GuildManager.Instance.GetMember(characterDAO.GetCharacterGuild().GuildId, Id);
            if (guildMember != null)
            {
                guildMember.CharacterConnected(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshPersonalShopTaxe()
        {
            foreach(var item in PersonalShop.Items)            
                MerchantTaxe += item.MerchantPrice * item.Quantity;            
            MerchantTaxe /= 1000;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SafeKick()
        {
            base.AddMessage(() =>
                {
                    if (KickEvent != null)
                        KickEvent();
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasSkill(SkillIdEnum id)
        {
            return HasSkill((int)id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasSkill(int id)
        {
            return CharacterJobs.HasSkill(id);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetCharacterGuild(GuildMember characterGuild)
        {
            CharacterGuild = characterGuild;
            if (CharacterGuild != null)
                m_guildDisplayInfos = CharacterGuild.Guild.Name + ";" + CharacterGuild.Guild.DisplayEmblem;
            else
                m_guildDisplayInfos = null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <param name="remoteEntity"></param>
        public override void DispatchChatMessage(ChatChannelEnum channel, string message, EntityBase remoteEntity = null)
        {
            switch(channel)
            {
                case ChatChannelEnum.CHANNEL_GROUP:
                    PartyManager.Instance.PartyMessage(PartyId, Id, Name, message);
                    return;

                case ChatChannelEnum.CHANNEL_GUILD:
                    if(CharacterGuild != null)                    
                        CharacterGuild.Guild.SafeDispatchChatMessage(Id, Name, message);                    
                    return;
            }

            base.DispatchChatMessage(channel, message, remoteEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        public void GuildCreationOpen()
        {
            CurrentAction = new GameGuildCreationAction(this);
            StartAction(GameActionTypeEnum.GUILD_CREATE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="waypoint"></param>
        public void WaypointStart(Waypoint waypoint)
        {
            CurrentAction = new GameWaypointAction(this, waypoint);
            StartAction(GameActionTypeEnum.WAYPOINT);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="npc"></param>
        public void NpcDialogStart(NonPlayerCharacterEntity npc)
        {
            CurrentAction = new GameNpcDialogAction(this, npc);
            StartAction(GameActionTypeEnum.NPC_DIALOG);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="npc"></param>
        public void ExchangeNpc(NonPlayerCharacterEntity npc)
        {
            CurrentAction = new GameNpcExchangeAction(this, npc);
            StartAction(GameActionTypeEnum.EXCHANGE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollector"></param>
        public void ExchangeTaxCollector(TaxCollectorEntity taxCollector)
        {
            CurrentAction = new GameTaxCollectorExchangeAction(this, taxCollector);
            taxCollector.CurrentAction = CurrentAction;

            StartAction(GameActionTypeEnum.EXCHANGE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchant"></param>
        public void ExchangeMerchant(MerchantEntity merchant)
        {
            CurrentAction = new GameMerchantExchangeAction(this, merchant);
            StartAction(GameActionTypeEnum.EXCHANGE);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ExchangePersonalShop()
        {
            CurrentAction = new GamePersonalShopExchangeAction(this);
            StartAction(GameActionTypeEnum.EXCHANGE);
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
        public void ExchangeShop(NonPlayerCharacterEntity entity)
        {
            CurrentAction = new GameShopExchangeAction(this, entity);
            StartAction(GameActionTypeEnum.EXCHANGE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storage"></param>
        public void ExchangeStorage(PersistentInventory storage)
        {
            CurrentAction = new GameStorageExchangeAction(this, storage);
            StartAction(GameActionTypeEnum.EXCHANGE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void ExchangeAuctionHouseBuy(NonPlayerCharacterEntity entity)
        {
            CurrentAction = new GameAuctionHouseBuyAction(this, entity);
            StartAction(GameActionTypeEnum.EXCHANGE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void ExchangeAuctionHouseSell(NonPlayerCharacterEntity entity)
        {
            CurrentAction = new GameAuctionHouseSellAction(this, entity);
            StartAction(GameActionTypeEnum.EXCHANGE);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DefendTaxCollector()
        {
            CurrentAction = new GameTaxCollectorDefenderAction(this);
            StartAction(GameActionTypeEnum.TAXCOLLECTOR_AGGRESSION);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="experience"></param>
        public void AddExperience(long experience)
        {
            Experience += experience;

            var currentLevel = Level;

            if (Experience > ExperienceFloorNext)
            {
                do
                {
                    LevelUp();
                }
                while (Experience > ExperienceFloorNext && ExperienceFloorNext != -1);

                base.Dispatch(WorldMessage.CHARACTER_NEW_LEVEL(Level));
            }

            if (Level != currentLevel)
            {
                base.Dispatch(WorldMessage.SPELLS_LIST(Spells));
                base.Dispatch(WorldMessage.ACCOUNT_STATS(this));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LevelUp()
        {
            Level++;
            SpellPoint++;
            CaractPoint += 5;
            Life = MaxLife;

            if (Level == 100)
            {
                DatabaseRecord.Ap += 1;
                Statistics.AddBase(EffectEnum.AddAP, 1);
            }

            if (Spells != null)
            {
                Spells.GenerateLevelUpSpell(Breed, Level);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeType"></param>
        /// <returns></returns>
        public override bool CanBeExchanged(ExchangeTypeEnum exchangeType)
        {
            return base.CanBeExchanged(exchangeType) && (exchangeType == ExchangeTypeEnum.EXCHANGE_PLAYER || exchangeType == ExchangeTypeEnum.EXCHANGE_PERSONAL_SHOP_EDIT);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshOnMap()
        {
            if (HasGameAction(GameActionTypeEnum.MAP))
            {
                Map.SafeDispatch(WorldMessage.GAME_MAP_INFORMATIONS(OperatorEnum.OPERATOR_REFRESH, this));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionType"></param>
        public override void StartAction(GameActionTypeEnum actionType)
        {
            base.StartAction(actionType);

            switch (actionType)
            {
                case GameActionTypeEnum.MAP_TELEPORT:
                    Dispatch(WorldMessage.GAME_ACTION(actionType, Id));
                    break;

                case GameActionTypeEnum.MAP:
                    if (!HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_IS_TOMBESTONE))
                    {
                        FrameManager.AddFrame(MapFrame.Instance);
                        FrameManager.AddFrame(InventoryFrame.Instance);
                        FrameManager.AddFrame(ExchangeFrame.Instance);
                        FrameManager.AddFrame(GameActionFrame.Instance);
                    }
                    break;

                case GameActionTypeEnum.WAYPOINT:
                    FrameManager.RemoveFrame(GameActionFrame.Instance);
                    FrameManager.RemoveFrame(InventoryFrame.Instance);
                    FrameManager.RemoveFrame(MapFrame.Instance);
                    FrameManager.AddFrame(WaypointFrame.Instance);
                    break;
                    
                case GameActionTypeEnum.NPC_DIALOG:                          
                    FrameManager.RemoveFrame(GameActionFrame.Instance);
                    FrameManager.RemoveFrame(InventoryFrame.Instance);
                    FrameManager.RemoveFrame(MapFrame.Instance);
                    FrameManager.AddFrame(NpcDialogFrame.Instance);
                    break;

                case GameActionTypeEnum.TAXCOLLECTOR_AGGRESSION:
                case GameActionTypeEnum.GUILD_CREATE:
                case GameActionTypeEnum.EXCHANGE:                    
                    FrameManager.RemoveFrame(GameActionFrame.Instance);
                    FrameManager.RemoveFrame(InventoryFrame.Instance);
                    FrameManager.RemoveFrame(MapFrame.Instance);
                    break;

                case GameActionTypeEnum.FIGHT:
                    if (Fight.Map.Id != MapId)
                    {
                        Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.MAP_TELEPORT, Id));
                        Dispatch(WorldMessage.GAME_DATA_MAP(Fight.Map.Id, Fight.Map.CreateTime, Fight.Map.DataKey));
                        FrameManager.AddFrame(GameInformationFrame.Instance);
                    }
                    FrameManager.AddFrame(FightPlacementFrame.Instance);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="args"></param>
        public override void AbortAction(GameActionTypeEnum actionType, params object[] args)
        {
            base.AbortAction(actionType, args);

            switch (actionType)
            {
                case GameActionTypeEnum.MAP:
                    FrameManager.RemoveFrame(MapFrame.Instance);
                    FrameManager.RemoveFrame(GameActionFrame.Instance);
                    FrameManager.RemoveFrame(ExchangeFrame.Instance);
                    break;

                case GameActionTypeEnum.WAYPOINT:
                    FrameManager.AddFrame(GameActionFrame.Instance);
                    FrameManager.AddFrame(InventoryFrame.Instance);
                    FrameManager.AddFrame(MapFrame.Instance);
                    FrameManager.RemoveFrame(WaypointFrame.Instance);
                    break;
                    
                case GameActionTypeEnum.NPC_DIALOG:
                    FrameManager.AddFrame(GameActionFrame.Instance);
                    FrameManager.AddFrame(InventoryFrame.Instance);
                    FrameManager.AddFrame(MapFrame.Instance);
                    FrameManager.RemoveFrame(NpcDialogFrame.Instance);
                    break;

                case GameActionTypeEnum.TAXCOLLECTOR_AGGRESSION:
                case GameActionTypeEnum.GUILD_CREATE:
                case GameActionTypeEnum.EXCHANGE:
                    FrameManager.AddFrame(GameActionFrame.Instance);
                    FrameManager.AddFrame(InventoryFrame.Instance);
                    FrameManager.AddFrame(MapFrame.Instance);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="args"></param>
        public override void StopAction(GameActionTypeEnum actionType, params object[] args)
        {
            base.StopAction(actionType, args);

            switch (actionType)
            {
                case GameActionTypeEnum.MAP_TELEPORT:
                    FrameManager.AddFrame(GameInformationFrame.Instance);
                    Dispatch(WorldMessage.GAME_DATA_MAP(MapId, Map.CreateTime, Map.DataKey));
                    break;

                case GameActionTypeEnum.WAYPOINT:
                    FrameManager.AddFrame(GameActionFrame.Instance);
                    FrameManager.AddFrame(InventoryFrame.Instance);
                    FrameManager.AddFrame(MapFrame.Instance);
                    FrameManager.RemoveFrame(WaypointFrame.Instance);
                    break;
                    
                case GameActionTypeEnum.NPC_DIALOG:
                    FrameManager.AddFrame(GameActionFrame.Instance);
                    FrameManager.AddFrame(InventoryFrame.Instance);
                    FrameManager.AddFrame(MapFrame.Instance);
                    FrameManager.RemoveFrame(NpcDialogFrame.Instance);
                    break;

                case GameActionTypeEnum.TAXCOLLECTOR_AGGRESSION:
                case GameActionTypeEnum.GUILD_CREATE:
                case GameActionTypeEnum.EXCHANGE:
                    FrameManager.AddFrame(GameActionFrame.Instance);
                    FrameManager.AddFrame(InventoryFrame.Instance);
                    FrameManager.AddFrame(MapFrame.Instance);
                    break;

                case GameActionTypeEnum.MAP:
                    FrameManager.RemoveFrame(MapFrame.Instance);
                    FrameManager.RemoveFrame(GameActionFrame.Instance);
                    FrameManager.RemoveFrame(ExchangeFrame.Instance);
                    break;

                case GameActionTypeEnum.FIGHT:
                    if (!IsDisconnected)
                    {
                        WorldService.Instance.AddUpdatable(this);
                        FrameManager.AddFrame(GameCreationFrame.Instance);
                        FrameManager.RemoveFrame(FightPlacementFrame.Instance);
                        FrameManager.RemoveFrame(FightFrame.Instance);
                    }
                    break;
            }
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
                        message.Append((int)Breed);
                        if (TitleId != 0)
                        {
                            message.Append(",");
                            message.Append(TitleId).Append('*');
                            message.Append(TitleParams);//  Goule de %1 = Goule de Tamere ?
                        }
                        message.Append(';');
                        message.Append(SkinBase).Append('^');
                        message.Append(SkinSizeBase).Append(';');
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
                        if (m_guildDisplayInfos != null) // && CharacterGuild.Guild.IsActive)
                        {
                            message.Append(m_guildDisplayInfos).Append(';');
                        }
                        else
                        {
                            message.Append("").Append(';'); // GuildInfos
                            message.Append("").Append(';');
                        }
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
        public void SerializeAs_PartyMemberListInformations(StringBuilder message)
        {
            message.Append(Id).Append(';');
            message.Append(Name).Append(';');
            message.Append(SkinBase).Append(';');
            message.Append(HexColor1).Append(';');
            message.Append(HexColor2).Append(';');
            message.Append(HexColor3).Append(';');
            Inventory.SerializeAs_ActorLookMessage(message);
            message.Append(';');
            message.Append(Life).Append(',').Append(MaxLife).Append(';');
            message.Append(Level).Append(';');
            message.Append(Initiative).Append(';');
            message.Append(Prospection).Append(';');
            message.Append(0); // TODO : What is that shit ?
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
            CharacterGuild = null;
            CharacterAlignment = null;

            FrameManager.Dispose();
            FrameManager = null;

            base.Dispose();
        }
    }
}
