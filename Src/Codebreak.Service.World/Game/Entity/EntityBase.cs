using Codebreak.Framework.Network;
using Codebreak.Service.World.Database.Structures;
using Codebreak.Service.World.Frames;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Database.Repository;
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

namespace Codebreak.Service.World.Game.Entity
{    
    /// <summary>
    /// 
    /// </summary>
    public enum EntityTypEnum
    {
        TYPE_CHARACTER = 0,
        TYPE_MONSTER_FIGHTER = -2,
        TYPE_MONSTER_GROUP = -3,
        TYPE_NPC = -4,
        TYPE_MERCHANT = -5,
        TYPE_TAX_COLLECTOR = -6,
        TYPE_MUTANT = -8,
        TYPE_MOUNT_PARK = -9,
        TYPE_PRISM = -10,
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

    public abstract class EntityBase : MessageDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
        public FrameManager<EntityBase, string> FrameManager
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public EntityTypEnum Type
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
        public abstract int CellId
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
                RealLife = value - (Statistics.GetTotal(EffectEnum.AddVitality) + Statistics.GetTotal(EffectEnum.AddLife));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ReflectDamage
        {
            get
            {
                return ((1 + (Statistics.GetTotal(EffectEnum.AddWisdom) / 100)) * Statistics.GetTotal(EffectEnum.AddReflectDamage)) + Statistics.GetTotal(EffectEnum.AddReflectDamageItem);
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
                return (int)Math.Floor((double)(MaxLife / 4 + Statistics.GetTotal(EffectEnum.AddInitiative)) * ((double)Life / MaxLife));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Prospection
        {
            get
            {
                return (int)Math.Floor((double)(Statistics.GetTotal(EffectEnum.AddChance) / 10)) + Statistics.GetTotal(EffectEnum.AddProspection);
            }
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
        public SpellBook Spells
        {
            get;
            private set;
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
        public InventoryBag Inventory
        {
            get;
            protected set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public MapInstance Map
        {
            get
            {
                if (_map == null || _map.Id != MapId)
                    _map = MapManager.Instance.GetById(MapId);
                return _map;
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
        public GameActionBase CurrentAction
        {
            get;
            set;
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
        private MapInstance _map;
        private Dictionary<ChatChannelEnum, Func<Action<string>>> _chatByChannel;

        /// <summary>
        /// 
        /// </summary>
        public EntityBase(EntityTypEnum type, long id)
        {
            Id = id;
            Type = type;
            Orientation = 1;

            _chatByChannel = new Dictionary<ChatChannelEnum, Func<Action<string>>>();
            ShopItems = new List<ItemTemplateDAO>();
            FrameManager = new FrameManager<EntityBase, string>(this);
            Spells = new SpellBook(Id, SpellBookEntryRepository.Instance.GetSpellEntries(id));

            // set channels
            _chatByChannel.Add(ChatChannelEnum.CHANNEL_GENERAL, () => MovementHandler.Dispatch);
            _chatByChannel.Add(ChatChannelEnum.CHANNEL_RECRUITMENT, () => Map == null ? default(Action<string>) : Map.SubArea.Area.SuperArea.Dispatch);
            _chatByChannel.Add(ChatChannelEnum.CHANNEL_DEALING, () => Map == null ? default(Action<string>) : Map.SubArea.Area.SuperArea.Dispatch);
            _chatByChannel.Add(ChatChannelEnum.CHANNEL_ADMIN, () => null);
            _chatByChannel.Add(ChatChannelEnum.CHANNEL_ALIGNMENT, () => null);
            _chatByChannel.Add(ChatChannelEnum.CHANNEL_GROUP, () => null);
            _chatByChannel.Add(ChatChannelEnum.CHANNEL_GUILD, () => null);
            _chatByChannel.Add(ChatChannelEnum.CHANNEL_TEAM, () => null);
            _chatByChannel.Add(ChatChannelEnum.CHANNEL_PRIVATE_RECEIVE, () => this.Dispatch);
            _chatByChannel.Add(ChatChannelEnum.CHANNEL_PRIVATE_SEND, () => this.Dispatch);

            if (HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_IS_TOMBESTONE))
            {
                SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_EXCHANGE, true);
                SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_USE_OBJECT, true);
                SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_USE_IO, true);
                SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_ASSAULT, true);
                SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CAN_ATTACK, false);
                SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CAN_ATTACK_DUNGEON_MONSTERS_WHEN_MUTANT, false);
                SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CAN_ATTACK_MONSTERS_ANYWHERE_WHEN_MUTANT, false);
                SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_BE_MERCHANT, true);
                SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_CHALLENGE, true);
                SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_INTERACT_WITH_PRISM, true);
                SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CANT_INTERACT_WITH_TAX_COLLECTOR, true);
                SetPlayerRestriction(PlayerRestrictionEnum.RESTRICTION_CAN_MOVE_IN_ALL_DIRECTIONS, false);
                
                SetEntityRestriction(EntityRestrictionEnum.RESTRICTION_CANT_BE_ASSAULT, true);
                SetEntityRestriction(EntityRestrictionEnum.RESTRICTION_CANT_BE_ATTACK, true);
                SetEntityRestriction(EntityRestrictionEnum.RESTRICTION_CANT_BE_CHALLENGE, true);
                SetEntityRestriction(EntityRestrictionEnum.RESTRICTION_CANT_EXCHANGE, true);
                SetEntityRestriction(EntityRestrictionEnum.RESTRICTION_CANT_SWITCH_TOCREATURE, true);                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelType"></param>
        /// <param name="channel"></param>
        public void SetChatChannel(ChatChannelEnum channelType, Func<Action<string>> channel)
        {
            _chatByChannel[channelType] = channel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <param name="remoteEntity"></param>
        public virtual void DispatchChatMessage(ChatChannelEnum channel, string message, EntityBase remoteEntity = null)
        {
            var raiser = _chatByChannel[channel];
            if (raiser != null)
            {
                var chan = raiser();
                if (chan != null)
                {
                    switch (channel)
                    {
                        case ChatChannelEnum.CHANNEL_PRIVATE_SEND:
                        case ChatChannelEnum.CHANNEL_PRIVATE_RECEIVE:
                            chan(WorldMessage.CHAT_MESSAGE(channel, remoteEntity.Id, remoteEntity.Name, message));
                            break;

                        default:
                            chan(WorldMessage.CHAT_MESSAGE(channel, Id, Name, message));
                            break;
                    }
                }
            }
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
                case GameActionTypeEnum.CHALLENGE_ACCEPT:
                    return CurrentAction != null && CurrentAction.Type == GameActionTypeEnum.CHALLENGE_REQUEST && ((GameChallengeRequestAction)CurrentAction).Entity.Id != Id;

                case GameActionTypeEnum.CHALLENGE_DECLINE:
                    return CurrentAction != null && CurrentAction.Type == GameActionTypeEnum.CHALLENGE_REQUEST;

                case GameActionTypeEnum.EXCHANGE:
                    return CanExchange();
            }
            return CurrentAction == null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeType"></param>
        /// <returns></returns>
        public virtual bool CanBeExchanged(ExchangeTypeEnum exchangeType)
        {
            return !HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_CANT_EXCHANGE) && CurrentAction == null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeType"></param>
        /// <returns></returns>
        public virtual bool CanExchange()
        {
            return !HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_CANT_EXCHANGE) && CurrentAction == null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public bool HasGameAction(GameActionTypeEnum actionType)
        {
            if (actionType == GameActionTypeEnum.MAP)
                return MovementHandler.FieldType == FieldTypeEnum.TYPE_MAP;
            if (actionType == GameActionTypeEnum.FIGHT)
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
                    if (!HasEntityRestriction(EntityRestrictionEnum.RESTRICTION_IS_TOMBESTONE))
                    {
                        FrameManager.AddFrame(GameMapFrame.Instance);
                        FrameManager.AddFrame(InventoryFrame.Instance);
                        FrameManager.AddFrame(ExchangeFrame.Instance);
                        FrameManager.AddFrame(GameActionFrame.Instance);
                    }
                    Map.SpawnEntity(this);
                    break;
                    
                case GameActionTypeEnum.EXCHANGE:
                case GameActionTypeEnum.GUILD_CREATE:
                    FrameManager.RemoveFrame(GameActionFrame.Instance);
                    FrameManager.RemoveFrame(InventoryFrame.Instance);
                    FrameManager.RemoveFrame(GameMapFrame.Instance);
                    break;

                case GameActionTypeEnum.MAP_MOVEMENT:
                    MovementHandler.Dispatch(WorldMessage.GAME_ACTION(actionType, Id, CurrentAction.SerializeAs_GameAction()));
                    break;

                case GameActionTypeEnum.MAP_TELEPORT:
                    Dispatch(WorldMessage.GAME_ACTION(actionType, Id));
                    StopAction(GameActionTypeEnum.MAP);
                    StopAction(GameActionTypeEnum.MAP_TELEPORT);
                    Map.AddUpdatable(this);
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
                case GameActionTypeEnum.MAP_TELEPORT:
                    FrameManager.AddFrame(GameInformationFrame.Instance);
                    Dispatch(WorldMessage.GAME_DATA_MAP(MapId, Map.CreateTime, Map.DataKey));
                    break;

                case GameActionTypeEnum.EXCHANGE:
                    FrameManager.AddFrame(GameActionFrame.Instance);
                    FrameManager.AddFrame(InventoryFrame.Instance);
                    FrameManager.AddFrame(GameMapFrame.Instance);
                    break;

                case GameActionTypeEnum.MAP:
                    FrameManager.RemoveFrame(GameMapFrame.Instance);
                    FrameManager.RemoveFrame(InventoryFrame.Instance);
                    FrameManager.RemoveFrame(GameActionFrame.Instance);
                    FrameManager.RemoveFrame(ExchangeFrame.Instance);
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
                    FrameManager.RemoveFrame(GameMapFrame.Instance);
                    FrameManager.RemoveFrame(InventoryFrame.Instance);
                    FrameManager.RemoveFrame(GameActionFrame.Instance);
                    FrameManager.RemoveFrame(ExchangeFrame.Instance);
                    Map.DestroyEntity(this);
                    break;

                case GameActionTypeEnum.EXCHANGE:                    
                    FrameManager.AddFrame(GameActionFrame.Instance);
                    FrameManager.AddFrame(InventoryFrame.Instance);
                    FrameManager.AddFrame(GameMapFrame.Instance);
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
        /// <param name="message"></param>
        public abstract void SerializeAs_GameMapInformations(OperatorEnum operation, StringBuilder message);
        public abstract void SerializeAs_ShopItemsListInformations(StringBuilder message);
    }
}
