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
        private int CellId
        {
            get; 
            set;
        }

        private int RealCellId
        {
            get;
            set;
        }

        public MoveAction(AIFighter fighter) 
            : base(fighter)
        {
            CellId = Fighter.Team.OpponentTeam.AliveFighters.First().Cell.Id;
        }

        public override AIActionResult Initialize()
        {
            var stringPath = Fighter.Fight.Map.Pathmaker.FindPathAsString(Fighter.Cell.Id, CellId, false, Fighter.MP, Fighter.Fight.Obstacles);
            var path = Fighter.Fight.Map.DecodeMovement(Fighter.Cell.Id, stringPath);

            CellId = path.EndCell;
            Timeout = (int)path.MovementTime;

            Fighter.Fight.Move(Fighter, Fighter.Cell.Id, stringPath);
            
			return AIActionResult.Running;
        }

        public override AIActionResult Execute()
        {
            if (!Timedout)
                return AIActionResult.Running;
            
            if (Fighter.CurrentAction != null)
                Fighter.CurrentAction.Stop();

            if (!Fighter.IsFighterDead && Fighter.Fight.CurrentFighter == Fighter && Fighter.Cell.Id != CellId && Fighter.Fight.GetCell(CellId).CanWalk && Fighter.MP > 0)
            {
                return Initialize();
            }

            Logger.Debug("AI MoveAction ended.");
            
            return AIActionResult.Success;
        }
    }
}
