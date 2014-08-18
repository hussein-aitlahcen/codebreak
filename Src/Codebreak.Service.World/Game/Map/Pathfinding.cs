
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Framework.Utils;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Spell;

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

    public class MovementPath
    {
        private StringBuilder serializedPath = new StringBuilder();
        private bool serialized = false;

        public override string ToString()
        {
            if (!serialized)
            {
                for (int i = 0; i < TransitCells.Count; i++)
                {
                    serializedPath.Append(Pathfinding.GetDirectionChar(Directions[i]));
                    serializedPath.Append(Util.CellToChar(TransitCells[i]));
                }
                serialized = true;
            }

            return serializedPath.ToString();
        }

        public int BeginCell
        {
            get
            {
                return TransitCells.FirstOrDefault();
            }
        }

        public int MovementLength
        {
            get;
            set;
        }

        public long MovementTime
        {
            get
            {
                return (long)Pathfinding.GetPathTime(MovementLength, GetDirection(LastStep));
            }
        }

        public int LastStep
        {
            get
            {
                return TransitCells[TransitCells.Count == 1 ? 0 : TransitCells.Count - 2];
            }
        }

        public int EndCell
        {
            get
            {
                return TransitCells.LastOrDefault();
            }
        }

        public List<int> TransitCells = new List<int>();
        public List<int> Directions = new List<int>();

        public void AddCell(int Cell, int Direction)
        {
            TransitCells.Add(Cell);
            Directions.Add(Direction);
        }

        public int GetDirection(int Cell)
        {
            return Directions[TransitCells.Count == 1 ? 0 : TransitCells.IndexOf(Cell) + 1];
        }

        public void Clean()
        {
            var transitCells = new List<int>();
            var directions = new List<int>();

            for (int i = 0; i < Directions.Count; i++)
            {
                if (i == Directions.Count - 1)
                {
                    TransitCells.Add(TransitCells[i]);
                    directions.Add(Directions[i]);
                }
                else
                {
                    if (this.Directions[i] != Directions[i + 1])
                    {
                        TransitCells.Add(TransitCells[i]);
                        directions.Add(Directions[i]);
                    }
                }
            }

            TransitCells = transitCells;
            Directions = directions;
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
        public static double[] RUN_SPEEDS = { 1.700000E-001, 1.500000E-001, 1.500000E-001, 1.500000E-001, 1.700000E-001, 1.500000E-001, 1.500000E-001, 1.500000E-001 };
        public static double[] WALK_SPEEDS = { 7.000000E-002, 6.000000E-002, 6.000000E-002, 6.000000E-002, 7.000000E-002, 6.000000E-002, 6.000000E-002, 6.000000E-002 };
        public static double[] MOUNT_SPEEDS = { 2.300000E-001, 2.000000E-001, 2.000000E-001, 2.000000E-001, 2.300000E-001, 2.000000E-001, 2.000000E-001, 2.000000E-001 };

        private static FastRandom PATHFIND_RANDOM = new FastRandom();
        private static int[] FIGHT_DIRECTIONS = { 1, 3, 5, 7 };

        private static Dictionary<int, int[]> MapDirections = new Dictionary<int, int[]>();
        private static Dictionary<int, int> CellDistances = new Dictionary<int, int>();
        private static Dictionary<int, bool> CellLines = new Dictionary<int, bool>();
        private static Dictionary<int, int> CellDirections = new Dictionary<int, int>();
        private static Dictionary<int, Dictionary<int, Point>> CellPoints = new Dictionary<int, Dictionary<int, Point>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static double GetPathTime(int length, int direction)
        {
            return ((length >= 4 ? Pathfinding.RUN_SPEEDS[direction] : Pathfinding.WALK_SPEEDS[direction]) * 1000 * length);
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
        /// <param name="Width"></param>
        /// <param name="CellsCount"></param>
        public static void GenerateGrid(int Width, int CellsCount)
        {
            var Grid = new Dictionary<int, Point>(CellsCount);

            for (int i = 0; i < CellsCount; i++)
            {
                Grid.Add(i, new Point(_GetX(Width, i), _GetY(Width, i)));
            }

            CellPoints.Add(CellsCount, Grid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Cell"></param>
        /// <returns></returns>
        private static double _GetX(int Width, int Cell)
        {
            double loc5 = Math.Floor((double)(Cell / (Width * 2 - 1)));
            double loc6 = Cell - loc5 * (Width * 2 - 1);
            double loc7 = loc6 % Width;

            return (Cell - (Width - 1) * (loc5 - loc7)) / Width;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Cell"></param>
        /// <returns></returns>
        private static double _GetY(int Width, int Cell)
        {
            double loc5 = Math.Floor((double)(Cell / (Width * 2 - 1)));
            double loc6 = Cell - loc5 * (Width * 2 - 1);
            double loc7 = loc6 % Width;

            return loc5 - loc7;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="Cell"></param>
        /// <returns></returns>
        public static Point GetPoint(MapInstance map, int Cell)
        {
            if (CellPoints.ContainsKey(map.Cells.Count))
                return CellPoints[map.Cells.Count][Cell];

            Pathfinding.GenerateGrid(map.Width, map.Cells.Count);

            return CellPoints[map.Cells.Count][Cell];
        }

        public static double GetX(MapInstance map, int Cell)
        {
            if (!CellPoints.ContainsKey(map.Cells.Count))
                Pathfinding.GenerateGrid(map.Width, map.Cells.Count);

            Point p = new Point();

            if (CellPoints[map.Cells.Count].TryGetValue(Cell, out p))
                return p.X;

            return -1000;
        }

        public static double GetY(MapInstance map, int Cell)
        {
            if (!CellPoints.ContainsKey(map.Cells.Count))
                Pathfinding.GenerateGrid(map.Width, map.Cells.Count);

            Point p = new Point();

            if (CellPoints[map.Cells.Count].TryGetValue(Cell, out p))
                return p.Y;

            return -1000;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="BeginCell"></param>
        /// <param name="EndCell"></param>
        /// <returns></returns>
        public static bool InLine(MapInstance map, int BeginCell, int EndCell)
        {
            var cryptedCell = BeginCell * BeginCell * EndCell + EndCell;
            if (CellLines.ContainsKey(cryptedCell))
                return CellLines[cryptedCell];

            var beginPoint = GetPoint(map, BeginCell);
            var endPoint = GetPoint(map, EndCell);

            var line = beginPoint.X == endPoint.X || beginPoint.Y == endPoint.Y;

            CellLines.Add(cryptedCell, line);

            return line;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="BeginCell"></param>
        /// <param name="EndCell"></param>
        /// <returns></returns>
        public static int GoalDistance(MapInstance map, int BeginCell, int EndCell)
        {
            var cryptedCell = BeginCell * BeginCell * EndCell + EndCell;
            if (CellDistances.ContainsKey(cryptedCell))
                return CellDistances[cryptedCell];

            var beginPoint = GetPoint(map, BeginCell);
            var endPoint = GetPoint(map, EndCell);
            var distance = (int)(Math.Abs(endPoint.X - beginPoint.X) + Math.Abs(endPoint.Y - beginPoint.Y));

            CellDistances.Add(cryptedCell, distance);

            return distance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public static char GetDirectionChar(int Direction)
        {
            return Util.HASH[Direction];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public static int GetDirection(char Direction)
        {
            return Util.HASH.IndexOf(Direction);
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
        /// <param name="BeginCell"></param>
        /// <param name="EndCell"></param>
        /// <returns></returns>
        public static int GetDirection(MapInstance map, int BeginCell, int EndCell)
        {
            var cryptedCell = BeginCell * BeginCell * EndCell + EndCell;
            if (CellDirections.ContainsKey(cryptedCell))
                return CellDirections[cryptedCell];

            var listChange = GetDirectionChanges(map);
            var result = EndCell - BeginCell;
            var direction = 0;

            for (int i = 7; i > -1; i--)
                if (result == listChange[i])
                    direction = i;

            var beginPoint = GetPoint(map, BeginCell);
            var endPoint = GetPoint(map, EndCell);
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

            CellDirections.Add(cryptedCell, direction);

            return direction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="CurrentCell"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static MovementPath DecodePath(MapInstance map, int CurrentCell, string Path)
        {
            MovementPath MovementPath = new MovementPath();

            if (Path == "")
                return MovementPath;

            MovementPath.AddCell(CurrentCell, GetDirection(map, CurrentCell, Util.CharToCell(Path.Substring(1, 2))));

            for (int i = 0; i < Path.Length; i += 3)
            {
                int curCell = Util.CharToCell(Path.Substring(i + 1, 2));
                int curDir = Util.HASH.IndexOf(Path[i]);

                MovementPath.AddCell(curCell, curDir);
            }

            return MovementPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public static int OppositeDirection(int Direction)
        {
            return (Direction >= 4 ? Direction - 4 : Direction + 4);
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
        public static MovementPath IsValidPath(MapInstance map, int currentCell, string encodedPath)
        {
            var DecodedPath = Pathfinding.DecodePath(map, currentCell, encodedPath);

            var Index = 0;
            int TransitCell = 0;
            do
            {
                TransitCell = DecodedPath.TransitCells[Index];

                var Length = Pathfinding.IsValidLine(map, TransitCell, DecodedPath.GetDirection(TransitCell), DecodedPath.TransitCells[Index + 1]);
                if (Length == -1)
                    return null;

                DecodedPath.MovementLength += Length;

                Index++;

            }
            while (TransitCell != DecodedPath.LastStep);

            return DecodedPath;
        }

         //<summary>
         
         //</summary>
         //<param name="fight"></param>
         //<param name="fighter"></param>
         //<param name="currentCell"></param>
         //<param name="encodedPath"></param>
         //<returns></returns>
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
        public static int IsValidLine(MapInstance map, int beginCell, int direction, int endCell)
        {
            var length = -1;
            var actualCell = beginCell;

            if (Pathfinding.InLine(map, beginCell, endCell))
                length = (int)GoalDistance(map, beginCell, endCell);
            else
                length = (int)(GoalDistance(map, beginCell, endCell) / 1.4);

            for (int i = 0; i < length; i++)
            {
                actualCell = Pathfinding.NextCell(map, actualCell, direction);
                if (!map.IsWalkable(actualCell))
                {
                    //return -1;
                }
            }

            return length;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="fight"></param>
        ///// <param name="fighter"></param>
        ///// <param name="path"></param>
        ///// <param name="beginCell"></param>
        ///// <param name="direction"></param>
        ///// <param name="endCell"></param>
        ///// <returns></returns>
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
            var ennemies = Pathfinding.GetEnnemyNear(fighter.Fight, fighter.Team, fighter.Cell.Id);

            if (ennemies.Count == 0 || ennemies.All(ennemy => ennemy.StateManager.HasState(FighterStateEnum.STATE_ROOTED)))
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

            if (GetEnnemyNear(fight, team, cellId).Count > 0)
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
        public static List<FighterBase> GetEnnemyNear(FightBase fight, FightTeam team, int cellId)
        {
            List<FighterBase> ennemies = new List<FighterBase>();
            foreach (var direction in Pathfinding.FIGHT_DIRECTIONS)
            {
                var ennemy = fight.GetFighterOnCell(Pathfinding.NextCell(fight.Map, cellId, direction));
                if (ennemy != null)
                    if (ennemy.Team != team)
                        if (!ennemy.IsFighterDead)
                            ennemies.Add(ennemy);
            }
            return ennemies;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="fight"></param>
        ///// <param name="beginCell"></param>
        ///// <param name="endCell"></param>
        ///// <returns></returns>
        //public static bool CheckView(BaseFight fight, int beginCell, int endCell)
        //{
        //    var _loc5 = new Point(GetX(fight.map.Template, beginCell), GetY(fight.map.Template, beginCell));
        //    var _loc6 = new Point(GetX(fight.map.Template, endCell), GetY(fight.map.Template, endCell));

        //    var _loc7 = 0.5;
        //    var _loc8 = 0;

        //    var _loc9 = fight.GetCell(beginCell).CanWalk ? 0 : 1.5;
        //    var _loc10 = fight.GetCell(endCell).CanWalk ? 0 : 1.5;

        //    _loc5.Z = _loc7 + _loc9;
        //    _loc6.Z = _loc8 + _loc10;

        //    var _loc11 = _loc6.Z - _loc5.Z;
        //    var _loc12 = Math.Max(Math.Abs(_loc5.Y - _loc6.Y), Math.Abs(_loc5.X - _loc6.X));
        //    var _loc13 = (_loc5.Y - _loc6.Y) / (_loc5.X - _loc6.X);
        //    var _loc14 = _loc5.Y - _loc13 * _loc5.X;
        //    var _loc15 = _loc6.X - _loc5.X >= 0 ? 1 : -1;
        //    var _loc16 = _loc6.Y - _loc5.Y >= 0 ? 1 : -1;
        //    var _loc17 = _loc5.Y;
        //    var _loc18 = _loc5.X;
        //    var _loc19 = _loc6.X * _loc15;
        //    var _loc20 = _loc6.Y * _loc16;

        //    var _loc21 = 0;
        //    var _loc22 = 0;
        //    var _loc26 = 0;

        //    var _loc27 = _loc5.X + 0.5 * _loc15;


        //    if (_loc27 * _loc15 <= _loc19)
        //    {

        //        while (true)
        //        {
        //            _loc27 += _loc15;

        //            if (_loc27 * _loc15 > _loc19)
        //                break; // TODO: might not be correct. Was : Exit While

        //            var _loc25 = _loc13 * _loc27 + _loc14;

        //            if (_loc16 > 0)
        //            {
        //                _loc21 = (int)Math.Round(_loc25);
        //                _loc22 = (int)Math.Ceiling(_loc25 - 0.5);
        //            }
        //            else
        //            {
        //                _loc21 = (int)Math.Ceiling(_loc25 - 0.5);
        //                _loc22 = (int)Math.Round(_loc25);
        //            }

        //            _loc26 = (int)_loc17;


        //            if ((_loc26 * _loc16 <= _loc22 * _loc16))
        //            {

        //                while (true)
        //                {
        //                    _loc26 += _loc16;

        //                    if (_loc26 * _loc16 > _loc22 * _loc16)
        //                    {
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        if (!CheckCellView(fight, (int)(_loc27 - _loc15 / 2), _loc26, false, _loc5, _loc6, (int)_loc11, (int)_loc12))
        //                            return false;
        //                    }

        //                }

        //            }
        //            _loc17 = _loc21;
        //        }
        //    }

        //    _loc26 = (int)_loc17;


        //    if (_loc26 * _loc16 <= _loc6.Y * _loc16)
        //    {

        //        while (true)
        //        {
        //            _loc26 += _loc16;

        //            if (_loc26 * _loc16 > _loc6.Y * _loc16)
        //            {
        //                break; // TODO: might not be correct. Was : Exit While
        //            }
        //            else
        //            {
        //                if (!CheckCellView(fight, (int)(_loc27 - 0.5 * _loc15), _loc26, false, _loc5, _loc6, (int)_loc11, (int)_loc12))
        //                    return false;
        //            }

        //        }
        //    }

        //    if (!CheckCellView(fight, (int)(_loc27 - 0.5 * _loc15), _loc26 - _loc16, true, _loc5, _loc6, (int)_loc11, (int)_loc12))
        //        return false;

        //    return true;
        //}

        //public static bool CheckCellView(BaseFight Fight, int x, int y, bool @bool, Point p1, Point p2, int zDiff, int d)
        //{

        //    var _loc10 = (x * Fight.map.Template.Width + y * (Fight.map.Template.Width - 1));
        //    var _loc11 = Fight.GetCell(_loc10);

        //    var _loc12 = Math.Max(Math.Abs(p1.Y - y), Math.Abs(p1.X - x));
        //    var _loc13 = _loc12 / d * zDiff + p1.Z;

        //    var _loc14 = 0.5;

        //    var _loc15 = Fight.GetCell(_loc10).CanWalk || (_loc12 == 0 || (@bool || (p2.X == x && p2.Y == y))) ? false : true;

        //    if (_loc11.LineOfSight && (_loc14 <= _loc13 && !_loc15))
        //    {
        //        return (true);
        //    }
        //    else
        //    {
        //        return @bool;
        //    }
        //}
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
        public Pathmaker(MapInstance map)
        {
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

        private IEnumerable<int> FindPath(int StartCell, int EndCell, bool Diagonal, int MovementPoints = -1, IEnumerable<int> Obstacles = null)
        {
            bool Success = false;

            PathNode[] CalcGrid = new PathNode[map.Cells.Count + 1];
            PriorityQueueB<int> OpenList = new PriorityQueueB<int>(new ComparePFNodeMatrix(CalcGrid));
            List<PathNode> ClosedList = new List<PathNode>();

            Point StartPoint = Pathfinding.GetPoint(map, StartCell);
            Point EndPoint = Pathfinding.GetPoint(map, EndCell);

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
                return ClosedList.Select(entry => entry.Cell);
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

                for (int i = 0; i <= (Diagonal ? 8 : 4 - 1); i++)
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

                Counter += 1;
                CalcGrid[Location].Status = NodeState.InCloseList;

            }

            if (Success)
            {
                var Node = CalcGrid[EndCell];

                while (Node.Parent != -1)
                {
                    ClosedList.Add(Node);

                    Node = CalcGrid[Node.Parent];
                }

                ClosedList.Add(Node);
            }

            ClosedList.Reverse();

            IEnumerable<PathNode> Result = null;
            if (MovementPoints > 0 && ClosedList.Count - 1 >= MovementPoints)
            {
                Result = ClosedList.Take(MovementPoints + 1);
            }
            else
            {
                Result = ClosedList;
            }

            return Result.Select(entry => entry.Cell);
        }

        internal class ComparePFNodeMatrix : IComparer<int>
        {
            private PathNode[] mMatrix;
            public ComparePFNodeMatrix(PathNode[] matrix)
            {
                mMatrix = matrix;
            }

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
