using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Action
{
    public sealed class GameFightMovementAction : GameFightActionBase
    {
        /// <summary>
        /// 
        /// </summary>
        public MovementPath Path
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="path"></param>
        public GameFightMovementAction(FighterBase entity, MovementPath path)
            : base(GameActionTypeEnum.MAP_MOVEMENT, entity, path.MovementTime + 1000)
        {
            Path = path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Abort(params object[] args)
        {
            base.Abort(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Stop(params object[] args)
        {
            Entity.MovementHandler.MovementFinish(Entity, Path, Path.EndCell);

            base.Stop(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string SerializeAs_GameAction()
        {
            return Path.ToString();
        }
    }
}
