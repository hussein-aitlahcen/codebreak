using Codebreak.Framework.Network;
using Codebreak.Service.World.Command;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Frame;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Game.Exchange;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Game.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
using Codebreak.Framework.Generic;

namespace Codebreak.Service.World.Game.Entity
{    
    /// <summary>
    /// 
    /// </summary>
    public enum EntityTypeEnum
    {
        // SPECIAL TYPES BDD ITEMS
        TYPE_STORAGE = -22,
        TYPE_AUCTION_HOUSE = -21,
        TYPE_BANK = -20,

        // GAME TYPES
        TYPE_MOUNT = -11,
        TYPE_PRISM = -10,
        TYPE_MOUNT_PARK = -9,
        TYPE_MUTANT_PLAYER = -8,
        TYPE_MUTANT = -7,
        TYPE_TAX_COLLECTOR = -6,
        TYPE_MERCHANT = -5,
        TYPE_NPC = -4,
        TYPE_MONSTER_GROUP = -3,
        TYPE_MONSTER_FIGHTER = -2,
        TYPE_UNKNOW_MONSTER = -1,
        TYPE_CHARACTER = 0,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum PlayerRestrictionEnum
    {
        RESTRICTION_CANT_ASSAULT = 1,
        RESTRICTION_CANT_CHALLENGE = 2,
        RESTRICTION_CANT_EXCHANGE = 4,
        RESTRICTION_CAN_ATTACK = 8,
        RESTRICTION_CANT_CHAT_TO_ALL = 16,
        RESTRICTION_CANT_BE_MERCHANT = 32,
        RESTRICTION_CANT_USE_OBJECT = 64,
        RESTRICTION_CANT_INTERACT_WITH_TAX_COLLECTOR = 128,
        RESTRICTION_CANT_USE_IO = 256,
        RESTRICTION_CANT_SPEAK_NPC = 512,
        RESTRICTION_CAN_ATTACK_DUNGEON_MONSTERS_WHEN_MUTANT = 4096,
        RESTRICTION_CAN_MOVE_IN_ALL_DIRECTIONS = 8192,
        RESTRICTION_CAN_ATTACK_MONSTERS_ANYWHERE_WHEN_MUTANT = 16384,
        RESTRICTION_CANT_INTERACT_WITH_PRISM = 32768,

        RESTRICTION_NEW_CHARACTER = RESTRICTION_CAN_MOVE_IN_ALL_DIRECTIONS,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum EntityRestrictionEnum
    {
        RESTRICTION_CANT_BE_ASSAULT = 1,
        RESTRICTION_CANT_BE_CHALLENGE = 2,
        RESTRICTION_CANT_EXCHANGE = 4,
        RESTRICTION_CANT_BE_ATTACK = 8,
        RESTRICTION_FORCEWALK = 16,
        RESTRICTION_SLOWED = 32,
        RESTRICTION_CANT_SWITCH_TOCREATURE = 64,
        RESTRICTION_IS_TOMBESTONE = 128,
    }

    public abstract class AbstractEntity : MessageDispatcher, IDisposable
    { 
        /// <summary>
        /// 
        /// </summary>
        public EntityTypeEnum Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract int MapId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual long Kamas
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract int CellId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int LastCellId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Orientation
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract int Level
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract int RealLife
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract int BaseLife
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Life
        {
            get
            {
                return RealLife + Statistics.GetTotal(EffectEnum.AddVitality) + Statistics.GetTotal(EffectEnum.AddLife);
            }
            set
            {                
                RealLife = value - ((Statistics.GetTotal(EffectEnum.AddVitality) + Statistics.GetTotal(EffectEnum.AddLife)));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ReflectDamage
        {
            get
            {
                return 
                    ((1 + (Statistics.GetTotal(EffectEnum.AddWisdom) / 100))
                    * Statistics.GetTotal(EffectEnum.AddReflectDamage)) + Statistics.GetTotal(EffectEnum.AddReflectDamageItem);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxLife
        {
            get
            {
                return BaseLife + Statistics.GetTotal(EffectEnum.AddVitality) + Statistics.GetTotal(EffectEnum.AddLife);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Initiative
        {
            get
            {
                return 1 + (int)Math.Floor((
                        Statistics.GetTotal(EffectEnum.AddStrength) +
                        Statistics.GetTotal(EffectEnum.AddChance) +
                        Statistics.GetTotal(EffectEnum.AddIntelligence) +
                        Statistics.GetTotal(EffectEnum.AddAgility) +
                        Statistics.GetTotal(EffectEnum.AddInitiative)) *
                        ((double)Life / MaxLife)
                    );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Prospection
        {
            get
            {
                return 1 + (int)Math.Floor((double)(Statistics.GetTotal(EffectEnum.AddChance) / 10)) + Statistics.GetTotal(EffectEnum.AddProspection);
            }
        }
                        
        /// <summary>
        /// 
        /// </summary>
        public abstract int Restriction
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int EntityRestriction
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public MapInstance Map
        {
            get
            {
                if (m_map == null || m_map.Id != MapId)
                    m_map = MapManager.Instance.GetById(MapId);
                return m_map;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual IMovementHandler MovementHandler
        {
            get
            {
                return Map;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AbstractGameAction CurrentAction
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public EntityInventory Inventory
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public GenericStats Statistics
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public SpellBook SpellBook
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long NextMovementTime
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public long MovementInterval
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        private MapInstance m_map;
        private Dictionary<ChatChannelEnum, ChatChannelData> m_chatByChannel;

        private class ChatChannelData
        {
            public Func<Action<string>> Dispatcher
            {
                get;
                set;
            }
            public long NextTime
            {
                get;
                set;
            }
            public string LastMessage
            {
                get;
                set;
            }

            public ChatChannelData(Func<Action<string>> dispatcher)
            {
                Dispatcher = dispatcher;
                NextTime = 0;
                LastMessage = string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AbstractEntity(EntityTypeEnum type, long id)
        {
            Id = id;
            Type = type;
            Orientation = 1;

            m_chatByChannel = new Dictionary<ChatChannelEnum, ChatChannelData>();
            m_chatByChannel.Add(ChatChannelEnum.CHANNEL_GENERAL, new ChatChannelData(() => MovementHandler == null ? default(Action<string>) : MovementHandler.Dispatch));
            m_chatByChannel.Add(ChatChannelEnum.CHANNEL_RECRUITMENT, new ChatChannelData(() => Map == null ? default(Action<string>) : WorldService.Instance.Dispatcher.SafeDispatch));
            m_chatByChannel.Add(ChatChannelEnum.CHANNEL_DEALING, new ChatChannelData(() => Map == null ? default(Action<string>) : WorldService.Instance.Dispatcher.SafeDispatch));
            m_chatByChannel.Add(ChatChannelEnum.CHANNEL_ADMIN, new ChatChannelData(() => null));
            m_chatByChannel.Add(ChatChannelEnum.CHANNEL_ALIGNMENT, new ChatChannelData(() => null));
            m_chatByChannel.Add(ChatChannelEnum.CHANNEL_GROUP, new ChatChannelData(() => null));
            m_chatByChannel.Add(ChatChannelEnum.CHANNEL_GUILD,new ChatChannelData(() => null));
            m_chatByChannel.Add(ChatChannelEnum.CHANNEL_TEAM, new ChatChannelData(() => null));
            m_chatByChannel.Add(ChatChannelEnum.CHANNEL_PRIVATE_RECEIVE, new ChatChannelData(() => base.Dispatch));
            m_chatByChannel.Add(ChatChannelEnum.CHANNEL_PRIVATE_SEND, new ChatChannelData(() => base.Dispatch));            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public void SetMap(MapInstance map)
        {
            m_map = map;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelType"></param>
        /// <param name="channel"></param>
        public void SetChatChannel(ChatChannelEnum channelType, Func<Action<string>> channel)
        {
            m_chatByChannel[channelType].Dispatcher = channel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emoteId"></param>
        public virtual void EmoteUse(int emoteId, int timeout = 360000)
        {            
            Map.Dispatch(WorldMessage.EMOTE_USE(Id, emoteId, timeout));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        public void ChangeDirection(int direction)
        {
            Map.Dispatch(WorldMessage.EMOTE_DIRECTION(Id, direction));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <param name="whispedCharacter"></param>
        public virtual bool DispatchChatMessage(ChatChannelEnum channel, string message, CharacterEntity whispedCharacter = null)
        {
            var channelData = m_chatByChannel[channel];
            if (channelData == null)            
                return false;
            
            if (channel != ChatChannelEnum.CHANNEL_PRIVATE_RECEIVE)
            {
                if (message.Equals(channelData.LastMessage, StringComparison.OrdinalIgnoreCase))
                {
                    base.Dispatch(WorldMessage.IM_ERROR_MESSAGE(InformationEnum.ERROR_CHAT_SAME_MESSAGE));
                    return false;
                }

                channelData.LastMessage = message;

                if (UpdateTime < channelData.NextTime)
                {
                    base.Dispatch(WorldMessage.IM_INFO_MESSAGE(InformationEnum.INFO_CHAT_SPAM_RESTRICTED, Math.Ceiling((channelData.NextTime - UpdateTime) * 0.001)));
                    return false;
                }

                var delay = WorldConfig.CHAT_RESTRICTED_DELAY.ContainsKey(channel) ? WorldConfig.CHAT_RESTRICTED_DELAY[channel] : 0;
                channelData.NextTime = UpdateTime + delay;
            }

            var dispatcher = channelData.Dispatcher();
            if (dispatcher != null)
            {
                switch (channel)
                {
                    case ChatChannelEnum.CHANNEL_PRIVATE_SEND:
                    case ChatChannelEnum.CHANNEL_PRIVATE_RECEIVE:
                        dispatcher(WorldMessage.CHAT_MESSAGE(channel, whispedCharacter.Id, whispedCharacter.Name, message));
                        break;

                    default:
                        dispatcher(WorldMessage.CHAT_MESSAGE(channel, Id, Name, message));
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public bool CanGameAction(GameActionTypeEnum actionType)
        {
            switch(actionType)
            {
                case GameActionTypeEnum.FIGHT_AGGRESSION:
                    return ((CurrentAction == null || CurrentAction.IsFinished)
                        || CurrentAction.Type == GameActionTypeEnum.MAP_MOVEMENT)
                        && !HasPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_ASSAULT)
                        && !HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_CANT_BE_ASSAULT);

                case GameActionTypeEnum.SKILL_USE:
                    return ((CurrentAction == null || CurrentAction.IsFinished) 
                        || CurrentAction.Type == GameActionTypeEnum.MAP_MOVEMENT)
                        && !HasPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_USE_IO)
                        && !HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_IS_TOMBESTONE);
                                        
                case GameActionTypeEnum.CHALLENGE_REQUEST:
                    return (CurrentAction == null || CurrentAction.IsFinished)
                        && HasGameAction(GameActionTypeEnum.MAP)
                        && !HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_CANT_BE_CHALLENGE)
                        && !HasPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_CHALLENGE)
                        && !HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_IS_TOMBESTONE);
                    
                case GameActionTypeEnum.CHALLENGE_ACCEPT:
                    return CurrentAction != null
                        && CurrentAction.Type == GameActionTypeEnum.CHALLENGE_REQUEST 
                        && ((GameChallengeRequestAction)CurrentAction).Entity.Id != Id;

                case GameActionTypeEnum.CHALLENGE_DECLINE:
                    return CurrentAction != null
                        && CurrentAction.Type == GameActionTypeEnum.CHALLENGE_REQUEST;

                case GameActionTypeEnum.EXCHANGE:
                    return (CurrentAction == null || CurrentAction.IsFinished)
                        && !HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_CANT_EXCHANGE)
                        && !HasPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_EXCHANGE)
                        && !HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_IS_TOMBESTONE);

                case GameActionTypeEnum.MAP_MOVEMENT:
                    return (CurrentAction == null || CurrentAction.IsFinished)
                        && !HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_IS_TOMBESTONE);

                case GameActionTypeEnum.FIGHT_WEAPON_USE:
                case GameActionTypeEnum.FIGHT_SPELL_LAUNCH:
                    return (CurrentAction == null || CurrentAction.IsFinished)
                        && HasGameAction(GameActionTypeEnum.FIGHT);

                case GameActionTypeEnum.MAP_TELEPORT:
                    return (CurrentAction == null || CurrentAction.IsFinished);

                case GameActionTypeEnum.NPC_DIALOG:
                    return (CurrentAction == null || CurrentAction.IsFinished)
                        && !HasPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_SPEAK_NPC);

                case GameActionTypeEnum.FIGHT_JOIN:
                case GameActionTypeEnum.FIGHT:
                    return (CurrentAction == null || CurrentAction.IsFinished)
                         && !HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_IS_TOMBESTONE)
                         && !HasPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_ASSAULT);

                case GameActionTypeEnum.TAXCOLLECTOR_AGGRESSION:
                    return (CurrentAction == null || CurrentAction.IsFinished)
                        && !HasPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_INTERACT_WITH_TAX_COLLECTOR);
            }

            return CurrentAction == null || CurrentAction.IsFinished;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeType"></param>
        /// <returns></returns>
        public virtual bool CanBeExchanged(ExchangeTypeEnum exchangeType)
        {
            return !HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_CANT_EXCHANGE) && (CurrentAction == null || CurrentAction.IsFinished);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool CanBeMoved()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public bool HasGameAction(GameActionTypeEnum actionType)
        {
            if (actionType == GameActionTypeEnum.MAP && MovementHandler != null)
                return MovementHandler.FieldType == FieldTypeEnum.TYPE_MAP && Map.GetEntity(Id) != null;
            if (actionType == GameActionTypeEnum.FIGHT && MovementHandler != null)
                return MovementHandler.FieldType == FieldTypeEnum.TYPE_FIGHT;
            return CurrentAction != null && CurrentAction.Type == actionType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public virtual void Move(MovementPath path)
        {
            CurrentAction = new GameMapMovementAction(this, path);
            StartAction(GameActionTypeEnum.MAP_MOVEMENT);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nextMap"></param>
        /// <param name="nextCell"></param>
        public virtual void Teleport(int nextMap, int nextCell)
        {
            CurrentAction = new GameMapTeleportAction(this, nextMap, nextCell);
            StartAction(GameActionTypeEnum.MAP_TELEPORT);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionType"></param>
        public virtual void StartAction(GameActionTypeEnum actionType)
        {
            if(CurrentAction != null && CurrentAction.Type == actionType)
            {
                CurrentAction.Start();
            }

            switch (actionType)
            {
                case GameActionTypeEnum.MAP:
                    Map.SpawnEntity(this);
                    break;

                case GameActionTypeEnum.SKILL_HARVEST:
                case GameActionTypeEnum.MAP_MOVEMENT:
                    MovementHandler.Dispatch(WorldMessage.GAME_ACTION(actionType, Id, CurrentAction.SerializeAs_GameAction()));
                    break;
                    
                case GameActionTypeEnum.MAP_TELEPORT:
                    StopAction(GameActionTypeEnum.MAP);
                    StopAction(GameActionTypeEnum.MAP_TELEPORT);
                    // Switch back to world context
                    WorldService.Instance.AddUpdatable(this);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionType"></param>
        public virtual void StopAction(GameActionTypeEnum actionType, params object[] args)
        {
            if (CurrentAction != null && CurrentAction.Type == actionType)
            {
                if (!CurrentAction.IsFinished)
                    CurrentAction.Stop(args);
                if(CurrentAction != null && CurrentAction.Type == actionType)
                    CurrentAction = null;
            }
            
            switch(actionType)
            {
                case GameActionTypeEnum.MAP:
                    if(Map != null)
                        Map.DestroyEntity(this);
                    break;
            }
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionType"></param>
        public virtual void AbortAction(GameActionTypeEnum actionType, params object[] args)
        {
            if (CurrentAction != null && CurrentAction.Type == actionType && CurrentAction.CanAbort)
            {
                if (!CurrentAction.IsFinished)
                    CurrentAction.Abort(args);
                if (CurrentAction != null && CurrentAction.Type == actionType)
                    CurrentAction = null;
            }

            switch(actionType)
            {
                case GameActionTypeEnum.MAP:
                    if(Map != null)
                        Map.DestroyEntity(this);
                    break;
             }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restriction"></param>
        /// <returns></returns>
        public bool HasPlayerRestriction(PlayerRestrictionEnum restriction)
        {
            return (Restriction & (int)restriction) == (int)restriction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restriction"></param>
        /// <param name="value"></param>
        public void SetPlayerRestriction(PlayerRestrictionEnum restriction, bool value)
        {
            if (value)
            {
                if (!HasPlayerRestriction(restriction))
                {
                    Restriction |= (int)restriction;
                }
            }
            else
            {
                if (HasPlayerRestriction(restriction))
                {
                    Restriction ^= (int)restriction;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restriction"></param>
        /// <returns></returns>
        public bool HasEntityRestriction(EntityRestrictionEnum restriction)
        {
            return (EntityRestriction & (int)restriction) == (int)restriction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restriction"></param>
        /// <param name="value"></param>
        public void SetEntityRestriction(EntityRestrictionEnum restriction, bool value)
        {
            if (value)
            {
                if (!HasEntityRestriction(restriction))
                {
                    EntityRestriction |= (int)restriction;
                }
            }
            else
            {
                if (HasEntityRestriction(restriction))
                {
                    EntityRestriction ^= (int)restriction;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            CurrentAction = null;

            if (Statistics != null)
            {
                Statistics.Dispose();
                Statistics = null;
            }

            if (SpellBook != null)
            {
                SpellBook.Dispose();
                SpellBook = null;
            }

            if (Inventory != null)
            {
                Inventory.Dispose();
                Inventory = null;
            }

            m_chatByChannel.Clear();
            m_chatByChannel = null;
            m_map = null;

            base.Dispose();
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public abstract void SerializeAs_GameMapInformations(OperatorEnum operation, StringBuilder message);
    }
}
