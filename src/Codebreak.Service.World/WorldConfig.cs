using Codebreak.Framework.Configuration;
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
        public static int[] MULTIPLE_INSTANCE_MAP_ID = 
        {
            7411,
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

        public const int RPC_ACCOUNT_TICKET_CHECK_INTERVAL = 1000;
        public const int RPC_ACCOUNT_TICKET_TIMEOUT = 5000;

        public const int WORLD_SAVE_INTERVAL = 60 * 1000;
        public const int WORLD_MAP_START = 7411;
        public const int WORLD_CELL_START = 265;

        public const int CHARACTER_CREATION_LEVEL = 1;
        public const int CHARACTER_CREATION_VITALITY = 0;
        public const int CHARACTER_CREATION_WISDOM = 0;
        public const int CHARACTER_CREATION_AGILITY = 0;
        public const int CHARACTER_CREATION_INTELLIGENCE = 0;
        public const int CHARACTER_CREATION_CHANCE = 0;
        public const int CHARACTER_CREATION_STRENGTH = 0;
        public const int CHARACTER_CREATION_AP = 6;
        public const int CHARACTER_CREATION_MP = 3;
        public const int CHARACTER_CREATION_SKIN_SIZE = 100;
        public const int CHARACTER_CREATION_SPELLPOINT = 0;
        public const int CHARACTER_CREATION_CARACPOINT = 0;
        public const int CHARACTER_CREATION_LIFE = 55;
        public const int CHARACTER_CREATION_ENERGY = 10000;
        public const int CHARACTER_CREATION_EMOTE_CAPACITY = 1376255;

        public const int LIVING_CHEST_ID = 285;

        public const int GHOST_SKIN_ID = 8004;

        public const int MAX_MERCHANT_PER_MAP = 5;

        public const int MAX_ENNEMIES = 100;
        public const int MAX_FRIENDS = 100;

        public const double REGEN_TIMER_SIT = 300;
        public const double REGEN_TIMER = 1500;

        public const int PVT_TELEPORT_DEFENDERS_TIMEOUT = 45000;
        public const int PVT_START_TIMEOUT = 60000;
        public const int PVT_TURN_TIME = 30000;

        public const int PVM_MAX_STAR_BONUS = 1000;
        public const int PVM_STAR_BONUS_PERCENT_SECONDS = 10;
        public const int PVM_CHALLENGE_COUNT = 3;
        public const int PVM_START_TIMEOUT = 60000;
        public const int PVM_TURN_TIME = 30000;

        public static double[] PVM_RATE_GROUP = { 1, 1.1, 1.5, 2.3, 3.1, 3.6, 4.2, 4.7 };

        public const double RATE_XP = 5;
        public const double RATE_DROP = 3;
        public const double RATE_KAMAS = 3;

        public const int PVP_START_TIMEOUT = 60000;
        public const int PVP_TURN_TIME = 30000;

        public const int AGGRESSION_KNGIHT_MONSTER_ID = 394;
        public const int AGGRESSION_START_TIMEOUT = 60000;
        public const int AGGRESSION_TURN_TIME = 30000;

        public const double TAXCOLLECTOR_XP_RATIO = 0.01;
        public const int TAXCOLLECTOR_MIN_NAME = 1;
        public const int TAXCOLLECTOR_MAX_NAME = 228;
        public const int TAXCOLLECTOR_MIN_FIRSTNAME = 1;
        public const int TAXCOLLECTOR_MAX_FIRSTNAME = 130;
        public const int TAXCOLLECTOR_SKIN_BASE = 6000;
        public const int TAXCOLLECTOR_SKIN_SIZE_BASE = 100;

        public const int FIGHT_DISCONNECTION_TURN = 20;
        public const int FIGHT_PUSH_CELL_TIME = 270;
        public const int FIGHT_PANDA_LAUNCH_CELL_TIME = 250;

        public static DateTime REFERENCE_DATE = new DateTime(1970, 1, 1);
        public const int GAME_ID = 1; // Jiva ? guess so;
    }
}
