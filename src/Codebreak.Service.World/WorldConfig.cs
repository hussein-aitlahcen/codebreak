using Codebreak.Framework.Configuration;
using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World
{
    /// <summary>
    /// 
    /// </summary>
    public static class WorldConfig
    {
        public static int GetStartCell(CharacterBreedEnum breed)
        {
            switch (breed)
            {
                case CharacterBreedEnum.BREED_CRA:
                    return CELL_START_CRA;

                case CharacterBreedEnum.BREED_ECAFLIP:
                    return CELL_START_ECAFLIP;

                case CharacterBreedEnum.BREED_ENIRIPSA:
                    return CELL_START_ENIRIPSA;

                case CharacterBreedEnum.BREED_ENUTROF:
                    return CELL_START_ENUTROF;

                case CharacterBreedEnum.BREED_FECA:
                    return CELL_START_FECA;

                case CharacterBreedEnum.BREED_IOP:
                    return CELL_START_IOP;

                case CharacterBreedEnum.BREED_OSAMODAS:
                    return CELL_START_OSAMODAS;

                case CharacterBreedEnum.BREED_PANDAWA:
                    return CELL_START_PANDAWA;

                case CharacterBreedEnum.BREED_SACRIEUR:
                    return CELL_START_SACRIEUR;

                case CharacterBreedEnum.BREED_SADIDAS:
                    return CELL_START_SADIDAS;

                case CharacterBreedEnum.BREED_SRAM:
                    return CELL_START_SRAM;

                case CharacterBreedEnum.BREED_XELOR:
                    return CELL_START_XELOR;

                default:
                    throw new Exception("Unknow breedId " + breed);
            }
        }

        public static int GetStartMap(CharacterBreedEnum breed)
        {
            switch (breed)
            {
                case CharacterBreedEnum.BREED_CRA:
                    return MAP_START_CRA;

                case CharacterBreedEnum.BREED_ECAFLIP:
                    return MAP_START_ECAFLIP;

                case CharacterBreedEnum.BREED_ENIRIPSA:
                    return MAP_START_ENIRIPSA;

                case CharacterBreedEnum.BREED_ENUTROF:
                    return MAP_START_ENUTROF;

                case CharacterBreedEnum.BREED_FECA:
                    return MAP_START_FECA;

                case CharacterBreedEnum.BREED_IOP:
                    return MAP_START_IOP;

                case CharacterBreedEnum.BREED_OSAMODAS:
                    return MAP_START_OSAMODAS;

                case CharacterBreedEnum.BREED_PANDAWA:
                    return MAP_START_PANDAWA;

                case CharacterBreedEnum.BREED_SACRIEUR:
                    return MAP_START_SACRIEUR;

                case CharacterBreedEnum.BREED_SADIDAS:
                    return MAP_START_SADIDAS;

                case CharacterBreedEnum.BREED_SRAM:
                    return MAP_START_SRAM;

                case CharacterBreedEnum.BREED_XELOR:
                    return MAP_START_XELOR;

                default:
                    throw new Exception("Unknow breedId " + breed);
            }
        }

        [Configurable()]
        public static int MAP_START_ENUTROF = 10299;
        [Configurable()]
        public static int CELL_START_ENUTROF = 272;

        [Configurable()]
        public static int MAP_START_FECA = 10300;
        public static int
                         CELL_START_FECA = 321;

        [Configurable()]
        public static int MAP_START_ECAFLIP = 10276;
        [Configurable()]
        public static int
            CELL_START_ECAFLIP = 297;

        [Configurable()]
        public static int MAP_START_SADIDAS = 10279;
        [Configurable()]
        public static int
                         CELL_START_SADIDAS = 255;

        [Configurable()]
        public static int MAP_START_ENIRIPSA = 10283;
        [Configurable()]
        public static int
                         CELL_START_ENIRIPSA = 270;

        [Configurable()]
        public static int MAP_START_OSAMODAS = 10285;
        [Configurable()]
        public static int
                         CELL_START_OSAMODAS = 219;

        [Configurable()]
        public static int MAP_START_SRAM = 10285;
        [Configurable()]
        public static int
                         CELL_START_SRAM = 219;

        [Configurable()]
        public static int MAP_START_PANDAWA = 10289;
        [Configurable()]
        public static int
                         CELL_START_PANDAWA = 249;

        [Configurable()]
        public static int MAP_START_CRA = 10285;
        [Configurable()]
        public static int
                         CELL_START_CRA = 219;

        [Configurable()]
        public static int MAP_START_IOP = 10294;
        [Configurable()]
        public static int
                         CELL_START_IOP = 235;

        [Configurable()]
        public static int MAP_START_SACRIEUR = 10296;
        [Configurable()]
        public static int
                         CELL_START_SACRIEUR = 229;

        [Configurable()]
        public static int MAP_START_XELOR = 10298;
        [Configurable()]
        public static int
                         CELL_START_XELOR = 286;

        public static Dictionary<int, int> BOOST_ITEMS = new Dictionary<int, int>()
        {
            { 8950, 8943 } // Shigekax orange
        };

        [Configurable()]
        public static bool NETWORK_CRYPT = false;

        public static int[] MULTIPLE_INSTANCE_MAP_ID = 
        {
        };

        public static string[] NPC_BEGIN_TRADE_SPEAK = 
        {
            "Encore un client, que désirez vous {0} ?",
            "Des nouveautés tous les jours !",
        };

        public static string[] NPC_BUY_TRADE_SPEAK = 
        {
            "{0} est riche !",
            "L'aura de {0} n'a d'egale que sa richesse.",
        };

        public static string[] NPC_LEAVE_TRADE_SPEAK = 
        {
            "J'espère vous revoir très prochainement.",
            "A bientôt, sachez que je suis toujours ouvert.",
        };

        [Configurable()]
        public static int SPAWN_MAX_GROUP_PER_MAP = 3;
        [Configurable()]
        public static int SPAWN_CHECK_INTERVAL = 1 * 60 * 1000;

        [Configurable()]
        public static int MAX_AWAY_TIME = 20 * 60 * 1000;
        [Configurable()]
        public static int INACTIVITY_CHECK_INTERVAL = MAX_AWAY_TIME / 2;

        [Configurable()]
        public static int RPC_ACCOUNT_TICKET_TIMEOUT = 5000;
        [Configurable()]
        public static int RPC_ACCOUNT_TICKET_CHECK_INTERVAL = RPC_ACCOUNT_TICKET_TIMEOUT / 2;

        [Configurable()]
        public static int WORLD_SAVE_INTERVAL = 20 * 60 * 1000;
        
        [Configurable()]
        public static string WORLD_SERVICE_IP = "127.0.0.1";

        [Configurable()]
        public static int WORLD_SERVICE_PORT = 5555;
        
        [Configurable()]
        public static string WORLD_DB_CONNECTION = "Server=localhost;Database=codebreak_world;Uid=root;Pwd=;";

        [Configurable()]
        public static string RPC_PASSWORD = "smarken";

        [Configurable()]
        public static string RPC_IP = "127.0.0.1";

        [Configurable()]
        public static int RPC_PORT = 4321;

        [Configurable()]
        public static int CHARACTER_CREATION_LEVEL = 1;
        [Configurable()]
        public static int CHARACTER_CREATION_VITALITY = 0;
        [Configurable()]
        public static int CHARACTER_CREATION_WISDOM = 0;
        [Configurable()]
        public static int CHARACTER_CREATION_AGILITY = 0;
        [Configurable()]
        public static int CHARACTER_CREATION_INTELLIGENCE = 0;
        [Configurable()]
        public static int CHARACTER_CREATION_CHANCE = 0;
        [Configurable()]
        public static int CHARACTER_CREATION_STRENGTH = 0;
        [Configurable()]
        public static int CHARACTER_CREATION_AP = 6;
        [Configurable()]
        public static int CHARACTER_CREATION_MP = 3;
        [Configurable()]
        public static int CHARACTER_CREATION_SKIN_SIZE = 100;
        [Configurable()]
        public static int CHARACTER_CREATION_SPELLPOINT = 0;
        [Configurable()]
        public static int CHARACTER_CREATION_CARACPOINT = 0;
        [Configurable()]
        public static int CHARACTER_CREATION_LIFE = 55;
        [Configurable()]
        public static int CHARACTER_CREATION_ENERGY = 10000;
        [Configurable()]
        public static int CHARACTER_CREATION_EMOTE_CAPACITY = 1376255;

        [Configurable()]
        public static int LIVING_CHEST_ID = 285;

        [Configurable()]
        public static int GHOST_SKIN_ID = 8004;

        [Configurable()]
        public static int MAX_MERCHANT_PER_MAP = 5;

        [Configurable()]
        public static int MAX_ENNEMIES = 100;
        [Configurable()]
        public static int MAX_FRIENDS = 100;

        [Configurable()]
        public static double REGEN_TIMER_SIT = 300;
        [Configurable()]
        public static double REGEN_TIMER = 1500;

        [Configurable()]
        public static int PVT_TELEPORT_DEFENDERS_TIMEOUT = 45000;
        [Configurable()]
        public static int PVT_START_TIMEOUT = 60000;
        [Configurable()]
        public static int PVT_TURN_TIME = 30000;

        [Configurable()]
        public static int PVM_MAX_STAR_BONUS = 1000;
        [Configurable()]
        public static int PVM_STAR_BONUS_PERCENT_SECONDS = 10;
        [Configurable()]
        public static int PVM_CHALLENGE_COUNT = 3;
        [Configurable()]
        public static int PVM_START_TIMEOUT = 60000;
        [Configurable()]
        public static int PVM_TURN_TIME = 30000;

        public static double[] PVM_RATE_GROUP = { 1, 1.1, 1.5, 2.3, 3.1, 3.6, 4.2, 4.7 };

        [Configurable()]
        public static double RATE_XP = 5;
        [Configurable()]
        public static double RATE_DROP = 3;
        [Configurable()]
        public static double RATE_KAMAS = 2;

        [Configurable()]
        public static int PVP_START_TIMEOUT = 60000;
        [Configurable()]
        public static int PVP_TURN_TIME = 30000;

        [Configurable()]
        public static int AGGRESSION_KNGIHT_MONSTER_ID = 394;
        [Configurable()]
        public static int AGGRESSION_START_TIMEOUT = 60000;
        [Configurable()]
        public static int AGGRESSION_TURN_TIME = 30000;

        [Configurable()]
        public static double TAXCOLLECTOR_XP_RATIO = 0.01;
        [Configurable()]
        public static int TAXCOLLECTOR_MIN_NAME = 1;
        [Configurable()]
        public static int TAXCOLLECTOR_MAX_NAME = 228;
        [Configurable()]
        public static int TAXCOLLECTOR_MIN_FIRSTNAME = 1;
        [Configurable()]
        public static int TAXCOLLECTOR_MAX_FIRSTNAME = 130;
        [Configurable()]
        public static int TAXCOLLECTOR_SKIN_BASE = 6000;
        [Configurable()]
        public static int TAXCOLLECTOR_SKIN_SIZE_BASE = 100;

        [Configurable()]
        public static int FIGHT_DISCONNECTION_TURN = 20;
        [Configurable()]
        public static int FIGHT_PUSH_CELL_TIME = 270;
        [Configurable()]
        public static int FIGHT_PANDA_LAUNCH_CELL_TIME = 250;

        [Configurable()]
        public static DateTime REFERENCE_DATE = new DateTime(1970, 1, 1);
        [Configurable()]
        public static int GAME_ID = 1; // Jiva ? guess so;
    }
}
