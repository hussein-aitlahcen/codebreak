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
    public sealed class Chest : InteractiveObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="cellId"></param>
        /// <param name="canWalkThrough"></param>
        public Chest(MapInstance map, int cellId, bool canWalkThrough = false) 
            : base(map, cellId, canWalkThrough)
        {
            Logger.Debug(string.Format("chest on mapId: {0}, cellId: {1}", map.Id, cellId));
        }
    }
}
