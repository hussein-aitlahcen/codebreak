using Codebreak.Service.World.Database.Structures;
using Codebreak.Service.World.Game.Entity;
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
    public sealed class GuildMember : MessageDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
        private CharacterEntity _characterEntity;
        private CharacterDAO _character;

        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            get
            {
                return _character.Id;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long GuildId
        {
            get
            {
                return _character.GetCharacterGuild().GuildId;
            }
            set
            {
                _character.GetCharacterGuild().GuildId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return _character.Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long XPGiven
        {
            get
            {
                return _character.GetCharacterGuild().XPGiven;
            }
            set
            {
                _character.GetCharacterGuild().XPGiven = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int XPSharePercent
        {
            get
            {
                return _character.GetCharacterGuild().XPSharePercent;
            }
            set
            {
                _character.GetCharacterGuild().XPSharePercent = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Power
        {
            get
            {
                return _character.GetCharacterGuild().Power;
            }
            set
            {
                _character.GetCharacterGuild().Power = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public GuildRankEnum Rank
        {
            get
            {
                return (GuildRankEnum)_character.GetCharacterGuild().Rank;
            }
            set
            {
                _character.GetCharacterGuild().Rank = (int)value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public GuildInstance Guild
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public GuildMember(GuildInstance guild, CharacterDAO character)
        {
            Guild = guild;
            _character = character;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profilName"></param>
        /// <param name="rank"></param>
        /// <param name="percent"></param>
        /// <param name="power"></param>
        public void MemberProfilUpdate(long profilId, int rank, int percent, int power)
        {
            Guild.MemberProfilUpdate(this, profilId, rank, percent, power);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kickedMemberName"></param>
        public void MemberKick(string kickedMemberName)
        {
            Guild.MemberKick(this, kickedMemberName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void CharacterConnected(CharacterEntity character)
        {
            base.AddHandler(character.SafeDispatch);
            _characterEntity = character;
            _characterEntity.SetCharacterGuild(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void MemberKick()
        {
            Guild.MemberKick(this, Name);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetBoss()
        {
            Rank = GuildRankEnum.BOSS;

            foreach(var value in (GuildRightEnum[])Enum.GetValues(typeof(GuildRightEnum)))           
                SetRight(value, true);            
        }

        /// <summary>
        /// 
        /// </summary>
        public void GuildLeave()
        {
            GuildId = -1;

            if (_characterEntity != null)
            {
                _characterEntity.SetCharacterGuild(null);
                _characterEntity.RefreshOnMap();
            }

            CharacterDisconnected();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void CharacterDisconnected()
        {
            if (_characterEntity != null)            
                base.RemoveHandler(_characterEntity.SafeDispatch);            
            _characterEntity = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendGuildStats()
        {
            base.Dispatch(WorldMessage.GUILD_STATS(Guild, Power));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public bool HasRight(GuildRightEnum right)
        {
            return (Power & (int)right) == (int)right;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="right"></param>
        /// <param name="value"></param>
        public void SetRight(GuildRightEnum right, bool value)
        {
            if(value)
                Power = Power | (int)right;
            else
                Power = Power ^ (int)right;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SerializeAs_GuildMemberInformations(StringBuilder message)
        {
            message.Append(_character.Id).Append(";");
            message.Append(_character.Name).Append(";");
            message.Append(_character.Level).Append(";");
            message.Append(_character.Skin).Append(";");
            message.Append((int)Rank).Append(";");
            message.Append(XPGiven).Append(";");
            message.Append(XPSharePercent).Append(";");
            message.Append(Power).Append(";");
            if (_characterEntity != null) // connected ?
                message.Append("1").Append(";");            
            else            
                message.Append("0").Append(";");           
            message.Append(_character.GetCharacterAlignment().AlignmentId).Append(";");
            message.Append("-1").Append('|');
        }
    }
}
