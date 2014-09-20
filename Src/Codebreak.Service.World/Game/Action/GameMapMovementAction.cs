using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Action
{
    public sealed class GameMapMovementAction : GameActionBase
    {
        public GameMapMovementAction(EntityBase entity, MovementPath path)
            : base(GameActionTypeEnum.MAP_MOVEMENT, entity)
        {
            Path = path;
            Duration = path.MovementTime;
        }

        /// <summary>
        /// 
        /// </summary>
        public MovementPath Path
        {
            get;
            private set;
        }

        public double Duration
        {
            get;
            private set;
        }

        public override bool CanAbort
        {
            get 
            {
                return true;
            }
        }

        public override void Start()
        {
        }

        public override void Abort(params object[] args)
        {
            int stopCell = 0;
            if (args.Length > 0)
            {                
                stopCell = int.Parse(args[0].ToString());
            }
            else
            {
                stopCell = Entity.CellId;
            }

            // Cas d'une deconnexion
            if (stopCell == Entity.Id)
                stopCell = Entity.CellId;

            Entity.MovementHandler.MovementFinish(Entity, Path, stopCell);

            base.Abort(args);
        }

        public override void Stop(params object[] args)
        {
            Entity.MovementHandler.MovementFinish(Entity, Path, Path.EndCell);

            base.Stop(args);
        }

        public override string SerializeAs_GameAction()
        {
            return Path.ToString();
        }
    }
}
