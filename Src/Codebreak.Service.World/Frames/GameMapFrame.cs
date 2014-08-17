using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game.Entity;

namespace Codebreak.Service.World.Frames
{
    public sealed class GameMapFrame : FrameBase<GameMapFrame, EntityBase, string>
    {
        public override Action<EntityBase, string> GetHandler(string message)
        {
            return null;
        }
    }
}
