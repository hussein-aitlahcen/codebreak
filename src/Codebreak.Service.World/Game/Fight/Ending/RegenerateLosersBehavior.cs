using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.Ending
{
    public sealed class RegenerateLosersBehavior : AbstractRegenerateBehavior
    {
        protected override bool CanRegenerate(AbstractFight fight, AbstractFighter fighter)
        {
            return fight.LoserFighters.Contains(fighter);
        }
    }
}
