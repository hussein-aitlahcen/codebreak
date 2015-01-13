using Codebreak.Service.World.Game.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight.Challenges;
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
        public List<CharacterEntity> Spectators
        {
            get
            {
                return m_spectators;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<CharacterEntity> m_spectators;

        /// <summary>
        /// 
        /// </summary>
        private FightBase m_fight;

        /// <summary>
        /// 
        /// </summary>
        public bool CanJoin
        {
            get
            {
                return m_fight.State == FightStateEnum.STATE_FIGHTING &&
                    !m_fight.Team0.IsOptionLocked(FightOptionTypeEnum.TYPE_SPECTATOR) &&
                    !m_fight.Team1.IsOptionLocked(FightOptionTypeEnum.TYPE_SPECTATOR);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        public SpectatorTeam(FightBase fight)
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
        public List<FighterBase> Fighters
        {
            get
            {
                return m_fighters;
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
        public FighterBase Leader
        {
            get
            {
                return m_fighters.FirstOrDefault();
            }
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
            get
            {
                if(m_placesCache == null)
                    m_placesCache = string.Concat(m_places.Select(cell => Util.CellToChar(cell.Id)));
                return m_placesCache;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Alignment
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ChallengeBase> SucceededChallenges
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
        private List<FighterBase> m_fighters;
        private List<FightCell> m_places;
        private List<ChallengeBase> m_challenges;
        private string m_placesCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        public FightTeam(int id, long leaderId, int alignment, int flagCell, FightBase fight, List<FightCell> places)
        {
            Id = id;
            Fight = fight;
            LeaderId = leaderId;
            Alignment = alignment;
            FlagCellId = flagCell;

            m_challenges = new List<ChallengeBase>();
            m_fighters = new List<FighterBase>();
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
        public void AddChallenge(ChallengeBase challenge)
        {
            challenge.AddHandler(base.Dispatch);
            m_challenges.Add(challenge);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void AddFighter(FighterBase fighter)
        {
            m_fighters.Add(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void RemoveFighter(FighterBase fighter)
        {
            m_fighters.Remove(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighterId"></param>
        public FighterBase GetFighter(long fighterId)
        {
            return m_fighters.Find(fighter => fighter.Id == fighterId);
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

                case FightTypeEnum.TYPE_AGGRESSION:
                    var leader = (CharacterEntity)GetFighter(LeaderId);
                    return leader.Alignment.AlignmentId == (int)AlignmentTypeEnum.ALIGNMENT_NEUTRAL || character.Alignment.AlignmentId == leader.Alignment.AlignmentId;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendChallengeInfos()
        {
            base.CachedBuffer = true;
            foreach (var challenge in m_challenges)
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
        public void BeginTurn(FighterBase fighter)
        {
            if (fighter.Type == EntityTypeEnum.TYPE_CHARACTER)
                foreach (var challenge in m_challenges)
                    challenge.BeginTurn(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="castInfos"></param>
        public void CheckSpell(FighterBase fighter, CastInfos castInfos)
        {
            if (fighter.Type == EntityTypeEnum.TYPE_CHARACTER)
                foreach (var challenge in m_challenges)
                    challenge.CheckSpell(fighter, castInfos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="movementLength"></param>
        public void CheckMovement(FighterBase fighter, int beginCell, int endCell, int movementLength)
        {
            if(fighter.Type == EntityTypeEnum.TYPE_CHARACTER)
                foreach (var challenge in m_challenges)
                    challenge.CheckMovement(fighter, beginCell, endCell, movementLength);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="weapon"></param>
        public void CheckWeapon(FighterBase fighter, ItemTemplateDAO weapon)
        {
            if (fighter.Type == EntityTypeEnum.TYPE_CHARACTER)
                foreach (var challenge in m_challenges)
                    challenge.CheckWeapon(fighter, weapon);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void CheckDeath(FighterBase fighter)
        {
            if (fighter.Type == EntityTypeEnum.TYPE_CHARACTER)
                foreach (var challenge in m_challenges)
                    challenge.CheckDeath(fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void EndTurn(FighterBase fighter)
        {
            if (fighter.Type == EntityTypeEnum.TYPE_CHARACTER)
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
            
            foreach (var challenge in m_challenges)
                challenge.RemoveHandler(base.Dispatch);

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
