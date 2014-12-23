using Codebreak.Service.World.Game.Fight.AI.Action.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.AI.Brain
{
    public sealed class DefaultAIBrain : AIBrain
    {
        public DefaultAIBrain(AIFighter fighter) 
            : base(fighter)
        {
        }

        public override void OnTurnStart()
        {
            CurrentAction = new MoveAction(Fighter);
            CurrentAction.LinkWith(new AttackAction(Fighter))
                .LinkWith(new EndTurnAction(Fighter));
        }
    }
}
