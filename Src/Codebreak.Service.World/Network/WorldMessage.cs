using Codebreak.Service.World.Database.Structures;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Exchange;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Network;
using Codebreak.WorldService;

namespace Codebreak.Service.World.Game
{
    /// <summary>
    /// 
    /// </summary>
    public enum ChatChannelEnum
    {
        CHANNEL_GENERAL = '*',
        CHANNEL_RECRUITMENT = '?',
        CHANNEL_DEALING = ':',
        CHANNEL_TEAM = '#',
        CHANNEL_ADMIN = '@',
        CHANNEL_GROUP = '$',
        CHANNEL_GUILD = '%',
        CHANNEL_ALIGNMENT = '!',
        CHANNEL_PRIVATE_SEND = 'T',
        CHANNEL_PRIVATE_RECEIVE = 'F',
    }

    /// <summary>
    /// 
    /// </summary>
    public enum InformationTypeEnum
    {
        INFO = 0,
        ERROR = 1,
        PVP = 2,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum InformationEnum
    {
        INFO_SERVER_MESSAGE = 0,
        INFO_BASIC_LAST_CONNECTION = 152, // precedente connexion ...
        INFO_BASIC_CURRENT_IP = 153, // votre ip actuelle est ...   

        INFO_FIGHT_SPECTATOR_JOINED = 36,
        INFO_FIGHT_TOGGLE_PARTY = 93,
        INFO_FIGHT_UNTOGGLE_PARTY = 94,
        INFO_FIGHT_TOGGLE_PLAYER = 95,
        INFO_FIGHT_UNTOGGLE_PLAYER = 96,
        INFO_FIGHT_TOGGLE_SPECTATOR = 40,
        INFO_FIGHT_UNTOGGLE_SPECTATOR = 39,
        INFO_FIGHT_TOGGLE_HELP = 103,
        INFO_FIGHT_UNTOGGLE_HELP = 104,
        INFO_FIGHT_DISCONNECT_TURN_REMAIN = 162,

        ERROR_WORLD_SAVING = 164,
        ERROR_WORLD_SAVING_FINISHED = 165,

        ERROR_PLAYER_AWAY_NOT_INVITABLE = 209,

        ERROR_SERVER_BETA = 225,
        ERROR_SERVER_WELCOME = 89,

        ERROR_FIGHT_SPECTATOR_LOCKED = 57,
        ERROR_FIGHTER_DISCONNECTED = 182,
        ERROR_FIGHTER_RECONNECTED = 184,
        ERROR_FIGHT_WAITING_PLAYERS = 29,
        ERROR_FIGHTER_KICKED_DUE_TO_DISCONNECTION = 30,
    }
    
    /// <summary>
    /// 
    /// </summary>
    public enum GameMessageEnum
    {
        MESSAGE_FREE_SOUL = 112,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ExchangeMoveEnum
    {
        MOVE_OBJECT = 'O',
        MOVE_GOLD = 'G',
    }

    /// <summary>
    /// 
    /// </summary>
    public enum OperatorEnum
    {
        OPERATOR_ADD = '+',
        OPERATOR_REMOVE = '-',
        OPERATOR_REFRESH = '~',
    }


    /// <summary>
    /// 
    /// </summary>
    public static class WorldMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string BASIC_NO_OPERATION()
        {
            return "BN";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spellId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static string SPELL_UPGRADE_SUCCESS(int spellId, int level)
        {
            return "SUK" + spellId + "~" + level;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string SPELL_UPGRADE_ERROR()
        {
            return "SUE";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string HELLO_GAME()
        {
            return "HG";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ACCOUNT_TICKET_ERROR()
        {
            return "ATE";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ACCOUNT_TICKET_SUCCESS()
        {
            return "ATK0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ACCOUNT_REGIONAL_VERSION()
        {
            return "AVfr";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CHARACTER_CREATION_SUCCESS()
        {
            return "AAK";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CHARACTER_CREATION_ERROR()
        {
            return "AAE";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CHARACTER_CREATION_ERROR_FULL()
        {
            return "AAEf";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CHARACTER_CREATION_ERROR_NAME_ALREADY_EXISTS()
        {
            return "AAEa";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CHARACTER_CREATION_ERROR_BAD_NAME()
        {
            return "AAEn";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CHARACTER_CREATION_ERROR_SUBSCRIPTION_OUT()
        {
            return "AAEs";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CHARACTER_DELETION_ERROR()
        {
            return "ADE";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CHARACTER_SELECTION_ERROR()
        {
            return "ASE";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string BASIC_TIME()
        {
            return "BT" + Math.Round(DateTime.Now.Subtract(WorldConfig.REFERENCE_DATE).TotalMilliseconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string BASIC_DATE()
        {
            return "BD" + (DateTime.Now.Year - 1970) + '|' + (DateTime.Now.Month - 1) + '|' + DateTime.Now.Day;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string BASIC_PONG()
        {
            return "pong";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string BASIC_QPONG()
        {
            return "qpong";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CHARACTER_LIST(List<CharacterDAO> characters)
        {
            var message = new StringBuilder("ALK31536000000");
            if (characters.Count > 0)
            {
                message.Append('|').Append(characters.Count);

                foreach (var character in characters)
                {
                    message.Append('|').Append(character.Id);
                    message.Append(';').Append(character.Name);
                    message.Append(';').Append(character.Level);
                    message.Append(';').Append(character.Skin);
                    message.Append(';').Append(character.GetHexColor1());
                    message.Append(';').Append(character.GetHexColor2());
                    message.Append(';').Append(character.GetHexColor3());
                    message.Append(';');
                    character.SerializeAs_ActorLookMessage(message);
                    message.Append(';').Append(0); // merchant
                    message.Append(';').Append(WorldConfig.GAME_ID);
                    message.Append(';').Append(character.Dead ? '1' : '0');
                    message.Append(';').Append(character.DeathCount);
                    message.Append(';').Append(character.MaxLevel);
                }
            }
            return message.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CHARACTER_SELECTION_SUCCESS(CharacterEntity character)
        {
            var message = new StringBuilder("ASK|");
            message.Append(character.Id).Append('|');
            message.Append(character.Name).Append('|');
            message.Append(character.Level).Append('|');
            message.Append((int)character.Breed).Append('|');
            message.Append(character.Sex).Append('|');
            message.Append(character.Skin).Append('|');
            message.Append(character.HexColor1).Append('|');
            message.Append(character.HexColor2).Append('|');
            message.Append(character.HexColor3).Append('|');
            character.Inventory.SerializeAs_BagContent(message);
            message.Append('|');
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public static string SPELLS_LIST(SpellBook book)
        {
            var message = new StringBuilder("SL");
            book.SerializeAs_SpellsListMessage(message);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string AREAS_LIST()
        {
            return "al|";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ACCOUNT_RIGHTS(int rights)
        {
            return "AR" + Util.EncodeBase36(rights);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string SPECIALISATION_SET(int alignmentId)
        {
            return "ZS" + alignmentId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string SPECIALISATION_CHANGE(int alignmentId)
        {
            return "ZC" + alignmentId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CHAT_ENABLED_CHANNELS()
        {
            return "cC+i*?:#@$%!TF";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string INVENTORY_WEIGHT(int pods, int maxPods)
        {
            return "Ow" + pods + "|" + maxPods;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string INFORMATION_MESSAGE(InformationTypeEnum type, InformationEnum id, params object[] args)
        {
            return "Im" + (int)type + (int)id + (args.Length > 0 ? ";" : "") + string.Join("~", args.Select(arg => arg.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GAME_CREATION_SUCCESS()
        {
            return "GCK|1|";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GAME_DATA_MAP(int mapId, string mapDate, string key)
        {
            return "GDM|" + mapId + '|' + mapDate + '|' + key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ACCOUNT_STATS(CharacterEntity character)
        {
            var message = new StringBuilder("As");
            message
                .Append(character.Experience).Append(',')                                               // CurExcperience
                .Append(character.ExperienceFloorCurrent).Append(',')     // LastExperience
                .Append(character.ExperienceFloorNext).Append('|');// NextExperience

            message.Append(character.Kamas).Append('|');
            message.Append(character.CaractPoint).Append('|');
            message.Append(character.SpellPoint).Append('|');

            message
                .Append(character.AlignmentId).Append('~')
                .Append(character.AlignmentId).Append(',')
                .Append(character.AlignmentLevel).Append(',')
                .Append(character.AlignmentPromotion).Append(',')
                .Append(character.AlignmentHonour).Append(',')
                .Append(character.AlignmentDishonour).Append(',')
                .Append(0).Append('|'); // Enabled ?

            message
                .Append(character.Life).Append(',')
                .Append(character.MaxLife).Append('|');

            message
                .Append(character.Energy).Append(',') // CurEnergy
                .Append(10000).Append('|');           // MAX_ENERGY;

            message.Append(character.Initiative).Append('|');
            message.Append(character.Prospection).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddAP).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddMP).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddStrength).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddVitality).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddWisdom).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddChance).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddAgility).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddIntelligence).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddPO).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddInvocationMax).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddDamage).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddDamagePhysic).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddDamageMagic).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddDamagePercent).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddHealCare).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddDamagePiege).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddDamagePiege).ToString()).Append('|'); /// ADD_DAMAGE_PIEGE_PERCENT
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReflectDamageItem).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddDamageCritic).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddEchecCritic).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddAPDodge).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddMPDodge).ToString()).Append('|');

            /* resistances */
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamageNeutral).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamagePercentNeutral).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamagePvPNeutral).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamagePercentPvPNeutral).ToString()).Append('|');

            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamageEarth).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamagePercentEarth).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamagePvPEarth).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamagePercentPvPEarth).ToString()).Append('|');

            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamageWater).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamagePercentWater).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamagePvPWater).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamagePercentPvPWater).ToString()).Append('|');

            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamageAir).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamagePercentAir).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamagePvPAir).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamagePercentPvPAir).ToString()).Append('|');

            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamageFire).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamagePercentFire).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamagePvPFire).ToString()).Append('|');
            message.Append(character.Statistics.GetTotalEffect(EffectEnum.AddReduceDamagePercentPvPFire).ToString()).Append('|');

            message.Append('1'); // UNKNOW

            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static string GAME_MAP_INFORMATIONS(OperatorEnum operation, params EntityBase[] entities)
        {
            var message = new StringBuilder("GM");
            foreach (var actor in entities)
            {
                message.Append("|");
                message.Append((char)operation);
                actor.SerializeAs_GameMapInformations(operation, message);
            }
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GAME_DATA_SUCCESS()
        {
            return "GDK";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="actorId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GAME_ACTION(GameActionTypeEnum type, long entityId, string args = "")
        {
            StringBuilder message = new StringBuilder("GA");

            switch (type)
            {
                case GameActionTypeEnum.CHANGE_MAP:
                case GameActionTypeEnum.MAP_MOVEMENT:
                case GameActionTypeEnum.FIGHT_SPELL_LAUNCH:
                    message.Append((int)type);
                    break;
            }

            message.Append(';');
            message.Append((int)type).Append(';');
            message.Append(entityId).Append(';');
            message.Append(args);

            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="entityId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GAME_ACTION(EffectEnum type, long entityId, string args = "")
        {
            return "GA" + ';' + (int)type + ';' + entityId + ';' + args;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="entityId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GAME_ACTION(int actionId, long entityId, string args = "")
        {
            return "GA" + ';' + (int)actionId + ';' + entityId + ';' + args;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="entityId"></param>
        /// <param name="entityName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string CHAT_MESSAGE(ChatChannelEnum channel, long entityId, string entityName, string message)
        {
            return "cMK" + (char)channel + '|' + entityId + '|' + entityName + '|' + message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="entityId"></param>
        /// <param name="entityName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string CHAT_MESSAGE_ERROR_PLAYER_OFFLINE()
        {
            return "cMEf";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static string CHARACTER_NEW_LEVEL(int level)
        {
            return "AN" + level;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GAME_MESSAGE(GameMessageEnum message)
        {
            return "M" + (int)message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GAME_OVER()
        {
            return "GO";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="cell"></param>
        /// <param name="length"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GAME_DATA_ZONE(OperatorEnum op, int cell, int length, int color)
        {
            return "GDZ" + (char)op + cell + ";" + length + ";" + color;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GAME_DATA_ZONE_CREATE(int cell, string data = "")
        {
            return "GDC" + cell + data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static string ENTITY_OBJECT_ACTUALIZE(EntityBase entity)
        {
            var message = new StringBuilder("Oa");
            message.Append(entity.Id);
            message.Append('|');
            entity.Inventory.SerializeAs_ActorLookMessage(message);

            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string OBJECT_ADD_SUCCESS(InventoryItemDAO item)
        {
            var message = new StringBuilder("OAKO");
            item.SerializeAs_BagContent(message);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string OBJECT_MOVE_ERROR()
        {
            return "OAE";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string OBJECT_MOVE_ERROR_REQUIRED_LEVEL()
        {
            return "OAEL";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string OBJECT_MOVE_ERROR_ALREADY_EQUIPED()
        {
            return "OAEA";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public static string OBJECT_MOVE_SUCCESS(long guid, ItemSlotEnum slot)
        {
            return "OM" + guid + "|" + (slot == ItemSlotEnum.SLOT_INVENTORY ? "" : ((int)slot).ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static string OBJECT_QUANTITY_ACTUALIZE(long guid, int quantity)
        {
            return "OQ" + guid + "|" + quantity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string OBJECT_REMOVE_SUCCESS(long guid)
        {
            return "OR" + guid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string OBJECT_SELL_ERROR()
        {
            return "OSE";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        public static string EXCHANGE_VALIDATE(long actorId, bool validate)
        {
            return "EK" + (validate ? '1' : '0') + actorId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="move"></param>
        /// <param name="operation"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string EXCHANGE_LOCAL_MOVEMENT(ExchangeMoveEnum move, OperatorEnum operation, string args)
        {
            return "EMK" + (char)move + (char)operation + args;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="move"></param>
        /// <param name="operation"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string EXCHANGE_DISTANT_MOVEMENT(ExchangeMoveEnum move, OperatorEnum operation, string args)
        {
            return "EmK" + (char)move + (char)operation + args;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string EXCHANGE_CREATE(ExchangeTypeEnum type, string args = "")
        {
            return "ECK" + (int)type + (args != "" ? "|" + args : "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public static string EXCHANGE_LEAVE(bool success = false)
        {
            return "EV" + (success ? "a" : "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string EXCHANGE_BUY_ERROR()
        {
            return "EBE";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string EXCHANGE_SELL_ERROR()
        {
            return "ESE";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string EXCHANGE_SELL_SUCCESS()
        {
            return "ESK";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string EXCHANGE_BUY_SUCCESS()
        {
            return "EBK";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="actorNpc"></param>
        /// <returns></returns>
        public static string EXCHANCE_ITEMS_LIST(EntityBase entity)
        {
            var message = new StringBuilder("EL");
            entity.SerializeAs_ShopItemsListInformations(message);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        public static string EXCHANGE_REQUEST(long characterId, long targetId)
        {
            return "ERK" + characterId + '|' + targetId + "|1";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        /// <returns></returns>
        public static string FIGHT_FLAG_DISPLAY(FightBase fight)
        {
            var message = new StringBuilder("Gc+");
            //fight.SerializeAs_FightFlagSpawn(message);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ope"></param>
        /// <param name="leaderId"></param>
        /// <param name="fighters"></param>
        /// <returns></returns>
        public static string FIGHT_FLAG_UPDATE(OperatorEnum ope, long leaderId, params FighterBase[] fighters)
        {
            var message = new StringBuilder("Gt").Append(leaderId);
            foreach (var fighter in fighters)
            {
                message.Append("|").Append((char)ope);
                message.Append(fighter.Id).Append(';');
                message.Append(fighter.Name).Append(';');
                message.Append(fighter.Level);
            }
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        /// <returns></returns>
        public static string FIGHT_FLAG_DESTROY(long fightId)
        {
            return "Gc-" + fightId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string FIGHT_JOIN_ERROR()
        {
            return "GJE";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fightState"></param>
        /// <param name="cancelButton"></param>
        /// <param name="challenge"></param>
        /// <param name="specator"></param>
        /// <param name="startTimeOut"></param>
        /// <returns></returns>
        public static string FIGHT_JOIN_SUCCESS(int fightState, bool cancelButton, bool challenge, bool specator, long startTimeOut)
        {
            return "GJK" + fightState + "|" + (cancelButton ? "1" : "0") + "|" + (challenge ? "1" : "0") + "|" + (specator ? "1" : "0") + "|" + (startTimeOut == -1 ? "" : startTimeOut.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="places"></param>
        /// <returns></returns>
        public static string FIGHT_AVAILABLE_PLACEMENTS(long teamId, string places)
        {
            return "GP" + places + "|" + teamId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="ready"></param>
        /// <returns></returns>
        public static string FIGHT_READY(long actorId, bool ready)
        {
            return "GR" + (ready ? "1" : "0") + actorId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighters"></param>
        /// <returns></returns>
        public static string FIGHT_COORDINATE_INFORMATIONS(params FighterBase[] fighters)
        {
            var message = new StringBuilder("GIC");            
            foreach (var fighter in fighters)
            {
                message.Append('|');
                message.Append(fighter.Id).Append(';');
                message.Append(fighter.Cell.Id).Append(';');
                message.Append('1'); // Dead ?
            }
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string FIGHT_STARTS()
        {
            return "GS";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighters"></param>
        /// <returns></returns>
        public static string FIGHT_TURN_LIST(IEnumerable<FighterBase> fighters)
        {
            var message = new StringBuilder("GTL");
            foreach (var fighter in fighters)
            {
                message.Append('|');
                message.Append(fighter.Id);
            }
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighterId"></param>
        /// <param name="turnTime"></param>
        /// <returns></returns>
        public static string FIGHT_TURN_STARTS(long fighterId, long turnTime)
        {
            return "GTS" + fighterId + "|" + turnTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighters"></param>
        /// <returns></returns>
        public static string FIGHT_TURN_MIDDLE(IEnumerable<FighterBase> fighters)
        {
            var message = new StringBuilder("GTM");

            foreach (var fighter in fighters)
            {
                message.Append('|');
                message.Append(fighter.Id).Append(';');
                message.Append(fighter.IsFighterDead ? '1' : '0').Append(';');
                message.Append(fighter.Life).Append(';');
                message.Append(fighter.AP).Append(';');
                message.Append(fighter.MP).Append(';');
                if (fighter.IsFighterDead || fighter.StateManager.HasState(FighterStateEnum.STATE_STEALTH))
                {
                    message.Append("").Append(";;");
                }
                else
                {
                    message.Append(fighter.Cell.Id).Append(";;");
                }
                message.Append(fighter.MaxLife);
            }

            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighterId"></param>
        /// <returns></returns>
        public static string FIGHT_TURN_READY(long fighterId)
        {
            return "GTR" + fighterId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighterId"></param>
        /// <returns></returns>
        public static string FIGHT_TURN_FINISHED(long fighterId)
        {
            return "GTF" + fighterId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string FIGHT_LEAVE()
        {
            return "GV";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string FIGHT_COUNT(long count)
        {
            return "fC" + count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="operation"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public static string FIGHT_OPTION(FightOptionTypeEnum type , bool blocked, long teamId)
        {
            return "Go" + (blocked ? "-" : "+") + (char)type + teamId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fights"></param>
        /// <returns></returns>
        public static string FightList(IEnumerable<FightBase> fights)
        {
            var message = new StringBuilder("fL");
            foreach (var fight in fights)            
                fight.SerializeAs_FightList(message);  
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        /// <returns></returns>
        public static string FightDetails(FightBase fight)
        {
            var message = new StringBuilder("fD");
            message.Append(fight.Id);
            message.Append('|');
            foreach (var fighter in fight.Team0.AliveFighters)
            {
                message.Append(fighter.Name);
                message.Append('~');
                message.Append(fighter.Level);
                message.Append(';');
            }
            message.Append('|');
            foreach (var fighter in fight.Team1.AliveFighters)
            {
                message.Append(fighter.Name);
                message.Append('~');
                message.Append(fighter.Level);
                message.Append(';');
            }
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="entityId"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <param name="chance"></param>
        /// <param name="duration"></param>
        /// <param name="spellId"></param>
        /// <returns></returns>
        public static string FIGHT_EFFECT_INFORMATION(EffectEnum effect, long entityId, string value1, string value2, string value3, string chance, string duration, string spellId)
        {
            return "GIE" + (int)effect + ";" + entityId + ";" + value1 + ";" + value2 + ";" + value3 + ";" + chance + ";" + duration + ";" + spellId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        public static string FIGHT_ACTION_START(long entityId)
        {
            return "GAS" + entityId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        public static string FIGHT_ACTION_FINISHED(long entityId)
        {
            return "GAF2|" + entityId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fightResult"></param>
        /// <returns></returns>
        public static string FIGHT_END_RESULT(FightEndResult fightResult)
        {
            return fightResult.Message;
        }
    }
}
