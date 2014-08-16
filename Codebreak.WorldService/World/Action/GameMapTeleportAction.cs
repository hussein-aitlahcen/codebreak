using Codebreak.WorldService.World.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Action
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameMapTeleportAction : GameActionBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="mapId"></param>
        /// <param name="cellId"></param>
        public GameMapTeleportAction(EntityBase entity, int mapId, int cellId)
            : base(GameActionTypeEnum.MAP_TELEPORT, entity)
        {
            MapId = mapId;
            CellId = cellId;
        }

        public int MapId
        {
            get;
            private set;
        }

        public int CellId
        {
            get;
            private set;
        }

        public override bool CanAbort
        {
            get
            { 
                return false; 
            }
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Abort(params object[] args)
        {
            base.Abort(args);
        }

        public override void Stop(params object[] args)
        {
            Entity.MapId = MapId;
            Entity.CellId = CellId;

            base.Stop(args);
        }

        public override string SerializeAs_GameAction()
        {
            throw new NotImplementedException();
        }
    }
}
