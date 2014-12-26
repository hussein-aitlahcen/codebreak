using Codebreak.Service.World.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.AI.Action.Type
{
    public enum MoveStateEnum
    {
        STATE_CALCULATE_CELL,
        STATE_MOVE,
        STATE_MOVING,
    }

    public class MoveAction : AIAction
    {
        private MoveStateEnum MoveState
        {
            get;
            set;
        }

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
        }

        public override AIActionResult Initialize()
        {
            MoveState = MoveStateEnum.STATE_CALCULATE_CELL;

            return Fighter.MP > 0 ? AIActionResult.RUNNING : AIActionResult.FAILURE;
        }

        public override AIActionResult Execute()
        {
            switch (MoveState)
            {
                case MoveStateEnum.STATE_CALCULATE_CELL:
                    CellId = Fighter.Team.OpponentTeam.AliveFighters.OrderBy(fighter => fighter.Life).First().Cell.Id;
                    MoveState = MoveStateEnum.STATE_MOVE;

                    return AIActionResult.RUNNING;

                case MoveStateEnum.STATE_MOVE:

                    var stringPath = Fighter.Fight.Map.Pathmaker.FindPathAsString(Fighter.Cell.Id, CellId, false, Fighter.MP, Fighter.Fight.Obstacles);
                    if (stringPath == string.Empty)
                        return AIActionResult.FAILURE;

                    var path = Fighter.Fight.Map.DecodeMovement(Fighter.Cell.Id, stringPath);
                    if (path == null)
                        return AIActionResult.FAILURE;

                    CellId = path.EndCell;
                    Timeout = (int)path.MovementTime;
                    Fighter.Fight.Move(Fighter, Fighter.Cell.Id, stringPath);

                    MoveState = MoveStateEnum.STATE_MOVING;

                    return AIActionResult.RUNNING;

                case MoveStateEnum.STATE_MOVING:

                    if (!Timedout)
                        return AIActionResult.RUNNING;  
  
                    if (Fighter.CurrentAction != null)
                        Fighter.CurrentAction.Stop();

                    if (!Fighter.IsFighterDead && Fighter.Fight.CurrentFighter == Fighter && Fighter.Cell.Id != CellId && Fighter.Fight.GetCell(CellId).CanWalk)
                        return Initialize();
                    
                    return AIActionResult.SUCCESS;

                default:
                    throw new Exception("AI movement action : invalid state.");
            }                       
        }
    }
}
