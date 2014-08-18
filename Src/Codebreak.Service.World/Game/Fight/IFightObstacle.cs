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
    public interface IFightObstacle : IComparable<IFightObstacle>
    {
        FightObstacleTypeEnum ObstacleType
        {
            get;
        }
        int Priority
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
        /// <summary>
        /// 
        /// </summary>
        FightCell Cell
        {
            get;
        }
    }
}
