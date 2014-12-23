using Codebreak.Service.World.Game.Fight.AI.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.AI.Brain
{
    public abstract class AIBrain
    {
        public AIFighter Fighter
        {
            get;
            private set;
        }

        public AIAction CurrentAction
        {
            get;
            protected set;
        }

        protected AIBrain(AIFighter fighter)
        {
            Fighter = fighter;
        }

        public virtual void OnTurnStart()
        {            
        }

        public virtual void OnUpdate()
        {
            if(CurrentAction != null)
            {
                CurrentAction.Update();
                if (CurrentAction.State == AIActionState.FINISH)
                    CurrentAction = CurrentAction.NextAction;
            }
        }
    }
}
