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
        public const int RPC_ACCOUNT_TICKET_TIMEOUT = 5000; 

        public const int
            WORLD_SAVE_INTERVAL = 60 * 1000,
            WORLD_MAP_START = 7411,
            WORLD_CELL_START = 265;

        public const int
            CHARACTER_CREATION_LEVEL = 1,
            CHARACTER_CREATION_VITALITY = 0,
            CHARACTER_CREATION_WISDOM = 0,
            CHARACTER_CREATION_AGILITY = 0,
            CHARACTER_CREATION_INTELLIGENCE = 0,
            CHARACTER_CREATION_CHANCE = 0,
            CHARACTER_CREATION_STRENGTH = 0,
            CHARACTER_CREATION_AP = 6,
            CHARACTER_CREATION_MP = 3,
            CHARACTER_CREATION_SKIN_SIZE = 100, // 100%
            CHARACTER_CREATION_SPELLPOINT = 0,
            CHARACTER_CREATION_CARACPOINT = 0,
            CHARACTER_CREATION_LIFE = 55,
            CHARACTER_CREATION_ENERGY = 10000,
            CHARACTER_CREATION_EMOTE_CAPACITY = 1376255;

        public const int
            MAX_MERCHANT_PER_MAP = 5;

        public const int
            MAX_ENNEMIES = 100,
            MAX_FRIENDS = 100;

        public const double
            REGEN_TIMER_SIT = 500,
            REGEN_TIMER = 1500;


        public const int
            TAXCOLLECTOR_MIN_NAME = 1,
            TAXCOLLECTOR_MAX_NAME = 228,
            TAXCOLLECTOR_MIN_FIRSTNAME = 1,
            TAXCOLLECTOR_MAX_FIRSTNAME = 130,
            TAXCOLLECTOR_SKIN_BASE = 6000,
            TAXCOLLECTOR_SKIN_SIZE_BASE = 100;

        public const int 
            FIGHT_DISCONNECTION_TURN = 20, 
            FIGHT_PUSH_CELL_TIME = 400,
            FIGHT_PANDA_LAUNCH_CELL_TIME = 250;

        public static DateTime REFERENCE_DATE = new DateTime(1970, 1, 1);
        public const int GAME_ID = 1; // Jiva ? guess so;
    }
}
