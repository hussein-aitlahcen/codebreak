using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Manager;
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
                return m_record.Id;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public GuildStatistics Statistics
        {
            get
            {
                return m_record.Statistics;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return m_record.Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SymbolId
        {
            get
            {
                return m_record.SymbolId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SymbolColor
        {
            get
            {
                return m_record.SymbolColor;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int BackgroundId
        {
            get
            {
                return m_record.BackgroundId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int BackgroundColor
        {
            get
            {
                return m_record.BackgroundColor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Emblem
        {
            get
            {
                if(m_emblem == null)
                    m_emblem = Util.EncodeBase36(BackgroundId) + "|" + Util.EncodeBase36(BackgroundColor) + "|" + Util.EncodeBase36(SymbolId) + "|" + Util.EncodeBase36(SymbolColor);
                return m_emblem;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayEmblem
        {
            get
            {
                if(m_displayEmblem == null)
                    m_displayEmblem = Util.EncodeBase36(BackgroundId) + "," + Util.EncodeBase36(BackgroundColor) + "," + Util.EncodeBase36(SymbolId) + "," + Util.EncodeBase36(SymbolColor);
                return m_displayEmblem;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Level
        {
            get
            {
                return m_record.Level;
            }
            set
            {
                m_record.Level = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int BoostPoint
        {
            get
            {
                return m_record.BoostPoint;
            }
            set
            {
                m_record.BoostPoint = value;
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
        private string m_emblem, m_displayEmblem;
        private List<GuildMember> m_members;
        private List<TaxCollectorEntity> m_taxCollectors;
        private GuildDAO m_record;
        private MessageDispatcher m_taxCollectorDispatcher;

        /// <summary>
        /// 
        /// </summary>
        public GuildInstance(GuildDAO record)
        {
            m_record = record;
            m_members = new List<GuildMember>();
            m_taxCollectors = new List<TaxCollectorEntity>();
            m_taxCollectorDispatcher = new MessageDispatcher();
            foreach (var character in CharacterRepository.Instance.FindAll(ch => ch.Guild.GuildId == m_record.Id))
            {
                AddMember(new GuildMember(this, character));
            }
            foreach(var taxCollectorDAO in TaxCollectorRepository.Instance.FindAll(taxC => taxC.GuildId == m_record.Id))
            {
                AddTaxCollector(EntityManager.Instance.CreateTaxCollector(this, taxCollectorDAO));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollector"></param>
        public void RemoveTaxCollector(GuildMember member, TaxCollectorEntity taxCollector)
        {
            if (taxCollector.Guild != this)
            {
                member.SendHasNotEnoughRights();
                return;
            }

            if (!member.HasRight(GuildRightEnum.COLLECT_TAXCOLLECTOR))
            {
                member.SendHasNotEnoughRights();
                return;
            }

            taxCollector.AddMessage(() =>
                {
                    if (!taxCollector.HasGameAction(GameActionTypeEnum.MAP))
                    {
                        member.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    taxCollector.StopAction(GameActionTypeEnum.MAP);

                    AddMessage(() =>
                        {
                            RemoveTaxCollector(taxCollector);

                            SafeDispatch(WorldMessage.GUILD_TAXCOLLECTOR_REMOVED(taxCollector, member.Name));
                        });
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollector"></param>
        public void RemoveTaxCollector(TaxCollectorEntity taxCollector)
        {
            TaxCollectorRepository.Instance.Remove(taxCollector.DatabaseRecord);
            EntityManager.Instance.RemoveTaxCollector(taxCollector);
            m_taxCollectors.Remove(taxCollector);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollector"></param>
        public void FarmTaxCollector(GuildMember member, TaxCollectorEntity taxCollector)
        {
            if (taxCollector.Guild != this)
            {
                member.SendHasNotEnoughRights();
                return;
            }

            if (!member.HasRight(GuildRightEnum.COLLECT_TAXCOLLECTOR))
            {
                member.SendHasNotEnoughRights();
                return;
            }

            taxCollector.AddMessage(() =>
            {
                if (!taxCollector.HasGameAction(GameActionTypeEnum.MAP))
                {
                    member.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                taxCollector.StopAction(GameActionTypeEnum.MAP);

                AddMessage(() =>
                {
                    RemoveTaxCollector(taxCollector);

                    SafeDispatch(WorldMessage.GUILD_TAXCOLLECTOR_FARMED(taxCollector, member.Name));
                });
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollector"></param>
        public void AddTaxCollector(TaxCollectorEntity taxCollector)
        {
            m_taxCollectors.Add(taxCollector);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        public void AddTaxCollectorListener(GuildMember member)
        {
            foreach (var taxCollector in m_taxCollectors)
            {
                taxCollector.AddMessage(() =>
                {
                    if (taxCollector.HasGameAction(Action.GameActionTypeEnum.FIGHT))
                    {
                        var fight = taxCollector.Fight as TaxCollectorFight;
                        if (fight.State == FightStateEnum.STATE_PLACEMENT)
                        {
                            member.Dispatch(WorldMessage.GUILD_TAXCOLLECTOR_ATTACKER_JOIN(taxCollector.Id, fight.Team0.Fighters.ToArray()));
                            if (taxCollector.Defenders.Count > 0)
                                member.Dispatch(WorldMessage.GUILD_TAXCOLLECTOR_DEFENDER_JOIN(taxCollector.Id, taxCollector.Defenders.ToArray()));
                        }
                    }
                });
            }

            m_taxCollectorDispatcher.AddHandler(member.Dispatch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        public void RemoveTaxCollectorListener(GuildMember member)
        {
            m_taxCollectorDispatcher.RemoveHandler(member.Dispatch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attacker"></param>
        public void TaxCollectorAttackerJoin(long taxCollectorId, FighterBase attacker)
        {
            m_taxCollectorDispatcher.Dispatch(WorldMessage.GUILD_TAXCOLLECTOR_ATTACKER_JOIN(taxCollectorId, attacker));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollectorId"></param>
        /// <param name="attacker"></param>
        public void TaxColectorAttackerLeave(long taxCollectorId, FighterBase attacker)
        {
            m_taxCollectorDispatcher.Dispatch(WorldMessage.GUILD_TAXCOLLECTOR_ATTACKER_LEAVE(taxCollectorId, attacker.Id));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="taxColectorId"></param>
        public void TaxCollectorJoin(GuildMember member, long taxCollectorId)
        {
            var collector = m_taxCollectors.Find(taxCollector => taxCollector.Id == taxCollectorId);
            if (collector == null)
            {
                member.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var character = member.Character;
            if (character == null)
            {
                return;
            }

            character.AddMessage(() =>
            {
                if (!character.CanGameAction(GameActionTypeEnum.TAXCOLLECTOR_AGGRESSION))
                {
                    character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_YOU_ARE_AWAY));
                    return;
                }

                character.DefendTaxCollector();

                collector.AddMessage(() =>
                    {
                        if (!collector.CanDefend)
                        {
                            character.AddMessage(() => character.StopAction(GameActionTypeEnum.TAXCOLLECTOR_AGGRESSION));
                            return;
                        }

                        collector.DefenderJoin(member);

                        // switch back to guild context
                        AddMessage(() =>
                        {
                            member.TaxCollectorJoinedId = taxCollectorId;

                            m_taxCollectorDispatcher.Dispatch(WorldMessage.GUILD_TAXCOLLECTOR_DEFENDER_JOIN(taxCollectorId, member));
                        });
                    });
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="taxCollectorId"></param>
        public void TaxCollectorLeave(GuildMember member)
        {
            if (member.TaxCollectorJoinedId == -1)
            {
                member.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var collector = m_taxCollectors.Find(taxCollector => taxCollector.Id == member.TaxCollectorJoinedId);
            if (collector == null)
            {
                member.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            member.TaxCollectorJoinedId = -1;

            collector.AddMessage(() => collector.DefenderLeft(member));

            m_taxCollectorDispatcher.Dispatch(WorldMessage.GUILD_TAXCOLLECTOR_DEFENDER_LEAVE(collector.Id, member.Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        public void HireTaxCollector(GuildMember member)
        {
            if (!member.HasRight(GuildRightEnum.HIRE_TAXCOLLECTOR))
            {
                member.SendHasNotEnoughRights();
                return;
            }

            if (m_taxCollectors.Count >= Statistics.MaxTaxcollector)
            {
                member.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Your guild has already hired the maximum TaxCollector."));
                return;
            }

            foreach (var collector in m_taxCollectors)
            {
                if (collector.Map.SubAreaId == member.Character.Map.SubAreaId)
                {
                    member.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_MAX_TAXCOLLECTOR_BY_SUBAREA_REACHED, 1)); // MAX COLLECTOR BY SUBAREA
                    return;
                }
            }

            if (member.Character == null)
            {
                return;
            }

            member.Character.AddMessage(() =>
                {
                    if (member.Character.Inventory.Kamas < TaxCollectorPrice)
                    {
                        member.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_NOT_ENOUGH_KAMAS, TaxCollectorPrice));
                        return;
                    }
                    
                    AddMessage(() =>
                        {
                            var taxCollectorDAO = new TaxCollectorDAO()
                            {
                                GuildId = Id,
                                OwnerId = member.Id,
                                Name = Util.Next(WorldConfig.TAXCOLLECTOR_MIN_NAME, WorldConfig.TAXCOLLECTOR_MAX_NAME),
                                FirstName = Util.Next(WorldConfig.TAXCOLLECTOR_MIN_FIRSTNAME, WorldConfig.TAXCOLLECTOR_MAX_FIRSTNAME),
                                MapId = member.Character.MapId,
                                CellId = member.Character.CellId,
                                Skin = WorldConfig.TAXCOLLECTOR_SKIN_BASE,
                                SkinSize = WorldConfig.TAXCOLLECTOR_SKIN_SIZE_BASE,
                                Kamas = 0,
                            };

                            if (!TaxCollectorRepository.Instance.Insert(taxCollectorDAO))
                            {
                                member.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Unable to create Taxcollector due to unknow error."));
                                return;
                            }

                            foreach (var spell in Statistics.Spells.GetSpells())
                                SpellBookEntryRepository.Instance.Insert(new SpellBookEntryDAO() { OwnerType = (int)EntityTypeEnum.TYPE_TAX_COLLECTOR, OwnerId = taxCollectorDAO.Id, SpellId = spell.SpellId, Level = spell.Level });

                            var taxCollector = EntityManager.Instance.CreateTaxCollector(this, taxCollectorDAO);

                            AddTaxCollector(taxCollector);

                            member.Character.AddMessage(() =>
                                {
                                    member.Character.Inventory.SubKamas(TaxCollectorPrice);
                                    member.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_KAMAS_LOST, TaxCollectorPrice));
                                });

                            base.Dispatch(WorldMessage.GUILD_TAXCOLLECTOR_HIRED(taxCollector, member.Character.Name));
                        });
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="spellId"></param>
        public void BoostSpell(GuildMember member, int spellId)
        {
            if (!member.HasRight(GuildRightEnum.MANAGE_BOOST))
            {
                member.SendHasNotEnoughRights();
                return;
            }

            if (!Statistics.Spells.HasSpell(spellId))
            {
                member.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Unknow spellId"));
                return;
            }

            if (BoostPoint < 5)
            {
                member.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Not enough point to boost this spell."));
                return;
            }

            BoostPoint -= 5;
            Statistics.Spells.LevelUpSpell(spellId);
            SendBoostInformations(member);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="statId"></param>
        public void BoostStats(GuildMember member, char statId)
        {
            if (!member.HasRight(GuildRightEnum.MANAGE_BOOST))
            {
                member.SendHasNotEnoughRights();
                return;
            }

            if (BoostPoint < 1)
            {
                member.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("You don't have any boost point."));
                return;
            }

            switch (statId)
            {
                case 'o':
                    if (Statistics.BaseStatistics.GetTotal(EffectEnum.AddPods) >= 5000)
                    {
                        member.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Your taxcollector has already reached the max Pods."));
                        return;
                    }

                    Statistics.BaseStatistics.AddBase(EffectEnum.AddPods, 20);
                    BoostPoint--;
                    break;

                case 'x':
                    if (Statistics.BaseStatistics.GetTotal(EffectEnum.AddWisdom) >= 400)
                    {
                        member.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Your taxcollector has already reached the max Wisdom."));
                        return;
                    }

                    Statistics.BaseStatistics.AddBase(EffectEnum.AddWisdom, 1);
                    BoostPoint--;
                    break;

                case 'p':
                    if (Statistics.BaseStatistics.GetTotal(EffectEnum.AddProspection) >= 500)
                    {
                        member.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Your taxcollector has already reached the max Prospection."));
                        return;
                    }

                    Statistics.BaseStatistics.AddBase(EffectEnum.AddProspection, 1);
                    BoostPoint--;
                    break;

                case 'k':
                    if (BoostPoint < 10)
                    {
                        member.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("You don't have enough boost point."));
                        return;
                    }
                    if (Statistics.MaxTaxcollector >= 50)
                    {
                        member.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Your guild has already reached the maximum Taxcollector count."));
                        return;
                    }

                    Statistics.MaxTaxcollector++;
                    BoostPoint -= 10;
                    break;

                default:
                    member.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Unknow boost statId"));
                    return;
            }

            SendBoostInformations(member);
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
            var targetMember = GetMember(profilId);
            if (targetMember == null)
            {
                member.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var rankChanged = rank != (int)targetMember.Rank;
            var powerChanged = power != targetMember.Power;
            var xpShareChanged = percent != targetMember.XPSharePercent;

            var canManageOwnExp = member.HasRight(GuildRightEnum.MANAGE_OWN_EXP_PERCENT);
            var canManageOthersExp = member.HasRight(GuildRightEnum.MANAGE_EXP_PERCENT);
            var canManageRank = member.HasRight(GuildRightEnum.MANAGE_RANK);
            var canManagePower = member.HasRight(GuildRightEnum.MANAGE_POWER);

            if (!canManageOwnExp && !canManageOthersExp && !canManageRank && !canManagePower)
            {
                member.SendHasNotEnoughRights();
                return;
            }

            if (!himSelf && !canManageOthersExp && xpShareChanged)
            {
                member.SendHasNotEnoughRights();
                return;
            }

            if (!canManagePower && powerChanged)
            {
                member.SendHasNotEnoughRights();
                return;
            }

            if (!canManageRank && rankChanged)
            {
                member.SendHasNotEnoughRights();
                return;
            }

            if (!canManageOwnExp && himSelf && xpShareChanged)
            {
                member.SendHasNotEnoughRights();
                return;
            }

            if (rankChanged && (GuildRankEnum)rank == GuildRankEnum.BOSS)
            {
                if (member.Rank == GuildRankEnum.BOSS && targetMember.Rank != GuildRankEnum.BOSS)
                {
                    targetMember.SetBoss();
                    MemberProfilUpdate(targetMember, member.Id, (int)GuildRankEnum.SECOND_IN_COMMAND, 0, 0);
                    member.Dispatch(WorldMessage.GUILD_MEMBERS_INFORMATIONS(member));
                }
            }
            else
            {
                targetMember.Power = power;
                targetMember.Rank = (GuildRankEnum)rank;
            }

            targetMember.XPSharePercent = percent;

            // update profil
            member.Dispatch(WorldMessage.GUILD_MEMBERS_INFORMATIONS(targetMember));
            targetMember.Dispatch(WorldMessage.GUILD_STATS(this, power));
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
                member.SendHasNotEnoughRights();
                return;
            }

            var kickedMember = m_members.Find(m => m.Name == kickedMemberName);
            if (kickedMember == null)
            {
                member.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if (kickedMember.Rank == GuildRankEnum.BOSS)
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
            m_members.Add(member);
            base.AddHandler(member.Dispatch);
            
            IsActive = m_members.Count > 10;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        public void RemoveMember(GuildMember member)
        {
            m_members.Remove(member);
            base.RemoveHandler(member.Dispatch);
            
            IsActive = m_members.Count > 10;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GuildMember GetMember(long id)
        {
            return m_members.Find(member => member.Id == id);
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
        /// <param name="member"></param>
        public void SendMembersInformations(GuildMember member)
        {
            member.Dispatch(WorldMessage.GUILD_MEMBERS_INFORMATIONS(m_members));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void SendBoostInformations(GuildMember member)
        {
            member.Dispatch(WorldMessage.GUILD_BOOST_INFORMATIONS(BoostPoint, TaxCollectorPrice, Statistics));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        public void SendTaxCollectorsList(GuildMember member)
        {
            if (m_taxCollectors.Count > 0)
            {
                member.Dispatch(WorldMessage.GUILD_TAXCOLLECTOR_LIST(m_taxCollectors));
            }
            else
            {
                member.Dispatch(WorldMessage.BASIC_NO_OPERATION());
            }
        }
          
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void SendGeneralInformations(GuildMember member)
        {            
            member.Dispatch(WorldMessage.GUILD_GENERAL_INFORMATIONS(IsActive, Level, 0, 0, 0));   
        }
    }
}
