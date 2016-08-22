using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Game.Map;

namespace Codebreak.Service.World.Game.Interactive.Type
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class HouseDoor : InteractiveObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="cellId"></param>
        /// <param name="canWalkThrough"></param>
        public HouseDoor(MapInstance map, int cellId, bool canWalkThrough = false)
            : base(map, cellId, canWalkThrough)
        {
            // TODO: implement houses
            Logger.Debug(string.Format("house door on mapId: {0}, cellId: {1}", map.Id, cellId));
        }
    }
}
