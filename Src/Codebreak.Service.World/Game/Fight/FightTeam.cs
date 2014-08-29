using Codebreak.Service.World.Game.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight.Challenges;
using Codebreak.Service.World.Database.Structures;
using Codebreak.Service.World.Game.Fight.Effect;

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
                switch(Fight.Type)
                {
                        // fight end on taxcollector death
                    case FightTypeEnum.TYPE_PVT:
                        if (_fighters[0].IsFighterDead)
                            return false;
                        break;
                }
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
                if(_placesCache == null)
                    _placesCache = string.Concat(_places.Select(cell => Util.CellToChar(cell.Id)));
                return _placesCache;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<FightOptionTypeEnum, bool> _blockedOption;
        private List<FighterBase> _fighters;
        private List<FightCell> _places;
        private List<ChallengeBase> _challenges;
        private string _placesCache;

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

            _challenges = new List<ChallengeBase>();
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
        /// <param name="challenge"></param>
        public void AddChallenge(ChallengeBase challenge)
        {
            challenge.AddHandler(base.Dispatch);
            _challenges.Add(challenge);
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
        public bool CanJoinBeforeStart(CharacterEntity character)
        {
            if (LeaderId < 0) // cant join taxcollector or monsters
                return false;

            // No more fighter accepted
            if (FreePlace == null || IsOptionLocked(FightOptionTypeEnum.TYPE_NEW_PLAYER_BIS))
            {
                character.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_JOIN, character.Id, "f"));
                return false;
            }
            
            if (IsOptionLocked(FightOptionTypeEnum.TYPE_PARTY) && character.PartyId != ((CharacterEntity)_fighters[0]).PartyId)
            {
                character.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_JOIN, character.Id, "f"));
                return false;
            }

            // no more place
            if(FreePlace == null)
            {
                character.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_JOIN, character.Id, "c")); // FULL
                return false;
            }

            switch(Fight.Type)
            {
                case FightTypeEnum.TYPE_PVT:
                    var taxCollector = OpponentTeam.Fighters[0] as TaxCollectorEntity;
                    if(character.CharacterGuild != null && character.CharacterGuild.GuildId == taxCollector.Guild.Id)
                    {
                        character.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("You can't take part in a fight against your own TaxCollector."));
                        return false;
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendChallengeInfos()
        {
            base.CachedBuffer = true;
            foreach (var challenge in _challenges)
                base.Dispatch(WorldMessage.FIGHT_CHALLENGE_INFORMATIONS(challenge.Id,
                    challenge.ShowTarget,
                    challenge.TargetId,
                    challenge.BasicXpBonus,
                    challenge.TeamXpBonus,
                    challenge.BasicDropBonus,
                    challenge.TeamDropBonus,
                    challenge.Success));
            base.CachedBuffer = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void SendChallengeInfos(FighterBase fighter)
        {
            foreach (var challenge in _challenges)
                fighter.Dispatch(WorldMessage.FIGHT_CHALLENGE_INFORMATIONS(challenge.Id,
                    challenge.ShowTarget,
                    challenge.TargetId,
                    challenge.BasicXpBonus,
                    challenge.TeamXpBonus,
                    challenge.BasicDropBonus,
                    challenge.TeamDropBonus,
                    challenge.Success));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void BeginTurn(FighterBase fighter)
        {
            foreach (var challenge in _challenges)
                challenge.BeginTurn(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="castInfos"></param>
        public void CheckSpell(FighterBase fighter, CastInfos castInfos)
        {
            foreach (var challenge in _challenges)
                challenge.CheckSpell(fighter, castInfos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="movementLength"></param>
        public void CheckMovement(int beginCell, int endCell, int movementLength)
        {
            foreach (var challenge in _challenges)
                challenge.CheckMovement(beginCell, endCell, movementLength);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="weapon"></param>
        public void CheckWeapon(FighterBase fighter, ItemTemplateDAO weapon)
        {
            foreach (var challenge in _challenges)
                challenge.CheckWeapon(fighter, weapon);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void CheckDeath(FighterBase fighter)
        {
            foreach (var challenge in _challenges)
                challenge.CheckDeath(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void EndTurn(FighterBase fighter)
        {
            foreach (var challenge in _challenges)
                challenge.EndTurn(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        public void FightEnd()
        {
            foreach (var challenge in _challenges)
                if (!challenge.Success && !challenge.Failed)
                    challenge.OnSuccess();
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
                entity.Dispatch(WorldMessage.FIGHT_OPTION(option.Key, option.Value, LeaderId));
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
            
            foreach (var challenge in _challenges)
                challenge.RemoveHandler(base.Dispatch);

            _challenges.Clear();
            _challenges = null;

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
