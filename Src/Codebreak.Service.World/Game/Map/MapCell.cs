using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Map
{
    public sealed class MapCell
    {
        public int Id;
        public bool Walkable;
        public bool LineOfSight;
        public int IOId;
        public int NextMap;
        public int NextCell;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="nextMap"></param>
        /// <param name="nextCell"></param>
        public MapCell(int id, byte[] data, int nextMap = 0, int nextCell = 0)
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
                IOId = ((data[0] & 2) << 12) + ((data[1] & 1) << 12) + (data[8] << 6) + data[9];
            }
        }
    }
}
