using Codebreak.Framework.Network;
using Codebreak.WorldService.World.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Handler
{
    public sealed class GameMapFrame : FrameBase<GameMapFrame, EntityBase, string>
    {
        public override Action<EntityBase, string> GetHandler(string message)
        {
            return null;
        }
    }
}
