using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.AI.Brains
{
    public abstract class AIBrain
    {
        public AIFighter Fighter
        {
            get;
            private set;
        }

        protected AIBrain(AIFighter fighter)
        {
            Fighter = fighter;
        }

        protected virtual void OnTurnStart()
        {
            
        }
    }
}
