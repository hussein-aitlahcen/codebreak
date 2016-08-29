using Codebreak.Service.World.Database.Structure;
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
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using Codebreak.Service.World.Game.Guild;
using Codebreak.Service.World.Game.Auction;
using Codebreak.Service.World.Manager;
using Codebreak.Framework.Util;
using Codebreak.Service.World.Game.Job;
using Codebreak.Service.World.Game.Interactive;
using Codebreak.Service.World.Game.Quest;

namespace Codebreak.Service.World.Network
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

        INFO_LIFE_RECOVERED = 1,
        INFO_JOB_LEARNT = 2,

        INFO_GAVE_KAMAS_TO_OPEN = 20,

        INFO_CARACTERISTIC_UPGRADED = 15,
        INFO_WON_JOB_XP = 17,

        INFO_EXPERIENCE_GAINED = 8,
        INFO_ENERGY_RECOVERED = 7,

        INFO_WAYPOINT_SAVED = 6,
        INFO_WAYPOINT_REGISTERED = 24,

        INFO_SPELLPOINT_GAINED = 16,

        INFO_JUST_REBORN = 33,
        INFO_ENERGY_LOST = 34,

        INFO_YOU_ARE_AWAY = 37,
        INFO_YOU_ARE_NOT_AWAY_ANYMORE = 38,

        INFO_KAMAS_WON = 45,
        INFO_KAMAS_LOST = 46,

        INFOS_QUEST_NEW = 54, //"Nouvelle quête : <b>%1</b>";
        INFOS_QUEST_UPDATE = 55,//"Quête mise à jour : <b>%1</b>";
        INFOS_QUEST_END = 56, //"Quête terminée : <b>%1</b>";

        INFO_YOU_ARE_AWAY_PLAYERS_CANT_RESPOND = 72,

        INFO_ALIGNMENT_DISHONOR_UP = 75,
        INFO_ALIGNMENT_DISHONOR_DOWN = 77,

        INFO_ALIGNMENT_HONOR_UP = 80,
        INFO_ALIGNMENT_HONOR_DOWN = 76,

        INFO_ALIGNMENT_RANK_UP = 82,
        INFO_ALIGNMENT_RANK_DOWN = 83,

        INFO_BASIC_WARNING_BEFORE_SANCTION = 116,
        INFO_BASIC_LAST_CONNECTION = 152, // precedente connexion ...
        INFO_BASIC_CURRENT_IP = 153, // votre ip actuelle est ...   

        INFO_FRIEND_ONLINE = 143,

        INFO_CRAFT_FAILED = 118,

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
        INFO_FIGHT_CHALLENGE_FAILED_DUE_TO = 188,

        INFO_CHAT_SPAM_RESTRICTED = 115,

        INFO_GUILD_KICKED_HIMSELF = 176,
        INFO_GUILD_KICKED = 177,
        
        INFO_CONTRACT_FAILED = 179,
        
        INFO_AUCTION_TOO_MANY_ITEMS = 59,
        INFO_AUCTION_RARE = 60,
        INFO_AUCTION_ADD_INVALID_TYPE = 61,
        INFO_ITEM_ALREADY_SOLD = 64,
        INFO_AUCTION_BANK_CREDITED = 65,
        INFO_AUCTION_EXPIRED = 67,
        INFO_AUCTION_LOT_BOUGHT = 68,

        ERROR_UNABLE_LEARN_JOB = 6,
        ERROR_TOO_MUCH_JOB = 9,
        ERROR_ALREADY_JOB = 11,

        ERROR_PLAYER_AWAY_MESSAGE = 14,

        ERROR_UNABLE_LEARN_SPELL = 7,
        
        ERROR_CONDITIONS_UNSATISFIED = 19,

        ERROR_STORAGE_ALREADY_IN_USE = 20,

        ERROR_CHAT_SAME_MESSAGE = 84,

        ERROR_AUCTION_HOUSE_TOO_MANY_ITEMS = 66,
        ERROR_INVALID_PRICE = 99,
        ERROR_NOT_ENOUGH_KAMAS_FOR_TAXE = 65,
        ERROR_MAX_TAXCOLLECTOR_BY_SUBAREA_REACHED = 168,
        ERROR_NOT_ENOUGH_KAMAS = 128,
        ERROR_YOU_ARE_AWAY = 116,
        ERROR_GUILD_NOT_ENOUGH_RIGHTS = 101,
        ERROR_WORLD_SAVING = 164,
        ERROR_WORLD_SAVING_FINISHED = 165,

        ERROR_MAX_INVOCATION_REACHED = 203,

        ERROR_PLAYER_AWAY_NOT_INVITABLE = 209,

        ERROR_SERVER_BETA = 225,
        ERROR_SERVER_WELCOME = 89,

        ERROR_PET_ALREADY_EQUIPPED = 88,

        ERROR_NOT_ENOUGH_KAMAS_TO_PAY_MERCHANT_MODE_TAXE = 76,
        ERROR_NOT_ENOUGH_ITEMS_TO_BE_MERCHANT = 23,
        ERROR_TOO_MANY_MERCHANT_ON_MAP = 25,

        ERROR_FIGHT_SPECTATOR_LOCKED = 57,
        ERROR_FIGHTER_DISCONNECTED = 182,
        ERROR_FIGHTER_RECONNECTED = 184,
        ERROR_FIGHT_WAITING_PLAYERS = 29,
        ERROR_FIGHTER_KICKED_DUE_TO_DISCONNECTION = 30,

        ERROR_MOUNT_MATURITY_LOW = 176,

        ERROR_GUILD_BOSS_LEFT_NEW_BOSS = 199,
    }
    
    /// <summary>
    /// 
    /// </summary>
    public enum GamePopupTypeEnum
    {
        TYPE_INSTANT = 1,
        TYPE_ON_DISCONNECT = 0,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum GameMessageEnum
    {
        MESSAGE_MUCH_SPAM = 0,
        MESSAGE_MUCH_INACTIVE = 1,
        MESSAGE_BANK_KAMAS_NEEDED = 10,
        MESSAGE_ENERGY_LOW = 11,
        MESSAGE_TOMBESTONE = 12,
        MESSAGE_MAINTENANCE = 13,
        MESSAGE_TRANSFORMED_TO_GHOST_NEED_PHEONIX = 15,
        MESSAGE_MAINTENANCE_SOON = 3,
        MESSAGE_CONNECTION_CLOSED_DUE_TO_MAINTENANCE = 4,
        MESSAGE_KICKED = 18,
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
    public enum MountEquipErrorEnum
    {
        INVENTORY_NOT_EMPTY = '-',
        ALREADY_HAVE_ONE = '+',
        UNKNOW_ERROR = 'r',
    }

    /// <summary>
    /// 
    /// </summary>
    public static class WorldMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string IM_INFO_MESSAGE(InformationEnum info, params object[] args)
        {
            return INFORMATION_MESSAGE(InformationTypeEnum.INFO, info, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string IM_ERROR_MESSAGE(InformationEnum info, params object[] args)
        {
            return INFORMATION_MESSAGE(InformationTypeEnum.ERROR, info, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string IM_PVP_MESSAGE(InformationEnum info, params object[] args)
        {
            return INFORMATION_MESSAGE(InformationTypeEnum.PVP, info, args);
        }

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
            //if(WorldConfig.NETWORK_CRYPT)
                //return "ATK" + Crypt.CRYPT_KEY_INDEX + Crypt.CRYPT_KEY;
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
            var message = new StringBuilder("ALK31536000000", characters.Count * 50);
            if (characters.Count > 0)
            {
                message.Append('|').Append(characters.Count);

                foreach (var character in characters)
                {
                    message.Append('|').Append(character.Id);
                    message.Append(';').Append(character.Name);
                    message.Append(';').Append(character.Level);
                    message.Append(';').Append(character.Skin);
                    message.Append(';').Append(character.HexColor1);
                    message.Append(';').Append(character.HexColor2);
                    message.Append(';').Append(character.HexColor3);
                    message.Append(';');
                    character.SerializeAs_ActorLookMessage(message);
                    message.Append(';').Append(character.Merchant ? '1' : '0');
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
        public static string ACCOUNT_RESTRICTIONS(int restrictions)
        {
            return "AR" + Util.EncodeBase36(restrictions);
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
        /// <param name="enabled"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static string CHAT_CHANNEL(bool enabled, ChatChannelEnum channel)
        {
            return "cC" + (enabled ? "+" : "-") + (char)channel;
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
            var message = new StringBuilder("As", 200);
            message
                .Append(character.Experience).Append(',')
                .Append(character.ExperienceFloorCurrent).Append(',') 
                .Append(character.ExperienceFloorNext).Append('|');

            message.Append(character.Kamas).Append('|');
            message.Append(character.CaractPoint).Append('|');
            message.Append(character.SpellPoint).Append('|');

            message
                .Append(character.AlignmentId).Append('~')
                .Append(character.AlignmentId).Append(',') // FAKE ALIGNMENT ???
                .Append('0').Append(',') //  ???
                .Append(character.AlignmentLevel).Append(',')
                .Append(character.Honour).Append(',')
                .Append(character.Dishonour).Append(',')
                .Append(character.AlignmentEnabled ? '1' : '0').Append('|');

            message
                .Append(character.Life).Append(',')
                .Append(character.MaxLife).Append('|');

            message
                .Append(character.Energy).Append(',')
                .Append(10000).Append('|');

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
        public static string GAME_MAP_INFORMATIONS(OperatorEnum operation, params AbstractEntity[] entities)
        {
            var message = new StringBuilder("GM", entities.Count() * 100);
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
                case GameActionTypeEnum.FIGHT_WEAPON_USE:
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
        /// <returns></returns>
        public static string GAME_ACTION_FAILED()
        {
            return "GAE";
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
        public static string GAME_MESSAGE(GamePopupTypeEnum type, GameMessageEnum message, params object[] args)
        {
            return "M" + (int)type + "" + (int)message + (args.Length > 0 ? "|" : "") + string.Join(";", args.Select(arg => arg.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string SERVER_MESSAGE(Color color, string message)
        {
            return "cs<font color='" + ColorTranslator.ToHtml(Color.FromArgb(color.A, color.R, color.G, color.B)) + "'>" + message + "</font>";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string SERVER_INFO_MESSAGE(string message)
        {
            return IM_INFO_MESSAGE(InformationEnum.INFO_SERVER_MESSAGE, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string SERVER_ERROR_MESSAGE(string message)
        {
            return SERVER_MESSAGE(Color.Red, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string SERVER_PVP_MESSAGE(string message)
        {
            return SERVER_MESSAGE(Color.Yellow, message);
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
        public static string ENTITY_OBJECT_ACTUALIZE(AbstractEntity entity)
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
        public static string OBJECT_ADD_SUCCESS(ItemDAO item)
        {
            return "OAKO"  + item.ToString();
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
        public static string OBJECT_MOVE_SUCCESS(long guid, int slot)
        {
            return "OM" + guid + "|" + (slot == (int)ItemSlotEnum.SLOT_INVENTORY ? "" : slot.ToString());
        }
              
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static string OBJECT_QUANTITY_UPDATE(long guid, int quantity)
        {
            return "OQ" + guid + "|" + quantity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string OBJECT_UPDATE(ItemDAO item)
        {
            var message = new StringBuilder("OC;");
            item.SerializeAs_BagContent(message);
            return message.ToString();
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
        /// <param name="move"></param>
        /// <param name="operation"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string EXCHANGE_STORAGE_MOVEMENT(ExchangeMoveEnum move, OperatorEnum operation, string args)
        {
            return "EsK" + (char)move + (char)operation + args;
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
        public static string EXCHANGE_SHOP_LIST(NonPlayerCharacterEntity npc)
        {
            var message = new StringBuilder("EL");
            npc.SerializeAs_ShopItemsListInformations(message);
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
        public static string FIGHT_FLAG_DISPLAY(AbstractFight fight)
        {
            var message = new StringBuilder("Gc+");
            fight.SerializeAs_FightFlag(message);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ope"></param>
        /// <param name="leaderId"></param>
        /// <param name="fighters"></param>
        /// <returns></returns>
        public static string FIGHT_FLAG_UPDATE(OperatorEnum ope, long leaderId, params AbstractFighter[] fighters)
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
        public static string FIGHT_COORDINATE_INFORMATIONS(params AbstractFighter[] fighters)
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
        public static string FIGHT_TURN_LIST(IEnumerable<AbstractFighter> fighters)
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
        public static string FIGHT_TURN_MIDDLE(IEnumerable<AbstractFighter> fighters)
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
            return "Go" + (blocked ? "+" : "-") + (char)type + teamId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fights"></param>
        /// <returns></returns>
        public static string FIGHT_LIST(IEnumerable<AbstractFight> fights)
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
        public static string FIGHT_DETAILS(AbstractFight fight)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerName"></param>
        /// <returns></returns>
        public static string PARTY_CREATE_SUCCESS(string ownerName)
        {
            return "PCK" + ownerName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leaderName"></param>
        /// <param name="memberInvited"></param>
        /// <returns></returns>
        public static string PARTY_INVITE_SUCCESS(string leaderName, string memberInvited)
        {
            return "PIK" + leaderName + "|" + memberInvited;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string PARTY_REFUSE()
        {
            return "PR";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leaderName"></param>
        /// <param name="memberInvited"></param>
        /// <returns></returns>
        public static string PARTY_INVITE_ERROR_FULL()
        {
            return "PIEf";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leaderName"></param>
        /// <param name="memberInvited"></param>
        /// <returns></returns>
        public static string PARTY_INVITE_ERROR_PLAYER_OFFLINE(string distantPlayerName)
        {
            return "PIEn" + distantPlayerName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leaderName"></param>
        /// <param name="memberInvited"></param>
        /// <returns></returns>
        public static string PARTY_INVITE_ERROR_ALREADY_IN_PARTY()
        {
            return "PIEa";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string PARTY_CREATE_ERROR_ALREADY_IN_PARTY()
        {
            return "PCEa";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string PARTY_CREATE_ERROR_FULL()
        {
            return "PCEf";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string PARTY_SET_LEADER(long id)
        {
            return "PL" + id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string PARTY_MEMBER_LEFT(long id)
        {
            return "PM-" + id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kickerId"></param>
        /// <returns></returns>
        public static string PARTY_LEAVE(string kickerId = "")
        {
            return "PV" + kickerId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public static string PARTY_MEMBER_LIST(params CharacterEntity[] members)
        {
            var message = new StringBuilder("PM+");
            foreach(var member in members)
            {
                member.SerializeAs_PartyMemberInformations(message);
                message.Append("|");
            }
            message.Remove(message.Length - 1, 1);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string FIGHT_CHALLENGE_FAILED(int id)
        {
            return "GdOO" + id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string FIGHT_CHALLENGE_SUCCESS(int id)
        {
            return "GdKK" + id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="showTarget"></param>
        /// <param name="targetId"></param>
        /// <param name="basicXpBonus"></param>
        /// <param name="teamXpBonus"></param>
        /// <param name="basicDropBonus"></param>
        /// <param name="teamDropBonus"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        public static string FIGHT_CHALLENGE_INFORMATIONS(int id, bool showTarget, long targetId, long basicXpBonus, long teamXpBonus, long basicDropBonus, long teamDropBonus, bool success)
        {
            return "Gd" + id + ";" + (showTarget ? "1" : "0") + ";" + targetId + ";" + basicXpBonus + ";" + teamXpBonus + ";" + basicDropBonus + ";" + teamDropBonus + ";" + (success ? "1" : "0");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellId"></param>
        /// <param name="fighterId"></param>
        /// <returns></returns>
        public static string FIGHT_CELL_FLAG(int cellId, long fighterId = 0)
        {
            return "Gf" + fighterId + "|" + cellId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static string GUILD_STATS(GuildInstance guild, int power)
        {
            return "gS" + guild.Name + "|" + guild.Emblem + "|" + Util.EncodeBase36(power);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public static string GUILD_MEMBERS_INFORMATIONS(IEnumerable<GuildMember> members)
        {
            var message = new StringBuilder("gIM+", members.Count() * 40);
            foreach(var member in members)
            {
                member.SerializeAs_GuildMemberInformations(message);
            }
            message.Remove(message.Length - 1, 1);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public static string GUILD_MEMBERS_INFORMATIONS(GuildMember member)
        {
            var message = new StringBuilder("gIM+");
            member.SerializeAs_GuildMemberInformations(message);
            message.Remove(message.Length - 1, 1);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="level"></param>
        /// <param name="experienceFloorCurrent"></param>
        /// <param name="experienceFloorNext"></param>
        /// <param name="experience"></param>
        /// <returns></returns>
        public static string GUILD_GENERAL_INFORMATIONS(bool isActive, int level, long experienceFloorCurrent, long experienceFloorNext, long experience)
        {
            return "gIG" + (isActive ? "1" : "0") + "|" + level + "|" + experienceFloorCurrent + "|" + experience + "|" + experienceFloorNext;
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GUILD_JOIN_ERROR_UNKNOW()
        {
            return "gJEu";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GUILD_JOIN_ERROR_OCCUPIED()
        {
            return "gJEo";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GUILD_JOIN_ERROR_ALREADY_IN_GUILD()
        {
            return "gJEa";
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GUILD_JOIN_ERROR_RESTRICTED()
        {
            return "gJEd";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GUILD_JOIN_ERROR_REFUSED_DISTANT(string name)
        {
            return "gJEr" + name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GUILD_JOIN_ERROR_REFUSED_LOCAL()
        {
            return "gJEc";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="characterName"></param>
        /// <param name="guildName"></param>
        /// <returns></returns>
        public static string GUILD_JOIN_REQUEST_DISTANT(long characterId, string characterName, string guildName)
        {
            return "gJr" + characterId + "|" + characterName + "|" + guildName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="distantCharacterName"></param>
        /// <returns></returns>
        public static string GUILD_JOIN_REQUEST_LOCAL(string distantCharacterName)
        {
            return "gJR" + distantCharacterName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GUILD_JOIN_CLOSE()
        {
            return "gJC";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GUILD_JOIN_ACCEPTED_LOCAL()
        {
            return "gJKj";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GUILD_JOIN_ACCEPTED_DISTANT(string distantCharacterName)
        {
            return "gJKa" + distantCharacterName;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kicker"></param>
        /// <param name="kicked"></param>
        /// <returns></returns>
        public static string GUIL_KICK_SUCCESS(string kicker, string kicked)
        {
            return "gKK" + kicker + "|" + kicked;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public static string GUILD_MEMBER_REMOVE(long memberId)
        {
            return "gIM-" + memberId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GUILD_CREATION_OPEN()
        {
            return "gn";
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GUILD_CREATION_CLOSE()
        {
            return "gV";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GUILD_CREATION_SUCCESS()
        {
            return "gCK";
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GUILD_CREATION_ERROR_NAME_ALREADY_EXISTS()
        {
            return "gCEan";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GUILD_CREATION_ERROR_ALREADY_IN_GUILD()
        {
            return "gCEa";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GUILD_CREATION_ERROR_EMBLEM_ALREADY_EXISTS()
        {
            return "gCEae";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stats"></param>
        /// <returns></returns>
        public static string GUILD_BOOST_INFORMATIONS(int boostPoint, int taxCollectorPrice, GuildStatistics stats)
        { 
            var message = new StringBuilder("gIB", 50);
            message.Append(stats.MaxTaxcollector).Append('|');
            message.Append(0).Append('|'); // currentTaxCollectorCount
            message.Append(stats.BaseStatistics.GetTotal(EffectEnum.AddVitality)).Append('|');
            message.Append(stats.BaseStatistics.GetTotal(EffectEnum.AddDamage)).Append('|');
            message.Append(stats.BaseStatistics.GetTotal(EffectEnum.AddPods)).Append('|');
            message.Append(stats.BaseStatistics.GetTotal(EffectEnum.AddProspection)).Append('|');
            message.Append(stats.BaseStatistics.GetTotal(EffectEnum.AddWisdom)).Append('|');
            message.Append(stats.MaxTaxcollector).Append('|'); // ???
            message.Append(boostPoint).Append('|');
            message.Append(taxCollectorPrice).Append('|'); // ??
            stats.Spells.SerializeAs_SpellsList(message);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollector"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static string GUILD_TAXCOLLECTOR_HIRED(TaxCollectorEntity taxCollector, string owner)
        {
            var message = new StringBuilder("gTS");
            message.Append(taxCollector.Name).Append('|');
            message.Append(taxCollector.Id).Append('|');
            message.Append(taxCollector.Map.X).Append('|');
            message.Append(taxCollector.Map.Y).Append('|');
            message.Append(owner);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollector"></param>
        /// <returns></returns>
        public static string GUILD_TAXCOLLECTOR_REMOVED(TaxCollectorEntity taxCollector, string remover)
        {
            var message = new StringBuilder("gTR");
            message.Append(taxCollector.Name).Append('|');
            message.Append(taxCollector.Id).Append('|');
            message.Append(taxCollector.Map.X).Append('|');
            message.Append(taxCollector.Map.Y).Append('|');
            message.Append(remover);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollector"></param>
        /// <returns></returns>
        public static string GUILD_TAXCOLLECTOR_FARMED(TaxCollectorEntity taxCollector, string farmer)
        {
            var message = new StringBuilder("gTG");
            message.Append(taxCollector.Name).Append('|');
            message.Append(taxCollector.Id).Append('|');
            message.Append(taxCollector.Map.X).Append('|');
            message.Append(taxCollector.Map.Y).Append('|');
            message.Append(farmer).Append('|');
            message.Append(taxCollector.ExperienceGathered);
            foreach(var farmedItem in taxCollector.FarmedItems)
            {
                message.Append(';');
                message.Append(farmedItem.Key).Append(','); // templateId
                message.Append(farmedItem.Value); // quantity
            }
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollectors"></param>
        /// <returns></returns>
        public static string GUILD_TAXCOLLECTOR_LIST(IEnumerable<TaxCollectorEntity> taxCollectors)
        {
            var message = new StringBuilder("gITM+");
            foreach (var taxCollector in taxCollectors)
            {
                message.Append(Util.EncodeBase36(taxCollector.Id)).Append(';');
                message.Append(taxCollector.Name).Append(';');
                message.Append(Util.EncodeBase36(taxCollector.MapId)).Append(';');
                if (taxCollector.HasGameAction(GameActionTypeEnum.FIGHT))
                {
                    message.Append('1').Append(';');
                    message.Append(WorldConfig.PVT_TELEPORT_DEFENDERS_TIMEOUT - taxCollector.Fight.UpdateTime).Append(';');
                }
                else
                {
                    message.Append('0').Append(';');
                    message.Append('0').Append(';');
                }
                message.Append(WorldConfig.PVT_TELEPORT_DEFENDERS_TIMEOUT).Append(';');
                message.Append('7').Append('|'); // allowed players to join            
            }
            message.Remove(message.Length - 1, 1);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mapX"></param>
        /// <param name="mapY"></param>
        /// <returns></returns>
        public static string GUILD_TAXCOLLECTOR_UNDER_ATTACK(string name, int mapX, int mapY)
        {
            return "gAA" + name + "|1|" + mapX + "|" + mapY;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mapX"></param>
        /// <param name="mapY"></param>
        /// <returns></returns>
        public static string GUILD_TAXCOLLECTOR_SURVIVED(string name, int mapX, int mapY)
        {
            return "gAS" + name + "|1|" + mapX + "|" + mapY;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mapX"></param>
        /// <param name="mapY"></param>
        /// <returns></returns>
        public static string GUILD_TAXCOLLECTOR_DIED(string name, int mapX, int mapY)
        {
            return "gAD" + name + "|1|" + mapX + "|" + mapY;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GUILD_TAXCOLLECTOR_DEFENDER_LEAVE(long taxCollectorId, long memberId)
        {
            return "gITP-" + Util.EncodeBase36(taxCollectorId) + '|' + Util.EncodeBase36(memberId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollectorId"></param>
        /// <param name="attackerId"></param>
        /// <returns></returns>
        public static string GUILD_TAXCOLLECTOR_ATTACKER_LEAVE(long taxCollectorId, long attackerId)
        {
            return "gITp-" + Util.EncodeBase36(taxCollectorId) + '|' + Util.EncodeBase36(attackerId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollectorId"></param>
        /// <param name="attackers"></param>
        /// <returns></returns>
        public static string GUILD_TAXCOLLECTOR_ATTACKER_JOIN(long taxCollectorId, params AbstractFighter[] attackers)
        {
            var message = new StringBuilder("gITp+").Append(Util.EncodeBase36(taxCollectorId));
            foreach (var attacker in attackers)
            {
                message.Append('|');
                message.Append(Util.EncodeBase36(attacker.Id)).Append(';');
                message.Append(attacker.Name).Append(';');
                message.Append(attacker.Level);
            }
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollectorId"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public static string GUILD_TAXCOLLECTOR_DEFENDER_JOIN(long taxCollectorId, params GuildMember[] members)
        {
            var message = new StringBuilder("gITP+").Append(Util.EncodeBase36(taxCollectorId));
            foreach(var member in members)
            {
                message.Append('|');
                member.SerializeAs_TaxCollectorDefender(message);
            }
            return message.ToString();
        }
             
        /// <summary>
        /// 
        /// </summary>
        /// <param name="npcId"></param>
        /// <param name="parameters"></param>
        /// <param name="responses"></param>
        /// <returns></returns>
        public static string DIALOG_CREATE(long npcId)
        {
            return "DCK" + npcId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string DIALOG_LEAVE()
        {
            return "DV";
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="responses"></param>
        /// <returns></returns>
        public static string DIALOG_QUESTION(int questionId, string parameters, IEnumerable<int> responseIds)
        {
            return "DQ" + questionId + ";" + parameters + "|" + string.Join(";", responseIds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entries"></param>
        /// <returns></returns>
        public static string AUCTION_HOUSE_AUCTION_OWNER_LIST(IEnumerable<AuctionEntry> entries)
        {
            StringBuilder message = new StringBuilder("EL");
            foreach (var entry in entries)
            {
                message.Append(entry.Item.Id).Append(';');
                message.Append(entry.Item.Quantity).Append(';');
                message.Append(entry.Item.TemplateId).Append(';');
                message.Append(entry.Item.StringEffects).Append(';');
                message.Append(entry.Price).Append(';');
                message.Append(entry.HoursLeft).Append('|');
            }
            if (message.Length > 2)
                message.Remove(message.Length - 1, 1);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static string AUCTION_HOUSE_TEMPLATE_LIST(int type, IEnumerable<int> templates)
        {
            return "EHL" + type + "|" + String.Join(";", templates);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="entries"></param>
        /// <returns></returns>
        public static string AUCTION_HOUSE_AUCTION_LIST(int templateId, IEnumerable<AuctionCategory> entries)
        {
            var message = new StringBuilder("EHl").Append(templateId);
            foreach(var entry in entries)
            {
                message.Append(entry.SerializeAs_BuyExchange());
            }
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string AUCTION_HOUSE_TEMPLATE_MOVEMENT(OperatorEnum op, int templateId)
        {
            return "EHM" + (char)op + templateId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static string AUCTION_HOUSE_CATEGORY_MOVEMENT(OperatorEnum op, AuctionCategory category)
        {
            var message = new StringBuilder("EHm").Append((char)op);
            category.SerializeAs_CategoryMovement(op, message);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="middlePrice"></param>
        /// <returns></returns>
        public static string AUCTION_HOUSE_MIDDLE_PRICE(int templateId, long middlePrice)
        {
            return "EHP" + templateId + "|" + middlePrice;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellId"></param>
        /// <param name="frameId"></param>
        /// <param name="activated"></param>
        /// <returns></returns>
        public static string INTERACTIVE_DATA_FRAME(int cellId, int frameId, bool activated)
        {
            return "GDF|" + cellId + ";" + frameId + ";" + (activated ? "1" : "0");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellId"></param>
        /// <param name="frameId"></param>
        /// <param name="activated"></param>
        /// <returns></returns>
        public static string INTERACTIVE_DATA_FRAME(IEnumerable<InteractiveObject> iobjects)
        {
            var message = new StringBuilder("GDF");
            foreach (var io in iobjects)
                io.SerializeAs_InteractiveListMessage(message);
            message.Remove(message.Length - 1, 1);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellId"></param>
        /// <param name="frameId"></param>
        /// <param name="activated"></param>
        /// <returns></returns>
        public static string INTERACTIVE_DATA_FRAME_FIGHT(IEnumerable<InteractiveObject> iobjects)
        {
            var message = new StringBuilder("GDF");
            foreach (var io in iobjects)
                io.SerializeAs_FightInteractiveListMessage(message);
            message.Remove(message.Length - 1, 1);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static string INTERACTIVE_FARMED_QUANTITY(long characterId, long quantity)
        {
            return "IQ" + characterId + "|" + quantity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string WAYPOINT_CREATE(CharacterEntity character)
        {
            var message = new StringBuilder("WC").Append(character.SavedMapId);
            foreach (var waypoint in character.Waypoints)
            {
                var price = 0;
                if(waypoint.MapId != character.MapId)
                    price = 10 * (Math.Abs(waypoint.Map.X - character.Map.X) + Math.Abs(waypoint.Map.Y - character.Map.Y) - 1);
                message.Append('|').Append(waypoint.MapId).Append(';').Append(price);
            }
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string WAYPOINT_LEAVE()
        {
            return "WV";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string WAYPOINT_USE_ERROR()
        {
            return "WUE";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string BASIC_CONSOLE_MESSAGE(string message)
        {
            return "BAT2" + message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string EXCHANGE_PERSONAL_SHOP_ITEMS_LIST(IEnumerable<ItemDAO> items)
        {
            var message = new StringBuilder("EL");
            foreach(var item in items)
            {
                message.Append(item.Id).Append(';');
                message.Append(item.Quantity).Append(';');
                message.Append(item.TemplateId).Append(';');
                message.Append(item.StringEffects).Append(';');
                message.Append(item.MerchantPrice).Append('|');
            }
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string EXCHANGE_STORAGE_ITEMS_LIST(IEnumerable<ItemDAO> items, long kamas)
        {
            var message = new StringBuilder("EL");
            foreach (var item in items)
            {
                message.Append('O');
                item.SerializeAs_BagContent(message);
            }
            message.Append(";G").Append(kamas);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string MERCHANT_MODE_TAXE(long value)
        {
            return "Eq1|1|" + value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EXCHANGE_STORAGE_KAMAS_VALUE(long value)
        {
            return "EsKG" + value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ALIGNMENT_DISABLE_COST(int value)
        {
            return "GIP" + value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static string LIFE_RESTORE_TIME_START(double interval)
        {
            return "ILS" + interval;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static string LIFE_RESTORE_TIME_FINISH(int lifeRestored)
        {
            return "ILF" + lifeRestored;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string FRIEND_DELETE_SUCCESS()
        {
            return "FD";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ENNEMY_DELETE_SUCCESS()
        {
            return "iD";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerPseudo"></param>
        /// <param name="friend"></param>
        /// <returns></returns>
        public static string FRIEND_ADD(string playerPseudo, SocialRelationDAO friend)
        {
            var message = new StringBuilder("FA");
            message.Append('|');
            message.Append(friend.Pseudo);
            var characterFriend = EntityManager.Instance.GetCharacterByNickname(friend.Pseudo);
            if (characterFriend != null)
                characterFriend.SerializeAs_FriendInformations(playerPseudo, message);
            return message.ToString();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string FRIEND_ADD_ERROR_UNKNOW_PLAYER()
        {
            return "FAEf";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string FRIEND_ADD_ERROR_ALREADY()
        {
            return "FAEa";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string FRIEND_ADD_ERROR_FULL()
        {
            return "FAEm";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string FRIEND_ADD_ERROR_YOURSELF()
        {
            return "FAEy";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="friends"></param>
        /// <returns></returns>
        public static string FRIENDS_LIST_ON_CONNECT(CharacterEntity character, IEnumerable<SocialRelationDAO> friends)
        {
            var message = new StringBuilder("FL");
            foreach (var friend in friends)
            {
                message.Append('|');
                message.Append(friend.Pseudo);
                var characterFriend = EntityManager.Instance.GetCharacterByNickname(friend.Pseudo);
                if(characterFriend != null)                
                {
                    characterFriend.SerializeAs_FriendInformations(character.Account.Pseudo, message);
                    if (characterFriend.NotifyOnFriendConnection)                    
                        characterFriend.SafeDispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_FRIEND_ONLINE, character.Name));                    
                }
            }
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="friends"></param>
        /// <returns></returns>
        public static string FRIENDS_LIST(string playerPseudo, IEnumerable<SocialRelationDAO> friends)
        {
            var message = new StringBuilder("FL");
            foreach(var friend in friends)
            {
                message.Append('|');
                message.Append(friend.Pseudo);
                var characterFriend = EntityManager.Instance.GetCharacterByNickname(friend.Pseudo);
                if(characterFriend != null)                
                    characterFriend.SerializeAs_FriendInformations(playerPseudo, message);                
            }
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerPseudo"></param>
        /// <param name="ennemies"></param>
        /// <returns></returns>
        public static string ENNEMIES_LIST(string playerPseudo, IEnumerable<SocialRelationDAO> ennemies)
        {
            var message = new StringBuilder("iL");
            foreach (var ennemy in ennemies)
            {
                message.Append('|');
                message.Append(ennemy.Pseudo);
                var characterEnnemy = EntityManager.Instance.GetCharacterByNickname(ennemy.Pseudo);
                if (characterEnnemy != null)
                    characterEnnemy.SerializeAs_EnnemyInformations(playerPseudo, message);
            }
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ennemies"></param>
        /// <returns></returns>
        public static string ENNEMIES_LIST_ON_CONNECT(CharacterEntity character, IEnumerable<SocialRelationDAO> ennemies)
        {
            var message = new StringBuilder("iL");
            foreach (var ennemy in ennemies)
            {
                message.Append('|');
                message.Append(ennemy.Pseudo);
                var characterEnnemy = EntityManager.Instance.GetCharacterByNickname(ennemy.Pseudo);
                if (characterEnnemy != null)                
                    characterEnnemy.SerializeAs_EnnemyInformations(character.Account.Pseudo, message);                
            }
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ENNEMY_ADD_ERROR_UNKNOW_PLAYER()
        {
            return "iAEf";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ENNEMY_ADD_ERROR_ALREADY()
        {
            return "iAEa";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ENNEMY_ADD_ERROR_FULL()
        {
            return "iAEm";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ENNEMY_ADD_ERROR_YOURSELF()
        {
            return "iAEy";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerPseudo"></param>
        /// <param name="ennemy"></param>
        /// <returns></returns>
        public static string ENNEMY_ADD(string playerPseudo, SocialRelationDAO ennemy)
        {
            var message = new StringBuilder("iA");
            message.Append('|');
            message.Append(ennemy.Pseudo);
            var characterEnnemy = EntityManager.Instance.GetCharacterByNickname(ennemy.Pseudo);
            if (characterEnnemy != null)
                characterEnnemy.SerializeAs_EnnemyInformations(playerPseudo, message);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static string EMOTE_DIRECTION(long playerId, int direction)
        {
            return "eD" + playerId + "|" + direction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="emoteId"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string EMOTE_USE(long targetId, int emoteId, long time = 360000)
        {
            return "eUK" + targetId + "|" + emoteId + "|" + time;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        /// <returns></returns>
        public static string EMOTES_LIST(int capacity)
        {
            return "eL" + capacity + "|" + capacity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string ITEM_SET(ItemSetDAO set, IEnumerable<ItemDAO> items)
        {
            if (set == null)
                return "";
            var message = new StringBuilder("OS+").Append(set.Id).Append('|');
            message.Append(String.Join(";", items.Select(item => item.TemplateId))).Append('|');
            message.Append(set.GetStats(items.Count()).ToItemStats());
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string OBJECT_CHANGE(IEnumerable<ItemDAO> items)
        {
            var message = new StringBuilder("OCK");
            foreach (var item in items)
                message.Append(item.ToString()).Append('*');
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public static string JOB_SKILL(JobBook book)
        {
            var message = new StringBuilder("JSK");
            book.SerializeAs_SkillListMessage(message);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        public static string JOB_SKILL(CharacterJobDAO job)
        {
            var message = new StringBuilder("JSK");
            job.Template.SerializeAs_SkillListMessage(job, message);
            message.Remove(message.Length - 1, 1);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public static string JOB_XP(List<CharacterJobDAO> jobs)
        {
            var message = new StringBuilder("JXK");
            foreach(var job in jobs)
            {
                if (job.JobId != (int)JobIdEnum.JOB_BASE)
                {
                    message.Append(job.JobId).Append(';');
                    message.Append(job.Level).Append(';');
                    message.Append(job.ExperienceFloorCurrent).Append(';');
                    message.Append(job.Experience).Append(';');
                    message.Append(job.ExperienceFloorNext).Append('|');
                }
            }
            if(jobs.Count > 1)
                message.Remove(message.Length - 1, 1);
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public static string JOB_XP(CharacterJobDAO job)
        {
            return "JXK" + job.JobId + ";"
                + job.Level + ";"
                + job.ExperienceFloorCurrent + ";"
                + job.Experience + ";"
                + job.ExperienceFloorNext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static string JOB_TOOL_EQUIPPED(string jobId = "n")
        {
            return "OT" + jobId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static string JOB_NEW_LEVEL(int jobId, int level)
        {
            return "JN" + jobId + "|" + level;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characteId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static string CRAFT_INTERACTIVE_SUCCESS(long characterId, int templateId)
        {
            return "IO" + characterId + "|+" + templateId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characteId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static string CRAFT_INTERACTIVE_FAILED(long characterId, int templateId)
        {
            return "IO" + characterId + "|-" + templateId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characteId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static string CRAFT_INTERACTIVE_NOTHING(long characterId)
        {
            return "IO" + characterId + "|-";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CRAFT_NO_RESULT()
        {
            return "EcEI";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static string CRAFT_TEMPLATE_CREATED(int templateId)
        {
            return "EcK;" + templateId; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static string CRAFT_TEMPLATE_FAILED(int templateId)
        {
            return "EcEF;" + templateId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string CRAFT_LOOP_COUNT(int count)
        {
            return "EA" + count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        public static string CRAFT_LOOP_END(int reason)
        {
            return "Ea" + reason;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static string MOUNT_EXPERIENCE_SHARED(int percentage)
            => "Rx" + percentage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string MOUNT_NAME(string name)
            => "Rn" + name;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string MOUNT_RIDING_START()
            => "Rr+";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string MOUNT_RIDING_STOP()
            => "Rr-";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultPrice"></param>
        /// <returns></returns>
        public static string PADDOCK_BUY_START(int defaultPrice)
            => "RD|" + defaultPrice;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string PADDOCK_BUY_LEAVE()
            => "Rv";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string MOUNT_UNEQUIP()
            => "Re-";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="informations"></param>
        /// <returns></returns>
        public static string MOUNT_EQUIP(string informations)
            => "Re+" + informations;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string MOUNT_EQUIP_ERROR(MountEquipErrorEnum error)
            => "ReE" + (char)error;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="price"></param>
        /// <param name="size"></param>
        /// <param name="items"></param>
        /// <param name="guildName"></param>
        /// <param name="guildEmblem"></param>
        /// <returns></returns>
        public static string PADDOCK_INFORMATIONS(int owner, long price, int size, int items, string guildName, string guildEmblem)
            => "Rp" + owner + ";" + price + ";" + size + ";" + items + ";" + guildName + ";" + guildEmblem;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quests"></param>
        /// <returns></returns>
        public static string QUEST_LIST(List<CharacterQuest> quests)
        {
            var message = new StringBuilder("QLK");
            foreach (var quest in quests)
            {
                message.Append(quest.Id).Append(';');
                message.Append(quest.Done ? "1" : "0").Append(';');
                message.Append("0").Append('|'); // TODO: nSortOrder
            }
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        public static string QUEST_STEPS(CharacterQuest quest)
        {
            var message = new StringBuilder("QS");
            message.Append(quest.Id).Append('|');
            message.Append(quest.CurrentStepId).Append('|');
            foreach (var objective in quest.CurrentStep.Objectives)
            {
                message.Append(objective.Id).Append(',');
                message.Append(objective.Done(quest.GetAdvancement(objective.Id)) ? "1" : "0");
                message.Append(';');
            }
            message.Append('|');
            foreach (var previousStep in quest.Template.Steps.Where(s => s.Order < quest.CurrentStep.Order))            
                message.Append(previousStep.Id).Append(';');            
            message.Append('|');
            foreach (var nextStep in quest.Template.Steps.Where(s => s.Order > quest.CurrentStep.Order))            
                message.Append(nextStep.Id).Append(';');            
            message.Append('|');
            message.Append(0).Append(';'); // TODO: dialogId
            message.Append(",,,"); //TODO: dialogParams
            return message.ToString();
        }
    }
}