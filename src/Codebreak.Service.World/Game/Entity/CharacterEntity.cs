using Codebreak.Framework.Generic;
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
    public enum EmoteTypeEnum
    {
        Sit = 1,
        Bye = 2,
        Applause = 4,
        Angry = 8,
        Fear = 16,
        Weapon = 32,
        Flute = 64,
        Pet = 128,
        Hello = 256,
        Kiss = 512,
        Stone = 1024,
        Sheet = 2048,
        Scissors = 4096,
        CrossArm = 8192,
        Point = 16384,
        Crow = 32768,
        Rest = 262144,
        Champ = 1048576,
        PowerAura = 2097152,
        VampyrAura = 4194304,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum DeathTypeEnum
    {
        TYPE_NORMAL = 1,
        TYPE_HEROIC = 2,
    }

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
        public string Ip
        {
            get;
            set;
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
        public int SavedCellId
        {
            get
            {
                return DatabaseRecord.SavedCellId;
            }
            set
            {
                DatabaseRecord.SavedCellId = value;
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
        public int LifeBeforeFight
        {
            get;
            private set;
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
        public int EmoteCapacity
        {
            get
            {
                return DatabaseRecord.EmoteCapacity;
            }
            set
            {
                DatabaseRecord.EmoteCapacity = value;
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
                var next = ExperienceManager.Instance.GetFloor(Level + 1, ExperienceTypeEnum.CHARACTER);
                if (next == -1)
                    return Experience;
                return next;
            }
        }
              
        /// <summary>
        /// 
        /// </summary>
        public int AlignmentId
        {
            get
            {
                return Alignment.AlignmentId;
            }
            set
            {
                Alignment.AlignmentId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Honour
        {
            get
            {
                return Alignment.Honour;
            }
            set
            {
                Alignment.Honour = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Dishonour
        {
            get
            {
                return Alignment.Dishonour;
            }
            set
            {
                Alignment.Dishonour = value;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int AlignmentLevel
        {
            get
            {
                return Alignment.Level;
            }
            set
            {
                Alignment.Level = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AlignmentPromotion
        {
            get
            {
                return Alignment.Promotion;
            }
            set
            {
                Alignment.Promotion = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AlignmentEnabled
        {
            get
            {
                return Alignment.Enabled;
            }
            set
            {
                Alignment.Enabled = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long AlignmentExperienceFloorNext
        {
            get
            {
                var next = ExperienceManager.Instance.GetFloor(AlignmentLevel + 1, ExperienceTypeEnum.PVP);
                if (next == -1)
                    return Honour;
                return next;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long AlignmentExperienceFloorCurrent
        {
            get
            {
                return ExperienceManager.Instance.GetFloor(AlignmentLevel, ExperienceTypeEnum.PVP);
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
                return DatabaseRecord.HexColor1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string HexColor2
        {
            get
            {
                return DatabaseRecord.HexColor2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string HexColor3
        {
            get
            {
                return DatabaseRecord.HexColor3;
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
        public int BreedId
        {
            get
            {
                return DatabaseRecord.Breed;
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
        public int DeathCount
        {
            get
            {
                return DatabaseRecord.DeathCount;
            }
            set
            {
                DatabaseRecord.DeathCount = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxLevel
        {
            get
            {
                return DatabaseRecord.MaxLevel;
            }
            set
            {
                DatabaseRecord.MaxLevel = value;
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
        public GuildMember GuildMember
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CharacterAlignmentDAO Alignment
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
        public AccountTicket Account
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Pseudo
        {
            get
            {
                if (Account == null)
                    return "[No Account ?]";
                return Account.Pseudo;
            }
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
        public IEnumerable<SocialRelationDAO> Friends
        {
            get
            {
                return Relations.Where(relation => relation.Type == SocialRelationTypeEnum.TYPE_FRIEND);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<SocialRelationDAO> Ennemies
        {
            get
            {
                return Relations.Where(relation => relation.Type == SocialRelationTypeEnum.TYPE_ENNEMY);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<SocialRelationDAO> Relations
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool NotifyOnFriendConnection
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int AutomaticSkillId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int AutomaticSkillCellId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int AutomaticSkillMapId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsGhost
        {
            get
            {
                return SkinBase == WorldConfig.GHOST_SKIN_ID;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsTombestone
        {
            get
            {
                return SkinBase == (BreedId * 10) + 3;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DeathTypeEnum DeathType
        {
            get
            {
                return (DeathTypeEnum)DatabaseRecord.DeathType;
            }
            set
            {
                DatabaseRecord.DeathType = (int)value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Away
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        protected string m_guildDisplayInfos;
        protected long m_lastRegenTime;
        protected double m_regenTimer;
        protected int m_lastEmoteId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="power"></param>
        /// <param name="characterDAO"></param>
        public CharacterEntity(AccountTicket account, CharacterDAO characterDAO, EntityTypeEnum type = EntityTypeEnum.TYPE_CHARACTER)
            : base(type, characterDAO.Id)
        {
            Away = false;
            DatabaseRecord = characterDAO;
            Alignment = characterDAO.Alignment;

            Account = account;
            PartyId = -1;
            PartyInvitedPlayerId = -1;
            PartyInviterPlayerId = -1;
            GuildInvitedPlayerId = -1;
            GuildInviterPlayerId = -1;
            NotifyOnFriendConnection = true;

            m_lastRegenTime = -1;
            m_lastEmoteId = -1;

            CharacterJobs = new JobBook(this);
            Statistics = new GenericStats(characterDAO);
            SpellBook = SpellBookFactory.Instance.Create(this);
            Waypoints = CharacterWaypointRepository.Instance.GetByCharacterId(Id);
            FrameManager = new FrameManager<CharacterEntity, string>(this);
            Inventory = new CharacterInventory(this);
            Bank = BankManager.Instance.GetBankByAccountId(AccountId);
            PersonalShop = new PersistentInventory((int)EntityTypeEnum.TYPE_MERCHANT, Id);
            Relations = SocialRelationRepository.Instance.GetByAccountId(AccountId);

            RefreshPersonalShopTaxe();
            
            var guildMember = GuildManager.Instance.GetMember(characterDAO.Guild.GuildId, Id);
            if (guildMember != null)
                if (type == EntityTypeEnum.TYPE_CHARACTER)
                    guildMember.CharacterConnected(this);
                else
                    SetCharacterGuild(guildMember); // Merchant

            base.SetChatChannel(ChatChannelEnum.CHANNEL_GUILD, () => DispatchGuildMessage);
            base.SetChatChannel(ChatChannelEnum.CHANNEL_GROUP, () => DispatchPartyMessage);

            CheckRestrictions();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendAccountStats()
        {
            base.Dispatch(WorldMessage.ACCOUNT_STATS(this));
        }

        /// <summary>
        /// 
        /// </summary>
        public void CheckRestrictions()
        {
            if (IsTombestone)
            {
                base.SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_EXCHANGE, true);
                base.SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_USE_OBJECT, true);
                base.SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_USE_IO, true);
                base.SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_ASSAULT, true);
                base.SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CAN_ATTACK, false);
                base.SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CAN_ATTACK_DUNGEON_MONSTERS_WHEN_MUTANT, false);
                base.SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CAN_ATTACK_MONSTERS_ANYWHERE_WHEN_MUTANT, false);
                base.SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_BE_MERCHANT, true);
                base.SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_CHALLENGE, true);
                base.SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_INTERACT_WITH_PRISM, true);
                base.SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_INTERACT_WITH_TAX_COLLECTOR, true);
                base.SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CAN_MOVE_IN_ALL_DIRECTIONS, false);
                
                base.SetEntityRestriction(EntityRestrictionEnum.RESTRICTION_IS_TOMBESTONE, true);

                base.SafeDispatch(WorldMessage.GAME_MESSAGE(GamePopupTypeEnum.TYPE_INSTANT, GameMessageEnum.MESSAGE_TOMBESTONE));
            }
            else if (IsGhost)
            {
                base.SetEntityRestriction(EntityRestrictionEnum.RESTRICTION_IS_TOMBESTONE, false);
                base.SetEntityRestriction(EntityRestrictionEnum.RESTRICTION_SLOWED, true);
                base.SetEntityRestriction(EntityRestrictionEnum.RESTRICTION_FORCEWALK, true);

                base.SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_USE_IO, false);
                base.SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CAN_MOVE_IN_ALL_DIRECTIONS, true);

                base.SafeDispatch(WorldMessage.GAME_MESSAGE(GamePopupTypeEnum.TYPE_INSTANT, GameMessageEnum.MESSAGE_TRANSFORMED_TO_GHOST_NEED_PHEONIX));
            }

            base.SafeDispatch(WorldMessage.ACCOUNT_RESTRICTIONS(Restriction));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reborn()
        {            
            SkinBase = (BreedId * 10) + Sex;
            Energy = 1000;
            Restriction = (int)PlayerRestrictionEnum.RESTRICTION_NEW_CHARACTER;
            EntityRestriction = 0;

            RefreshOnMap();

            base.CachedBuffer = true;
            base.Dispatch(WorldMessage.ACCOUNT_RESTRICTIONS(Restriction));
            base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_JUST_REBORN));
            base.Dispatch(WorldMessage.ACCOUNT_STATS(this));
            base.CachedBuffer = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void FreeSoul()
        {
            switch(DeathType)
            {
                case DeathTypeEnum.TYPE_NORMAL:
                    SkinBase = WorldConfig.GHOST_SKIN_ID;
                    CheckRestrictions();
                    if (!DisableAlignment())
                        RefreshOnMap();
                    break;

                case DeathTypeEnum.TYPE_HEROIC:
                    Dead = true;
                    DeathCount++;
                    if(Level > MaxLevel)
                        MaxLevel = Level;
                    base.Dispatch(WorldMessage.GAME_OVER());
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnLoseFight(DeathTypeEnum type)
        {
            DeathType = type;            
            Life = 1;

            switch(type)
            {
                case DeathTypeEnum.TYPE_HEROIC:
                    Energy = 1;
                    LoseEnergy();
                    break;

                case DeathTypeEnum.TYPE_NORMAL:
                    LoseEnergy();
                    if (Energy > 0)
                    {
                        // get back home if u still have energy babe
                        MapId = SavedMapId;
                        CellId = SavedCellId;
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void LoseEnergy()
        {
            var energyLost = Math.Min(Energy, Level * 10);
            if (energyLost < 1)
                return;

            Energy -= energyLost;
            base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_ENERGY_LOST, energyLost));

            if (Energy == 0)
            {
                SkinBase = (BreedId * 10) + 3;
                CheckRestrictions();
            }
            else if (Energy < 1000)
            {
                base.Dispatch(WorldMessage.GAME_MESSAGE(GamePopupTypeEnum.TYPE_INSTANT, GameMessageEnum.MESSAGE_ENERGY_LOW, Energy));
            }
        }
               
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        /// <param name="team"></param>
        public override void JoinFight(FightBase fight, FightTeam team)
        {
            LifeBeforeFight = Life;

            base.JoinFight(fight, team);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        public virtual void JoinSpectator(FightBase fight)
        {
            Fight = fight;
            IsSpectating = true;

            Fight.SpectatorTeam.AddSpectator(this);
            Fight.SpectatorTeam.AddUpdatable(this);
            Fight.SpectatorTeam.AddHandler(Dispatch);

            SetChatChannel(ChatChannelEnum.CHANNEL_TEAM, () => Fight.SpectatorTeam.Dispatch);
            SetChatChannel(ChatChannelEnum.CHANNEL_GENERAL, () => null);

            StartAction(GameActionTypeEnum.FIGHT);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override FightActionResultEnum EndTurn()
        {
            if (IsDisconnected)
            {
                if (DisconnectedTurnLeft == 0)
                {
                    Fight.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_FIGHTER_KICKED_DUE_TO_DISCONNECTION, Name));

                    if (Fight.FightQuit(this) == FightActionResultEnum.RESULT_END)
                        return FightActionResultEnum.RESULT_END;
                }
                else
                {
                    Fight.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_FIGHT_DISCONNECT_TURN_REMAIN, Name, DisconnectedTurnLeft));
                }

                DisconnectedTurnLeft--;
            }

            return base.EndTurn();
        }

        /// <summary>
        /// 
        /// </summary>
        public void LeaveFight(bool kicked = false)
        {
            if (IsSpectating)
            {
                Fight.SpectatorTeam.RemoveSpectator(this);
                Fight.SpectatorTeam.RemoveUpdatable(this);
                Fight.SpectatorTeam.RemoveHandler(Dispatch);
            }
            else
            {
                Team.RemoveFighter(this);
                Team.RemoveUpdatable(this);
                Team.RemoveHandler(Dispatch);

                if (!kicked)
                {
                    Fight.Result.AddResult(this, FightEndTypeEnum.END_LOSER, true);
                    switch (Fight.Type)
                    {
                        case FightTypeEnum.TYPE_CHALLENGE:
                            break;

                        case FightTypeEnum.TYPE_AGGRESSION:
                        case FightTypeEnum.TYPE_PVT:
                            OnLoseFight(DeathTypeEnum.TYPE_NORMAL);
                            break;

                        case FightTypeEnum.TYPE_PVM:
                            OnLoseFight(DeathTypeEnum.TYPE_HEROIC);
                            break;
                    }
                }

                Fight.TurnProcessor.RemoveFighter(this);
            }

            base.EndFight();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="win"></param>
        public override void EndFight(bool win = false)
        {
            if (!IsSpectating)
            {
                if (IsFighterDead)
                {
                    switch (Fight.Type)
                    {
                        case FightTypeEnum.TYPE_AGGRESSION:
                        case FightTypeEnum.TYPE_PVM:
                        case FightTypeEnum.TYPE_PVT:
                            Life = 1;
                            break;
                    }
                }

                if (!win)
                {
                    switch (Fight.Type)
                    {
                        case FightTypeEnum.TYPE_AGGRESSION:
                        case FightTypeEnum.TYPE_PVT:
                            OnLoseFight(DeathTypeEnum.TYPE_NORMAL);
                            break;

                        case FightTypeEnum.TYPE_PVM:
                            OnLoseFight(DeathTypeEnum.TYPE_HEROIC);
                            break;
                    }
                }

                switch (Fight.Type)
                {
                    case FightTypeEnum.TYPE_CHALLENGE:
                        Life = LifeBeforeFight;
                        break;
                }
                
                base.CachedBuffer = true;
                var items = Inventory.Items.FindAll(item => item.IsBoostEquiped);
                foreach (var item in items)
                {
                    if (item.Statistics.HasEffect(EffectEnum.AddBoost))
                    {
                        var effect = item.Statistics.GetEffect(EffectEnum.AddBoost);
                        effect.Value3--;
                        item.SaveStats();
                        if (effect.Value3 <= 0)
                            Inventory.RemoveItem(item.Id);
                    }
                }
                if (items.Count > 0)
                {
                    base.Dispatch(WorldMessage.OBJECT_CHANGE(items));
                    SendAccountStats();
                }
                base.CachedBuffer = false;
            }
            else
            {
                Fight.SpectatorTeam.RemoveSpectator(this);
                Fight.SpectatorTeam.RemoveUpdatable(this);
                Fight.SpectatorTeam.RemoveHandler(Dispatch);
            }

            if (IsDisconnected)
                EntityManager.Instance.RemoveCharacter(this);

            base.EndFight(win);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emoteId"></param>
        public override void EmoteUse(int emoteId, int timeout = 360000)
        {
            if (IsTombestone || IsGhost)
                return;

            if (m_lastEmoteId == 1)            
                StopRegeneration();            
            else if (emoteId == 1)
                StartRegeneration(WorldConfig.REGEN_TIMER_SIT);

            timeout = emoteId == m_lastEmoteId ? 0 : timeout;
            m_lastEmoteId = emoteId == m_lastEmoteId ? 0 : emoteId;

            base.EmoteUse(m_lastEmoteId, timeout);
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopEmote()
        {
            if(m_lastEmoteId == 1)            
                StopRegeneration();            
            m_lastEmoteId = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartRegeneration(double timer)
        {
            if (Life >= MaxLife)
                return;
            m_regenTimer = timer;
            m_lastRegenTime = Environment.TickCount;
            base.Dispatch(WorldMessage.LIFE_RESTORE_TIME_START(timer));
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopRegeneration()
        {
            if (Life >= MaxLife || m_lastRegenTime == -1)
                return;
            var lifeRestored = (int)Math.Floor((Environment.TickCount - m_lastRegenTime) / m_regenTimer);
            if (Life + lifeRestored > MaxLife)
                lifeRestored = MaxLife - Life;
            Life += lifeRestored;
            m_lastRegenTime = -1;

            base.CachedBuffer = true;
            base.Dispatch(WorldMessage.ACCOUNT_STATS(this));
            base.Dispatch(WorldMessage.LIFE_RESTORE_TIME_FINISH(lifeRestored));
            base.CachedBuffer = false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SubstractDishonour(int value)
        {
            if (value < 1)
            {
                Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if(Dishonour < 1)
            {
                Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            Dishonour -= value;
            if (Dishonour < 0)
                Dishonour = 0;

            base.CachedBuffer = true;
            base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_ALIGNMENT_DISHONOR_DOWN, value));
            base.Dispatch(WorldMessage.ACCOUNT_STATS(this));
            base.CachedBuffer = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void AddDishonour(int value)
        {
            if (value < 1)
            {
                Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            Dishonour += value;
            if (Dishonour > 499)
                Dishonour = 500;

            base.CachedBuffer = true;
            base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_ALIGNMENT_DISHONOR_UP, value));
            base.Dispatch(WorldMessage.ACCOUNT_STATS(this));
            base.CachedBuffer = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SubstractHonour(int value)
        {
            if (value < 1)
            {
                Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if (Honour < 1)
            {
                Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var currentLevel = AlignmentLevel;
            Honour -= value;

            if (Honour < 0)
                Honour = 0;

            while (Honour < AlignmentExperienceFloorCurrent && AlignmentLevel > 1)
                AlignmentLevel--;

            base.CachedBuffer = true;
            if (currentLevel != AlignmentLevel)
            {
                base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_ALIGNMENT_RANK_DOWN, AlignmentLevel));
                RefreshOnMap();
            }
            base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_ALIGNMENT_HONOR_DOWN, value));
            base.Dispatch(WorldMessage.ACCOUNT_STATS(this));
            base.CachedBuffer = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void AddHonour(int value)
        {
            if(value < 1)
            {
                Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if(Dishonour > 0)
            {
                Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var currentLevel = Alignment.Level;
            Honour += value;

            while (Honour > AlignmentExperienceFloorNext && AlignmentLevel < 10)
                AlignmentLevel++;

            base.CachedBuffer = true;
            if (currentLevel != AlignmentLevel)
            {
                Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_ALIGNMENT_RANK_UP, AlignmentLevel));
                RefreshOnMap();
            }
            Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_ALIGNMENT_HONOR_UP, value));
            Dispatch(WorldMessage.ACCOUNT_STATS(this));
            base.CachedBuffer = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void EnableAlignment()
        {
            if(AlignmentEnabled)
            {
                Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if (AlignmentId == 0)
            {
                Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            AlignmentEnabled = true;
            base.Dispatch(WorldMessage.ACCOUNT_STATS(this));
            RefreshOnMap();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DisableAlignment(bool force = false)
        {
            if (!AlignmentEnabled)
            {
                Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return false;
            }
            
            if(Dishonour > 0)
            {
                Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return false;
            }

            AlignmentEnabled = false;
            if(!force)
                SubstractHonour((Honour / 100) * 5);
            base.Dispatch(WorldMessage.ACCOUNT_STATS(this));
            RefreshOnMap();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alignment"></param>
        public void SetAlignment(int alignmentId)
        {
            ResetAlignment(alignmentId);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetAlignment(int alignmentId = 0)
        {
            AlignmentId = alignmentId;
            AlignmentLevel = 1;
            AlignmentPromotion = 0;
            Honour = 0;
            Dishonour = 0;
            AlignmentEnabled = false;

            base.Dispatch(WorldMessage.ACCOUNT_STATS(this));
            RefreshOnMap();
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
        /// <param name="reason"></param>
        public void ServerKick(string reason = "")
        {
            SafeKick("[Server]", reason);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SafeKick(string kicker = "", string reason = "")
        {
            base.AddMessage(() =>
                {
                    if (reason != "")
                        base.Dispatch(WorldMessage.GAME_MESSAGE(GamePopupTypeEnum.TYPE_ON_DISCONNECT, GameMessageEnum.MESSAGE_KICKED, kicker, reason));

                    if (KickEvent != null)
                        KickEvent();
                });
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Disconnected()
        {
            if (HasGameAction(GameActionTypeEnum.FIGHT))
            {
                if (CurrentAction != null)
                    AbortAction(CurrentAction.Type);
                AbortAction(GameActionTypeEnum.FIGHT);
                return false;
            }

            StopRegeneration();

            if (CurrentAction != null)
                AbortAction(CurrentAction.Type, Id);
            if (HasGameAction(GameActionTypeEnum.MAP))
                AbortAction(GameActionTypeEnum.MAP);
            if (GuildMember != null)
                GuildMember.CharacterDisconnected();

            Dispose();

            if (Merchant)
            {
                WorldService.Instance.AddMessage(() =>
                {
                    var merchant = EntityManager.Instance.CreateMerchant(DatabaseRecord);
                    merchant.StartAction(GameActionTypeEnum.MAP);
                });
            }

            return true;
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
            GuildMember = characterGuild;
            if (GuildMember != null)
                m_guildDisplayInfos = GuildMember.Guild.Name + ";" + GuildMember.Guild.DisplayEmblem;            
            else
                m_guildDisplayInfos = null;            
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetAway()
        {
            Away = Away == false;

            if (Away)
                base.Dispatch(WorldMessage.IM_INFO_MESSAGE(InformationEnum.INFO_YOU_ARE_AWAY));
            else
                base.Dispatch(WorldMessage.IM_INFO_MESSAGE(InformationEnum.INFO_YOU_ARE_NOT_AWAY_ANYMORE));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <param name="whispedCharacter"></param>
        public override bool DispatchChatMessage(ChatChannelEnum channel, string message, CharacterEntity whispedCharacter = null)
        {
            if(channel == ChatChannelEnum.CHANNEL_PRIVATE_SEND)
            {
                if(whispedCharacter.Away || whispedCharacter.HasEnnemy(Pseudo))
                {
                    base.Dispatch(WorldMessage.IM_ERROR_MESSAGE(InformationEnum.ERROR_PLAYER_AWAY_MESSAGE, whispedCharacter.Name));
                    return false;
                }
                if(Away)
                {
                    base.Dispatch(WorldMessage.IM_INFO_MESSAGE(InformationEnum.INFO_YOU_ARE_AWAY_PLAYERS_CANT_RESPOND));
                }
            }
            return base.DispatchChatMessage(channel, message, whispedCharacter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pseudo"></param>
        /// <returns></returns>
        public bool HasEnnemy(string pseudo)
        {
            return Ennemies.Any(ennemy => ennemy.Pseudo.Equals(pseudo, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pseudo"></param>
        /// <returns></returns>
        public bool HasFriend(string pseudo)
        {
            return Friends.Any(friend => friend.Pseudo.Equals(pseudo, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void DispatchPartyMessage(string message)
        {
            PartyManager.Instance.PartyMessage(PartyId, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void DispatchGuildMessage(string message)
        {
            if(GuildMember != null)
            {
                GuildMember.Guild.SafeDispatch(message);
            }
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
        /// <param name="resource"></param>
        public void HarvestStart(HarvestableResource resource, int duration)
        {
            CurrentAction = new GameHarvestAction(this, resource, duration);
            StartAction(GameActionTypeEnum.SKILL_HARVEST);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="skill"></param>
        public void CraftStart(CraftPlan plan, JobSkill skill)
        {
            CurrentAction = new GameCraftPlanExchangeAction(this, plan, skill);
            StartAction(GameActionTypeEnum.EXCHANGE);
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
        public void ExchangeStorage(StorageInventory storage)
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

            while (Experience > ExperienceFloorNext)
                LevelUp();

            if (Level != currentLevel)
            {
                base.CachedBuffer = true;
                base.Dispatch(WorldMessage.CHARACTER_NEW_LEVEL(Level));
                base.Dispatch(WorldMessage.SPELLS_LIST(SpellBook));
                base.Dispatch(WorldMessage.ACCOUNT_STATS(this));
                base.CachedBuffer = false;
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

            if (SpellBook != null)
            {
                SpellBook.GenerateLevelUpSpell(Breed, Level);
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
                case GameActionTypeEnum.MAP_MOVEMENT:
                    StopEmote();
                    if (AutomaticSkillId != -1 && HasGameAction(GameActionTypeEnum.MAP))
                    {
                        var movement = CurrentAction as GameMapMovementAction;
                        movement.SkillId = AutomaticSkillId;
                        movement.SkillCellId = AutomaticSkillCellId;
                        movement.SkillMapId = AutomaticSkillMapId;

                        AutomaticSkillId = -1;
                        AutomaticSkillCellId = -1;
                        AutomaticSkillMapId = -1;
                    }
                    break;

                case GameActionTypeEnum.MAP_TELEPORT:
                    StopEmote(); 
                    base.Dispatch(WorldMessage.GAME_ACTION(actionType, Id));
                    break;

                case GameActionTypeEnum.MAP:
                    if(Map == null)
                    {
                        MapId = SavedMapId;
                        CellId = SavedCellId;
                    }
                    if (HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_IS_TOMBESTONE))                    
                        FrameManager.AddFrame(GameTombestoneFrame.Instance);
                    FrameManager.AddFrame(MapFrame.Instance);
                    FrameManager.AddFrame(InventoryFrame.Instance);
                    FrameManager.AddFrame(ExchangeFrame.Instance);
                    FrameManager.AddFrame(GameActionFrame.Instance);                    
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
                    StopEmote();
                    FrameManager.RemoveFrame(MapFrame.Instance);
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
                    base.Dispatch(WorldMessage.GAME_DATA_MAP(MapId, Map.CreateTime, Map.DataKey));
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

                        message.Append(AlignmentId).Append(',');
                        message.Append(AlignmentId).Append(',');
                        if (AlignmentEnabled)                        
                            message.Append(AlignmentLevel).Append(',');                        
                        else                        
                            message.Append('0').Append(',');                        
                        message.Append(Id + Level).Append(';');

                        message.Append(HexColor1).Append(';');
                        message.Append(HexColor2).Append(';');
                        message.Append(HexColor3).Append(';');

                        Inventory.SerializeAs_ActorLookMessage(message);
                        message.Append(';');
                        message.Append(Aura).Append(';');
                        message.Append(m_lastEmoteId).Append(';'); // DisplayEmotes
                        message.Append(360000).Append(';'); // EmotesTimer
                        if (m_guildDisplayInfos != null && GuildMember.Guild.IsActive)
                        {
                            message.Append(m_guildDisplayInfos).Append(';');
                        }
                        else
                        {
                            message.Append("").Append(';'); // GuildName
                            message.Append("").Append(';'); // GuildEmblem
                        }
                        message.Append(Util.EncodeBase36(EntityRestriction)).Append(';');
                        message.Append("").Append(';'); // MountLightInfos
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

                        message.Append(AlignmentId).Append(',');
                        message.Append(AlignmentId).Append(',');
                        if (AlignmentEnabled)
                            message.Append(AlignmentLevel).Append(',');
                        else
                            message.Append('0').Append(',');
                        message.Append(Id + Level).Append(';');

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
                        message.Append("").Append(';'); // MountLightInfos
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SerializeAs_PartyMemberInformations(StringBuilder message)
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
            message.Append(0); // 'Side' Wtf ?
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SerializeAs_EnnemyInformations(string playerPseudo, StringBuilder message)
        {
            message.Append(';');
            if (HasEnnemy(playerPseudo))
            {
                if (HasGameAction(GameActionTypeEnum.FIGHT))
                    message.Append('2').Append(';');
                else
                    message.Append('1').Append(';');
                message.Append(Name).Append(';');
                message.Append(Level).Append(';');
                message.Append(AlignmentId).Append(';');
            }
            else
            {
                message.Append("?;"); // fight state
                message.Append(Name).Append(';'); // name
                message.Append("?;"); // level
                message.Append("-1;"); // align
            }
            if (GuildMember != null)
                message.Append(GuildMember.Guild.Name).Append(';');
            else
                message.Append(';');
            message.Append(Sex).Append(';');
            message.Append(SkinBase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SerializeAs_FriendInformations(string playerPseudo, StringBuilder message)
        {
            message.Append(';');
            if (HasFriend(playerPseudo))
            {
                if (HasGameAction(GameActionTypeEnum.FIGHT))
                    message.Append('2').Append(';');
                else
                    message.Append('1').Append(';');
                message.Append(Name).Append(';');
                message.Append(Level).Append(';');
                message.Append(AlignmentId).Append(';');
            }
            else
            {
                message.Append("?;"); // fight state
                message.Append(Name).Append(';'); // name
                message.Append("?;"); // level
                message.Append("-1;"); // align
            }
            if (GuildMember != null && GuildMember.Guild != null)
                message.Append(GuildMember.Guild.Name).Append(';');
            else
                message.Append(';');
            message.Append(Sex).Append(';');
            message.Append(SkinBase);
        }
                
        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            GuildMember = null;
            Alignment = null;

            FrameManager.Dispose();
            FrameManager = null;

            base.Dispose();
        }
    }
}
