using System;
using System.Linq;
using System.Collections.Generic;
using Codebreak.Framework.Utils;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Entity;
using System.Text;
using System.Web;
using System.Globalization;

namespace Codebreak.Service.World
{
    /// <summary>
    /// 
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// 
        /// </summary>
        public static List<char> HASH = new List<char>() {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
                't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'};

        /// <summary>
        /// 
        /// </summary>
        private static char[] CHAR_LIST = new char[] 
                                         {
                                             '0', '1','2','3','4','5','6','7','8','9',
                                             'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
                                         };
        
        /// <summary>
        /// 
        /// </summary>
        private static FastRandom Random = new FastRandom();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Next(int min, int max)
        {
            return Random.Next(min, max);
        }        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int NextJet(int min, int max)
        {
            max++;
            if (max <= min)
                return min;
            return Next(min, max);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String EncodeBase36(long input)
        {
            var result = new Stack<char>();
            bool negative = input < 0;
            input = Math.Abs(input);

            while (input != 0)
            {
                result.Push(CHAR_LIST[input % 36]);
                input /= 36;
            }

            if (negative) result.Push('-');

            return new string(result.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellId"></param>
        /// <returns></returns>
        public static string CellToChar(int cellId)
        {
            return HASH[cellId / 64].ToString() + HASH[cellId % 64];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellCode"></param>
        /// <returns></returns>
        public static int CharToCell(string cellCode)
        {
            char char1 = cellCode[0], char2 = cellCode[1];
            int code1 = 0, code2 = 0, a = 0;
            while (a < HASH.Count)
            {
                if (HASH[a] == char1)
                {
                    code1 = a * 64;
                }
                if (HASH[a] == char2)
                {
                    code2 = a;
                }
                a++;
            }
            return (code1 + code2);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="winnersLevel"></param>
        /// <param name="losersLevel"></param>
        /// <returns></returns>
        public static int CalculWinHonor(int level, int winnersLevel, int losersLevel)
        {
            var basic = Math.Sqrt(level) * 10;
            var coef = losersLevel / winnersLevel;

            return (int)Math.Floor(basic * coef);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="winnersLevel"></param>
        /// <param name="losersLevel"></param>
        /// <returns></returns>
        public static int CalculLoseHonor(int level, int winnersLevel, int losersLevel)
        {
            var basic = Math.Sqrt(level) * 10;
            var coef = losersLevel / winnersLevel;

            return (int)Math.Floor(basic * coef);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loot"></param>
        /// <param name="PP"></param>
        /// <param name="winnersTotalPP"></param>
        /// <returns></returns>
        public static long CalculPVMKamas(long loot, int PP, long winnersTotalPP)
        {
            return (long)Math.Round(loot * (PP / (double)winnersTotalPP) * WorldConfig.RATE_KAMAS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monsters"></param>
        /// <param name="droppers"></param>
        /// <param name="level"></param>
        /// <param name="wisdom"></param>
        /// <param name="ageBonus"></param>
        /// <returns></returns>
        public static long CalculPVMExperienceTaxCollector(IEnumerable<MonsterEntity> monsters, IEnumerable<FighterBase> droppers, int level, int wisdom, int challengeBonus = 0, int ageBonus = 0)
        {
            return (long)(CalculPVMExperience(monsters, droppers, level, wisdom, challengeBonus, ageBonus) * WorldConfig.TAXCOLLECTOR_XP_RATIO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monsters"></param>
        /// <param name="droppers"></param>
        /// <param name="level"></param>
        /// <param name="wisdom"></param>
        /// <param name="ageBonus"></param>
        /// <returns></returns>
        public static long CalculPVMExperience(IEnumerable<MonsterEntity> monsters, IEnumerable<FighterBase> droppers, int level, int wisdom, int challengeBonus = 0, int ageBonus = 0)
        {
            var monstersExperience = monsters.Sum(monster => monster.Grade.Experience);
            var monstersTotalLevel = monsters.Sum(monster => monster.Grade.Level);
            var monstersMaxLevel = monsters.Max(monster => monster.Grade.Level);

            var playersTotalLevel = droppers.Sum(player => player.Level);

            double totalLevelDeltaRate = 1;
            if (playersTotalLevel - 5 > monstersTotalLevel)
                totalLevelDeltaRate = monstersTotalLevel / (double)playersTotalLevel;
            else if (playersTotalLevel + 10 < monstersTotalLevel)
                totalLevelDeltaRate = (playersTotalLevel + 10) / (double)monstersTotalLevel;

            double levelDeltaRate = 1;
            if (level - 5 > monstersTotalLevel)
                levelDeltaRate = monstersTotalLevel / (double)level;
            else if (level + 10 < monstersTotalLevel)
                levelDeltaRate = (level + 10) / (double)monstersTotalLevel;

            var a = Math.Min(level, Math.Truncate(2.5 * monstersMaxLevel));
            var b = Math.Truncate(a / (double)level * 100);
            var c = Math.Truncate(a / (double)playersTotalLevel * 100);
            var d = Math.Truncate(monstersExperience * WorldConfig.PVM_RATE_GROUP[0] * levelDeltaRate);
            var e = Math.Truncate(monstersExperience * WorldConfig.PVM_RATE_GROUP[Math.Min(WorldConfig.PVM_RATE_GROUP.Length - 1, droppers.Count() - 1)] * totalLevelDeltaRate);
            var f = Math.Truncate(b / 100 * d);
            var g = Math.Truncate(c / 100 * e);
            var h = wisdom;
            var i = Math.Max(1, ageBonus / 100.0) + challengeBonus;
            var j = Math.Truncate((g * (100 + h) / 100.0) * i);
            var k = WorldConfig.RATE_XP;

            return (long)Math.Truncate(j * k);
        }       
    }
}
