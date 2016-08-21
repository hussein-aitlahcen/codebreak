using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Interactive.Type;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Action
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameWaypointAction : AbstractGameAction
    {
        /// <summary>
        /// 
        /// </summary>
        public override bool CanAbort => false;

        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity Character
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Waypoint Waypoint
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="waypoint"></param>
        public GameWaypointAction(CharacterEntity character, Waypoint waypoint)
            : base(GameActionTypeEnum.WAYPOINT, character)
        {
            Character = character;
            Waypoint = waypoint;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            Character.Dispatch(WorldMessage.WAYPOINT_CREATE(Character));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Abort(params object[] args)
        {
            Character.Dispatch(WorldMessage.WAYPOINT_LEAVE());
            base.Abort(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Stop(params object[] args)
        {
            Character.Dispatch(WorldMessage.WAYPOINT_LEAVE());
            base.Stop(args);
        }
    }
}
