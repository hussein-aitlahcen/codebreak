using Codebreak.Service.World.Frame;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight.Effect;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Game.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Fight.Challenges;
using Codebreak.Service.World.Game.Fight.AI;
using Codebreak.Service.World.Game.Stats;

namespace Codebreak.Service.World.Game.Fight
{
    /// <summary>
    /// 
    /// </summary>
    public enum FightTypeEnum
    {
        TYPE_CHALLENGE = 0,
        TYPE_AGGRESSION = 1,
        TYPE_PVMA = 2,
        TYPE_MXVM = 3,
        TYPE_PVM = 4,
        TYPE_PVT = 5,
        TYPE_PVMU = 6,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FightStateEnum
    {
        STATE_PLACEMENT = 2,
        STATE_FIGHTING = 3,
        STATE_ENDED = 4,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FightLoopStateEnum
    {
        STATE_INIT,
        STATE_WAIT_START,
        STATE_WAIT_TURN,
        STATE_WAIT_SUBACTION,
        STATE_WAIT_ACTION,
        STATE_PROCESS_EFFECT,
        STATE_WAIT_READY,
        STATE_WAIT_END,
        STATE_WAIT_AI,
        STATE_ENDED,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FightEndStateEnum
    {
        STATE_INIT_CALCULATION,
        STATE_PROCESS_CALCULATION,
        STATE_END_CALCULATION,
        STATE_ENDED,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FightActionResultEnum
    {
        RESULT_NOTHING,
        RESULT_END_TURN,
        RESULT_PROCESS_EFFECT,
        RESULT_DEATH,
        RESULT_END,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FightSpellLaunchResultEnum
    {
        RESULT_NO_AP,
        RESULT_NEED_MOVE,
        RESULT_WRONG_TARGET,
        RESULT_OK,
        RESULT_NO_LOS,
        RESULT_ERROR,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FightEndTypeEnum
    {
        END_LOSER = 0,
        END_WINNER = 2,
        END_TAXCOLLECTOR = 5,
    }

    /// <summary>
    /// /
    /// </summary>
    public sealed class FightEndResult : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public long FightId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanWinHonor
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get
            {
                return m_message.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private StringBuilder m_message;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fightId"></param>
        /// <param name="CanWinHonor"></param>
        public FightEndResult(long fightId, bool canWinHonor)
        {
            CanWinHonor = canWinHonor;

            m_message = new StringBuilder("GE");
            m_message.Append(0).Append('|');
            m_message.Append(fightId).Append('|');
            m_message.Append(CanWinHonor ? '1' : '0');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="win"></param>
        /// <param name="leave"></param>
        /// <param name="kamas"></param>
        /// <param name="exp"></param>
        /// <param name="honour"></param>
        /// <param name="dishonour"></param>
        /// <param name="guildxp"></param>
        /// <param name="mountxp"></param>
        /// <param name="items"></param>
        public void AddResult(FighterBase fighter,
            FightEndTypeEnum type = FightEndTypeEnum.END_LOSER,
            bool leave = false,
            long kamas = 0,
            long exp = 0,
            long honour = 0,
            long dishonour = 0,
            long guildxp = 0,
            long mountxp = 0,
            Dictionary<int, int> items = null)
        {
            m_message.Append('|').Append((int)type).Append(';');                   
            m_message.Append(fighter.Id).Append(';');
            m_message.Append(fighter.Name).Append(';');
            m_message.Append(fighter.Level).Append(';');
            m_message.Append((fighter.IsFighterDead || leave) ? '1' : '0').Append(';');

            if (CanWinHonor)
            {
                switch(fighter.Type)
                {
                    case EntityTypeEnum.TYPE_CHARACTER:
                        var character = (CharacterEntity)fighter;
                        if (character.AlignmentId != (int)AlignmentTypeEnum.ALIGNMENT_NEUTRAL)
                        {
                            m_message.Append(character.AlignmentExperienceFloorCurrent).Append(';');
                            m_message.Append(character.Alignment.Honour).Append(';');
                            m_message.Append(character.AlignmentExperienceFloorNext).Append(';');
                            m_message.Append(honour).Append(';');
                            m_message.Append(character.AlignmentLevel).Append(';');
                            m_message.Append(character.Dishonour).Append(';');
                            m_message.Append(dishonour).Append(';');
                        }
                        else
                        {
                            m_message.Append("0;0;0;0;0;0;0;");
                        }
                        if (items != null && items.Count > 0)
                            m_message.Append(string.Join(",", items.Select(itemEntry => itemEntry.Key + "~" + itemEntry.Value))).Append(';');
                        else
                            m_message.Append("").Append(';');
                        m_message.Append(kamas > 0 ? kamas.ToString() : "").Append(';');
                        m_message.Append(character.ExperienceFloorCurrent).Append(';');
                        m_message.Append(character.Experience).Append(';');
                        m_message.Append(character.ExperienceFloorNext).Append(';');
                        m_message.Append(exp);
                        break;

                    case EntityTypeEnum.TYPE_MONSTER_FIGHTER:
                         m_message.Append("0;0;0;0;0;0;0;");
                        if (items != null && items.Count > 0)
                            m_message.Append(string.Join(",", items.Select(itemEntry => itemEntry.Key + "~" + itemEntry.Value))).Append(';');
                        else
                            m_message.Append("").Append(';');
                        m_message.Append(kamas > 0 ? kamas.ToString() : "").Append(';');
                        m_message.Append(0).Append(';');
                        m_message.Append(0).Append(';');
                        m_message.Append(0).Append(';');
                        m_message.Append(0);
                        break;
                }
            }
            else
            {
                switch(fighter.Type)
                {
                    case EntityTypeEnum.TYPE_CHARACTER:                        
                        var character = (CharacterEntity)fighter;
                        m_message.Append(character.ExperienceFloorCurrent).Append(';');
                        m_message.Append(character.Experience).Append(';');
                        m_message.Append(character.ExperienceFloorNext).Append(';');
                        m_message.Append(exp).Append(';');
                        m_message.Append(guildxp).Append(';');
                        m_message.Append(mountxp).Append(';');
                        break;

                    case EntityTypeEnum.TYPE_TAX_COLLECTOR:
                        var taxCollector = (TaxCollectorEntity)fighter;
                        m_message.Append(taxCollector.Level).Append(';');
                        m_message.Append("").Append(';');
                        m_message.Append("").Append(';');
                        m_message.Append("").Append(';');
                        m_message.Append(guildxp).Append(';');
                        m_message.Append("").Append(';');
                        break;
                        
                    default:
                        m_message.Append(";;;;;;");
                        break;
                }

                if (items != null && items.Count > 0)
                    m_message.Append(string.Join(",", items.Select(itemEntry => itemEntry.Key + "~" + itemEntry.Value))).Append(';');
                else
                    m_message.Append("").Append(';');
                m_message.Append(kamas > 0 ? kamas.ToString() : "");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            m_message.Clear();
            m_message = null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class FightCell : IDisposable
    {
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
        public bool Walkable
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool LineOfSight
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public PriorityQueue<IFightObstacle> FightObjects
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanWalk
        {
            get
            {
                return Walkable && FightObjects.All(obj => obj.CanGoThrough);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanPutObject
        {
            get
            {
                return Walkable && FightObjects.Where(obj => obj.Cell.Id == Id).All(obj => obj.CanStack);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="walkable"></param>
        /// <param name="los"></param>
        public FightCell(int id, bool walkable, bool los)
        {
            Id = id;
            Walkable = walkable;
            LineOfSight = los;

            FightObjects = new PriorityQueue<IFightObstacle>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool HasObject(FightObstacleTypeEnum type)
        {
            return FightObjects.Any(obj => obj.ObstacleType == type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fightObject"></param>
        /// <returns></returns>
        public FightActionResultEnum AddObject(IFightObstacle fightObject)
        {
            FightObjects.Add(fightObject);

            if (fightObject.ObstacleType == FightObstacleTypeEnum.TYPE_FIGHTER)
            {
                var fighter = (FighterBase)fightObject;

                for (int i = FightObjects.Count - 1; i > -1; i--)
                {
                    if (FightObjects[i] is FightActivableObject)
                    {
                        var activableObject = (FightActivableObject)FightObjects[i];

                        if (activableObject.ActivationType == ActiveType.ACTIVE_ENDMOVE)
                        {
                            if (!fighter.IsFighterDead)
                            {
                                activableObject.LoadTargets(fighter);
                                activableObject.Activate(fighter);
                            }
                        }
                    }
                }
            }

            return FightActionResultEnum.RESULT_NOTHING;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obstacle"></param>
        /// <returns></returns>
        public FightActionResultEnum RemoveObject(IFightObstacle obstacle)
        {
            FightObjects.Remove(obstacle);

            return FightActionResultEnum.RESULT_NOTHING;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <returns></returns>
        public FightActionResultEnum BeginTurn(FighterBase fighter)
        {
            for (int i = FightObjects.Count - 1; i > -1; i--)
            {
                if (FightObjects[i] is FightActivableObject)
                {
                    var activableObject = (FightActivableObject)FightObjects[i];
                    if (activableObject.ActivationType == ActiveType.ACTIVE_BEGINTURN)
                    {
                        activableObject.LoadTargets(fighter);
                        activableObject.Activate(fighter);
                    }
                }
            }

            return FightActionResultEnum.RESULT_NOTHING;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            FightObjects.Clear();
            FightObjects = null;
        }
    }

    public abstract class FightBase : MessageDispatcher, IMovementHandler, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public FightTypeEnum Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FieldTypeEnum FieldType
        {
            get
            {
                return FieldTypeEnum.TYPE_FIGHT;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CancelButton
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FightStateEnum State
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FightLoopStateEnum LoopState
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FightLoopStateEnum NextLoopState
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FightEndStateEnum LoopEndState
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public MapInstance Map
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FighterBase CurrentFighter
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FighterBase CurrentProcessingFighter
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FightTeam Team0
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FightTeam Team1
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<int, FightCell> Cells
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FightTurnProcessor TurnProcessor
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public SpectatorTeam SpectatorTeam
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long TurnTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long StartTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long TurnTimeLeft
        {
            get
            {
                if (NextTurnTimeout < UpdateTime)
                    return 0;
                return NextTurnTimeout - UpdateTime;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool LoopTimedout
        {
            get
            {
                return NextLoopTimeout <= UpdateTime;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long CurrentLoopTimeout
        {
            get
            {
                if (NextLoopTimeout < UpdateTime)
                    return 0;
                return NextLoopTimeout - UpdateTime;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long NextLoopTimeout
        {
            get
            {
                return m_loopTimeout;
            }
            set
            {
                m_loopTimeout = UpdateTime + value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool TurnTimedout
        {
            get
            {
                return NextTurnTimeout <= UpdateTime;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long NextTurnTimeout
        {
            get
            {
                return m_turnTimeout;
            }
            set
            {
                m_turnTimeout = UpdateTime + value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SubActionTimedout
        {
            get
            {
                return NextSubActionTimeout <= UpdateTime;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long CurrentSubActionTimeout
        {
            get
            {
                if (NextSubActionTimeout < UpdateTime)
                    return 0;
                return NextSubActionTimeout - UpdateTime;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long NextSubActionTimeout
        {
            get
            {
                return m_subActionTimeout;
            }
            set
            {
                m_subActionTimeout = UpdateTime + value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ActionTimedout
        {
            get
            {
                if (CurrentAction == null)
                    return true;
                return CurrentAction.Timeout <= UpdateTime;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SynchronizationTimedout
        {
            get
            {
                return NextSynchroTimeout <= UpdateTime;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long NextSynchroTimeout
        {
            get
            {
                return m_synchronizationTimeout;
            }
            set
            {
                m_synchronizationTimeout = UpdateTime + value;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string FightPlaces
        {
            get
            {
                return Team0.Places + "|" + Team1.Places;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool IsAllReady
        {
            get
            {
                return Fighters.OfType<CharacterEntity>().All(fighter => fighter.TurnReady || fighter.IsFighterDead);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool IsAllReadyToStart
        {
            get
            {
                switch(Type)
                {
                    case FightTypeEnum.TYPE_PVT:
                        return false;

                    case FightTypeEnum.TYPE_AGGRESSION:
                        if (Team0.Fighters.First().Type == EntityTypeEnum.TYPE_MONSTER_FIGHTER)
                            return false;
                        break;
                }

                return IsAllReady;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<FighterBase> Fighters
        {
            get
            {
                foreach (var fighter in Team0.Fighters)
                    yield return fighter;
                foreach (var fighter in Team1.Fighters)
                    yield return fighter;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<FighterBase> AliveFighters
        {
            get
            {
                foreach (var fighter in Fighters)
                    if (!fighter.IsFighterDead)
                        yield return fighter;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public GameFightActionBase CurrentAction
        {
            get
            {
                if (CurrentFighter != null)
                    if(CurrentFighter.CurrentAction != null && CurrentFighter.CurrentAction is GameFightActionBase)
                        return (GameFightActionBase)CurrentFighter.CurrentAction;
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Func<FightActionResultEnum> CurrentSubAction
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FightEndResult Result
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<int> Obstacles
        {
            get
            {
                return Cells.Values.Where(cell => !cell.CanWalk).Select(cell => cell.Id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long NextFighterId
        {
            get
            {
                return Fighters.Min(fighter => fighter.Id) - 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected IEnumerable<FighterBase> m_winnersFighter, m_losersFighter;
        protected FightTeam m_winnersTeam, m_losersTeam;
        private long m_loopTimeout, m_turnTimeout, m_subActionTimeout, m_synchronizationTimeout;
        private Dictionary<FighterBase, List<FightActivableObject>> m_activableObjects;
        private LinkedList<CastInfos> m_processingTargets;
        private int m_currentApCost;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapInstance"></param>
        public FightBase(FightTypeEnum type, MapInstance  mapInstance, long id, long team0LeaderId, int team0Alignment, int team0FlagCell, long team1LeaderId, int team1Alignment, int team1FlagCell, long startTimeout, long turnTime, bool cancelButton = false, bool canWinHonor = false)
        {
            m_activableObjects = new Dictionary<FighterBase, List<FightActivableObject>>();
            m_processingTargets = new LinkedList<CastInfos>();
            m_currentApCost = -1;

            Type = type;
            Id = id;
            Map = mapInstance;
            State = FightStateEnum.STATE_PLACEMENT;
            LoopState = FightLoopStateEnum.STATE_INIT;
            CancelButton = cancelButton;
            TurnTime = turnTime;
            StartTime = startTimeout;
            NextLoopTimeout = startTimeout;
            Result = new FightEndResult(Id, canWinHonor);
            Cells = new Dictionary<int, FightCell>();
            TurnProcessor = new FightTurnProcessor();

            foreach (var cell in mapInstance.Cells)            
                Cells.Add(cell.Id, new FightCell(cell.Id, cell.Walkable , cell.LineOfSight));
            
            SpectatorTeam = new SpectatorTeam(this);
            Team0 = new FightTeam(0, team0LeaderId, team0Alignment, team0FlagCell, this, new List<FightCell>(Cells.Values.Where(cell => mapInstance.FightTeam0Cells.Contains(cell.Id))));
            Team1 = new FightTeam(1, team1LeaderId, team1Alignment, team1FlagCell, this, new List<FightCell>(Cells.Values.Where(cell => mapInstance.FightTeam1Cells.Contains(cell.Id))));
            Team0.OpponentTeam = Team1;
            Team1.OpponentTeam = Team0;
            
            base.AddUpdatable(SpectatorTeam);
            base.AddUpdatable(Team0);
            base.AddUpdatable(Team1);
            base.AddHandler(SpectatorTeam.Dispatch);
            base.AddHandler(Team0.Dispatch);
            base.AddHandler(Team1.Dispatch);
        }
        
        /// <summary>
        /// 
        /// </summary>
        protected void Start()
        {
            AddMessage(() =>
                {
                    LoopState = FightLoopStateEnum.STATE_WAIT_START;
                    Map.CachedBuffer = true;
                    Map.Dispatch(WorldMessage.FIGHT_FLAG_DISPLAY(this));
                    Map.Dispatch(WorldMessage.FIGHT_FLAG_UPDATE(OperatorEnum.OPERATOR_ADD, Team0.LeaderId, Team0.Fighters.ToArray()));
                    Map.Dispatch(WorldMessage.FIGHT_FLAG_UPDATE(OperatorEnum.OPERATOR_ADD, Team1.LeaderId, Team1.Fighters.ToArray()));
                    Map.CachedBuffer = false;
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="timeout"></param>
        public void SetSubAction(Func<FightActionResultEnum> action, int timeout)
        {
            CurrentSubAction = action;
            NextSubActionTimeout = timeout;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infos"></param>
        public void AddProcessingTarget(CastInfos infos)
        {
            if(infos.Target == null)
            {
                Logger.Debug("AddProcessingTarget first (Null target)");
                m_processingTargets.AddFirst(infos);
            }
            else if (CurrentProcessingFighter == infos.Target)
            {
                Logger.Debug("AddProcessingTarget first (CurrentProcessingFighter) : " + infos.Target.Name);
                m_processingTargets.AddFirst(infos);
            }
            else if (CurrentProcessingFighter == null && CurrentFighter == infos.Target)
            {
                Logger.Debug("AddProcessingTarget first (CurrentFighter) : " + infos.Target.Name);
                m_processingTargets.AddFirst(infos);
            }
            else
            {
                Logger.Debug("AddProcessingTarget last : " + infos.Target.Name);
                m_processingTargets.AddLast(infos);
            }
        }

         /// <summary>
        /// 
        /// </summary>
        /// <param name="cellId"></param>
        /// <returns></returns>
        public FightCell GetCell(int cellId)
        {
            if (Cells.ContainsKey(cellId))
                return Cells[cellId];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void KickSpectators()
        {
            AddMessage(() =>
            {
                if (LoopState == FightLoopStateEnum.STATE_WAIT_END || LoopState == FightLoopStateEnum.STATE_ENDED)
                    return;

                for (int i = SpectatorTeam.Spectators.Count() - 1; i > -1; i--)
                {
                    FightQuit(SpectatorTeam.Spectators.ElementAt(i), true);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void TrySpectate(CharacterEntity character)
        {
            AddMessage(() =>
                    {
                        if (LoopState == FightLoopStateEnum.STATE_WAIT_END || LoopState == FightLoopStateEnum.STATE_ENDED)
                        {
                            character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                            return;
                        }

                        if (State != FightStateEnum.STATE_FIGHTING)
                        {
                            Logger.Debug("FightBase::TrySpectate cannot spectate placement " + character.Name);
                            character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_FIGHT_SPECTATOR_LOCKED));
                            return;
                        }
                        
                        if (!SpectatorTeam.CanJoin)
                        {
                            character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_FIGHT_SPECTATOR_LOCKED));
                            return;
                        }

                        character.JoinSpectator(this);

                        SendFightJoinInfos(character);                        
                    });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void TryJoin(CharacterEntity character, long teamId)
        {
            AddMessage(() =>
                {
                    if (LoopState == FightLoopStateEnum.STATE_WAIT_END || LoopState == FightLoopStateEnum.STATE_ENDED)
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    if (State != FightStateEnum.STATE_PLACEMENT)
                    {
                        Logger.Debug("FightBase::TryJoin fight already started " + character.Name);
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    if (!CanJoin(character))
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    FightTeam team = teamId == Team0.LeaderId ? Team0 : Team1;

                    if (!team.CanJoinBeforeStart(character))
                    {
                        Logger.Debug("FightBase::TryJoin cannot join team before start " + character.Name);
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    OnCharacterJoin(character, team);

                    JoinFight(character, team);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="team"></param>
        public void JoinFight(FighterBase fighter, FightTeam team)
        {
            if (team.FreePlace == null)
                return;

            if (!fighter.IsDisconnected)
            {
                fighter.JoinFight(this, team);
                base.Dispatch(WorldMessage.GAME_MAP_INFORMATIONS(OperatorEnum.OPERATOR_ADD, fighter));
            }

            if (fighter.MapId == Map.Id)
                SendFightJoinInfos(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="team"></param>
        /// <param name="cellId"></param>
        public FightActionResultEnum SummonFighter(FighterBase fighter, FightTeam team, int cellId)
        {
            fighter.JoinFight(this, team);
            fighter.TurnReady = true;

            var result = fighter.SetCell(GetCell(cellId));
            if (result != FightActionResultEnum.RESULT_NOTHING)
                return result;

            var message = new StringBuilder("+");
            fighter.SerializeAs_GameMapInformations(OperatorEnum.OPERATOR_ADD, message);
                      
            if (fighter.Invocator != null)
                base.Dispatch(WorldMessage.GAME_ACTION(fighter.StaticInvocation ? EffectEnum.InvocationStatic : EffectEnum.Invocation, fighter.Invocator.Id, message.ToString()));
            else
                base.Dispatch("GM|" + message.ToString());

            switch(State)
            {
                case FightStateEnum.STATE_PLACEMENT:
                    // implicit turnready after start fighting
                    break;

                case FightStateEnum.STATE_FIGHTING:
                    fighter.TurnReady = true;
                    TurnProcessor.SummonFighter(fighter);
                    base.Dispatch(WorldMessage.FIGHT_TURN_LIST(TurnProcessor.FighterOrder));
                    break;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void FighterDisconnect(FighterBase fighter)
        {
            AddMessage(() =>
            {
                // fight just ended
                if (LoopState == FightLoopStateEnum.STATE_WAIT_END || LoopState == FightLoopStateEnum.STATE_ENDED)                
                    return;
                
                fighter.IsDisconnected = true;
                
                // disconnected during placement or spectator disconnected
                if (fighter.IsSpectating)
                {
                    FightQuit((CharacterEntity)fighter, true);
                    return;
                }

                Logger.Debug("Fight::Disconnect fighter disconnected : " + fighter.Name);
                
                if (fighter.DisconnectedTurnLeft == 0)
                    fighter.DisconnectedTurnLeft = WorldConfig.FIGHT_DISCONNECTION_TURN;

                base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_FIGHTER_DISCONNECTED, fighter.Name, fighter.DisconnectedTurnLeft));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="killerId"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public FightActionResultEnum TryKillFighter(FighterBase fighter, long killerId, bool force = false, bool quit = false)
        {
            if (LoopState == FightLoopStateEnum.STATE_ENDED ||
                LoopState == FightLoopStateEnum.STATE_WAIT_END ||
                LoopState == FightLoopStateEnum.STATE_INIT)
                return FightActionResultEnum.RESULT_NOTHING;

            if (force)            
                fighter.Life = 0;            

            if (fighter.IsFighterDead)
            {
                Logger.Debug("FightBase::KillFighter on " + fighter.Name);

                if (quit)
                {
                    base.Dispatch(WorldMessage.GAME_MAP_INFORMATIONS(OperatorEnum.OPERATOR_REMOVE, fighter));
                }
                else
                {
                    base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_KILL, killerId, fighter.Id.ToString()));
                }

                fighter.OnDeath();
                Team0.CheckDeath(fighter);
                Team1.CheckDeath(fighter);

                foreach (var invocation in fighter.Team.AliveFighters.Where(ally => ally.Invocator == fighter))                
                    TryKillFighter(invocation, invocation.Id, true);

                if (fighter.Invocator != null)
                {
                    TurnProcessor.RemoveFighter(fighter);
                    base.Dispatch(WorldMessage.FIGHT_TURN_LIST(TurnProcessor.FighterOrder));
                }
                
                if (State != FightStateEnum.STATE_PLACEMENT)
                    NextLoopTimeout = CurrentLoopTimeout + 1200; // time delayed because of the death

                if (WillFinish())
                {
                    if (State == FightStateEnum.STATE_PLACEMENT)
                        NextLoopTimeout = -1;
                    return FightActionResultEnum.RESULT_END;
                }

                return FightActionResultEnum.RESULT_DEATH;
            }

            if (WillFinish())            
                return FightActionResultEnum.RESULT_END;            

            return FightActionResultEnum.RESULT_NOTHING;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void FighterReady(FighterBase fighter)
        {
            AddMessage(() =>
            {
                fighter.TurnReady = fighter.TurnReady == false;

                base.Dispatch(WorldMessage.FIGHT_READY(fighter.Id, fighter.TurnReady));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="cellId"></param>
        public void FighterPlacementChange(FighterBase fighter, int cellId)
        {
            AddMessage(() =>
            {
                if (State != FightStateEnum.STATE_PLACEMENT)
                {
                    fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }
                    
                var cell = GetCell(cellId);
                if (cell != null)
                {
                    if (cell.CanWalk)
                    {
                        fighter.SetCell(cell);
                        base.Dispatch(WorldMessage.FIGHT_COORDINATE_INFORMATIONS(fighter));
                    }
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetAllUnReady()
        {
            foreach (var fighter in Fighters)           
                fighter.TurnReady = false;            
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartFight()
        {
            AddMessage(() =>
            {
                OnFightStart();

                TurnProcessor.InitTurns(Fighters);

                Map.Dispatch(WorldMessage.FIGHT_FLAG_DESTROY(Id));

                base.CachedBuffer = true;
                base.Dispatch(WorldMessage.FIGHT_STARTS());
                base.Dispatch(WorldMessage.FIGHT_TURN_MIDDLE(Fighters));
                base.Dispatch(WorldMessage.FIGHT_COORDINATE_INFORMATIONS(AliveFighters.ToArray()));
                base.Dispatch(WorldMessage.FIGHT_TURN_LIST(TurnProcessor.FighterOrder));
                Team0.SendChallengeInfos();
                Team1.SendChallengeInfos();
                base.CachedBuffer = false;

                State = FightStateEnum.STATE_FIGHTING;
                NextLoopTimeout = -1;

                SetAllUnReady();

                BeginTurn();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        private void BeginTurn()
        {
            AddMessage(() =>
                {
                    CurrentFighter = TurnProcessor.NextFighter;
                    
                    base.Dispatch(WorldMessage.FIGHT_TURN_STARTS(CurrentFighter.Id, TurnTime));

                    NextTurnTimeout = TurnTime;

                    CurrentFighter.Team.BeginTurn(CurrentFighter);

                    switch (CurrentFighter.BeginTurn())
                    {
                        case FightActionResultEnum.RESULT_END:
                            return;

                        case FightActionResultEnum.RESULT_END_TURN:
                        case FightActionResultEnum.RESULT_DEATH:
                            CurrentFighter.TurnPass = true;
                            EndTurn();
                            return;
                    }

                    LoopState = FightLoopStateEnum.STATE_PROCESS_EFFECT;

                    switch(CurrentFighter.Type)
                    {
                        case EntityTypeEnum.TYPE_CHARACTER:
                            NextLoopState = FightLoopStateEnum.STATE_WAIT_TURN;
                            if (CurrentFighter.IsDisconnected)
                                CurrentFighter.TurnPass = true;
                            break;

                        default:
                            NextLoopState = FightLoopStateEnum.STATE_WAIT_AI;                            
                            if (CurrentFighter is AIFighter)
                            {
                                ((AIFighter)CurrentFighter).CurrentBrain.OnTurnStart();
                            }
                            break;
                    }
                });
        }

        /// <summary>
        /// 
        /// </summary>
        private void MiddleTurn()
        {
            AddMessage(() =>
                {
                    if(!HasLeft(CurrentFighter))
                        CurrentFighter.MiddleTurn();
                    base.Dispatch(WorldMessage.FIGHT_TURN_MIDDLE(Fighters));
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <returns></returns>
        public bool HasLeft(FighterBase fighter)
        {
            return !Fighters.Contains(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        private void EndTurn()
        {
            AddMessage(() =>
            {
                if (!HasLeft(CurrentFighter))
                {
                    CurrentFighter.Team.EndTurn(CurrentFighter);

                    // fin du combat ?
                    if (!CurrentFighter.IsFighterDead)
                    {
                        if (CurrentFighter.EndTurn() == FightActionResultEnum.RESULT_END)
                        {
                            Logger.Debug("Fight::EndTurn turn finished and caused the end.");
                            return;
                        }
                    }

                    if (m_activableObjects.ContainsKey(CurrentFighter))
                    {
                        foreach (var glyph in m_activableObjects[CurrentFighter].OfType<FightGlyph>())
                        {
                            glyph.DecrementDuration();
                        }

                        m_activableObjects[CurrentFighter].RemoveAll(fightObject => fightObject.ObstacleType == FightObstacleTypeEnum.TYPE_GLYPH && fightObject.Duration <= 0);
                    }
                }
                else
                {
                    if (m_activableObjects.ContainsKey(CurrentFighter))
                    {
                        m_activableObjects[CurrentFighter].RemoveAll(fightObject => fightObject.ObstacleType == FightObstacleTypeEnum.TYPE_GLYPH);
                    }
                }

                base.CachedBuffer = true;
                base.Dispatch(WorldMessage.FIGHT_TURN_FINISHED(CurrentFighter.Id));
                base.Dispatch(WorldMessage.FIGHT_TURN_READY(CurrentFighter.Id));
                base.CachedBuffer = false;

                SetAllUnReady();

                LoopState = FightLoopStateEnum.STATE_PROCESS_EFFECT;
                NextLoopState = FightLoopStateEnum.STATE_WAIT_READY;

                NextSynchroTimeout = 5000;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool WillFinish()
        {
            if (LoopState == FightLoopStateEnum.STATE_WAIT_END)
                return true;

            if (GetWinners() != null)
            {
                m_winnersTeam = GetWinners();
                m_winnersFighter = m_winnersTeam.Fighters.Where(f => f.Invocator == null);

                m_losersTeam = m_winnersTeam.OpponentTeam;
                m_losersFighter = m_losersTeam.Fighters.Where(f => f.Invocator == null);

                LoopState = FightLoopStateEnum.STATE_WAIT_END;

                if(CurrentAction != null)
                    base.Dispatch(WorldMessage.FIGHT_ACTION_FINISHED(CurrentFighter.Id));

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellId"></param>
        /// <returns></returns>
        public FighterBase GetFighterOnCell(int cellId)
        {
            return AliveFighters.FirstOrDefault(fighter => fighter.Cell != null && fighter.Cell.Id == cellId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FightTeam GetWinners()
        {
            if (!Team0.HasSomeoneAlive)
                return Team1;
            else if (!Team1.HasSomeoneAlive)
                return Team0;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateDelta"></param>
        public override void Update(long updateDelta)
        {
            switch (LoopState)
            {
                case FightLoopStateEnum.STATE_WAIT_START:
                    if(IsAllReadyToStart || LoopTimedout)
                    {
                        StartFight();
                    }
                    break;

                case FightLoopStateEnum.STATE_WAIT_READY:
                    if (IsAllReady)
                    {
                        MiddleTurn();
                        BeginTurn();
                    }
                    else if (SynchronizationTimedout)
                    {
                        var fighters = AliveFighters.OfType<CharacterEntity>().Where(fighter => !fighter.TurnReady);
                        var fightersName = string.Join(", ", fighters.Select(fighter => fighter.Name));

                        base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_FIGHT_WAITING_PLAYERS, fightersName));

                        MiddleTurn();
                        BeginTurn();
                    }
                    break;

                case FightLoopStateEnum.STATE_WAIT_TURN:
                    if (LoopTimedout) // death time
                    {
                        if (TurnTimedout || HasLeft(CurrentFighter) || CurrentFighter.TurnPass || CurrentFighter.IsFighterDead)
                        {
                            EndTurn();
                        }
                        else if (CurrentFighter is AIFighter)
                        {
                            LoopState = FightLoopStateEnum.STATE_WAIT_AI;
                        }
                    }
                    break;

                case FightLoopStateEnum.STATE_PROCESS_EFFECT:
                    if(m_processingTargets.Count > 0)
                    {
                        var castInfos = m_processingTargets.First();
                        m_processingTargets.RemoveFirst();

                        CurrentProcessingFighter = castInfos.Target;
                                                
                        if (CurrentProcessingFighter != null && !CurrentProcessingFighter.IsFighterDead)
                        {
                            Logger.Debug("Processing effect : " + CurrentProcessingFighter.Name);                                                        
                            var effectResult = EffectManager.Instance.TryApplyEffect(castInfos);
                            if (effectResult == FightActionResultEnum.RESULT_END)
                                break;
                        }
                        else
                        {
                            if (castInfos.Target == null || !castInfos.Target.IsFighterDead)
                            {
                                var effectResult = EffectManager.Instance.TryApplyEffect(castInfos);
                                if (effectResult == FightActionResultEnum.RESULT_END)
                                    break;
                            }
                        }

                        LoopState = FightLoopStateEnum.STATE_WAIT_SUBACTION;
                    }
                    else
                    {
                        CurrentProcessingFighter = null;
                        LoopState = NextLoopState;
                    }
                    break;
                    
                case FightLoopStateEnum.STATE_WAIT_ACTION:
                    if (ActionTimedout || CurrentAction.IsFinished)
                    {
                        if (CurrentAction != null && !CurrentAction.IsFinished)
                        {
                            CurrentFighter.StopAction(CurrentAction.Type);

                            if (CurrentSubAction != null)
                            {
                                LoopState = FightLoopStateEnum.STATE_WAIT_SUBACTION;
                                Logger.Debug("FightBase::Update waiting for subaction");
                                break;
                            }
                        }

                        if (m_processingTargets.Count > 0 && LoopState != FightLoopStateEnum.STATE_WAIT_END)
                        {
                            LoopState = FightLoopStateEnum.STATE_PROCESS_EFFECT;
                            NextLoopState = FightLoopStateEnum.STATE_WAIT_ACTION;
                            break;
                        }

                        // THAT WAS THE FUCKING FIX, NICE CLIENT WAITING FOR THAT TO REFHRESH PLAYER STATE !!!!!!!!!!!! ANKAMAAAAAAAAARGHHHHHHHH
                        if(m_currentApCost != -1)
                        {
                            base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_PA_LOST, CurrentFighter.Id, CurrentFighter.Id + ",-" + m_currentApCost));
                            m_currentApCost = -1;
                        }

                        base.Dispatch(WorldMessage.FIGHT_ACTION_FINISHED(CurrentFighter.Id));

                        if (LoopState == FightLoopStateEnum.STATE_WAIT_END)
                            break;

                        switch (CurrentFighter.Type)
                        {
                            case EntityTypeEnum.TYPE_CHARACTER:
                                LoopState = FightLoopStateEnum.STATE_WAIT_TURN;
                                break;

                            default:
                                LoopState = FightLoopStateEnum.STATE_WAIT_AI;
                                break;
                        }
                    }
                    break;

                case FightLoopStateEnum.STATE_WAIT_SUBACTION:
                    if(SubActionTimedout)
                    {
                        if (CurrentSubAction == null)
                        {
                            LoopState = FightLoopStateEnum.STATE_PROCESS_EFFECT;
                            NextLoopState = FightLoopStateEnum.STATE_WAIT_ACTION;
                        }
                        else
                        {
                            var currentAction = CurrentSubAction;
                            var result = currentAction();
                            switch (result)
                            {
                                case FightActionResultEnum.RESULT_END:
                                    Logger.Debug("FightBase::Update end of fight after subAction.");
                                    return;

                                case FightActionResultEnum.RESULT_DEATH:
                                    if (CurrentFighter.IsFighterDead)
                                        CurrentFighter.TurnPass = true;
                                    break;
                            }

                            if (CurrentSubAction == currentAction)
                            {
                                CurrentSubAction = null;
                                LoopState = FightLoopStateEnum.STATE_PROCESS_EFFECT;
                                NextLoopState = FightLoopStateEnum.STATE_WAIT_ACTION;
                            }
                        }
                    }
                    break;

                case FightLoopStateEnum.STATE_WAIT_AI:
                    if (CurrentFighter is AIFighter)
                    {
                        try
                        {
                            ((AIFighter)CurrentFighter).CurrentBrain.OnUpdate();
                        }
                        catch(Exception ex)
                        {
                            Logger.Error(ex.ToString());
                            CurrentFighter.TurnPass = true;
                        }
                    }
                    LoopState = FightLoopStateEnum.STATE_WAIT_TURN;
                    break;

                case FightLoopStateEnum.STATE_WAIT_END:
                    switch (LoopEndState)
                    {
                        case FightEndStateEnum.STATE_INIT_CALCULATION:
                            LoopEndState = FightEndStateEnum.STATE_PROCESS_CALCULATION;
                            Team0.FightEnd();
                            Team1.FightEnd(); 
                            InitEndCalculation();
                            break;

                        case FightEndStateEnum.STATE_PROCESS_CALCULATION:
                            LoopEndState = FightEndStateEnum.STATE_END_CALCULATION;
                            ApplyEndCalculation();
                            break;

                        case FightEndStateEnum.STATE_END_CALCULATION:
                            if (LoopTimedout)
                            {
                                FightEnd();
                                LoopEndState = FightEndStateEnum.STATE_ENDED;
                            }
                            break;
                    }
                    break;

                case FightLoopStateEnum.STATE_ENDED:
                    FightEnded();
                    break;
            }

            base.Update(updateDelta);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="spellLevel"></param>
        /// <param name="cellId"></param>
        /// <param name="targets"></param>
        /// <returns></returns>
        public FightSpellLaunchResultEnum CanLaunchSpell(FighterBase fighter, SpellLevel spellLevel, int spellId, int cellId, int castCell)
        {
            if(LoopState != FightLoopStateEnum.STATE_WAIT_TURN && LoopState != FightLoopStateEnum.STATE_WAIT_AI)            
                return FightSpellLaunchResultEnum.RESULT_ERROR;
            
            if (CurrentFighter != fighter)            
                return FightSpellLaunchResultEnum.RESULT_ERROR;            
            
            if (GetCell(castCell) == null)            
                return FightSpellLaunchResultEnum.RESULT_ERROR;
            
            if (fighter.AP < spellLevel.APCost)            
                return FightSpellLaunchResultEnum.RESULT_NO_AP;
           
            var distance = Pathfinding.GoalDistance(Map, cellId, castCell);
            var maxPo = spellLevel.AllowPOBoost ?  spellLevel.MaxPO + fighter.Statistics.GetTotal(EffectEnum.AddPO) : spellLevel.MaxPO;
          
            if (maxPo < spellLevel.MinPO)
                maxPo = spellLevel.MinPO;

            if (distance > maxPo || distance < spellLevel.MinPO)            
                return FightSpellLaunchResultEnum.RESULT_NEED_MOVE;            

            if(spellLevel.EmptyCell && !GetCell(castCell).CanWalk)            
                return FightSpellLaunchResultEnum.RESULT_WRONG_TARGET;            

            if(spellLevel.InLine && !Pathfinding.InLine(Map, cellId, castCell))            
                return FightSpellLaunchResultEnum.RESULT_NEED_MOVE;

            if (spellLevel.EmptyCell && GetCell(castCell).HasObject(FightObstacleTypeEnum.TYPE_FIGHTER))
                return FightSpellLaunchResultEnum.RESULT_NO_LOS;

            if (spellLevel.LOS && !Pathfinding.CheckView(this, cellId, castCell))
                return FightSpellLaunchResultEnum.RESULT_NO_LOS;

            if (spellLevel.Effects != null)
            {
                if (spellLevel.Effects.Any(effect => effect.TypeEnum == EffectEnum.Invocation))
                {
                    var invocationCount = fighter.Team.AliveFighters.Count(f => f.Invocator == fighter && !f.StaticInvocation);
                    if (invocationCount >= fighter.Statistics.GetTotal(EffectEnum.AddInvocationMax))
                    {
                        fighter.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_MAX_INVOCATION_REACHED, fighter.Statistics.GetTotal(EffectEnum.AddInvocationMax)));
                        return FightSpellLaunchResultEnum.RESULT_ERROR;
                    }
                }
            }

            var target = GetFighterOnCell(castCell);
            long targetId = 0;
            if (target != null)
                targetId = target.Id;

            if (!fighter.SpellManager.CanLaunchSpell(spellLevel, spellId, targetId))            
                return FightSpellLaunchResultEnum.RESULT_WRONG_TARGET;
            
            return FightSpellLaunchResultEnum.RESULT_OK;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="cellId"></param>
        /// <returns></returns>
        public bool CanUseWeapon(FighterBase fighter, InventoryItemDAO weapon, int cellId)
        {
            var template = weapon.Template;

            if (LoopState != FightLoopStateEnum.STATE_WAIT_TURN)
            {
                Logger.Debug("Fight::CanUseWeapon trying to cast spell withouth being in turn wait phase : " + fighter.Name);
                return false;
            }

            if (CurrentFighter != fighter)
            {
                Logger.Debug("Fight::CanUseWeapon fighter try to use weapon but its not his turn : " + fighter.Name);
                return false;
            }

            if (GetCell(cellId) == null)
            {
                Logger.Debug("Fight::CanUseWeapon null cast cell : " + fighter.Name);
                return false;
            }

            if (fighter.AP < template.APCost)
            {
                Logger.Debug("Fight::CanUseWeapon not enought AP : " + fighter.Name);
                return false;
            }

            var distance = Pathfinding.GoalDistance(Map, fighter.Cell.Id, cellId);
            var poMax = template.POMax + fighter.Statistics.GetTotal(EffectEnum.AddPO);

            if (poMax - template.POMin < 1)
                poMax = template.POMin;

            if (distance > poMax || distance < template.POMin)
            {
                Logger.Debug("Fight::CanUseWeapon target cell not in range : " + fighter.Name);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="cellId"></param>
        public void TryUseWeapon(FighterBase fighter, int cellId, int actionTime = 5000)
        {
            AddMessage(() =>
                {
                    if (LoopState == FightLoopStateEnum.STATE_WAIT_END || LoopState == FightLoopStateEnum.STATE_ENDED)
                    {
                        fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    if(State != FightStateEnum.STATE_FIGHTING)
                    {
                        Logger.Debug("Fight::TryUseWeapon fight is not in fighting state : " + fighter.Name);
                        fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    if (m_currentApCost != -1)
                    {
                        Logger.Debug("Fight::TryUseWeapon fight already processing spell launch and not finished : " + fighter.Name);
                        fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }
                    
                    var weapon = fighter.Inventory.Items.Find(item => item.Slot == ItemSlotEnum.SLOT_WEAPON);

                    if(weapon == null)
                    {
                        TryLaunchSpell(fighter, 0, cellId);
                        return;
                    }

                    if(!CanUseWeapon(fighter, weapon, cellId))
                    {
                        Logger.Debug("Fight::TryUseWeapon unable to use weapon : " + fighter.Name);
                        fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    var weaponTemplate = weapon.Template;

                    CurrentFighter.Team.CheckWeapon(fighter, weaponTemplate);

                    var isMelee = Pathfinding.GoalDistance(Map, fighter.Cell.Id, cellId) == 1;

                    fighter.UsedAP += weaponTemplate.APCost;

                    base.Dispatch(WorldMessage.FIGHT_ACTION_START(CurrentFighter.Id));

                    var failure = false;
                    if (weaponTemplate.CFRate != 0)
                    {
                        var criticalFailureRate = weaponTemplate.CFRate - fighter.Statistics.GetTotal(EffectEnum.AddEchecCritic);

                        if (criticalFailureRate < 2)
                            criticalFailureRate = 2;

                        if (Util.Next(0, criticalFailureRate) == 0)
                            failure = true;
                    }

                    if(failure)
                    {
                        base.CachedBuffer = true;
                        base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_WEAPON_FAILURE, fighter.Id, weaponTemplate.Id.ToString()));
                        base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_PA_LOST, fighter.Id, fighter.Id + ",-" + weaponTemplate.APCost));
                        base.Dispatch(WorldMessage.FIGHT_ACTION_FINISHED(CurrentFighter.Id));
                        base.CachedBuffer = false;

                        CurrentFighter.TurnPass = true;
                        return;
                    }

                    var criticalHit = false;
                    if (weaponTemplate.CSRate != 0)
                    {
                        var criticalHitRate = weaponTemplate.CSRate - fighter.Statistics.GetTotal(EffectEnum.AddDamageCritic);

                        fighter.CalculCriticalHitRate(ref criticalHitRate);

                        if (criticalHitRate < 2)
                            criticalHitRate = 2;

                        if (Util.Next(0, criticalHitRate) == 0)
                            criticalHit = true;
                    }

                    if(criticalHit)                   
                        base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_CRITICAL_HIT, fighter.Id, "0"));

                    var effects = weapon.Statistics.WeaponEffects;
                    var targetLists = new List<Tuple<GenericEffect, List<FighterBase>>>();

                    foreach (var effect in effects)
                    {
                        var targetList = new List<FighterBase>();
                        foreach (var currentCellId in CellZone.GetCells(Map, cellId, fighter.Cell.Id, weaponTemplate.RangeType))
                        {
                            var fightCell = GetCell(currentCellId);
                            if (fightCell != null)
                            {
                                foreach(var fighterObject in fightCell.FightObjects.OfType<FighterBase>())
                                {
                                    if (fighter == fighterObject)
                                        continue;

                                    targetList.Add(fighterObject);
                                }
                            }
                        }
                        targetLists.Add(Tuple.Create(effect.Value, targetList));
                    }

                    LoopState = FightLoopStateEnum.STATE_WAIT_ACTION;

                    fighter.UseWeapon(cellId, actionTime, () =>
                    {
                        if (LoopState == FightLoopStateEnum.STATE_WAIT_END || LoopState == FightLoopStateEnum.STATE_ENDED)
                        {
                            fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                            return;
                        }

                        foreach (var targetsByEffect in targetLists)
                        {
                            targetsByEffect.Item2.RemoveAll(affectedTarget => affectedTarget.IsFighterDead);

                            if (targetsByEffect.Item2.Count == 0)
                            {
                                AddProcessingTarget(
                                        new CastInfos(
                                                        targetsByEffect.Item1.EffectType,
                                                        -1,
                                                        cellId,
                                                        criticalHit && CastInfos.IsDamageEffect(targetsByEffect.Item1.EffectType) ? targetsByEffect.Item1.Value1 + weaponTemplate.CSBonus : targetsByEffect.Item1.Value1,
                                                        criticalHit && CastInfos.IsDamageEffect(targetsByEffect.Item1.EffectType) ? targetsByEffect.Item1.Value2 + weaponTemplate.CSBonus : targetsByEffect.Item1.Value2,
                                                        -1,
                                                        -1,
                                                        0,
                                                        fighter,
                                                        null,
                                                        weaponTemplate.RangeType, 
                                                        0, 
                                                        -1,
                                                        isMelee)
                                                     );
                            }
                            else
                            {
                                foreach (var target in targetsByEffect.Item2)
                                {
                                    AddProcessingTarget(new CastInfos(
                                                        targetsByEffect.Item1.EffectType,
                                                        -1,
                                                        cellId,
                                                        criticalHit && CastInfos.IsDamageEffect(targetsByEffect.Item1.EffectType) ? targetsByEffect.Item1.Value1 + weaponTemplate.CSBonus : targetsByEffect.Item1.Value1,
                                                        criticalHit && CastInfos.IsDamageEffect(targetsByEffect.Item1.EffectType) ? targetsByEffect.Item1.Value2 + weaponTemplate.CSBonus : targetsByEffect.Item1.Value2,
                                                        -1,
                                                        -1,
                                                        0,
                                                        fighter,
                                                        target,
                                                        weaponTemplate.RangeType,
                                                        target.Cell.Id,
                                                        -1,
                                                        isMelee));
                                }
                            }

                        }

                        m_currentApCost = weaponTemplate.APCost;
                    });
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="spellId"></param>
        /// <param name="castCellId"></param>
        public void TryLaunchSpell(FighterBase fighter, int spellId, int castCellId, int actionTime = 5000)
        {
            AddMessage(() =>
            {
                if (LoopState == FightLoopStateEnum.STATE_WAIT_END || LoopState == FightLoopStateEnum.STATE_ENDED)
                {
                    fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (State != FightStateEnum.STATE_FIGHTING)
                {
                    Logger.Debug("Fight::TryLaunchSpell fight is not in fighting state : " + fighter.Name);
                    fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (m_currentApCost != -1)
                {
                    Logger.Debug("Fight::TryLaunchSpell fight already processing spell launch and not finished : " + fighter.Name);
                    fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                if (fighter.SpellBook == null)
                {
                    Logger.Debug("Fight::TryLaunchSpell empty spellbook : " + fighter.Name);
                    fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                var spellLevel = fighter.SpellBook.GetSpellLevel(spellId);

                if (spellLevel == null)
                {
                    Logger.Debug("Fight::TryLaunchSpell unnknow spellId : " + fighter.Name);
                    fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                var launchResult = CanLaunchSpell(fighter, spellLevel, spellId, fighter.Cell.Id, castCellId);
                if (launchResult != FightSpellLaunchResultEnum.RESULT_OK)
                {
                    Logger.Debug("Fight::TryLaunchSpell unable to launch spell : " + fighter.Name + " reason=" + launchResult);
                    fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                var isMelee = Pathfinding.GoalDistance(Map, fighter.Cell.Id, castCellId) == 1;

                fighter.UsedAP += spellLevel.APCost;
                                
                base.Dispatch(WorldMessage.FIGHT_ACTION_START(CurrentFighter.Id));

                var isEchec = false;
                if (spellLevel.ECSRate != 0)
                {
                    var echecRate = spellLevel.ECSRate - fighter.Statistics.GetTotal(EffectEnum.AddEchecCritic);

                    if (echecRate < 2)
                        echecRate = 2;

                    if (Util.Next(0, echecRate) == 0)
                        isEchec = true;

                    if (isEchec)
                    {
                        base.CachedBuffer = true;
                        base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_CRITICAL_FAILURE, fighter.Id, spellId.ToString()));
                        base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_PA_LOST, fighter.Id, fighter.Id + ",-" + spellLevel.APCost));
                        base.Dispatch(WorldMessage.FIGHT_ACTION_FINISHED(CurrentFighter.Id));
                        base.CachedBuffer = false;

                        if (spellLevel.IsECSEndTurn == 1)
                        {
                            CurrentFighter.TurnPass = true;
                        }
                        return;
                    }
                }

                var target = GetFighterOnCell(castCellId);
                if (target != null)
                {
                    fighter.SpellManager.Actualize(spellLevel, spellId, target.Id);
                }

                var isCritic = false;
                if (spellLevel.CSRate != 0 && spellLevel.CriticalEffects.Count > 0)
                {
                    var criticalHitRate = spellLevel.CSRate - fighter.Statistics.GetTotal(EffectEnum.AddDamageCritic);
                    
                    fighter.CalculCriticalHitRate(ref criticalHitRate);

                    if (criticalHitRate < 2)
                        criticalHitRate = 2;

                    if (Util.Next(0, criticalHitRate) == 0)
                        isCritic = true;
                }
                
                if (isCritic)                
                    base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_CRITICAL_HIT, fighter.Id, spellId.ToString()));                

                var effects = isCritic ? spellLevel.CriticalEffects : spellLevel.Effects;
                var targetLists = new Dictionary<SpellEffect, List<FighterBase>>();
                int effectIndex = 0;

                foreach (var effect in effects)
                {
                    targetLists.Add(effect, new List<FighterBase>());

                    var targetType = spellLevel.Template.Targets != null ? spellLevel.Template.Targets.Count > effectIndex ? spellLevel.Template.Targets[effectIndex] : -1 : -1;

                    if (effect.TypeEnum != EffectEnum.UseGlyph && effect.TypeEnum != EffectEnum.UseTrap)
                    {
                        foreach (var currentCellId in CellZone.GetCells(Map, castCellId, fighter.Cell.Id, spellLevel.RangeType))
                        {
                            var fightCell = GetCell(currentCellId);
                            if (fightCell != null)
                            {
                                foreach (var fighterObject in fightCell.FightObjects.OfType<FighterBase>())
                                {
                                    if (targetType != -1)
                                    {
                                        // affect caster : 32
                                        if (((((targetType >> 5) & 1) == 1) && (fighter.Id != fighterObject.Id)))
                                        {
                                            if (!targetLists[effect].Contains(fighter))
                                                targetLists[effect].Add(fighter);
                                            continue;
                                        }

                                        // doesnt affect team mates : 1
                                        if (((targetType & 1) == 1) && fighter.Team == fighterObject.Team)
                                            continue;

                                        // doesnt affect the caster : 2
                                        if ((((targetType >> 1) & 1) == 1) && fighter.Id == fighterObject.Id)
                                            continue;

                                        // doesnt affect ennemies : 4
                                        if ((((targetType >> 2) & 1) == 1) && fighter.Team != fighterObject.Team)
                                            continue;

                                        // only invocation : 8
                                        if (((((targetType >> 3) & 1) == 1) && (fighterObject.Invocator == null)))
                                            continue;

                                        // doesnt affect invocs : 16
                                        if (((((targetType >> 4) & 1) == 1) && (fighterObject.Invocator != null)))
                                            continue;                                        
                                    }

                                    if (!targetLists[effect].Contains(fighterObject))
                                        targetLists[effect].Add(fighterObject);
                                }
                            }
                        }
                    }
                    effectIndex++;
                }

                LoopState = FightLoopStateEnum.STATE_WAIT_ACTION;

                var template = SpellManager.Instance.GetTemplate(spellId);

                fighter.LaunchSpell(castCellId, spellId, spellLevel.Level, template.Sprite.ToString(), template.SpriteInfos, actionTime, () =>
                {
                    if (LoopState == FightLoopStateEnum.STATE_WAIT_END || LoopState == FightLoopStateEnum.STATE_ENDED)
                    {
                        fighter.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    var actualChance = 0;

                    foreach (var effect in effects)
                    {
                        if (effect.Chance > 0)
                        {
                            if (Util.Next(0, 100) > (effect.Chance + actualChance))
                            {
                                actualChance += effect.Chance;
                                continue;
                            }
                            actualChance -= 100;
                        }

                        targetLists[effect].RemoveAll(affectedTarget => affectedTarget.IsFighterDead);

                        if (targetLists[effect].Count == 0)
                        {
                            var castInfos = new CastInfos(
                                                    effect.TypeEnum,
                                                    spellId,
                                                    castCellId,
                                                    effect.Value1,
                                                    effect.Value2,
                                                    effect.Value3,
                                                    effect.Chance,
                                                    effect.Duration,
                                                    fighter,
                                                    null,
                                                    spellLevel.RangeType,
                                                    0,
                                                    spellLevel.Level,
                                                    isMelee);
                            AddProcessingTarget(castInfos);
                            CurrentFighter.Team.CheckSpell(CurrentFighter, castInfos);
                        }
                        else
                        {
                            foreach (var effectTarget in targetLists[effect])
                            {
                                var castInfos = new CastInfos(
                                                    effect.TypeEnum,
                                                    spellId,
                                                    castCellId,
                                                    effect.Value1,
                                                    effect.Value2,
                                                    effect.Value3,
                                                    effect.Chance,
                                                    effect.Duration,
                                                    fighter,
                                                    effectTarget,
                                                    spellLevel.RangeType,
                                                    effectTarget.Cell.Id,
                                                    spellLevel.Level,
                                                    isMelee);
                                AddProcessingTarget(castInfos);
                                CurrentFighter.Team.CheckSpell(CurrentFighter, castInfos);
                            }
                        }

                    }

                    m_currentApCost = spellLevel.APCost;

                });
            });
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="winnerTeam"></param>
        /// <param name="looserTeam"></param>
        private void FightEnd()
        {
            if (State == FightStateEnum.STATE_PLACEMENT)            
                Map.Dispatch(WorldMessage.FIGHT_FLAG_DESTROY(Id));            

            State = FightStateEnum.STATE_ENDED;
            LoopState = FightLoopStateEnum.STATE_ENDED;
            
            base.Dispatch(WorldMessage.FIGHT_END_RESULT(Result));
            
            foreach (var character in Fighters.OfType<CharacterEntity>())
                // delay execution
                character.AddMessage(() => Map.FightManager.ExecuteFightActions(Type, FightStateEnum.STATE_ENDED, character));

            foreach (var fighter in m_winnersTeam.Fighters.ToArray())
                fighter.EndFight(true);

            foreach (var fighter in m_losersTeam.Fighters.ToArray())
                fighter.EndFight();

            foreach (var spectator in SpectatorTeam.Spectators.ToArray())
                spectator.EndFight();
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void FightEnded()
        {
            Map.FightManager.Remove(this);

            Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            foreach (var cell in Cells)
                cell.Value.Dispose();

            Cells.Clear();
            Cells = null;

            SpectatorTeam = null;
            Team0 = null;
            Team1 = null;

            CurrentFighter = null;
            CurrentProcessingFighter = null;            
            CurrentSubAction = null;

            TurnProcessor.Dispose();
            TurnProcessor = null;

            Result.Dispose();
            Result = null;
            
            Map = null;

            m_activableObjects.Clear();
            m_activableObjects = null;
            m_losersFighter = null;
            m_winnersFighter = null;
            m_winnersTeam = null;
            m_losersTeam = null;
            m_processingTargets.Clear();
            m_processingTargets = null;

            base.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool HasObjectOnCell(FightObstacleTypeEnum type, int cell)
        {
            var fightCell = GetCell(cell);
            if (fightCell == null)
                return false;

            return fightCell.HasObject(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool CanPutObject(int cellId)
        {
            var cell = GetCell(cellId);
            if (cell == null)
                return false;
            return cell.CanPutObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="obj"></param>
        public void AddActivableObject(FighterBase caster, FightActivableObject obj)
        {
            if (!m_activableObjects.ContainsKey(caster))
                m_activableObjects.Add(caster, new List<FightActivableObject>());
            m_activableObjects[caster].Add(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanAbortMovement
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cellId"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public void Move(EntityBase entity, int cellId, string path)
        {
            AddMessage(() =>
            {
                if (LoopState != FightLoopStateEnum.STATE_WAIT_TURN)
                {
                    Logger.Debug("Fight::Move trying to move withouth being in turn wait phase : " + entity.Name);
                    return;
                }

                if (entity != CurrentFighter)
                {
                    Logger.Debug("Fight::Move not his turn : " + entity.Name);
                    return;
                }

                var fighter = (FighterBase)entity;
                var movementPath = Pathfinding.IsValidPath(this, fighter, fighter.Cell.Id, path);

                //
                if (movementPath == null)
                {
                    Logger.Debug("Fight::Move null movement path : " + entity.Name);
                    return;
                }

                // Pas assez de point de mouvement
                if (movementPath.MovementLength > fighter.MP)
                {
                    Logger.Debug("Fight::Move no enought mp to move : " + entity.Name);
                    return;
                }

                var tacledChance = Pathfinding.TryTacle(fighter);
                
                // Si tacle
                if (tacledChance != -1 && !CurrentFighter.StateManager.HasState(FighterStateEnum.STATE_ROOTED))
                {
                    base.CachedBuffer = true;

                    // XX A été taclé
                    base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_TACLE, fighter.Id));

                    // perte d'ap
                    var lostAP = (fighter.AP * tacledChance / 100) - 1;

                    if (lostAP < 0)
                        lostAP = 1;

                    if (lostAP > fighter.AP)
                        lostAP = fighter.AP;

                    fighter.UsedAP += lostAP;

                    base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_PA_LOST, fighter.Id, fighter.Id + ",-" + lostAP));

                    var lostMP = fighter.MP;

                    if (lostMP < 0)
                        lostMP = 1;

                    if (lostMP > fighter.MP)
                        lostMP = fighter.MP;

                    fighter.UsedMP += lostMP;

                    base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_PM_LOST, fighter.Id, fighter.Id + ",-" + lostMP));
                    base.CachedBuffer = false;

                    return;
                }

                LoopState = FightLoopStateEnum.STATE_WAIT_ACTION;

                base.Dispatch(WorldMessage.FIGHT_ACTION_START(CurrentFighter.Id));

                CurrentFighter.Team.CheckMovement(fighter, fighter.Cell.Id, movementPath.EndCell, movementPath.MovementLength);

                fighter.Move(movementPath);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="path"></param>
        /// <param name="cellId"></param>
        public void MovementFinish(EntityBase entity, MovementPath movementPath, int cellId)
        {
            var fighter = (FighterBase)entity;

            fighter.UsedMP += movementPath.MovementLength;

            base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_PM_LOST, fighter.Id, fighter.Id + ",-" + movementPath.MovementLength));

            fighter.Orientation = movementPath.GetDirection(movementPath.LastStep);
            fighter.SetCell(GetCell(cellId));
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendMapFightInfos(EntityBase entity)
        {
            if (State == FightStateEnum.STATE_PLACEMENT)
            {
                entity.Dispatch(WorldMessage.FIGHT_FLAG_DISPLAY(this));
                Team0.SendMapFightInfos(entity);
                Team1.SendMapFightInfos(entity);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void SendFightJoinInfos(FighterBase fighter)
        {
            if (fighter.Type == EntityTypeEnum.TYPE_CHARACTER)
            {
                fighter.CachedBuffer = true;
                if(fighter.IsSpectating)
                    fighter.Dispatch(WorldMessage.FIGHT_JOIN_SUCCESS((int)FightStateEnum.STATE_FIGHTING, false, false, true, 0)); // GameJoin
                else
                    fighter.Dispatch(WorldMessage.FIGHT_JOIN_SUCCESS((int)State, CancelButton, true, false, StartTime - UpdateTime)); // GameJoin
                fighter.Dispatch(WorldMessage.GAME_MAP_INFORMATIONS(OperatorEnum.OPERATOR_ADD, AliveFighters.ToArray()));

                switch (State)
                {
                    case FightStateEnum.STATE_PLACEMENT:                        
                        fighter.Dispatch(WorldMessage.FIGHT_AVAILABLE_PLACEMENTS(fighter.Team.Id, FightPlaces)); // GamePlace
                        if (fighter.IsDisconnected)
                        {
                            fighter.IsDisconnected = false;
                            fighter.Dispatch(WorldMessage.GAME_DATA_SUCCESS());
                            base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_FIGHTER_RECONNECTED, fighter.Name));
                        }
                        else
                        {
                            Map.Dispatch(WorldMessage.FIGHT_FLAG_UPDATE(OperatorEnum.OPERATOR_ADD, fighter.Team.LeaderId, fighter));
                            if (fighter.MapId != Map.Id)
                            {
                                fighter.Dispatch(WorldMessage.GAME_DATA_SUCCESS());
                            }
                        }
                        break;

                    case FightStateEnum.STATE_FIGHTING:
                        if(fighter.IsSpectating)
                        {
                            base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_FIGHT_SPECTATOR_JOINED, fighter.Name));
                        }
                        else if (fighter.IsDisconnected)
                        {
                            fighter.IsDisconnected = false;
                            fighter.Team.SendChallengeInfos(fighter);
                            fighter.Dispatch(WorldMessage.GAME_DATA_SUCCESS());
                            base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_FIGHTER_RECONNECTED, fighter.Name));
                        }                        
                        fighter.Dispatch(WorldMessage.FIGHT_COORDINATE_INFORMATIONS(AliveFighters.ToArray()));
                        fighter.Dispatch(WorldMessage.FIGHT_STARTS());
                        fighter.Dispatch(WorldMessage.FIGHT_TURN_LIST(TurnProcessor.FighterOrder));
                        fighter.Dispatch(WorldMessage.FIGHT_TURN_STARTS(CurrentFighter.Id, TurnTimeLeft));
                        break;
                }
                fighter.CachedBuffer = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnFightStart()
        {
            foreach (var character in Fighters.OfType<CharacterEntity>())
            {
                character.FrameManager.RemoveFrame(FightPlacementFrame.Instance);
                character.FrameManager.RemoveFrame(InventoryFrame.Instance);
                character.FrameManager.AddFrame(GameActionFrame.Instance);
                character.FrameManager.AddFrame(FightFrame.Instance);

                Map.FightManager.ExecuteFightActions(Type, FightStateEnum.STATE_PLACEMENT, character);
            }

            foreach (var fighter in Fighters.OfType<AIFighter>())
                fighter.TurnReady = true;                
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public virtual void OnCharacterJoin(CharacterEntity character, FightTeam team)
        {
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public abstract bool CanJoin(CharacterEntity character);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public abstract FightActionResultEnum FightQuit(CharacterEntity character, bool kick = false);

        /// <summary>
        /// 
        /// </summary>
        public abstract void InitEndCalculation();

        /// <summary>
        /// 
        /// </summary>
        public abstract void ApplyEndCalculation();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public abstract void SerializeAs_FightList(StringBuilder message);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public abstract void SerializeAs_FightFlag(StringBuilder message);
    }
}
