using Codebreak.Service.World.Frames;
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
using Codebreak.WorldService;

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
        STATE_END_FIGHT,
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


    public sealed class FightEndResult
    {
        public long FightId
        {
            get;
            private set;
        }

        public bool CanWinHonor
        {
            get;
            private set;
        }

        public string Message
        {
            get
            {
                return _message.ToString();
            }
        }

        private StringBuilder _message;

        public FightEndResult(long fightId, bool CanWinHonor)
        {
            _message = new StringBuilder("GE");
            _message.Append(0).Append('|');
            _message.Append(fightId).Append('|');
            _message.Append(CanWinHonor ? '1' : '0');
        }

        public void AddResult(FighterBase fighter,
            bool win,
            bool leave = false,
            long kamas = 0,
            long exp = 0,
            long honor = 0,
            long disHonor = 0,
            long guildXp = 0,
            long mountXp = 0,
            Dictionary<int, uint> items = null)
        {
            _message.Append('|').Append(win ? '2' : '0').Append(';');
            _message.Append(fighter.Id).Append(';');
            _message.Append(fighter.Name).Append(';');
            _message.Append(fighter.Level).Append(';');
            _message.Append((fighter.IsFighterDead || leave) ? '1' : '0').Append(';');

            if (CanWinHonor)
            {
                if (fighter.Type == EntityTypEnum.TYPE_CHARACTER)
                {
                    var actorCharacter = (CharacterEntity)fighter;

                    //message.Append(DatabaseEntities.GetExperienceFloor(character.Character.AlignmentCache.AlignmentLevel).Pvp).Append(';');
                    //message.Append(character.Character.AlignmentCache.AlignmentHonor).Append(';');
                    //message.Append(DatabaseEntities.GetExperienceFloor(character.Character.AlignmentCache.AlignmentLevel + 1).Pvp).Append(';');
                    //message.Append(result.WinHonor).Append(';');
                    //message.Append(character.Character.AlignmentCache.AlignmentLevel).Append(';');
                    //message.Append(character.Character.AlignmentCache.AlignmentDishonor).Append(';');
                    //message.Append(result.WinDisHonor).Append(';');
                    //if (result.WinItems != null)
                    //    message.Append(string.Join(",", result.WinItems.Select(x => x.Key + "~" + x.Value))).Append(';');
                    //else
                    //    message.Append("").Append(';');

                    //message.Append(result.WinKamas.ToString()).Append(';');
                    //message.Append(DatabaseEntities.GetExperienceFloor(result.Fighter.Level).Character).Append(';');    // LastExperience
                    //message.Append((result.Fighter as CharacterFighter).Character.Experience).Append(';');
                    //message.Append(DatabaseEntities.GetExperienceFloor(result.Fighter.Level + 1).Character).Append(';');// NextExperience
                    //message.Append(result.WinExp);
                }
            }
            else
            {
                if (fighter.Type == EntityTypEnum.TYPE_CHARACTER)
                {
                    var actorCharacter = (CharacterEntity)fighter;

                    _message.Append(actorCharacter.ExperienceFloorCurrent).Append(';');    // LastExperience
                    _message.Append(actorCharacter.Experience).Append(';');
                    _message.Append(actorCharacter.ExperienceFloorNext).Append(';');// NextExperience
                    _message.Append(exp).Append(';');
                    _message.Append(guildXp).Append(';');
                    _message.Append(mountXp).Append(';');
                }
                else
                {
                    _message.Append(";;;;;;");
                }

                if (items != null && items.Count > 0)
                    _message.Append(string.Join(",", items.Select(itemEntry => itemEntry.Key + "~" + itemEntry.Value))).Append(';');
                else
                    _message.Append("").Append(';');

                _message.Append(kamas > 0 ? kamas.ToString() : "");
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class FightCell
    {
        public int Id;
        public List<IFightObstacle> FightObjects;
        public bool Walkable;
        public bool LineOfSight;

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

            if (fightObject.ObstacleType == FightObstacleTypeEnum.TYPE_FIGHTER || fightObject.ObstacleType == FightObstacleTypeEnum.TYPE_CAWWOT)
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
    }

    public abstract class FightBase : MessageDispatcher, IMovementHandler
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
                return _loopTimeout;
            }
            set
            {
                _loopTimeout = UpdateTime + value;
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
                return _turnTimeout;
            }
            set
            {
                _turnTimeout = UpdateTime + value;
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
                return _subActionTimeout;
            }
            set
            {
                _subActionTimeout = UpdateTime + value;
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
                return _synchronizationTimeout;
            }
            set
            {
                _synchronizationTimeout = UpdateTime + value;
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
                return Fighters.All(fighter => fighter.TurnReady || fighter.IsFighterDead);
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
                    if(CurrentFighter.CurrentAction != null)
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
        protected FightTeam _winnerTeam, _loserTeam;
        private long _loopTimeout, _turnTimeout, _subActionTimeout, _synchronizationTimeout;
        private Dictionary<FighterBase, List<FightActivableObject>> _activableObjects;
        private LinkedList<CastInfos> _processingTargets;
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapInstance"></param>
        public FightBase(FightTypeEnum type, MapInstance  mapInstance, long id, long team0LeaderId, int team0FlagCell, long team1LeaderId, int team1FlagCell, long startTimeout, long turnTime, bool cancelButton = false)
        {
            _activableObjects = new Dictionary<FighterBase, List<FightActivableObject>>();
            _processingTargets = new LinkedList<CastInfos>();

            Type = type;
            Id = id;
            Map = mapInstance;
            State = FightStateEnum.STATE_PLACEMENT;
            LoopState = FightLoopStateEnum.STATE_INIT;
            CancelButton = cancelButton;
            TurnTime = turnTime;
            StartTime = startTimeout;
            NextLoopTimeout = startTimeout;
            Result = new FightEndResult(Id, false);
            Cells = new Dictionary<int, FightCell>();
            TurnProcessor = new FightTurnProcessor();

            foreach (var cell in mapInstance.Cells)
            {
                Cells.Add(cell.Id, new FightCell() { Id = cell.Id, LineOfSight = cell.LineOfSight, Walkable = cell.Walkable, FightObjects = new List<IFightObstacle>() });
            }

            SpectatorTeam = new SpectatorTeam(this);
            Team0 = new FightTeam(0, team0LeaderId, team0FlagCell, this, new List<FightCell>(Cells.Values.Where(cell => mapInstance.FightTeam0Cells.Contains(cell.Id))));
            Team1 = new FightTeam(1, team1LeaderId, team1FlagCell, this, new List<FightCell>(Cells.Values.Where(cell => mapInstance.FightTeam1Cells.Contains(cell.Id))));
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
                    Map.Dispatch(WorldMessage.FIGHT_FLAG_DISPLAY(this));
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
            if (CurrentProcessingFighter == infos.Target)
            {
                Logger.Debug("AddProcessingTarget first (CurrentProcessingFighter) : " + infos.Target.Name);
                _processingTargets.AddFirst(infos);
            }
            else if (CurrentProcessingFighter == null && CurrentFighter == infos.Target)
            {
                Logger.Debug("AddProcessingTarget first (CurrentFighter) : " + infos.Target.Name);
                _processingTargets.AddFirst(infos);
            }
            else
            {
                Logger.Debug("AddProcessingTarget last : " + infos.Target.Name);
                _processingTargets.AddLast(infos);
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
                for (int i = SpectatorTeam.Spectators.Count() - 1; i > -1; i--)
                {
                    SpectatorTeam.Spectators.ElementAt(i).AbortAction(GameActionTypeEnum.FIGHT);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void TryJoin(FighterBase fighter, long teamId)
        {
            if (State != FightStateEnum.STATE_PLACEMENT)
            {
                Logger.Debug("Fight::TryJoin fight already started " + fighter.Name);
                fighter.Dispatch(WorldMessage.FIGHT_JOIN_ERROR());
                return;
            }

            if (!CanJoin(fighter))
            {
                fighter.Dispatch(WorldMessage.FIGHT_JOIN_ERROR());
                return;
            }

            FightTeam team = teamId == Team0.LeaderId ? Team0 : Team1;

            if (!team.CanJoinBeforeStart(fighter))
            {
                Logger.Debug("Fight::TryJoin cannot join team before start " + fighter.Name);
                fighter.Dispatch(WorldMessage.FIGHT_JOIN_ERROR());
                return;
            }

            JoinFight(fighter, team);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="team"></param>
        public void JoinFight(FighterBase fighter, FightTeam team)
        {
            AddMessage(() =>
            {
                if (!fighter.Disconnected)
                {
                    fighter.JoinFight(this, team);
                    base.Dispatch(WorldMessage.GAME_MAP_INFORMATIONS(OperatorEnum.OPERATOR_ADD, fighter));
                    team.AddHandler(fighter.Dispatch);
                }
                
                if (fighter.Type == EntityTypEnum.TYPE_CHARACTER)
                {
                    fighter.CachedBuffer = true;

                    fighter.Dispatch(WorldMessage.FIGHT_JOIN_SUCCESS((int)State, CancelButton, true, false, StartTime)); // GameJoin
                    fighter.Dispatch(WorldMessage.FIGHT_AVAILABLE_PLACEMENTS(fighter.Team.Id, FightPlaces)); // GamePlace
                    fighter.Dispatch(WorldMessage.GAME_MAP_INFORMATIONS(OperatorEnum.OPERATOR_ADD, AliveFighters.ToArray()));

                    switch (State)
                    {
                        case FightStateEnum.STATE_PLACEMENT:
                            Map.Dispatch(WorldMessage.FIGHT_FLAG_UPDATE(OperatorEnum.OPERATOR_ADD, team.LeaderId, fighter));
                            break;

                        case FightStateEnum.STATE_FIGHTING:
                            if (fighter.Disconnected)
                            {
                                fighter.Disconnected = false;
                                fighter.Dispatch(WorldMessage.FIGHT_COORDINATE_INFORMATIONS(AliveFighters.ToArray()));
                                fighter.Dispatch(WorldMessage.FIGHT_STARTS());
                                fighter.Dispatch(WorldMessage.FIGHT_TURN_LIST(TurnProcessor.FighterOrder));
                                fighter.Dispatch(WorldMessage.GAME_DATA_SUCCESS());
                                if (CurrentFighter != null)
                                {
                                    fighter.Dispatch(WorldMessage.FIGHT_TURN_STARTS(CurrentFighter.Id, TurnTimeLeft));
                                }
                                base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_FIGHTER_RECONNECTED, fighter.Name));
                            }
                            break;
                    }

                    fighter.CachedBuffer = false;
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void FightDisconnect(FighterBase fighter)
        {
            AddMessage(() =>
            {
                if (State == FightStateEnum.STATE_PLACEMENT)
                {
                    Logger.Debug("Fight::Disconnect fighter left during fight placement : " + fighter.Name);
                    FightQuit(fighter);
                    return;
                }

                Logger.Debug("Fight::Disconnect fighter disconnected : " + fighter.Name);

                fighter.Disconnected = true;

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
            if (force)
            {
                fighter.Life = 0;
            }

            if (fighter.IsFighterDead)
            {
                if (quit)
                {
                    base.Dispatch(WorldMessage.GAME_MAP_INFORMATIONS(OperatorEnum.OPERATOR_REMOVE, fighter));
                }
                else
                {
                    base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_KILL, killerId, fighter.Id.ToString()));
                }

                fighter.OnDeath();

                foreach (var invocation in fighter.Team.AliveFighters.Where(ally => ally.Invocator == fighter))
                {
                    TryKillFighter(invocation, invocation.Id, true);
                }

                if (State != FightStateEnum.STATE_PLACEMENT)
                    NextLoopTimeout = CurrentLoopTimeout + 1200; // time delayed because of the death

                if (WillFinish())
                {
                    if (State == FightStateEnum.STATE_PLACEMENT)
                        NextLoopTimeout = -1;
                    return FightActionResultEnum.RESULT_END;
                }

                if (CurrentFighter == fighter)
                {
                    fighter.TurnPass = true;
                }

                return FightActionResultEnum.RESULT_DEATH;
            }

            if (WillFinish())
            {
                return FightActionResultEnum.RESULT_END;
            }

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
            {
                fighter.TurnReady = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartFight()
        {
            AddMessage(() =>
            {
                foreach (var fighter in Fighters)
                {
                    fighter.FrameManager.RemoveFrame(GameFightPlacementFrame.Instance);
                    fighter.FrameManager.AddFrame(GameActionFrame.Instance);
                    fighter.FrameManager.AddFrame(GameFightFrame.Instance);
                }

                TurnProcessor.InitTurns(Fighters);

                Map.Dispatch(WorldMessage.FIGHT_FLAG_DESTROY(Id));
                base.Dispatch(WorldMessage.FIGHT_COORDINATE_INFORMATIONS(Fighters.ToArray()));
                base.Dispatch(WorldMessage.FIGHT_STARTS());
                base.Dispatch(WorldMessage.FIGHT_TURN_LIST(TurnProcessor.FighterOrder));

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

                    switch (CurrentFighter.BeginTurn())
                    {
                        case FightActionResultEnum.RESULT_END:
                            return;

                        case FightActionResultEnum.RESULT_END_TURN:
                        case FightActionResultEnum.RESULT_DEATH:
                            CurrentFighter.TurnPass = true;
                            return;
                    }

                    LoopState = FightLoopStateEnum.STATE_PROCESS_EFFECT;
                    NextLoopState = FightLoopStateEnum.STATE_WAIT_TURN;
                });
        }

        /// <summary>
        /// 
        /// </summary>
        private void MiddleTurn()
        {
            AddMessage(() =>
                {
                    if (CurrentFighter.MiddleTurn() == FightActionResultEnum.RESULT_END)
                    {
                        Logger.Debug("Fight::MiddleTurn fight already finished.");
                        return;
                    }

                    base.Dispatch(WorldMessage.FIGHT_TURN_MIDDLE(Fighters));
                });
        }

        /// <summary>
        /// 
        /// </summary>
        private void EndTurn()
        {
            AddMessage(() =>
                {
                    // fin du combat ?
                    if (!CurrentFighter.IsFighterDead)
                    {
                        if (CurrentFighter.EndTurn() == FightActionResultEnum.RESULT_END)
                        {
                            Logger.Debug("Fight::EndTurn fight already finished.");
                            return;
                        }
                    }

                    if (_activableObjects.ContainsKey(CurrentFighter))
                    {
                        foreach (var glyph in _activableObjects[CurrentFighter].OfType<FightGlyph>())
                        {
                            glyph.DecrementDuration();
                        }

                        _activableObjects[CurrentFighter].RemoveAll(fightObject => fightObject.ObstacleType == FightObstacleTypeEnum.TYPE_GLYPH && fightObject.Duration <= 0);
                    }

                    SetAllUnReady();

                    base.Dispatch(WorldMessage.FIGHT_TURN_FINISHED(CurrentFighter.Id));
                    base.Dispatch(WorldMessage.FIGHT_TURN_READY(CurrentFighter.Id));

                    LoopState = FightLoopStateEnum.STATE_PROCESS_EFFECT;
                    NextLoopState = FightLoopStateEnum.STATE_WAIT_READY;

                    // turn ready timeout : synchronize players
                    NextSynchroTimeout = 5000;
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool WillFinish()
        {
            if (GetWinners() != null)
            {
                _winnerTeam = GetWinners();
                _loserTeam = _winnerTeam.OpponentTeam;

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
            return AliveFighters.FirstOrDefault(fighter => fighter.Cell.Id == cellId);
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
                    if(IsAllReady || LoopTimedout)
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
                        var fighters = AliveFighters.Where(fighter => !fighter.TurnReady);
                        var fightersName = string.Join(", ", fighters.Select(fighter => fighter.Name));

                        base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_FIGHT_WAITING_PLAYERS, fightersName));

                        MiddleTurn();
                        BeginTurn();
                    }
                    break;

                case FightLoopStateEnum.STATE_WAIT_TURN:
                    if(TurnTimedout || CurrentFighter.TurnPass)
                    {
                        EndTurn();
                    }
                    break;

                case FightLoopStateEnum.STATE_PROCESS_EFFECT:
                    if(_processingTargets.Count > 0)
                    {
                        var castInfos = _processingTargets.First();
                        _processingTargets.RemoveFirst();

                        CurrentProcessingFighter = castInfos.Target;

                        Logger.Debug("Processing effect : " + CurrentProcessingFighter.Name);

                        var effectResult = EffectManager.Instance.TryApplyEffect(castInfos);
                        if (effectResult == FightActionResultEnum.RESULT_END)
                            break;

                        LoopState = FightLoopStateEnum.STATE_WAIT_SUBACTION;
                    }
                    else
                    {
                        CurrentProcessingFighter = null;
                        LoopState = NextLoopState;
                    }
                    break;
                    
                case FightLoopStateEnum.STATE_WAIT_ACTION:     
                    if(ActionTimedout || CurrentAction.IsFinished)
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

                        if(_processingTargets.Count > 0)
                        {
                            LoopState = FightLoopStateEnum.STATE_PROCESS_EFFECT;
                            NextLoopState = FightLoopStateEnum.STATE_WAIT_ACTION;
                            break;
                        }

                        base.Dispatch(WorldMessage.FIGHT_ACTION_FINISHED(CurrentFighter.Id));

                        if(LoopState != FightLoopStateEnum.STATE_END_FIGHT)
                        {
                            switch(CurrentFighter.Type)
                            {
                                case EntityTypEnum.TYPE_CHARACTER:
                                    LoopState = FightLoopStateEnum.STATE_WAIT_TURN;
                                    break;

                                default:
                                    LoopState = FightLoopStateEnum.STATE_WAIT_AI;
                                    break;
                            }
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
                                    break;

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
                    // Process AI calculation
                    break;

                case FightLoopStateEnum.STATE_WAIT_END:
                    switch (LoopEndState)
                    {
                        case FightEndStateEnum.STATE_INIT_CALCULATION:
                            InitEndCalculation();
                            LoopEndState = FightEndStateEnum.STATE_PROCESS_CALCULATION;
                            break;

                        case FightEndStateEnum.STATE_PROCESS_CALCULATION:
                            ApplyEndCalculation();
                            LoopEndState = FightEndStateEnum.STATE_END_CALCULATION;
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
            if(LoopState != FightLoopStateEnum.STATE_WAIT_TURN)
            {
                Logger.Debug("Fight::CanLaunchSpell trying to cast spell withouth being in turn wait phase : " + fighter.Name);
                return FightSpellLaunchResultEnum.RESULT_ERROR;
            }

            if (CurrentFighter != fighter)
            {
                Logger.Debug("Fight::CanLaunchSpell fighter try to launch a spell but its not his turn : " + fighter.Name);
                return FightSpellLaunchResultEnum.RESULT_ERROR;
            }

            if (!fighter.CanGameAction(GameActionTypeEnum.FIGHT_SPELL_LAUNCH))
            {
                Logger.Debug("Fight::CanLaunchSpell fighter cannot game action : " + fighter.Name);
                return FightSpellLaunchResultEnum.RESULT_ERROR;
            }

            if (GetCell(castCell) == null)
            {
                Logger.Debug("Fight::CanLaunchSpell null cast cell : " + fighter.Name);
                return FightSpellLaunchResultEnum.RESULT_ERROR;
            }

            if (fighter.AP < spellLevel.APCost)
            {
                Logger.Debug("Fight::CanLaunchSpell not enought AP : " + fighter.Name);
                return FightSpellLaunchResultEnum.RESULT_NO_AP;
            }

            var distance = Pathfinding.GoalDistance(Map, cellId, castCell);
            var maxPo = spellLevel.MaxPO + (spellLevel.AllowPOBoost ? fighter.Statistics.GetTotal(EffectEnum.AddPO) : 0);

            if (maxPo - spellLevel.MinPO < 1)
                maxPo = spellLevel.MinPO;

            if (distance > maxPo || distance < spellLevel.MinPO - 1)
            {
                Logger.Debug("Fight::CanLaunchSpell target cell not in range : " + fighter.Name);
                return FightSpellLaunchResultEnum.RESULT_NEED_MOVE;
            }

            //if (spellLevel.LineOfSight && !Pathfinding.CheckView(this, cellId, castCell))
            //    return FightSpellLaunchResultEnum.RESULT_NO_LOS;

            var target = GetFighterOnCell(castCell);
            if (target == null)
            {
                return FightSpellLaunchResultEnum.RESULT_OK;
            }

            if (!fighter.SpellManager.CanLaunchSpell(spellLevel, spellId, target.Id))
            {
                Logger.Debug("Fight::CanLaunchSpell unable to hit same target anymore : " + fighter.Name);
                return FightSpellLaunchResultEnum.RESULT_WRONG_TARGET;
            }

            return FightSpellLaunchResultEnum.RESULT_OK;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="spellId"></param>
        /// <param name="castCellId"></param>
        public void LaunchSpell(FighterBase fighter, int spellId, int castCellId, int actionTime = 5000)
        {
            AddMessage(() =>
            {
                if (State != FightStateEnum.STATE_FIGHTING)
                {
                    Logger.Debug("Fight::LaunchSpell fight is not in fighting state : " + fighter.Name);
                    return;
                }

                if (fighter.Spells == null)
                {
                    Logger.Debug("Fight::LaunchSpell empty spellbook : " + fighter.Name);
                    return;
                }

                var spellLevel = fighter.Spells.GetSpellLevel(spellId);

                if (spellLevel == null)
                {
                    Logger.Debug("Fight::LaunchSpell unnknow spellId : " + fighter.Name);
                    return;
                }

                if (CanLaunchSpell(fighter, spellLevel, spellId, fighter.Cell.Id, castCellId) != FightSpellLaunchResultEnum.RESULT_OK)
                {
                    Logger.Debug("Fight::LaunchSpell unable to launch spell : " + fighter.Name);
                    return;
                }

                fighter.UsedAP += spellLevel.APCost;

                var isEchec = false;
                var echecRate = spellLevel.ESCRate - fighter.Statistics.GetTotal(EffectEnum.AddEchecCritic);

                if (echecRate < 2)
                    echecRate = 2;

                if (spellLevel.ESCRate != 0 && (Util.Next(0, spellLevel.ESCRate) == 1))
                    isEchec = true;

                if (isEchec)
                {
                    base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_SPELL_ECHEC, fighter.Id, spellId.ToString()));
                    base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_PA_LOST, fighter.Id, fighter.Id + ",-" + spellLevel.APCost));

                    if (spellLevel.IsECSEndTurn == 1)
                    {
                        CurrentFighter.TurnPass = true;
                    }
                }
                else
                {
                    var target = GetFighterOnCell(castCellId);
                    if (target != null)
                    {
                        fighter.SpellManager.Actualize(spellLevel, spellId, target.Id);
                    }

                    var isCritic = false;
                    if (spellLevel.CSRate != 0 && spellLevel.CriticalEffects.Count > 0)
                    {
                        var criticRate = spellLevel.CSRate - fighter.Statistics.GetTotal(EffectEnum.AddDamageCritic);

                        if (criticRate < 2)
                            criticRate = 2;

                        if (Util.Next(0, criticRate) == 1)
                            isCritic = true;
                    }

                    base.Dispatch(WorldMessage.FIGHT_ACTION_START(CurrentFighter.Id));

                    if (isCritic)
                    {
                        base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_SPELL_CRITIC, fighter.Id, spellId.ToString()));
                    }

                    var effects = isCritic ? spellLevel.CriticalEffects : spellLevel.Effects;
                    var targetLists = new Dictionary<SpellEffect, List<FighterBase>>();

                    foreach (var effect in effects)
                    {
                        targetLists.Add(effect, new List<FighterBase>());

                        if (effect.TypeEnum != EffectEnum.UseGlyph && effect.TypeEnum != EffectEnum.UseTrap)
                        {
                            foreach (var currentCellId in CellZone.GetCells(Map, castCellId, fighter.Cell.Id, spellLevel.RangeType))
                            {
                                var fightCell = GetCell(currentCellId);
                                if (fightCell != null)
                                {
                                    if (fightCell.HasObject(FightObstacleTypeEnum.TYPE_FIGHTER))
                                    {
                                        targetLists[effect].AddRange(fightCell.FightObjects.OfType<FighterBase>());
                                    }
                                }
                            }
                        }
                    }

                    LoopState = FightLoopStateEnum.STATE_WAIT_ACTION;

                    var template = SpellManager.Instance.GetTemplate(spellId);

                    fighter.LaunchSpell(castCellId, spellId, spellLevel.Level, template.Sprite.ToString(), template.SpriteInfos, actionTime, () =>
                    {
                        var actualChance = 0;

                        foreach (var effect in effects)
                        {
                            if (effect.Chance > 0)
                            {
                                if (Util.Next(1, 101) > (effect.Chance + actualChance))
                                {
                                    actualChance += effect.Chance;
                                    continue;
                                }

                                actualChance -= 100;
                            }

                            targetLists[effect].RemoveAll(affectedTarget => affectedTarget.IsFighterDead);

                            if (targetLists[effect].Count == 0)
                            {
                                var effectResult = EffectManager.Instance.TryApplyEffect(
                                        new CastInfos(
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
                                                        spellLevel.RangeType)
                                                     );
                                if (effectResult == FightActionResultEnum.RESULT_END)
                                    return;
                            }
                            else
                            {
                                foreach (var effectTarget in targetLists[effect])
                                {
                                    AddProcessingTarget(new CastInfos(
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
                                                        spellLevel.RangeType));
                                }
                            }
                        }
                        
                        base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_PA_LOST, fighter.Id, fighter.Id + ",-" + spellLevel.APCost));
                    });
                }
            });
        }
        

        // <summary>
        /// 
        /// </summary>
        /// <param name="winnerTeam"></param>
        /// <param name="looserTeam"></param>
        private void FightEnd()
        {
            if (State == FightStateEnum.STATE_PLACEMENT)
            {
                Map.Dispatch(WorldMessage.FIGHT_FLAG_DESTROY(Id));
            }

            base.Dispatch(WorldMessage.FIGHT_END_RESULT(Result));

            foreach (var fighter in _winnerTeam.Fighters)
            {
                fighter.EndFight(true);
            }

            foreach (var fighter in _loserTeam.Fighters)
            {
                fighter.EndFight();
            }

            foreach (var spectator in SpectatorTeam.Spectators)
            {
                spectator.EndFight();
            }

            //// respawn
            //if (this is MonsterFight)
            //{
            //    var monsterGroup = ((MonsterFight)this).MonsterGroup;
            //    if (_winnerTeam.Id == 1)
            //    {
            //        Map.AddMessage(() =>
            //        {
            //            monsterGroup.EndAction(GameActionTypeEnum.FIGHT);

            //            Map.SpawnMonster(monsterGroup);
            //        });
            //    }
            //    else
            //    {
            //        Map.AddMessage(() =>
            //        {
            //            Map.SpawnMonster();
            //        });

            //        ActorManager.Destroy(monsterGroup);
            //    }
            //}

            FightManager.Instance.Remove(this);

            State = FightStateEnum.STATE_ENDED;
            LoopState = FightLoopStateEnum.STATE_END_FIGHT;
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
        public bool CanPutObject(FightObstacleTypeEnum type, int cell)
        {
            return !HasObjectOnCell(FightObstacleTypeEnum.TYPE_CAWWOT, cell) && !HasObjectOnCell(FightObstacleTypeEnum.TYPE_FIGHTER, cell);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="obj"></param>
        public void AddActivableObject(FighterBase caster, FightActivableObject obj)
        {
            if (!_activableObjects.ContainsKey(caster))
                _activableObjects.Add(caster, new List<FightActivableObject>());
            _activableObjects[caster].Add(obj);
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
                    // XX A été taclé
                    base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_TACLE, fighter.Id));

                    var lostAP = (fighter.AP * tacledChance / 100) - 1;
                    var lostMP = (int)Math.Abs(lostAP);

                    if (lostAP < 0)
                        lostAP = 1;

                    if (lostAP > fighter.AP)
                        lostAP = fighter.AP;

                    fighter.UsedAP += lostAP;

                    base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_PA_LOST, fighter.Id, fighter.Id + ",-" + lostAP));

                    if (lostMP < 0)
                        lostMP = 1;

                    if (lostMP > fighter.MP)
                        lostMP = fighter.MP;

                    fighter.UsedMP += lostMP;

                    base.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_PM_LOST, fighter.Id, fighter.Id + ",-" + lostMP));

                    return;
                }

                LoopState = FightLoopStateEnum.STATE_WAIT_ACTION;

                base.Dispatch(WorldMessage.FIGHT_ACTION_START(CurrentFighter.Id));
                                
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
        /// <param name="actor"></param>
        /// <returns></returns>
        public abstract bool CanJoin(FighterBase fighter);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public abstract FightActionResultEnum FightQuit(FighterBase fighter, bool kick = false);

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

    }
}
