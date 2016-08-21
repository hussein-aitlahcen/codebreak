using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.Ending
{
    public abstract class AbstractRegenerateBehavior : AbstractEndingBehavior
    {
        public override void Execute(AbstractFight fight)
        {
            foreach (var fighter in fight.Fighters.Where(fighter => CanRegenerate(fight, fighter)))
                fighter.Life = fighter.MaxLife;
        }

        protected abstract bool CanRegenerate(AbstractFight fight, AbstractFighter fighter);
    }
}
