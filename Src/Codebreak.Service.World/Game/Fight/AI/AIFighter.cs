using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Game.Entity;

namespace Codebreak.Service.World.Game.Fight.AI
{
    public abstract class AIFighter : FighterBase
    {
        protected AIFighter(EntityTypEnum type, long id) : base(type, id)
        {

        }

    }
}
