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
        /// <summary>
        /// 
        /// </summary>
        FightObstacleTypeEnum ObstacleType
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        int Priority
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        bool CanGoThrough
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
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
