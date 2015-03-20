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

        private string StringPath
        {
            get;
            set;
        }

        private MovementPath Path
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
                    var WeakestEnnemies = Fighter.Team.OpponentTeam.AliveFighters.OrderBy(fighter => Pathfinding.GoalDistance(Map, Fighter.Cell.Id, fighter.Cell.Id));
                    foreach(var ennemy in WeakestEnnemies)
                    {
                        StringPath = Fighter.Fight.Map.Pathmaker.FindPathAsString(Fighter.Cell.Id, ennemy.Cell.Id, false, Fighter.MP, Fighter.Fight.Obstacles);
                        if (StringPath == string.Empty)
                            continue;

                        Path = Fighter.Fight.Map.DecodeMovement(Fighter, ennemy.Cell.Id, StringPath);
                        if (Path != null)
                            break;
                    }

                    MoveState = MoveStateEnum.STATE_MOVE;

                    return AIActionResult.RUNNING;

                case MoveStateEnum.STATE_MOVE:

                    if (StringPath == null || StringPath == string.Empty || Path == null)
                        return AIActionResult.FAILURE;

                    CellId = Path.EndCell;
                    Timeout = (int)Path.MovementTime;
                    Fighter.Fight.Move(Fighter, Fighter.Cell.Id, StringPath);

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
