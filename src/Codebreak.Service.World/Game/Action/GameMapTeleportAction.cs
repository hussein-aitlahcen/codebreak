using Codebreak.Service.World.Game.Entity;
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
    public sealed class GameMapTeleportAction : GameActionBase
    {
        /// <summary>
        /// 
        /// </summary>
        public override bool CanAbort
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MapId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int CellId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="mapId"></param>
        /// <param name="cellId"></param>
        public GameMapTeleportAction(AbstractEntity entity, int mapId, int cellId)
            : base(GameActionTypeEnum.MAP_TELEPORT, entity)
        {
            MapId = mapId;
            CellId = cellId;
        }
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Stop(params object[] args)
        {
            Entity.MapId = MapId;
            Entity.CellId = CellId;

            base.Stop(args);
        }
    }
}
