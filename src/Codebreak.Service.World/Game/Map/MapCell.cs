using Codebreak.Service.World.Game.Interactive;
using Codebreak.Service.World.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Map
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MapCell
    {
        public int Id;
        public bool Walkable;
        public bool LineOfSight;
        public int InteractiveObjectId;
        public int NextMap;
        public int NextCell;

        /// <summary>
        /// 
        /// </summary>
        public InteractiveObject InteractiveObject
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="nextMap"></param>
        /// <param name="nextCell"></param>
        public MapCell(MapInstance map, int id, byte[] data, int nextMap = 0, int nextCell = 0)
        {
            Id = id;
            NextMap = nextMap;
            NextCell = nextCell;
            Walkable = ((data[2] & 56) >> 3) > 0;
            if (!Walkable && ((data[2] & 56) >> 3) != 0)
            {
                return;
            }
            LineOfSight = (data[0] & 1) == 1;
            if ((data[7] & 2) >> 1 == 1)
            {
                InteractiveObjectId = ((data[0] & 2) << 12) + ((data[1] & 1) << 12) + (data[8] << 6) + data[9];
                if (InteractiveObjectManager.Instance.Exists(InteractiveObjectId))
                {
                    InteractiveObject = InteractiveObjectManager.Instance.Generate(InteractiveObjectId, map, Id);
                }
            }
        }
    }
}
