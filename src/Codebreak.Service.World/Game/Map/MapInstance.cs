using Codebreak.Service.World.Game.Area;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Job;
using Codebreak.Framework.Utils;
using Codebreak.Service.World.Game.Condition;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Game.Spawn;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Interactive;

namespace Codebreak.Service.World.Game.Map
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MapInstance : MessageDispatcher, IMovementHandler, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private static string HASH_CELL = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

        /// <summary>
        /// 
        /// </summary>
        private static object SynckLock = new object();

        /// <summary>
        /// 
        /// </summary>
        private static long m_NextMonsterId;

        /// <summary>
        /// 
        /// </summary>
        private static long NextMonsterId
        {
            get
            {
                lock (SynckLock)
                    return m_NextMonsterId--;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public FieldTypeEnum FieldType
        {
            get
            {
                return FieldTypeEnum.TYPE_MAP;
            }
        }

        public Pathmaker Pathmaker
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FightManager FightManager
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int SubAreaId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int X
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Y
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Width
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Height
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Data
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string DataKey
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string CreateTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<int> FightTeam0Cells
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<int> FightTeam1Cells
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public SubAreaInstance SubArea
        {
            get
            {
                if (m_subArea == null)
                    m_subArea = AreaManager.Instance.GetSubArea(SubAreaId);
                return m_subArea;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<EntityBase> Entities
        {
            get
            {
                return m_entityById.Values;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<MapCell> Cells
        {
            get
            {
                return m_cells;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanAbortMovement
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int RandomTeleportCell
        {
            get
            {
                var actionCell = m_cells.Find(cell => cell.Trigger != null);
                if (actionCell != null)
                    return actionCell.Id;
                actionCell = m_cells.Find(cell => cell.Walkable);
                if (actionCell != null)
                    return actionCell.Id;
                return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int RandomFreeCell
        {
            get
            {
                MapCell freeCell = null;
                do
                {
                    freeCell = m_cells[Util.Next(0, m_cells.Count - 100)];
                }
                while (freeCell == null || !freeCell.Walkable);
                return freeCell.Id;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PlayerCount
        {
            get
            {
                return m_playerCount;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, EntityBase> m_entityById;
        private Dictionary<string, EntityBase> m_entityByName;
        private Dictionary<int, MapCell> m_cellById;
        private List<MapCell> m_cells;
        private List<InteractiveObject> m_interactiveObjects;
        private SubAreaInstance m_subArea;
        private bool m_movementInitialized;
        private bool m_subInstance;
        private int m_playerCount;
        private SpawnQueue m_spawnQueue;
        private List<MonsterGradeDAO> m_monsters;
        private int m_spawnCounter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subArea"></param>
        /// <param name="id"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="data"></param>
        /// <param name="dataKey"></param>
        /// <param name="createTime"></param>
        public MapInstance(int subAreaId, int id, int x, int y, int width, int height, string data, string dataKey, string createTime, List<int> f0teamCells, List<int> f1teamCells, bool subInstance = false)
        {
            Id = id;
            SubAreaId = subAreaId;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Data = data;
            DataKey = dataKey;
            CreateTime = createTime;
            FightTeam0Cells = f0teamCells;
            FightTeam1Cells = f1teamCells;

            m_subInstance = subInstance;
            m_movementInitialized = false;
            m_cells = new List<MapCell>();
            m_interactiveObjects = new List<InteractiveObject>();
            m_cellById = new Dictionary<int, MapCell>();
            m_entityById = new Dictionary<long, EntityBase>();
            m_entityByName = new Dictionary<string, EntityBase>();

            FightManager = new FightManager(this);
            SubArea.AddUpdatable(this);
            SubArea.SafeAddHandler(base.Dispatch);
            SpawnManager.Instance.RegisterMap(this);
            Initialize();
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void Initialize()
        {
            var triggers = MapTriggerRepository.Instance.GetTriggers(Id);
            for (int i = 0; i < Data.Length; i += 10)
            {
                string currentCell = Data.Substring(i, 10);
                var id = i / 10;

                byte[] cellData = new byte[currentCell.Length];
                for (int j = 0; j < currentCell.Length; j++)
                {
                    cellData[j] = (byte)HASH_CELL.IndexOf(currentCell[j]);
                }
                
                var cell = new MapCell(this, id, cellData, triggers.Find(trigger => trigger.CellId == id));
                if(cell.InteractiveObject != null)
                {
                    base.AddUpdatable(cell.InteractiveObject);
                    cell.InteractiveObject.AddHandler(base.Dispatch);
                    m_interactiveObjects.Add(cell.InteractiveObject);
                }
                m_cellById.Add(id, cell);
                m_cells.Add(cell);
            }            

            Pathmaker = new Pathmaker(this);
            int nextNpcId = 1;
            foreach(var npc in NpcManager.Instance.GetByMapId(Id))            
                SpawnEntity(new NonPlayerCharacterEntity(npc, nextNpcId++));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MapInstance Clone()
        {
            return new MapInstance(SubAreaId, Id, X, Y, Width, Height, Data, DataKey, CreateTime, FightTeam0Cells, FightTeam1Cells, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spawnQueue"></param>
        public void SetSpawnQueue(SpawnQueue spawnQueue)
        {
            m_spawnQueue = spawnQueue;
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitEntitiesMovements()
        {
            m_monsters = new List<MonsterGradeDAO>(MonsterSpawnRepository.Instance.GetById(SpawnTypeEnum.TYPE_MAP, Id).Select(spawn => spawn.Grade));
            m_spawnCounter = m_monsters.Count > 0 ? WorldConfig.SPAWN_MAX_GROUP_PER_MAP : 0;    

            while (m_spawnCounter > 0)
                SpawnMonsters();

            m_movementInitialized = true;
            foreach (var entity in m_entityById.Values.Where(entry => entry.CanBeMoved()))
                InitEntityMovement(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        private void InitEntityMovement(EntityBase entity)
        {
            entity.AddTimer(Util.Next(1000, 10000), () =>
            {
                var timer = new UpdatableTimer(Util.Next(20000, 38000), () => MoveEntity(entity));
                entity.MovementTimer = timer;
                entity.AddTimer(timer);
            }, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void MoveEntity(EntityBase entity)
        {
            if(!entity.HasGameAction(GameActionTypeEnum.MAP))
            {
                entity.RemoveTimer(entity.MovementTimer);
                return;
            }

            // Move only if there is a player on the map, else it is useless
            if (m_playerCount == 0)
                return;

            var cellId = entity.LastCellId;
            if (cellId < 1)
                cellId = GetNearestMovementCell(entity.CellId);

            if (entity.LastCellId == 0)
                entity.LastCellId = entity.CellId;
            else
                entity.LastCellId = 0;

            if (cellId < 1)
                return;

            AddLinkedMessages
            (
                () => Move(entity, entity.CellId, Pathmaker.FindPathAsString(entity.CellId, cellId, false)),
                () => entity.StopAction(GameActionTypeEnum.MAP_MOVEMENT)                
            );            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MapCell GetCell(int id)
        {
            MapCell cell = null;
            m_cellById.TryGetValue(id, out cell);
            return cell;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellId"></param>
        /// <returns></returns>
        public int GetNearestCell(int cellId)
        {
            foreach(var nextCell in CellZone.GetAdjacentCells(this, cellId))
            {
                var cell = GetCell(nextCell);
                if (cell != null && cell.Walkable)
                    return nextCell;
            }
            return -1;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellId"></param>
        /// <returns></returns>
        public int GetNearestMovementCell(int cellId)
        {
            var rand = Util.Next(0, 101);
            var direction = 1;
            if (rand < 25)
                direction = 3;
            else if (rand < 50)
                direction = 5;
            else if (rand < 75)
                direction = 7;

            var nextCellId = Pathfinding.NextCell(this, cellId, direction);            
            var cell = GetCell(nextCellId);
            if(cell != null && cell.Walkable)            
                if (cell.Walkable)
                    return nextCellId;            
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellId"></param>
        /// <returns></returns>
        public bool IsWalkable(int cellId)
        {
            MapCell cell = GetCell(cellId);
            if (cell != null)
                return cell.Walkable;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntityBase GetEntity(long id)
        {
            if (m_entityById.ContainsKey(id))
                return m_entityById[id];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SpawnMonsters()
        {
            if (m_monsters.Count() > 0 && FightTeam1Cells.Count > 0)            
                SpawnEntity(new MonsterGroupEntity(NextMonsterId, Id, RandomFreeCell, m_monsters, FightTeam1Cells.Count));            
            m_spawnCounter--;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SpawnMonsters(IEnumerable<MonsterGradeDAO> monsters)
        {
            if(monsters.Count() > 0)
                SpawnEntity(new MonsterGroupEntity(NextMonsterId, Id, RandomFreeCell, monsters, FightTeam1Cells.Count));    
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void SpawnEntity(EntityBase entity)
        {
            AddMessage(() =>
            {
                if (!m_entityById.ContainsKey(entity.Id))
                {
                    m_entityById.Add(entity.Id, entity);

                    base.Dispatch(WorldMessage.GAME_MAP_INFORMATIONS(OperatorEnum.OPERATOR_ADD, entity));
                    base.AddUpdatable(entity);

                    if (m_subInstance) // For npc etc
                        entity.SetMap(this);

                    if (entity.Type == EntityTypeEnum.TYPE_CHARACTER)
                    {
                        m_playerCount++;
                        m_entityByName.Add(entity.Name.ToLower(), entity);

                        if(!m_movementInitialized)                        
                            InitEntitiesMovements();

                        base.AddHandler(entity.Dispatch);
                        entity.CachedBuffer = true;
                        entity.Dispatch(WorldMessage.GAME_MAP_INFORMATIONS(OperatorEnum.OPERATOR_ADD, entity.Map.Entities.ToArray()));
                        entity.Dispatch(WorldMessage.INTERACTIVE_DATA_FRAME(m_interactiveObjects));
                        entity.Dispatch(WorldMessage.GAME_DATA_SUCCESS());
                        entity.Dispatch(WorldMessage.FIGHT_COUNT(FightManager.FightCount));
                        foreach (var fight in FightManager.Fights)                        
                            fight.SendMapFightInfos(entity);                        
                        entity.CachedBuffer = false;
                    }
                    else if(entity.CanBeMoved() && m_movementInitialized)
                    {
                        InitEntityMovement(entity);
                    }
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void DestroyEntity(EntityBase entity)
        {
            if (m_entityById.ContainsKey(entity.Id))
            {
                m_entityById.Remove(entity.Id);

                base.RemoveUpdatable(entity);
                base.Dispatch(WorldMessage.GAME_MAP_INFORMATIONS(OperatorEnum.OPERATOR_REMOVE, entity));

                if (entity.Type == EntityTypeEnum.TYPE_CHARACTER)
                {
                    base.RemoveHandler(entity.Dispatch);
                    m_entityByName.Remove(entity.Name.ToLower());
                    m_playerCount--;

                    // Multiple instance released
                    if(m_playerCount == 0 && m_subInstance)
                    {
                        MapManager.Instance.ReleaseInstance(this);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="skillId"></param>
        public void InteractiveExecute(CharacterEntity character, int cellId, int skillId)
        {
            var cell = GetCell(cellId);
            if(cell != null)
            {
                if(cell.InteractiveObject != null)
                {
                    cell.InteractiveObject.UseWithSkill(character, character.CharacterJobs.GetSkill(skillId));
                }
                else
                {
                    character.Dispatch(WorldMessage.SERVER_INFO_MESSAGE("Not implemented yet."));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellId"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public MovementPath DecodeMovement(EntityBase entity, int cellId, string path)
        {
            return Pathfinding.IsValidPath(entity, this, cellId, path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cellId"></param>
        /// <param name="movementPath"></param>
        public void Move(EntityBase entity, int cellId, string movementPath)
        {
            AddMessage(() =>
                {
                    var path = DecodeMovement(entity, cellId, movementPath);
                    if (path != null)
                        entity.Move(path);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="path"></param>
        /// <param name="cellId"></param>
        public void MovementFinish(EntityBase entity, MovementPath path, int cellId)
        {
            if (entity.CellId == cellId)
                return;

            entity.Orientation = path.GetDirection(path.LastStep);

            if (entity.Type == EntityTypeEnum.TYPE_CHARACTER)
            {
                var character = (CharacterEntity)entity;
                var cell = GetCell(cellId);
                if (cell != null)
                {
                    if (cell.Trigger != null)
                    {
                        if(!cell.SatisfyConditions(character))
                        {
                            entity.Dispatch(WorldMessage.IM_ERROR_MESSAGE(InformationEnum.ERROR_CONDITIONS_UNSATISFIED));
                            return;
                        }

                        entity.CellId = cellId;
                        cell.ApplyActions(character);
                        return;
                    }

                    if (character.CanGameAction(GameActionTypeEnum.FIGHT))
                    {
                        foreach (var monsterGroup in m_entityById.Values.OfType<MonsterGroupEntity>())
                        {
                            if (Pathfinding.GoalDistance(this, cellId, monsterGroup.CellId) <= monsterGroup.AggressionRange)
                            {
                                if (FightTeam0Cells.Count == 0 || FightTeam1Cells.Count == 0)
                                {
                                    entity.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Unable to start fight withouth fightCells"));
                                }
                                else
                                {
                                    monsterGroup.StopAction(GameActionTypeEnum.MAP);
                                    FightManager.StartMonsterFight(entity as CharacterEntity, monsterGroup);
                                    return;
                                }
                            }
                        }
                    }
                }     
            }

            entity.CellId = cellId;
        }

        /// <summary>
        /// 
        /// </summary>
        public new void Dispose()
        {
            SubArea.RemoveUpdatable(this);
            SubArea.RemoveHandler(base.Dispatch);
            
            m_entityById.Clear();
            m_entityById = null;

            m_entityByName.Clear();
            m_entityByName = null;

            m_cells.Clear();
            m_cells = null;

            m_subArea = null;

            Pathmaker = null;

            base.Dispose();
        }
    }    
}
