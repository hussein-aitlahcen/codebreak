
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Framework.Utils;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Game.Interactive.Type;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Job;
using log4net;

namespace Codebreak.Service.World.Game.Map
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPriorityQueue<T>
    {
        int Push(T item);
        T Pop();
        T Peek();
        void Update(int i);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueueB<T> : IPriorityQueue<T>
    {
        #region "Variables Declaration"
        protected List<T> InnerList = new List<T>();
        protected IComparer<T> mComparer;
        #endregion

        #region "Contructors"
        public PriorityQueueB()
        {
            mComparer = Comparer<T>.Default;
        }

        public PriorityQueueB(IComparer<T> comparer)
        {
            mComparer = comparer;
        }

        public PriorityQueueB(IComparer<T> comparer, int capacity)
        {
            mComparer = comparer;
            InnerList.Capacity = capacity;
        }
        #endregion

        #region "Methods"
        protected void SwitchElements(int i, int j)
        {
            T h = InnerList[i];
            InnerList[i] = InnerList[j];
            InnerList[j] = h;
        }

        protected virtual int OnCompare(int i, int j)
        {
            return mComparer.Compare(InnerList[i], InnerList[j]);
        }

        /// <summary>
        /// Push an object onto the PQ
        /// </summary>
        /// <param name="O">The new object</param>
        /// <returns>The index in the list where the object is _now_. This will change when objects are taken from or put onto the PQ.</returns>
        public int Push(T item)
        {
            int p = InnerList.Count;
            int p2 = 0;
            InnerList.Add(item);
            // E[p] = O
            do
            {
                if (p == 0)
                {
                    break; // TODO: might not be correct. Was : Exit Do
                }
                p2 = (p - 1) / 2;
                if (OnCompare(p, p2) < 0)
                {
                    SwitchElements(p, p2);
                    p = p2;
                }
                else
                {
                    break; // TODO: might not be correct. Was : Exit Do
                }
            } while (true);
            return p;
        }

        /// <summary>
        /// Get the smallest object and remove it.
        /// </summary>
        /// <returns>The smallest object</returns>
        public T Pop()
        {
            T result = InnerList[0];
            int p = 0;
            int p1 = 0;
            int p2 = 0;
            int pn = 0;
            InnerList[0] = InnerList[InnerList.Count - 1];
            InnerList.RemoveAt(InnerList.Count - 1);
            do
            {
                pn = p;
                p1 = 2 * p + 1;
                p2 = 2 * p + 2;
                if (InnerList.Count > p1 && OnCompare(p, p1) > 0)
                {
                    // links kleiner
                    p = p1;
                }
                if (InnerList.Count > p2 && OnCompare(p, p2) > 0)
                {
                    // rechts noch kleiner
                    p = p2;
                }

                if (p == pn)
                {
                    break; // TODO: might not be correct. Was : Exit Do
                }
                SwitchElements(p, pn);
            } while (true);

            return result;
        }

        /// <summary>
        /// Notify the PQ that the object at position i has changed
        /// and the PQ needs to restore order.
        /// Since you dont have access to any indexes (except by using the
        /// explicit IList.this) you should not call this function without knowing exactly
        /// what you do.
        /// </summary>
        /// <param name="i">The index of the changed object.</param>
        public void Update(int i)
        {
            int p = i;
            int pn = 0;
            int p1 = 0;
            int p2 = 0;
            do
            {
                // aufsteigen
                if (p == 0)
                {
                    break; // TODO: might not be correct. Was : Exit Do
                }
                p2 = (p - 1) / 2;
                if (OnCompare(p, p2) < 0)
                {
                    SwitchElements(p, p2);
                    p = p2;
                }
                else
                {
                    break; // TODO: might not be correct. Was : Exit Do
                }
            } while (true);
            if (p < i)
            {
                return;
            }
            do
            {
                // absteigen
                pn = p;
                p1 = 2 * p + 1;
                p2 = 2 * p + 2;
                if (InnerList.Count > p1 && OnCompare(p, p1) > 0)
                {
                    // links kleiner
                    p = p1;
                }
                if (InnerList.Count > p2 && OnCompare(p, p2) > 0)
                {
                    // rechts noch kleiner
                    p = p2;
                }

                if (p == pn)
                {
                    break; // TODO: might not be correct. Was : Exit Do
                }
                SwitchElements(p, pn);
            } while (true);
        }

        /// <summary>
        /// Get the smallest object without removing it.
        /// </summary>
        /// <returns>The smallest object</returns>
        public T Peek()
        {
            if (InnerList.Count > 0)
            {
                return InnerList[0];
            }
            return default(T);
        }

        public void Clear()
        {
            InnerList.Clear();
        }

        public int Count
        {
            get { return InnerList.Count; }
        }

        public void RemoveLocation(T item)
        {
            int index = -1;

            for (int i = 0; i <= InnerList.Count - 1; i++)
            {
                if (mComparer.Compare(InnerList[i], item) == 0)
                {
                    index = i;
                }
            }

            if (index != -1)
            {
                InnerList.RemoveAt(index);
            }
        }

        public T this[int index]
        {
            get { return InnerList[index]; }
            set
            {
                InnerList[index] = value;
                Update(index);
            }
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class MovementPath
    {
        /// <summary>
        /// 
        /// </summary>
        public List<int> TransitCells
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<int> Directions
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int BeginCell
        {
            get
            {
                return TransitCells.FirstOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MovementLength
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public double MovementTime
        {
            get
            {
                return Pathfinding.GetPathTime(MovementLength, GetDirection(LastStep));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int LastStep
        {
            get
            {
                return TransitCells[TransitCells.Count < 2 ? 0 : TransitCells.Count - 2];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int EndCell
        {
            get
            {
                return TransitCells.LastOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private StringBuilder m_serializedPath;
        
        /// <summary>
        /// 
        /// </summary>
        public MovementPath()
        {
            TransitCells = new List<int>();
            Directions = new List<int>();
        }
                   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cell"></param>
        /// <param name="Direction"></param>
        public void AddCell(int Cell, int Direction)
        {
            TransitCells.Add(Cell);
            Directions.Add(Direction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cell"></param>
        /// <returns></returns>
        public int GetDirection(int Cell)
        {
            return Directions[TransitCells.Count == 1 ? 0 : TransitCells.IndexOf(Cell) + 1];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (m_serializedPath == null)
            {
                m_serializedPath = new StringBuilder();
                for (int i = 0; i < TransitCells.Count; i++)
                {
                    m_serializedPath.Append(Pathfinding.GetDirectionChar(Directions[i]));
                    m_serializedPath.Append(Util.CellToChar(TransitCells[i]));
                }
            }
            return m_serializedPath.ToString();
        }
    }

    public struct Point
    {
        public double X;
        public double Y;
        public double Z;

        public Point(double x, double y, double z = 0)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class Pathfinding
    {
        private static ILog Logger = LogManager.GetLogger(typeof(Pathfinding));

        public static double[] RUN_SPEEDS = { 1.700000E-001, 1.500000E-001, 1.500000E-001, 1.500000E-001, 1.700000E-001, 1.500000E-001, 1.500000E-001, 1.500000E-001 };
        public static double[] WALK_SPEEDS = { 7.000000E-002, 6.000000E-002, 6.000000E-002, 6.000000E-002, 7.000000E-002, 6.000000E-002, 6.000000E-002, 6.000000E-002 };
        public static double[] MOUNT_SPEEDS = { 2.300000E-001, 2.000000E-001, 2.000000E-001, 2.000000E-001, 2.300000E-001, 2.000000E-001, 2.000000E-001, 2.000000E-001 };

        private static FastRandom PATHFIND_RANDOM = new FastRandom();
        private static int[] FIGHT_DIRECTIONS = { 1, 3, 5, 7 };

        private static Dictionary<int, int[]> MapDirections = new Dictionary<int, int[]>();
        private static Dictionary<int, Dictionary<int, Point>> CellPoints = new Dictionary<int, Dictionary<int, Point>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static double GetPathTime(int length, int direction)
        {
            return ((length >= 4 ? Pathfinding.RUN_SPEEDS[direction] : Pathfinding.WALK_SPEEDS[direction]) * 1100 * length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="beginCell"></param>
        /// <param name="encodedPath"></param>
        /// <returns></returns>
        public static int GetPathLength(MapInstance map, int beginCell, string encodedPath)
        {
            var lastCell = beginCell;
            var length = 0;

            for (int i = 0; i < encodedPath.Length; i += 3)
            {
                var actualCell = Util.CharToCell(encodedPath.Substring(i + 1, 2));
                length += Pathfinding.GoalDistance(map, lastCell, actualCell);
                lastCell = actualCell;
            }

            return length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="cellsCount"></param>
        public static void GenerateGrid(int width, int cellsCount)
        {
            var Grid = new Dictionary<int, Point>(cellsCount);

            for (int i = 0; i < cellsCount; i++)
            {
                Grid.Add(i, new Point(_GetX(width, i), _GetY(width, i)));
            }

            CellPoints.Add(cellsCount, Grid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static double _GetX(int width, int cell)
        {
            double loc5 = Math.Floor((double)(cell / (width * 2 - 1)));
            double loc6 = cell - loc5 * (width * 2 - 1);
            double loc7 = loc6 % width;

            return (cell - (width - 1) * (loc5 - loc7)) / width;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static double _GetY(int width, int cell)
        {
            double loc5 = Math.Floor((double)(cell / (width * 2 - 1)));
            double loc6 = cell - loc5 * (width * 2 - 1);
            double loc7 = loc6 % width;

            return loc5 - loc7;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="Cell"></param>
        /// <returns></returns>
        public static Point GetPoint(MapInstance map, int cell)
        {
            if (CellPoints.ContainsKey(map.Cells.Count))
            {
                if (!CellPoints[map.Cells.Count].ContainsKey(cell))
                {
                    Logger.Info("Pathfinding::GetPoint unknow cell : cellId=" + cell + " cellCount=" + map.Cells.Count);
                    var point = new Point(_GetX(map.Width, cell), _GetY(map.Width, cell));
                    CellPoints[map.Cells.Count].Add(cell, point);
                    return point;   
                }
                else
                {
                    return CellPoints[map.Cells.Count][cell];
                }
            }

            Pathfinding.GenerateGrid(map.Width, map.Cells.Count);

            return GetPoint(map, cell);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static double GetX(MapInstance map, int cell)
        {
            if (!CellPoints.ContainsKey(map.Cells.Count))
                Pathfinding.GenerateGrid(map.Width, map.Cells.Count);

            Point p = new Point();

            if (CellPoints[map.Cells.Count].TryGetValue(cell, out p))
                return p.X;

            return -1000;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static double GetY(MapInstance map, int cell)
        {
            if (!CellPoints.ContainsKey(map.Cells.Count))
                Pathfinding.GenerateGrid(map.Width, map.Cells.Count);

            Point p = new Point();

            if (CellPoints[map.Cells.Count].TryGetValue(cell, out p))
                return p.Y;

            return -1000;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="beginCell"></param>
        /// <param name="endCell"></param>
        /// <returns></returns>
        public static bool InLine(MapInstance map, int beginCell, int endCell)
        {
            var beginPoint = GetPoint(map, beginCell);
            var endPoint = GetPoint(map, endCell);
            
            return beginPoint.X == endPoint.X || beginPoint.Y == endPoint.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="beginCell"></param>
        /// <param name="endCell"></param>
        /// <returns></returns>
        public static int GoalDistance(MapInstance map, int beginCell, int endCell)
        {
            var beginPoint = GetPoint(map, beginCell);
            var endPoint = GetPoint(map, endCell);
            var distance = (int)(Math.Abs(endPoint.X - beginPoint.X) + Math.Abs(endPoint.Y - beginPoint.Y));
            
            return distance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static char GetDirectionChar(int direction)
        {
            return Util.HASH[direction];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static int GetDirection(char direction)
        {
            return Util.HASH.IndexOf(direction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static int[] GetDirectionChanges(MapInstance map)
        {
            if (MapDirections.ContainsKey(map.Id))
                return MapDirections[map.Id];

            var directions = new int[] 
            { 
                1, 
                map.Width,
                map.Width * 2 - 1, 
                map.Width - 1, -1,
                -map.Width,
                -map.Width * 2 + 1, 
                -(map.Width - 1)
            };

            MapDirections.Add(map.Id, directions);

            return directions;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="beginCell"></param>
        /// <param name="dndCell"></param>
        /// <returns></returns>
        public static int GetDirection(MapInstance map, int beginCell, int dndCell)
        {
            var listChange = GetDirectionChanges(map);
            var result = dndCell - beginCell;
            var direction = 0;

            for (int i = 7; i > -1; i--)
                if (result == listChange[i])
                    direction = i;

            var beginPoint = GetPoint(map, beginCell);
            var endPoint = GetPoint(map, dndCell);
            var resultX = endPoint.X - beginPoint.X;
            var resultY = endPoint.Y - beginPoint.Y;

            if (resultX == 0)
                if (resultY > 0)
                    direction = 3;
                else
                    direction = 7;
            else if (resultX > 0)
                direction = 1;
            else
                direction = 5;
            
            return direction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="currentCell"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static MovementPath DecodePath(MapInstance map, int currentCell, string path)
        {
            MovementPath movementPath = new MovementPath();

            if (path == "")
                return movementPath;

            movementPath.AddCell(currentCell, GetDirection(map, currentCell, Util.CharToCell(path.Substring(1, 2))));

            for (int i = 0; i < path.Length; i += 3)
            {
                int curCell = Util.CharToCell(path.Substring(i + 1, 2));
                int curDir = Util.HASH.IndexOf(path[i]);

                movementPath.AddCell(curCell, curDir);
            }

            return movementPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static int OppositeDirection(int direction)
        {
            return (direction >= 4 ? direction - 4 : direction + 4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="cellId"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static int NextCell(MapInstance map, int cellId, int direction, int length = 1)
        {
            switch (direction)
            {
                case 0:
                    return cellId + (1 * length);
                case 1:
                    return cellId + (map.Width * length);
                case 2:
                    return cellId + (((map.Width * 2) - 1) * length);
                case 3:
                    return cellId + ((map.Width - 1) * length);
                case 4:
                    return cellId - (1 * length);
                case 5:
                    return cellId - (map.Width * length);
                case 6:
                    return cellId - (((map.Width * 2) - 1) * length);
                case 7:
                    return cellId - ((map.Width - 1) * length);
                default:
                    return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="currentCell"></param>
        /// <param name="encodedPath"></param>
        /// <returns></returns>
        public static MovementPath IsValidPath(AbstractEntity entity, MapInstance map, int currentCell, string encodedPath)
        {
            var decodedPath = DecodePath(map, currentCell, encodedPath);
            if(decodedPath.TransitCells.Count == 0)
                return null;
            var finalPath = new MovementPath();
            var index = 0;
            int transitCell = 0;
            int nextTransitCell = 0;
            int direction = 0;
            do
            {
                transitCell = decodedPath.TransitCells[index];
                nextTransitCell = decodedPath.TransitCells[index + 1];
                direction = decodedPath.GetDirection(transitCell);
                var length = Pathfinding.IsValidLine(entity ,map, finalPath, transitCell, direction, nextTransitCell, decodedPath.EndCell);
                if (length == -1)
                    return null;
                else if (length == -2)
                    break;                
                index++;
            }
            while (transitCell != decodedPath.LastStep);

            if(entity.Type == EntityTypeEnum.TYPE_CHARACTER)
            {
                var mapCell = map.GetCell(decodedPath.EndCell);
                if(mapCell != null && mapCell.InteractiveObject != null && mapCell.InteractiveObject is Pheonix)
                {
                    var character = (CharacterEntity)entity;
                    character.AutomaticSkillId = (int)SkillIdEnum.SKILL_USE_PHOENIX;
                    character.AutomaticSkillCellId = decodedPath.EndCell;
                    character.AutomaticSkillMapId = map.Id;
                }
            }

            return finalPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        /// <param name="fighter"></param>
        /// <param name="currentCell"></param>
        /// <param name="encodedPath"></param>
        /// <returns></returns>
        public static MovementPath IsValidPath(FightBase fight, FighterBase fighter, int currentCell, string encodedPath)
        {
            if (encodedPath == "")
                return null;

            var decodedPath = DecodePath(fight.Map, currentCell, encodedPath);
            var finalPath = new MovementPath();

            var index = 0;
            int transitCell = 0;
            do
            {
                transitCell = decodedPath.TransitCells[index];
                var length = Pathfinding.IsValidLine(fight, fighter, finalPath, transitCell, decodedPath.GetDirection(transitCell), decodedPath.TransitCells[index + 1]);
                if (length == -1)
                    return null;
                else if (length == -2)
                    break;
                index++;
            }
            while (transitCell != decodedPath.LastStep);

            return finalPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="beginCell"></param>
        /// <param name="direction"></param>
        /// <param name="endCell"></param>
        /// <returns></returns>
        public static int IsValidLine(AbstractEntity entity, MapInstance map, MovementPath finalPath, int beginCell, int direction, int endCell, int finalCell)
        {
            var length = -1;
            var actualCell = beginCell;
            var lastCell = beginCell;
            
            finalPath.AddCell(actualCell, direction);

            const int MAX_LOOP = 100;
            var time = 0;
            do
            {
                time++;
                if(time > MAX_LOOP)
                    return -1;

                actualCell = Pathfinding.NextCell(map, actualCell, direction);

                // io
                var mapCell = map.GetCell(actualCell);
                if (mapCell != null)
                {
                    if (mapCell.InteractiveObject != null && (!mapCell.InteractiveObject.CanWalkThrough || (entity.Type == EntityTypeEnum.TYPE_CHARACTER && actualCell == finalCell && mapCell.InteractiveObject.IsActive)))
                    {
                        length = -2;
                        break;
                    }
                }

                // impossible de marcher
                if (!mapCell.Walkable)
                {
                    length = -2;
                    break;
                }

                // aggressé par un groupe de mobs
                if (entity.Type == EntityTypeEnum.TYPE_CHARACTER && map.Entities.OfType<MonsterGroupEntity>().Any(monsters => map.CanBeAggro((CharacterEntity)entity, lastCell, monsters)))
                {
                    length = -2;
                    break;
                }

                length++;
                lastCell = actualCell;
                finalPath.MovementLength++;
            } while (actualCell != endCell);

            finalPath.AddCell(lastCell, direction);

            return length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        /// <param name="fighter"></param>
        /// <param name="path"></param>
        /// <param name="beginCell"></param>
        /// <param name="direction"></param>
        /// <param name="endCell"></param>
        /// <returns></returns>
        public static int IsValidLine(FightBase fight, FighterBase fighter, MovementPath path, int beginCell, int direction, int endCell)
        {
            var length = -1;
            var actualCell = beginCell;

            if (!Pathfinding.InLine(fight.Map, beginCell, endCell))
                return length;

            length = (int)GoalDistance(fight.Map, beginCell, endCell);

            path.AddCell(actualCell, direction);

            for (int i = 0; i < length; i++)
            {
                actualCell = Pathfinding.NextCell(fight.Map, actualCell, direction);

                if (!fight.Map.IsWalkable(actualCell))
                {
                    return -2;
                }

                if (fight.GetFighterOnCell(actualCell) != null)
                    return -2;

                path.AddCell(actualCell, direction);
                path.MovementLength++;

                if (Pathfinding.IsStopCell(fighter.Fight, fighter.Team, actualCell))
                    return -2;
            }

            return length;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="fighter"></param>
        ///// <returns></returns>
        public static int TryTacle(FighterBase fighter)
        {
            var ennemies = Pathfinding.GetEnnemiesNear(fighter.Fight, fighter.Team, fighter.Cell.Id);

            if (ennemies.Count() == 0 || ennemies.All(ennemy => ennemy.StateManager.HasState(FighterStateEnum.STATE_ROOTED)))
                return -1;

            return Pathfinding.TryTacle(fighter, ennemies);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="fighter"></param>
        ///// <param name="nearestEnnemies"></param>
        ///// <returns></returns>
        private static int TryTacle(FighterBase fighter, IEnumerable<FighterBase> nearestEnnemies)
        {
            var fighterAgility = fighter.Statistics.GetTotal(EffectEnum.AddAgility);
            int ennemiesAgility = 0;

            foreach (var ennemy in nearestEnnemies)
                if (!ennemy.StateManager.HasState(FighterStateEnum.STATE_ROOTED))
                    ennemiesAgility += ennemy.Statistics.GetTotal(EffectEnum.AddAgility);

            var A = fighterAgility + 25;
            var B = fighterAgility + ennemiesAgility + 50;
            if (B == 0)
                B = 1;
            var chance = (int)((long)(300 * A / B) - 100);
            var rand = Pathfinding.PATHFIND_RANDOM.Next(0, 99);

            return rand > chance ? rand : -1;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="cellId"></param>
        ///// <returns></returns>
        public static bool IsStopCell(FightBase fight, FightTeam team, int cellId)
        {
            if (fight.GetCell(cellId).HasObject(FightObstacleTypeEnum.TYPE_TRAP))
                return true;

            if (GetEnnemiesNear(fight, team, cellId).Count() > 0)
                return true;

            return false;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="fight"></param>
        ///// <param name="team"></param>
        ///// <param name="cellId"></param>
        ///// <returns></returns>
        public static IEnumerable<FighterBase> GetEnnemiesNear(FightBase fight, FightTeam team, int cellId)
        {
            return GetFightersNear(fight, cellId).Where(fighter => fighter.Team != team);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="fight"></param>
        ///// <param name="team"></param>
        ///// <param name="cellId"></param>
        ///// <returns></returns>
        public static List<FighterBase> GetFightersNear(FightBase fight, int cellId)
        {
            List<FighterBase> fighters = new List<FighterBase>();
            foreach (var direction in Pathfinding.FIGHT_DIRECTIONS)
            {
                var fighter = fight.GetFighterOnCell(Pathfinding.NextCell(fight.Map, cellId, direction));
                if (fighter != null)
                    if (!fighter.IsFighterDead)
                        fighters.Add(fighter);
            }
            return fighters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <returns></returns>
        //public static bool CheckView(FightBase fight, int beginCell, int endCell)
        //{
        //    var begin = GetPoint(fight.Map, beginCell);
        //    var end = GetPoint(fight.Map, endCell);
        //    var x0 = begin.X + 5.000000E-001;
        //    var y0 = begin.Y;
        //    var x1 = end.X;
        //    var y1 = end.Y;
        //    bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
        //    if (steep)
        //    {
        //        double t;
        //        t = x0; // swap x0 and y0
        //        x0 = y0;
        //        y0 = t;
        //        t = x1; // swap x1 and y1
        //        x1 = y1;
        //        y1 = t;
        //    }
        //    if (x0 > x1)
        //    {
        //        double t;
        //        t = x0; // swap x0 and x1
        //        x0 = x1;
        //        x1 = t;
        //        t = y0; // swap y0 and y1
        //        y0 = y1;
        //        y1 = t;
        //    }
        //    var dx = x1 - x0;
        //    var dy = Math.Abs(y1 - y0);
        //    var error = dx / 2;
        //    var ystep = (y0 < y1) ? 1 : -1;
        //    var y = y0;
        //    for (var x = x0 + 1; x < x1; x++)
        //    {
        //        var nextCell = GetCell(fight.Map, (steep ? y : x), (steep ? x : y));
        //        var fightCell = fight.GetCell(nextCell);
        //        if (fightCell == null)
        //            return false;
        //        if (fightCell.HasObject(FightObstacleTypeEnum.TYPE_FIGHTER))
        //            return false;
        //        if (!fightCell.LineOfSight)
        //            return false;
        //        error = error - dy;
        //        if (error < 0)
        //        {
        //            y += ystep;
        //            error += dx;
        //        }
        //    }

        //    return true;
        //}

        // Swap the values of A and B
        private static void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }

        // Returns the list of points from p0 to p1 
        private static bool BresenhamLine(FightBase fight, int beginCell, int endCell)
        {
            if (beginCell == endCell)
                return true;

            var begin = GetPoint(fight.Map, beginCell);
            var end = GetPoint(fight.Map, endCell);
            return BresenhamLine(fight, beginCell, endCell, (int)begin.X, (int)begin.Y, (int)end.X, (int)end.Y);
        }

        private static bool BresenhamLine(FightBase fight, int beginCell, int endCell, int x0, int y0, int x1, int y1)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int deltax = x1 - x0;
            int deltay = Math.Abs(y1 - y0);
            int error = 0;
            int ystep;
            int y = y0;
            if (y0 < y1) ystep = 1; else ystep = -1;
            for (int x = x0; x <= x1; x++)
            {
                int cellId = -1;
                if (steep)
                { 
                    cellId = GetCell(fight.Map, y, x);
                }
                else
                {
                    cellId = GetCell(fight.Map, x, y);
                }
                if (cellId != beginCell && cellId != endCell)
                {
                    var fightCell = fight.GetCell(cellId);
                    if (fightCell == null)
                        return false;
                    if (!fightCell.LineOfSight)
                        return false;
                    if (fightCell.HasObject(FightObstacleTypeEnum.TYPE_FIGHTER))
                        return false;
                }

                error += deltay;
                if (2 * error >= deltax)
                {
                    y += ystep;
                    error -= deltax;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        /// <param name="beginCell"></param>
        /// <param name="endCell"></param>
        /// <returns></returns>
        public static bool CheckView(FightBase fight, int beginCell, int endCell)
        {
            return BresenhamLine(fight, beginCell, endCell);

            var begin = GetPoint(fight.Map, beginCell);
            var end = GetPoint(fight.Map, endCell);
            var deltax = Math.Abs(end.X - begin.X);
            var deltay = Math.Abs(end.Y - begin.Y);
            var error = deltax / 2;
            var ystep = 1;

            if (end.Y < begin.Y)
            {
                ystep = -1;
            }

            var nextPoint = begin;

            while (nextPoint.X < end.X)
            {
                if (!nextPoint.Equals(begin) && !nextPoint.Equals(end))
                {
                    var fightCell = fight.GetCell(GetCell(fight.Map, nextPoint.X, nextPoint.Y));
                    if (fightCell == null)
                        return false;
                    else if (!fightCell.LineOfSight)
                        return false;
                    else if (fightCell.HasObject(FightObstacleTypeEnum.TYPE_FIGHTER))
                        return false;
                }

                nextPoint.X++;

                error -= deltay;
                if (error < 0)
                {
                    nextPoint.Y += ystep;
                    error += deltax;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int GetCell(MapInstance map, double x, double y)
        {
            return (int)x * map.Width + (int)y * (map.Width - 1);
        }

    }

    public struct PathNode
    {
        public int Cell;
        public double F;
        public double G;
        public int Parent;
        public NodeState Status;
    }

    public enum NodeState : byte
    {
        None = 0,
        InOpenList,
        InCloseList
    }

    /// <summary>
    /// 
    /// </summary>
    public class Pathmaker
    {
        public const int estimatedHeuristic = 1;
        public MapInstance map;
        private int cellCount;

        /// <summary>
        /// 
        /// </summary>
        private int[] directions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public Pathmaker(MapInstance mapInstance)
        {
            map = mapInstance;
            cellCount = map.Cells.Count;

            directions = new int[]
            {
                map.Width,
                map.Width - 1,
                -map.Width,
                -map.Width + 1,
                1,
                (map.Width * 2) - 1,
                -1,
                -((map.Width * 2) + 1)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startCell"></param>
        /// <param name="endCell"></param>
        /// <param name="diagonal"></param>
        /// <param name="movementPoints"></param>
        /// <param name="obstacles"></param>
        /// <returns></returns>
        public string FindPathAsString(int startCell, int endCell, bool diagonal, int movementPoints = -1, IEnumerable<int> obstacles = null)
        {
            var movementPath = FindPath(startCell, endCell, diagonal, movementPoints, obstacles == null ? new List<int>() : obstacles);

            StringBuilder PathAsString = new StringBuilder();

            for (int i = 0; i <= movementPath.Count() - 2; i++)
            {
                PathAsString.Append(Pathfinding.GetDirectionChar((Pathfinding.GetDirection(map, movementPath.ElementAt(i), movementPath.ElementAt(i + 1)))));
                PathAsString.Append(Util.CellToChar(movementPath.ElementAt(i + 1)));
            }

            return PathAsString.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StartCell"></param>
        /// <param name="EndCell"></param>
        /// <param name="Diagonal"></param>
        /// <param name="MovementPoints"></param>
        /// <param name="Obstacles"></param>
        /// <returns></returns>
        private IEnumerable<int> FindPath(int StartCell, int EndCell, bool Diagonal, int MovementPoints = -1, IEnumerable<int> Obstacles = null)
        {
            bool Success = false;

            PathNode[] CalcGrid = new PathNode[map.Cells.Count + 1];
            PriorityQueueB<int> OpenList = new PriorityQueueB<int>(new ComparePFNodeMatrix(CalcGrid));
            List<PathNode> ClosedList = new List<PathNode>();

            Point StartPoint = Pathfinding.GetPoint(map, StartCell);
            Point EndPoint = Pathfinding.GetPoint(map, EndCell);

            int BestLocation = StartCell;
            int Location = StartCell;
            Point LocationPoint = Pathfinding.GetPoint(map, Location);
            int NewLocation = 0;
            Point NewLocationPoint = default(Point);

            double NewG = 0;
            int Counter = 0;

            OpenList.Clear();
            ClosedList.Clear();

            if (MovementPoints == 0)
            {
                return new List<int>();
            }

            CalcGrid[Location].Cell = Location;
            CalcGrid[Location].G = 0;
            CalcGrid[Location].F = estimatedHeuristic;
            CalcGrid[Location].Parent = -1;
            CalcGrid[Location].Status = NodeState.InOpenList;

            OpenList.Push(Location);
            while (OpenList.Count > 0 && !Success)
            {
                Location = OpenList.Pop();
                LocationPoint = Pathfinding.GetPoint(map, Location);

                if (CalcGrid[Location].Status == NodeState.InCloseList)
                {
                    continue;
                }

                if (Location == EndCell)
                {
                    CalcGrid[Location].Status = NodeState.InCloseList;
                    Success = true;
                    break; // TODO: might not be correct. Was : Exit While
                }

                for (int i = 0; i <= (Diagonal ? 8 - 1 : 4 - 1); i++)
                {
                    NewLocation = Location + directions[i];
                    if (Pathfinding.GetPoint(map, NewLocation).X == -1000)
                        continue;
                    NewLocationPoint = Pathfinding.GetPoint(map, NewLocation);

                    if ((NewLocation >= cellCount))
                    {
                        continue;
                    }

                    if ((!map.IsWalkable(NewLocation) || (Obstacles != null && Obstacles.Contains(NewLocation))) && NewLocation != EndCell)
                    {                        
                        continue;
                    }

                    NewG = CalcGrid[Location].G + 1;
                    // 1 should be the cost of the cell, but it's always 1

                    if (CalcGrid[NewLocation].Status == NodeState.InOpenList || CalcGrid[NewLocation].Status == NodeState.InCloseList)
                    {
                        if (CalcGrid[NewLocation].G <= NewG)
                        {
                            continue;
                        }
                    }

                    CalcGrid[NewLocation].Cell = NewLocation;
                    CalcGrid[NewLocation].Parent = Location;
                    CalcGrid[NewLocation].G = NewG;

                    var xDiff = Math.Abs(NewLocationPoint.X - EndPoint.X);
                    var yDiff = Math.Abs(NewLocationPoint.Y - EndPoint.Y);
                    double H = xDiff + yDiff;

                    var Xbegin = StartPoint.X;
                    var Xend = EndPoint.X;
                    var Xcurrent = NewLocationPoint.X;
                    var Ycurrent = NewLocationPoint.Y;
                    var Ybegin = StartPoint.Y;
                    var Yend = EndPoint.Y;

                    var XdiffCurrentEnd = Xcurrent - Xend;
                    var XdiffStartEnd = Xbegin - Xend;
                    var YdiffCurrentEnd = Ycurrent - Yend;
                    var YdiffStartEnd = Ybegin - Yend;

                    var Cross = Math.Abs(XdiffCurrentEnd * YdiffStartEnd - XdiffStartEnd * YdiffCurrentEnd);

                    CalcGrid[NewLocation].F = NewG + H + Cross;
                    OpenList.Push(NewLocation);

                    CalcGrid[NewLocation].Status = NodeState.InOpenList;
                }

                if (BestLocation == -1 || Pathfinding.GoalDistance(map, Location, EndCell) < Pathfinding.GoalDistance(map, BestLocation, EndCell))
                    BestLocation = Location;
                
                Counter += 1;
                CalcGrid[Location].Status = NodeState.InCloseList;               
            }

            if (!Success)
                EndCell = BestLocation;

            var Node = CalcGrid[EndCell];

            while (Node.Parent != -1)
            {
                ClosedList.Add(Node);

                Node = CalcGrid[Node.Parent];
            }

            ClosedList.Add(Node);

            ClosedList.Reverse();

            if (MovementPoints > 0 && ClosedList.Count - 1 >= MovementPoints)
                return ClosedList.Take(MovementPoints + 1).Select(node => node.Cell);

            return ClosedList.Select(node => node.Cell);
        }

        /// <summary>
        /// 
        /// </summary>
        internal class ComparePFNodeMatrix : IComparer<int>
        {
            /// <summary>
            /// 
            /// </summary>
            private PathNode[] mMatrix;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="matrix"></param>
            public ComparePFNodeMatrix(PathNode[] matrix)
            {
                mMatrix = matrix;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public int Compare(int a, int b)
            {
                if (mMatrix[a].F > mMatrix[b].F)
                {
                    return 1;
                }
                else if (mMatrix[a].F < mMatrix[b].F)
                {
                    return -1;
                }
                return 0;
            }
        }
    }
}
