using Codebreak.Service.World.Database.Structure;
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
        static GuildRightEnum[] RIGHTS = (GuildRightEnum[])Enum.GetValues(typeof(GuildRightEnum));

        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            get
            {
                return m_character.Id;
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
                return m_character.Guild.GuildId;
            }
            set
            {
                m_character.Guild.GuildId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return m_character.Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long XPGiven
        {
            get
            {
                return m_character.Guild.XPGiven;
            }
            set
            {
                m_character.Guild.XPGiven = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int XPSharePercent
        {
            get
            {
                return m_character.Guild.XPSharePercent;
            }
            set
            {
                m_character.Guild.XPSharePercent = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Power
        {
            get
            {
                return m_character.Guild.Power;
            }
            set
            {
                m_character.Guild.Power = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public GuildRankEnum Rank
        {
            get
            {
                return (GuildRankEnum)m_character.Guild.Rank;
            }
            set
            {
                m_character.Guild.Rank = (int)value;
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
        private CharacterDAO m_character;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public GuildMember(GuildInstance guild, CharacterDAO character)
        {
            TaxCollectorJoinedId = -1;
            Guild = guild;
            m_character = character;
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
            Guild.AddMessage(() =>
                {
                    Guild.MemberProfilUpdate(this, profilId, rank, percent, power);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kickedMemberName"></param>
        public void MemberKick(string kickedMemberName)
        {
            Guild.AddMessage(() =>
                {
                    Guild.MemberKick(this, kickedMemberName);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        public void HireTaxCollector()
        {
            Guild.AddMessage(() =>
                {
                    Guild.HireTaxCollector(this);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statId"></param>
        public void BoostGuildStats(char statId)
        {
            Guild.AddMessage(() =>
                {
                    Guild.BoostStats(this, statId);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spellId"></param>
        public void BoostGuildSpell(int spellId)
        {
            Guild.AddMessage(() =>
                {
                    Guild.BoostSpell(this, spellId);
                });
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
            Guild.AddMessage(() =>
                {
                    Guild.MemberKick(this, Name);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetBoss()
        {
            Rank = GuildRankEnum.BOSS;

            foreach(var right in RIGHTS)           
                SetRight(right, true);            
        }

        /// <summary>
        /// 
        /// </summary>
        public void GuildLeave()
        {
            GuildId = -1;
            XPGiven = 0;
            XPSharePercent = 0;
            Power = 0;

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
                base.RemoveHandler(Character.SafeDispatch);            
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
            Guild.AddMessage(() =>
                {
                    Guild.SendMembersInformations(this);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendBoostInformations()
        {
            Guild.AddMessage(() =>
                {
                    Guild.SendBoostInformations(this);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        public void TaxCollectorsInterfaceJoin()
        {
            Guild.AddMessage(() =>
                {
                    Guild.AddTaxCollectorListener(this);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        public void TaxCollectorsInterfaceLeave()
        {
            Guild.AddMessage(() =>
                {
                    Guild.RemoveTaxCollectorListener(this);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollector"></param>
        public void RemoveTaxCollector(TaxCollectorEntity taxCollector)
        {
            Guild.AddMessage(() =>
                {
                    Guild.RemoveTaxCollector(this, taxCollector);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollector"></param>
        public void FarmTaxCollector(TaxCollectorEntity taxCollector)
        {
            Guild.AddMessage(() =>
                {
                    Guild.FarmTaxCollector(this, taxCollector);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendTaxCollectorsList()
        {
            Guild.AddMessage(() =>
                {
                    Guild.SendTaxCollectorsList(this);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendGeneralInformations()
        {
            Guild.AddMessage(() =>
                {
                    Guild.SendGeneralInformations(this);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void TaxCollectorJoin(long id)
        {
            Guild.AddMessage(() =>
                {
                    Guild.TaxCollectorJoin(this, id);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        public void TaxCollectorLeave()
        {
            Guild.AddMessage(() =>
                {
                    Guild.TaxCollectorLeave(this);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SerializeAs_GuildMemberInformations(StringBuilder message)
        {
            message.Append(m_character.Id).Append(";");
            message.Append(m_character.Name).Append(";");
            message.Append(m_character.Level).Append(";");
            message.Append(m_character.Skin).Append(";");
            message.Append((int)Rank).Append(";");
            message.Append(XPGiven).Append(";");
            message.Append(XPSharePercent).Append(";");
            message.Append(Power).Append(";");
            if (Character != null)
                message.Append("1").Append(";");            
            else            
                message.Append("0").Append(";");           
            message.Append(m_character.Alignment.AlignmentId).Append(";");
            message.Append("-1").Append('|');
        }

        /// <summary>
        ///    
        /// </summary>
        /// <param name="message"></param>
        public void SerializeAs_TaxCollectorDefender(StringBuilder message)
        {
            message.Append(Util.EncodeBase36(m_character.Id)).Append(';');
            message.Append(m_character.Name).Append(';');
            message.Append(m_character.Skin).Append(';');
            message.Append(m_character.Level).Append(';');
            message.Append(m_character.HexColor1).Append(';');
            message.Append(m_character.HexColor2).Append(';');
            message.Append(m_character.HexColor3);
        }
    }
}
