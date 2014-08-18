using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Game.Map
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CellZone
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="cellId"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetAdjacentCells(MapInstance map, int cellId)
        {
            for (int i = 1; i < 8; i += 2)
                yield return Pathfinding.NextCell(map, cellId, i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="cellId"></param>
        /// <param name="direction"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetLineCells(MapInstance map, int cellId, int direction, int length)
        {
            yield return cellId;
            for (int i = 1; i < length + 1; i++)            
                yield return Pathfinding.NextCell(map, cellId, direction, i);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="currentCell"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetCircleCells(MapInstance map, int currentCell, int radius)
        {
            var cells = new List<int>() { currentCell };
            for (int i = 0; i < radius; i++)
            {
                var copy = cells.ToArray();
                foreach (var cell in copy)
                    cells.AddRange(GetAdjacentCells(map, cell).Where(x => !cells.Contains(x)));
            }
            return cells;           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="currentCell"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetCrossCells(MapInstance map, int currentCell, int radius)
        {
            foreach (var cell in GetCircleCells(map, currentCell, radius))
                if (Pathfinding.InLine(map, currentCell, cell))
                    yield return cell;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="cellId"></param>
        /// <param name="direction"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetTLineCells(MapInstance map, int cellId, int direction, int length)
        {
            var lineDirection = direction <= 5 ? direction + 2 : direction - 6;
            yield return cellId;
            foreach (var cell in GetLineCells(map, cellId, lineDirection, length))
                yield return cell;
            foreach(var cell in (GetLineCells(map, cellId, Pathfinding.OppositeDirection(lineDirection), length)))
                yield return cell;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="cellId"></param>
        /// <param name="currentCell"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetCells(MapInstance map, int cellId, int currentCell, string range)
        {
            switch (range[0])
            {
                case 'C':
                    foreach (var cell in GetCircleCells(map, cellId, Util.HASH.IndexOf(range[1])))
                        yield return cell;
                    break;

                case 'X':
                    foreach (var cell in GetCrossCells(map, cellId, Util.HASH.IndexOf(range[1])))
                        yield return cell;
                    break;

                case 'T':
                    foreach (var cell in GetTLineCells(map, cellId, Pathfinding.GetDirection(map, currentCell, cellId), Util.HASH.IndexOf(range[1])))
                        yield return cell;
                    break;

                case 'L':
                    foreach (var cell in GetLineCells(map, cellId, Pathfinding.GetDirection(map, currentCell, cellId), Util.HASH.IndexOf(range[1])))
                        yield return cell;
                    break;

                default:
                    yield return cellId;
                    break;
            }
            
        }
    }
}
