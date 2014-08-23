using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.AI.Actions.Movement
{
    public class AIMoveAction : AIAction
    {
        public int CellId
        {
            get; 
            private set;
        }

        public AIMoveAction(AIFighter fighter, int cellId) : base(fighter)
        {
            CellId = cellId;
        }

        public override AIActionResult Invoke()
        {
            if (Fighter.CellId == CellId)
                return AIActionResult.Success;

			return AIActionResult.Failure;
        }
    }
}
