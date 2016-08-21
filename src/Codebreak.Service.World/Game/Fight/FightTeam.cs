using Codebreak.Service.World.Game.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight.Challenge;
using Codebreak.Service.World.Database.Structure;
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
        public List<CharacterEntity> Spectators => m_spectators;

        /// <summary>
        /// 
        /// </summary>
        private List<CharacterEntity> m_spectators;

        /// <summary>
        /// 
        /// </summary>
        private AbstractFight m_fight;

        /// <summary>
        /// 
        /// </summary>
        public bool CanJoin => m_fight.State == FightStateEnum.STATE_FIGHTING &&
                               !m_fight.Team0.IsOptionLocked(FightOptionTypeEnum.TYPE_SPECTATOR) &&
                               !m_fight.Team1.IsOptionLocked(FightOptionTypeEnum.TYPE_SPECTATOR);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        public SpectatorTeam(AbstractFight fight)
        {
            m_fight = fight;
            m_spectators = new List<CharacterEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void AddSpectator(CharacterEntity fighter)
        {
            m_spectators.Add(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void RemoveSpectator(CharacterEntity fighter)
        {
            m_spectators.Remove(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            m_fight = null;
            m_spectators.Clear();
            m_spectators = null;

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
        public List<AbstractFighter> Fighters => m_fighters;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<AbstractFighter> AliveFighters => Fighters.Where(fighter => !fighter.IsFighterDead);

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
        public AbstractFight Fight
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public AbstractFighter Leader => m_fighters.FirstOrDefault();

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
                return m_places.Find(cell => cell.CanWalk);
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
                        if (m_fighters[0].IsFighterDead)
                            return false;
                        break;
                }
                return m_fighters.Any(fighter => !fighter.IsFighterDead);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Places
        {
            get {
                return m_placesCache ??
                       (m_placesCache = string.Concat(m_places.Select(cell => Util.CellToChar(cell.Id))));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AlignmentId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<AbstractChallenge> SucceededChallenges
        {
            get
            {
                return m_challenges.Where(challenge => challenge.Success);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<FightOptionTypeEnum, bool> m_blockedOption;
        private List<AbstractFighter> m_fighters;
        private List<FightCell> m_places;
        private List<AbstractChallenge> m_challenges;
        private string m_placesCache;

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="leaderId"></param>
        /// <param name="alignment"></param>
        /// <param name="flagCell"></param>
        /// <param name="fight"></param>
        /// <param name="places"></param>
        public FightTeam(int id, long leaderId, int alignment, int flagCell, AbstractFight fight, List<FightCell> places)
        {
            Id = id;
            Fight = fight;
            LeaderId = leaderId;
            AlignmentId = alignment;
            FlagCellId = flagCell;
            
            m_challenges = new List<AbstractChallenge>();
            m_fighters = new List<AbstractFighter>();
            m_places = places;
            m_blockedOption = new Dictionary<FightOptionTypeEnum, bool>()
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
        public void AddChallenge(AbstractChallenge challenge)
        {
            challenge.AddHandler(base.Dispatch);
            m_challenges.Add(challenge);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void AddFighter(AbstractFighter fighter)
        {
            m_fighters.Add(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void RemoveFighter(AbstractFighter fighter)
        {
            m_fighters.Remove(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighterId"></param>
        public AbstractFighter GetFighter(long fighterId)
        {
            return m_fighters.Find(fighter => fighter.Id == fighterId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool CanJoinBeforeStart(CharacterEntity character)
        {
            if (LeaderId < 0 && AlignmentId == -1) // cant join taxcollector or monsters
                return false;

            // No more fighter accepted
            if (FreePlace == null || IsOptionLocked(FightOptionTypeEnum.TYPE_NEW_PLAYER_BIS))
            {
                character.Dispatch(WorldMessage.GAME_ACTION(GameActionTypeEnum.FIGHT_JOIN, character.Id, "f"));
                return false;
            }
            
            if (IsOptionLocked(FightOptionTypeEnum.TYPE_PARTY) && character.PartyId != ((CharacterEntity)m_fighters[0]).PartyId)
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
                    if (taxCollector == null)
                        return false;

                    if(character.GuildMember != null && character.GuildMember.GuildId == taxCollector.Guild.Id)
                    {
                        character.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("You can't take part in a fight against your own TaxCollector."));
                        return false;
                    }
                    break;

                case FightTypeEnum.TYPE_PVM:
                case FightTypeEnum.TYPE_AGGRESSION:
                    return AlignmentId <= (int)AlignmentTypeEnum.ALIGNMENT_NEUTRAL || character.AlignmentId == AlignmentId;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendChallengeInfos()
        {
            foreach (var challenge in m_challenges)
            {
                challenge.StartFight(this);
                Dispatch(WorldMessage.FIGHT_CHALLENGE_INFORMATIONS(challenge.Id,
                    challenge.ShowTarget,
                    challenge.TargetId,
                    challenge.BasicXpBonus,
                    challenge.TeamXpBonus,
                    challenge.BasicDropBonus,
                    challenge.TeamDropBonus,
                    challenge.Success));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void SendChallengeInfos(AbstractFighter fighter)
        {
            foreach (var challenge in m_challenges)
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
        public void BeginTurn(AbstractFighter fighter)
        {
                foreach (var challenge in m_challenges)
                    challenge.BeginTurn(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="castInfos"></param>
        public void CheckSpell(AbstractFighter fighter, CastInfos castInfos)
        {
                foreach (var challenge in m_challenges)
                    challenge.CheckSpell(fighter, castInfos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="beginCell"></param>
        /// <param name="endCell"></param>
        /// <param name="movementLength"></param>
        public void CheckMovement(AbstractFighter fighter, int beginCell, int endCell, int movementLength)
        {
                foreach (var challenge in m_challenges)
                    challenge.CheckMovement(fighter, beginCell, endCell, movementLength);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="weapon"></param>
        public void CheckWeapon(AbstractFighter fighter, ItemTemplateDAO weapon)
        {
                foreach (var challenge in m_challenges)
                    challenge.CheckWeapon(fighter, weapon);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void CheckDeath(AbstractFighter fighter)
        {
                foreach (var challenge in m_challenges)
                    challenge.CheckDeath(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void EndTurn(AbstractFighter fighter)
        {
                foreach (var challenge in m_challenges)
                    challenge.EndTurn(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        public void FightEnd()
        {
            foreach (var challenge in m_challenges)
                if (!challenge.Success && !challenge.Failed && HasSomeoneAlive)
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

                    m_blockedOption[type] = m_blockedOption[type] == false;

                    var value = m_blockedOption[type];

                    var infoType = InformationEnum.INFO_FIGHT_TOGGLE_PARTY;

                    if (Fight.State == FightStateEnum.STATE_PLACEMENT)
                    {
                        Fight.Map.Dispatch(WorldMessage.FIGHT_OPTION(type, value, LeaderId));
                    }

                    switch (type)
                    {
                        case FightOptionTypeEnum.TYPE_HELP:
                            infoType = value ? InformationEnum.INFO_FIGHT_TOGGLE_HELP : InformationEnum.INFO_FIGHT_UNTOGGLE_HELP;
                            break;

                        case FightOptionTypeEnum.TYPE_NEW_PLAYER_BIS:
                            infoType = value ? InformationEnum.INFO_FIGHT_TOGGLE_PLAYER : InformationEnum.INFO_FIGHT_UNTOGGLE_PLAYER;
                            break;

                        case FightOptionTypeEnum.TYPE_PARTY:
                            infoType = value ? InformationEnum.INFO_FIGHT_TOGGLE_PARTY : InformationEnum.INFO_FIGHT_UNTOGGLE_PARTY;
                            break;

                        case FightOptionTypeEnum.TYPE_SPECTATOR:
                            if (value)
                            {
                                Fight.KickSpectators();
                                Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_FIGHT_TOGGLE_SPECTATOR));
                            }
                            else
                            {
                                Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_FIGHT_UNTOGGLE_SPECTATOR));
                            }
                            return;
                    }

                    Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, infoType));
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void SendMapFightInfos(AbstractEntity entity)
        {
            entity.Dispatch(WorldMessage.FIGHT_FLAG_UPDATE(OperatorEnum.OPERATOR_ADD, LeaderId, Fighters.ToArray()));
            foreach (var option in m_blockedOption)            
                entity.Dispatch(WorldMessage.FIGHT_OPTION(option.Key, option.Value, LeaderId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toggle"></param>
        /// <returns></returns>
        public bool IsOptionLocked(FightOptionTypeEnum toggle)
        {
            return m_blockedOption[toggle];
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            Fight = null;
            OpponentTeam = null;
                        
            m_challenges.ForEach(challenge => challenge.Dispose());
            m_challenges.Clear();
            m_challenges = null;

            m_places.Clear();
            m_places = null;

            m_fighters.Clear();
            m_fighters = null;

            m_blockedOption.Clear();
            m_blockedOption = null;

            base.Dispose();
        }
    }
}
