using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight
{
    public enum FightObstacleTypeEnum
    {
        TYPE_FIGHTER,
        TYPE_TRAP,
        TYPE_GLYPH,
        TYPE_CAWWOT,
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IFightObstacle
    {
        FightObstacleTypeEnum ObstacleType
        {
            get;
        }
        bool CanGoThrough
        {
            get;
        }
        bool CanStack
        {
            get;
        }
    }
}
