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
using Codebreak.WorldService;
using System.Drawing;
using Codebreak.Service.World.Game.Guild;
using Codebreak.Service.World.Game.Auction;

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
        INFO_GUILD_KICKED_HIMSELF = 176,
        INFO_GUILD_KICKED = 177,
        
        INFO_AUCTION_TOO_MANY_ITEMS = 59,
        INFO_AUCTION_RARE = 60,
        INFO_AUCTION_ADD_INVALID_TYPE = 61,
        INFO_AUCTION_ALREADY_SOLD = 64,
        INFO_AUCTION_BANK_CREDITED = 65,
        INFO_AUCTION_EXPIRED = 67,
        INFO_AUCTION_LOT_BOUGHT = 68,

        ERROR_AUCTION_HOUSE_TOO_MANY_ITEMS = 66,
        ERROR_INVALID_PRICE = 99,
        ERROR_NOT_ENOUGH_KAMAS_FOR_TAXE = 65,
        ERROR_MAX_TAXCOLLECTOR_BY_SUBAREA_REACHED = 168,
        ERROR_NOT_ENOUGH_KAMAS = 128,
        ERROR_YOU_ARE_AWAY = 116,
        ERROR_GUILD_NOT_ENOUGH_RIGHTS = 101,
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
        MESSAGE_FREE_SOUL = 12,
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
            var message = new StringBuilder("As", 100);
            message
                .Append(character.Experience).Append(',')                                               // CurExcperience
                .Append(character.ExperienceFloorCurrent).Append(',')     // LastExperience
                .Append(character.ExperienceFloorNext).Append('|');// NextExperience

            message.Append(character.Kamas).Append('|');
            message.Append(character.CaractPoint).Append('|');
            message.Append(character.SpellPoint).Append('|');

            message
                .Append(character.CharacterAlignment.AlignmentId).Append('~')
                .Append(character.CharacterAlignment.AlignmentId).Append(',')
                .Append(character.CharacterAlignment.Level).Append(',')
                .Append(character.CharacterAlignment.Promotion).Append(',')
                .Append(character.CharacterAlignment.Honour).Append(',')
                .Append(character.CharacterAlignment.Dishonour).Append(',')
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
        public static string GAME_MESSAGE(InformationTypeEnum type, GameMessageEnum message)
        {
            return "M" + (int)type + "" + (int)message;
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
            return SERVER_MESSAGE(Color.Green, message);
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
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static string OBJECT_WEAPON_EQUIPPED(int templateId)
        {
            if (templateId == -1)
                return "OT";
            return "OT" + templateId;
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
        public static string OBJECT_UPDATE(InventoryItemDAO item)
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
            return "Go" + (blocked ? "+" : "-") + (char)type + teamId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fights"></param>
        /// <returns></returns>
        public static string FIGHT_LIST(IEnumerable<FightBase> fights)
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
        public static string FIGHT_DETAILS(FightBase fight)
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
                member.SerializeAs_PartyMemberListInformations(message);
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
        public static string GUILD_GENERAL_INFORMATIONS(bool isActive, int level, int experienceFloorCurrent, int experienceFloorNext, long experience)
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
                    message.Append(TaxCollectorFight.PVT_TELEPORT_DEFENDERS_TIMEOUT - taxCollector.Fight.UpdateTime).Append(';');
                }
                else
                {
                    message.Append('0').Append(';');
                    message.Append('0').Append(';');
                }
                message.Append(TaxCollectorFight.PVT_TELEPORT_DEFENDERS_TIMEOUT).Append(';');
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
        public static string GUILD_TAXCOLLECTOR_ATTACKER_JOIN(long taxCollectorId, params FighterBase[] attackers)
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
        /// <param name="targetId"></param>
        /// <param name="emoteId"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string EMOTE_PLAY(long targetId, int emoteId, long time = 360000)
        {
            return "eUK" + targetId + "|" + emoteId + "|" + time;
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
    }
}
