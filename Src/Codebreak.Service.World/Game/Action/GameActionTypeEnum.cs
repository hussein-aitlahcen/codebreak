using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Action
{
    /// <summary>
    /// 
    /// </summary>
    public enum GameActionTypeEnum : int
    {
        MAP_MOVEMENT = 1,
        CHANGE_MAP = 2,
        MAP_TELEPORT = 4,
        MAP_PUSHBACK = 5,
        CHALLENGE_REQUEST = 900,
        CHALLENGE_ACCEPT = 901,
        CHALLENGE_DECLINE = 902,
        FIGHT_JOIN = 903,
        FIGHT_AGGRESSION = 906,
        FIGHT_KILL = 103,
        FIGHT_TACLE = 104,
        FIGHT_ARMOR = 105,
        FIGHT_PA_LOST = 102,
        FIGHT_PM_LOST = 129,

        FIGHT_DAMAGE = 100,
        FIGHT_HEAL = FIGHT_DAMAGE,

        FIGHT_SPELL_LAUNCH = 300,
        FIGHT_CRITICAL_HIT = 301,
        FIGHT_CRITICAL_FAILURE = 302,
        FIGHT_WEAPON_USE = 303,
        FIGHT_WEAPON_FAILURE = 305,
        FIGHT_DODGE_SUBPA = 308,
        FIGHT_DODGE_SUBPM = 309,

        // custom
        MAP,
        EXCHANGE,
        PLAYER_EXCHANGE,
        NPC_EXCHANGE,
        PARTY_REQUEST,
        FIGHT,
        GUILD_CREATE,
        NOTHING,
    }
}
