using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Map
{
    /// <summary>
    /// 
    /// </summary>
    public enum FieldTypeEnum
    {
        TYPE_MAP,
        TYPE_FIGHT,
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IMovementHandler
    {
        /// <summary>
        /// 
        /// </summary>
        bool CanAbortMovement
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        FieldTypeEnum FieldType
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cellId"></param>
        /// <param name="movementPath"></param>
        void Move(EntityBase entity, int cellId, string movementPath);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="path"></param>
        /// <param name="cellId"></param>
        void MovementFinish(EntityBase entity, MovementPath path, int cellId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Dispatch(string message);
    }
}
