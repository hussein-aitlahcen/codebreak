using Codebreak.Service.World.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.AI.Action.Type
{
    public class MoveAction : AIAction
    {
        public int CellId
        {
            get; 
            private set;
        }

        public MoveAction(AIFighter fighter, int cellId) 
            : base(fighter)
        {
            CellId = cellId;
        }

        public override AIActionResult Initialize()
        {
            Fighter.Fight.Move(Fighter, Fighter.Cell.Id, new Pathmaker(Fighter.Fight.Map).FindPathAsString(Fighter.Cell.Id, CellId, false, Fighter.MP, Fighter.Fight.Obstacles));

			return AIActionResult.Running;
        }

        public override AIActionResult Execute()
        {
            if (!Timedout)
                return AIActionResult.Running;
            
            if(Fighter.CurrentAction != null)
                Fighter.CurrentAction.Stop();

            return AIActionResult.Success;
        }
    }
}
