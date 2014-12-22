using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.AI.Action.Type
{
    public class EndTurnAction : AIAction
    {
        public EndTurnAction(AIFighter fighter)
            : base(fighter)
        {
        }

        public override AIActionResult Initialize()
        {
            return AIActionResult.Running;
        }

        public override AIActionResult Execute()
        {
            Fighter.TurnPass = true;

            return AIActionResult.Success;
        }
    }
}
