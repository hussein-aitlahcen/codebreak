using Codebreak.Service.World.Game.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.Game.Entity;

namespace Codebreak.Service.World.Game.Fight
{
    /// <summary>
    /// 
    /// </summary>
    public enum FightOptionTypeEnum
    {
        TYPE_NEW_PLAYER_BIS = 'A',
        TYPE_NEW_PLAYER = 'N',
        TYPE_HELP = 'H',
        TYPE_PARTY = 'P',
        TYPE_SPECTATOR = 'S',
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SpectatorTeam : MessageDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
        public List<FighterBase> Spectators
        {
            get
            {
                return _spectators;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<FighterBase> _spectators;

        /// <summary>
        /// 
        /// </summary>
        private FightBase _fight;

        /// <summary>
        /// 
        /// </summary>
        public bool CanJoin
        {
            get
            {
                return _fight.State == FightStateEnum.STATE_FIGHTING &&
                    !_fight.Team0.IsOptionLocked(FightOptionTypeEnum.TYPE_SPECTATOR) &&
                    !_fight.Team1.IsOptionLocked(FightOptionTypeEnum.TYPE_SPECTATOR);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        public SpectatorTeam(FightBase fight)
        {
            _fight = fight;
            _spectators = new List<FighterBase>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void AddSpectator(FighterBase fighter)
        {
            _spectators.Add(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void RemoveSpectator(FighterBase fighter)
        {
            _spectators.Remove(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            _fight = null;
            _spectators.Clear();
            _spectators = null;

            base.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class FightTeam : MessageDispatcher
    {       
        /// <summary>
        /// 
        /// </summary>
        public List<FighterBase> Fighters
        {
            get
            {
                return _fighters;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<FighterBase> AliveFighters
        {
            get
            {
                return Fighters.Where(fighter => !fighter.IsFighterDead);
            }
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
        public FightBase Fight
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long LeaderId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int FlagCellId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FightTeam OpponentTeam
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FightCell FreePlace
        {
            get
            {
                return _places.Find(cell => cell.CanWalk);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasSomeoneAlive
        {
            get
            {
                return _fighters.Any(fighter => !fighter.IsFighterDead);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Places
        {
            get
            {
                return string.Join("", _places.Select(cell => Util.CellToChar(cell.Id)));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<FightOptionTypeEnum, bool> _blockedOption;
        private List<FighterBase> _fighters;
        private List<FightCell> _places;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        public FightTeam(int id, long leaderId, int flagCell, FightBase fight, List<FightCell> places)
        {
            Id = id;
            Fight = fight;
            LeaderId = leaderId;
            FlagCellId = flagCell;

            _fighters = new List<FighterBase>();
            _places = places;
            _blockedOption = new Dictionary<FightOptionTypeEnum, bool>()
            {            
                { FightOptionTypeEnum.TYPE_NEW_PLAYER_BIS, false },
                { FightOptionTypeEnum.TYPE_HELP, false },
                { FightOptionTypeEnum.TYPE_PARTY, false },
                { FightOptionTypeEnum.TYPE_SPECTATOR, false },
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void AddFighter(FighterBase fighter)
        {
            _fighters.Add(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void RemoveFighter(FighterBase fighter)
        {
            _fighters.Remove(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighterId"></param>
        public FighterBase GetFighter(long fighterId)
        {
            return _fighters.Find(fighter => fighter.Id == fighterId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <returns></returns>
        public bool CanJoinBeforeStart(FighterBase fighter)
        {
            // monster
            if (LeaderId < 0)
                return false;

            // No more fighter accepted
            if (FreePlace == null || IsOptionLocked(FightOptionTypeEnum.TYPE_NEW_PLAYER_BIS))
            {
                fighter.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_JOIN, fighter.Id, "f"));
                return false;
            }

            // TODO : party
            if (IsOptionLocked(FightOptionTypeEnum.TYPE_PARTY))
            {
                fighter.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_JOIN, fighter.Id, "f"));
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="team"></param>
        /// <param name="type"></param>
        public void OptionLock(FightOptionTypeEnum type)
        {
            AddMessage(() =>
                {
                    if (type == FightOptionTypeEnum.TYPE_NEW_PLAYER)
                        type = FightOptionTypeEnum.TYPE_NEW_PLAYER_BIS;

                    _blockedOption[type] = _blockedOption[type] == false;

                    var value = _blockedOption[type];

                    InformationEnum infoType = InformationEnum.INFO_FIGHT_TOGGLE_PARTY;

                    if (Fight.State == FightStateEnum.STATE_PLACEMENT)
                    {
                        Fight.Map.Dispatch(WorldMessage.FIGHT_OPTION(type, value, LeaderId));
                    }

                    switch (type)
                    {
                        case FightOptionTypeEnum.TYPE_HELP:
                            if (value)
                                infoType = InformationEnum.INFO_FIGHT_TOGGLE_HELP;
                            else
                                infoType = InformationEnum.INFO_FIGHT_UNTOGGLE_HELP;
                            break;

                        case FightOptionTypeEnum.TYPE_NEW_PLAYER_BIS:
                            if (value)
                                infoType = InformationEnum.INFO_FIGHT_TOGGLE_PLAYER;
                            else
                                infoType = InformationEnum.INFO_FIGHT_UNTOGGLE_PLAYER;
                            break;

                        case FightOptionTypeEnum.TYPE_PARTY:
                            if (value)
                                infoType = InformationEnum.INFO_FIGHT_TOGGLE_PARTY;
                            else
                                infoType = InformationEnum.INFO_FIGHT_UNTOGGLE_PARTY;
                            break;

                        case FightOptionTypeEnum.TYPE_SPECTATOR:
                            if (value)
                            {
                                Fight.KickSpectators();
                                base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_FIGHT_TOGGLE_SPECTATOR));
                            }
                            else
                            {
                                base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_FIGHT_UNTOGGLE_SPECTATOR));
                            }
                            return;
                    }

                    base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, infoType));
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void SendMapFightInfos(EntityBase entity)
        {
            entity.Dispatch(WorldMessage.FIGHT_FLAG_UPDATE(OperatorEnum.OPERATOR_ADD, LeaderId, Fighters.ToArray()));
            foreach (var option in _blockedOption)
            {
                entity.Dispatch(WorldMessage.FIGHT_OPTION(option.Key, option.Value, LeaderId));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toggle"></param>
        /// <returns></returns>
        public bool IsOptionLocked(FightOptionTypeEnum toggle)
        {
            return _blockedOption[toggle];
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            Fight = null;
            OpponentTeam = null;

            _places.Clear();
            _places = null;
            _fighters.Clear();
            _fighters = null;
            _blockedOption.Clear();
            _blockedOption = null;

            base.Dispose();
        }
    }
}
