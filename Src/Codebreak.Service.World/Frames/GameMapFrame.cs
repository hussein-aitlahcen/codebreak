using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game.Entity;

namespace Codebreak.Service.World.Frames
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameMapFrame : FrameBase<GameMapFrame, EntityBase, string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Action<EntityBase, string> GetHandler(string message)
        {
            return null;
        }
    }
}
