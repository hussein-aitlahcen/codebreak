using Codebreak.Service.World.Database.Structures;
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
    public sealed class GuildMember : MessageDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
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
        public long TaxCollectorJoinedId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity Character
        {
            get;
            private set;
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
            TaxCollectorJoinedId = -1;
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
        public void HireTaxCollector()
        {
            Guild.HireTaxCollector(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statId"></param>
        public void BoostGuildStats(char statId)
        {
            Guild.BoostStats(this, statId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spellId"></param>
        public void BoostGuildSpell(int spellId)
        {
            Guild.BoostSpell(this, spellId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void CharacterConnected(CharacterEntity character)
        {
            base.AddHandler(character.SafeDispatch);
            Character = character;
            Character.SetCharacterGuild(this);
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

            if (Character != null)
            {
                Character.SetCharacterGuild(null);
                Character.RefreshOnMap();
            }

            CharacterDisconnected();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void CharacterDisconnected()
        {
            if (Character != null)
            {
                base.RemoveHandler(Character.SafeDispatch);
            }
            Character = null;
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
        public void SendHasNotEnoughRights()
        {
            base.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_GUILD_NOT_ENOUGH_RIGHTS));
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendMembersInformations()
        {
            Guild.SendMembersInformations(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendBoostInformations()
        {
            Guild.SendBoostInformations(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void TaxCollectorsInterfaceJoin()
        {
            Guild.AddTaxCollectorListener(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void TaxCollectorsInterfaceLeave()
        {
            Guild.RemoveTaxCollectorListener(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollector"></param>
        public void RemoveTaxCollector(TaxCollectorEntity taxCollector)
        {
            Guild.RemoveTaxCollector(this, taxCollector);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendTaxCollectorsList()
        {
            Guild.SendTaxCollectorsList(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendGeneralInformations()
        {
            Guild.SendGeneralInformations(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void TaxCollectorJoin(long id)
        {
            Guild.TaxCollectorJoin(this, id);
        }

        /// <summary>
        /// 
        /// </summary>
        public void TaxCollectorLeave()
        {
            Guild.TaxCollectorLeave(this);
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
            if (Character != null) // connected ?
                message.Append("1").Append(";");            
            else            
                message.Append("0").Append(";");           
            message.Append(_character.GetCharacterAlignment().AlignmentId).Append(";");
            message.Append("-1").Append('|');
        }

        /// <summary>
        ///    
        /// </summary>
        /// <param name="message"></param>
        public void SerializeAs_TaxCollectorDefender(StringBuilder message)
        {
            message.Append(Util.EncodeBase36(_character.Id)).Append(';');
            message.Append(_character.Name).Append(';');
            message.Append(_character.Skin).Append(';');
            message.Append(_character.Level).Append(';');
            message.Append(_character.GetHexColor1()).Append(';');
            message.Append(_character.GetHexColor2()).Append(';');
            message.Append(_character.GetHexColor3());
        }
    }
}
