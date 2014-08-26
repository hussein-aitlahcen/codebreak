using Codebreak.Service.World.Database.Structures;
using Codebreak.Service.World.Game.Database.Repositories;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Guild
{
    /// <summary>
    /// 
    /// </summary>
    public enum GuildRightEnum
    {
        BOSS = 1,
        MANAGE_BOOST = 2,
        MANAGE_POWER = 4,
        INVITE = 8,
        BAN = 16,
        MANAGE_EXP_PERCENT = 32,
        MANAGE_RANK = 64,
        HIRE_TAXCOLLECTOR = 128,
        MANAGE_OWN_EXP_PERCENT = 256,
        COLLECT_TAXCOLLECTOR = 512,
        USE_MOUNTPARK = 4096,
        ARRANGE_MOUNTPARK = 8192,
        MANAGE_OTHERS_MOUNT = 16384,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum GuildRankEnum
    {
        BOSS = 1,
        SECOND_IN_COMMAND = 2,
        TREASURER = 3,
        PROTECTOR = 4,
        CRAFTSMAN  = 5,
        RESERVIST = 6,
        DOGSBODY = 7,
        GUARD = 8,
        SCOUT = 9,
        SPY = 10,
        DIPLOMAT= 11,
        SECRETARY = 12, 
        PET_KILLER = 33,
        TRAITOR = 21,
        POACHER = 31,
        TREASURE_HUNTER = 30,
        THIEF = 29,
        INITIATE = 28, 
        MURDERER = 27, 
        GOVERNOR = 26,
        MUSE = 25,
        COUNSELLOR = 24,
        CHOSEN_ONE = 23,
        GUIDE = 21,
        MENTOR = 22,
        RECRUITING_OFFICER = 20,
        BREEDER = 19,
        MERCHANT = 18,
        APPRENTICE = 17,
        ON_TRIAL = 0,
        TORTUER = 16,
        DESERTER = 15,
        NUISANCE= 14,
        PENITENT = 13,
        MASCOT = 34,
        PERCEPTOR_KILLER = 35,
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GuildInstance : MessageDispatcher
    {

        /// <summary>
        /// 
        /// </summary>
        public volatile bool IsActive;

        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            get
            {
                return _record.Id;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public GuildStatistics Statistics
        {
            get
            {
                return _record.GetStatistics();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return _record.Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SymbolId
        {
            get
            {
                return _record.SymbolId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SymbolColor
        {
            get
            {
                return _record.SymbolColor;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int BackgroundId
        {
            get
            {
                return _record.BackgroundId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int BackgroundColor
        {
            get
            {
                return _record.BackgroundColor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Emblem
        {
            get
            {
                if(_emblem == null)
                    _emblem = Util.EncodeBase36(BackgroundId) + "|" + Util.EncodeBase36(BackgroundColor) + "|" + Util.EncodeBase36(SymbolId) + "|" + Util.EncodeBase36(SymbolColor);
                return _emblem;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayEmblem
        {
            get
            {
                if(_displayEmblem == null)
                    _displayEmblem = Util.EncodeBase36(BackgroundId) + "," + Util.EncodeBase36(BackgroundColor) + "," + Util.EncodeBase36(SymbolId) + "," + Util.EncodeBase36(SymbolColor);
                return _displayEmblem;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Level
        {
            get
            {
                return _record.Level;
            }
            set
            {
                _record.Level = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int BoostPoint
        {
            get
            {
                return _record.BoostPoint;
            }
            set
            {
                _record.BoostPoint = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TaxCollectorPrice
        {
            get
            {
                return 1000 + (Level * 100);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _emblem, _displayEmblem;
        private List<GuildMember> _members;
        private GuildDAO _record;

        /// <summary>
        /// 
        /// </summary>
        public GuildInstance(GuildDAO record)
        {
            _record = record;
            _members = new List<GuildMember>();
            foreach (var character in CharacterRepository.Instance.FindAll(ch => ch.GetCharacterGuild().GuildId == _record.Id))
            {
                AddMember(new GuildMember(this, character));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void MemberJoin(CharacterEntity character)
        {
            var member = new GuildMember(this, character.DatabaseRecord);
            member.GuildId = Id;
            member.Rank = GuildRankEnum.ON_TRIAL; // a l'essai
            member.Power = 0;
            member.CharacterConnected(character);
            member.SendGuildStats();
            character.RefreshOnMap();
            AddMember(member);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void MemberBoss(CharacterEntity character)
        {
            var member = new GuildMember(this, character.DatabaseRecord);
            member.GuildId = Id;
            member.SetBoss();
            member.CharacterConnected(character);
            member.SendGuildStats();
            character.RefreshOnMap();
            AddMember(member);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="profilName"></param>
        /// <param name="rank"></param>
        /// <param name="percent"></param>
        /// <param name="power"></param>
        public void MemberProfilUpdate(GuildMember member, long profilId, int rank, int percent, int power)
        {
            var himSelf = member.Id == profilId;
            var memberProfil = GetMember(profilId);
            if(memberProfil == null)
            {
                member.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var rankChanged = rank == (int)memberProfil.Rank;
            var powerChanged = power == memberProfil.Power;
            var xpShareChanged = percent == memberProfil.XPSharePercent;

            var canManageOwnExp = member.HasRight(GuildRightEnum.MANAGE_OWN_EXP_PERCENT);
            var canManageOthersExp = member.HasRight(GuildRightEnum.MANAGE_EXP_PERCENT);
            var canManageRank = member.HasRight(GuildRightEnum.MANAGE_RANK);
            var canManagePower = member.HasRight(GuildRightEnum.MANAGE_POWER);

            if(!canManageOwnExp && !canManageOthersExp && !canManageRank && !canManagePower)
            {
                member.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_GUILD_NOT_ENOUGH_RIGHTS));
                return;
            }

            if(!himSelf && !canManageOthersExp && xpShareChanged)
            {
                member.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_GUILD_NOT_ENOUGH_RIGHTS));
                return;
            }

            if(!canManagePower && powerChanged)
            {
                member.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_GUILD_NOT_ENOUGH_RIGHTS));
                return;
            }

            if(!canManageRank && rankChanged)
            {
                member.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_GUILD_NOT_ENOUGH_RIGHTS));
                return;
            }

            if(!canManageOwnExp && himSelf && xpShareChanged)
            {
                member.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_GUILD_NOT_ENOUGH_RIGHTS));
                return;
            }

            memberProfil.XPSharePercent = percent;
            memberProfil.Rank = (GuildRankEnum)rank;
            memberProfil.Power = power;

            // update profil
            member.Dispatch(WorldMessage.GUILD_MEMBERS_INFORMATIONS(memberProfil));
            memberProfil.Dispatch(WorldMessage.GUILD_STATS(this, power));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="kickedMember"></param>
        public void MemberKick(GuildMember member, string kickedMemberName)
        {
            if (kickedMemberName != member.Name && !member.HasRight(GuildRightEnum.BAN))
            {
                member.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_GUILD_NOT_ENOUGH_RIGHTS));
                return;
            }

            var kickedMember = _members.Find(m => m.Name == kickedMemberName);
            if(kickedMember == null)
            {
                member.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if(kickedMember.Rank == GuildRankEnum.BOSS)
            {
                if (kickedMemberName == member.Name)
                {
                    member.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("As a boss, you are unable to leave the guild."));
                    return;   
                }

                member.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("The boss cannot be kicked."));
                return;    
            }

            member.Dispatch(WorldMessage.GUIL_KICK_SUCCESS(member.Name, kickedMemberName));

            if (member.Name != kickedMemberName)
                kickedMember.Dispatch(WorldMessage.GUIL_KICK_SUCCESS(member.Name, kickedMemberName));

            RemoveMember(kickedMember);

            kickedMember.GuildLeave();

            base.Dispatch(WorldMessage.GUILD_MEMBER_REMOVE(kickedMember.Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void AddMember(GuildMember member)
        {
            _members.Add(member);
            base.AddHandler(member.Dispatch);
            
            IsActive = _members.Count > 10;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        public void RemoveMember(GuildMember member)
        {
            _members.Remove(member);
            base.RemoveHandler(member.Dispatch);
            
            IsActive = _members.Count > 10;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GuildMember GetMember(long id)
        {
            return _members.Find(member => member.Id == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SafeDispatchChatMessage(long memberId, string memberName, string message)
        {
            base.SafeDispatch(WorldMessage.CHAT_MESSAGE(ChatChannelEnum.CHANNEL_GUILD, memberId, memberName, message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void SendMembersInformations(CharacterEntity character)
        {
            character.SafeDispatch(WorldMessage.GUILD_MEMBERS_INFORMATIONS(_members));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void SendBoostInformations(CharacterEntity character)
        {
            character.SafeDispatch(WorldMessage.GUILD_BOOST_INFORMATIONS(BoostPoint, Statistics));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void SendGeneralInformations(CharacterEntity character)
        {
            // TODO : REPLACE TRUE BY ISACTIVE
            character.SafeDispatch(WorldMessage.GUILD_GENERAL_INFORMATIONS(true, Level, 0, 0, 0));   
        }
    }
}
