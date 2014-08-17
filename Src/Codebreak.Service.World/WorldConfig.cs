using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService
{
    public static class WorldConfig
    {
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
            CHARACTER_CREATION_ENERGY = 10000;

        public const int FIGHT_DISCONNECTION_TURN = 20;

        public static DateTime REFERENCE_DATE = new DateTime(1970, 1, 1);
        public const int GAME_ID = 1; // Jiva ? guess so;
    }
}
