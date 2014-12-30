using System;
using System.Collections.Generic;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Game.Stats;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Guild;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Frame
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BasicFrame : FrameBase<BasicFrame, CharacterEntity, string>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, EffectEnum> _statById = new Dictionary<int, EffectEnum>()
        {
            {10, EffectEnum.AddStrength},
            {11, EffectEnum.AddVitality},
            {12, EffectEnum.AddWisdom},
            {13, EffectEnum.AddChance},
            {14, EffectEnum.AddAgility},
            {15, EffectEnum.AddIntelligence},
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Action<CharacterEntity, string> GetHandler(string message)
        {
            if (message.Length < 2)
                return null;

            switch (message[0])
            {
                case 'g':
                    switch(message[1])
                    {                        
                        case 'P': // member profil update
                            return GuildProfilUpdate;
                        case 'K': // kick member
                            return GuildKick;
                        case 'V': // creation leave
                            return GuildCreationLeave;
                        case 'C': // creation
                            return GuildCreationRequest;
                        case 'B': // boost caract
                            return GuildBoostStats;
                        case 'H': // hire tax collector
                            return GuildHireTaxcollector;
                        case 'F': // farm tax collector
                            return GuildTaxCollectorRemove;
                        case 'f': // teleport to guild farm ?? 
                            break;
                        case 'h': // teleport to guild house
                            break;
                        case 'b': // boost spell
                            return GuildBoostSpell;
                        case 'J':
                            switch (message[2])
                            {
                                case 'R': // invite 
                                    return GuildJoinInvite;
                                case 'K': // accept invite
                                    return GuildJoinAccept;
                                case 'E': // refuse invite
                                    return GuildJoinRefuse;
                            }
                            break;
                        case 'T':
                            switch(message[2])
                            {
                                case 'J': // join tax collector fight
                                    return GuildTaxCollectorJoin;
                                case 'V': // leave tax collector fight
                                    return GuildTaxCollectorLeave;
                            }
                            break;
                        case 'I':
                            switch(message[2])
                            {
                                case 'M': // guildMemberInfos
                                    return GuildMembersInformations;
                                case 'B': // boost infos
                                    return GuildBoostInformations;
                                case 'G': // general infos
                                    return GuildGeneralInformations;
                                case 'F': // mount park infos
                                    break;
                                case 'H': // house info
                                    break;
                                case 'T':
                                    if(message.Length > 3)
                                        return GuildTaxCollectorInterfaceLeave;
                                    else
                                        return GuildTaxCollectorsList;
                            }
                            break;
                    }
                    break;

                case 'P':
                    switch(message[1])
                    {
                        case 'I': // PartyInvite
                            return PartyInvite;

                        case 'A': // PartyAccept
                            return PartyAccept;
                        
                        case 'R': // PartyRefuse
                            return PartyRefuse;

                        case 'V': // Leave
                            return PartyLeave;
                    }
                    break;
                case 'A':
                    switch (message[1])
                    {
                        case 'B':
                            return BoostStats;
                    }
                    break;

                case 'B':
                    switch (message[1])
                    {
                        case 'D':
                            return BasicDate;

                        case 'T':
                            return BasicTime;

                        case 'M':
                            return BasicMessage;
                    }
                    break;

                case 'p':
                    if (message == "ping")
                        return BasicPong;
                    break;

                case 'q':
                    if (message == "qping")
                        return BasicQPong;
                    break;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GuildTaxCollectorRemove(CharacterEntity character, string message)
        {           
            character.AddMessage(() =>
                {
                    if(!character.HasGameAction(GameActionTypeEnum.MAP))
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    long taxCollectorId = -1;
                    if (!long.TryParse(message.Substring(2), out taxCollectorId))
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    var taxCollector = character.Map.GetEntity(taxCollectorId) as TaxCollectorEntity;
                    if(taxCollector == null)
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    WorldService.Instance.AddMessage(() =>
                        {
                            if (character.CharacterGuild == null)
                            {
                                character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                                return;
                            }

                            character.CharacterGuild.RemoveTaxCollector(taxCollector);
                        });
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GuildTaxCollectorInterfaceLeave(CharacterEntity character, string message)
        {
            if (character.CharacterGuild == null)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.CharacterGuild.TaxCollectorsInterfaceLeave();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        public void GuildTaxCollectorLeave(CharacterEntity character, string message)
        {
            if (character.CharacterGuild == null)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.AddMessage(() =>
                {
                    if (!character.HasGameAction(GameActionTypeEnum.TAXCOLLECTOR_AGGRESSION))
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    character.StopAction(GameActionTypeEnum.TAXCOLLECTOR_AGGRESSION);
                });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GuildTaxCollectorJoin(CharacterEntity character, string message)
        {
            if (character.CharacterGuild == null)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            long taxCollectorId = -1;
            if (!long.TryParse(message.Substring(3), out taxCollectorId))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.CharacterGuild.TaxCollectorJoin(taxCollectorId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GuildHireTaxcollector(CharacterEntity entity, string message)
        {            
            entity.AddMessage(() =>
                {
                    if (!entity.HasGameAction(GameActionTypeEnum.MAP))
                    {
                        entity.SafeDispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_YOU_ARE_AWAY));
                        return;
                    }

                    WorldService.Instance.AddMessage(() => 
                        {
                            if (entity.CharacterGuild == null)
                            {
                                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                                return;
                            }

                            entity.CharacterGuild.HireTaxCollector();
                        });
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GuildBoostStats(CharacterEntity entity, string message)
        {
            if (entity.CharacterGuild == null)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            entity.CharacterGuild.BoostGuildStats(message[2]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GuildBoostSpell(CharacterEntity entity, string message)
        {
            if (entity.CharacterGuild == null)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var spellId = -1;
            if (!int.TryParse(message.Substring(2), out spellId))
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            entity.CharacterGuild.BoostGuildSpell(spellId);
        }  
        
        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GuildTaxCollectorsList(CharacterEntity entity, string message)
        {
            if (entity.CharacterGuild == null)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
            
            entity.CharacterGuild.SendTaxCollectorsList();
            entity.CharacterGuild.TaxCollectorsInterfaceJoin();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GuildBoostInformations(CharacterEntity entity, string message)
        {
            if (entity.CharacterGuild == null)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            entity.CharacterGuild.SendBoostInformations();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GuildCreationRequest(CharacterEntity entity, string message)
        {
            if (entity.CharacterGuild != null)
            {
                entity.SafeDispatch(WorldMessage.GUILD_CREATION_ERROR_ALREADY_IN_GUILD());
                return;
            }

            var guildData = message.Substring(2).Split('|');
            var backId = int.Parse(guildData[0]);
            var backColor = int.Parse(guildData[1]);
            var symbolId = int.Parse(guildData[2]);
            var symbolColor = int.Parse(guildData[3]);
            var name = guildData[4];

            entity.AddMessage(() =>
            {
                if (!entity.HasGameAction(GameActionTypeEnum.GUILD_CREATE))
                {
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                WorldService.Instance.AddMessage(() =>
                    {
                        if(GuildManager.Instance.Exists(name))
                        {
                            entity.SafeDispatch(WorldMessage.GUILD_CREATION_ERROR_NAME_ALREADY_EXISTS());
                            return;
                        }

                        if (!GuildManager.Instance.Create(entity, name, backId, backColor, symbolId, symbolColor))
                        {
                            entity.SafeDispatch(WorldMessage.SERVER_ERROR_MESSAGE("Unable to create the guild, unknow error."));
                            return;
                        }

                        entity.SafeDispatch(WorldMessage.GUILD_CREATION_SUCCESS());
                        entity.CharacterGuild.SendGuildStats();
                        entity.RefreshOnMap();

                        entity.AddMessage(() =>
                            {
                                entity.StopAction(GameActionTypeEnum.GUILD_CREATE);
                            });
                    });
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GuildCreationLeave(CharacterEntity entity, string message)
        {
            entity.AddMessage(() =>
                {
                    if (!entity.HasGameAction(GameActionTypeEnum.GUILD_CREATE))
                    {
                        entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    entity.StopAction(GameActionTypeEnum.GUILD_CREATE);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GuildKick(CharacterEntity entity, string message)
        {
            var character = (CharacterEntity)entity;
            if(character.CharacterGuild == null)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.CharacterGuild.MemberKick(message.Substring(2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GuildProfilUpdate(CharacterEntity entity, string message)
        {
            if (entity.CharacterGuild == null)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var messageData = message.Substring(2).Split('|');
            var profilId = long.Parse(messageData[0]);
            var rank = int.Parse(messageData[1]);
            var xpSharePercent = int.Parse(messageData[2]);
            var power = int.Parse(messageData[3]);

            entity.CharacterGuild.MemberProfilUpdate(profilId, rank, xpSharePercent, power);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        public void GuildJoinRefuse(CharacterEntity entity, string message)
        {
            // if not being invited and not even inviting
            if (entity.GuildInvitedPlayerId == -1 && entity.GuildInviterPlayerId == -1)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            CharacterEntity distantCharacter = null;

            // get the remote character
            if (entity.GuildInvitedPlayerId != -1)
                distantCharacter = EntityManager.Instance.GetCharacter(entity.GuildInvitedPlayerId);
            else
                distantCharacter = EntityManager.Instance.GetCharacter(entity.GuildInviterPlayerId);
            
            // be safe even if this should never happend
            if (distantCharacter != null)
            {
                if (entity.Id == distantCharacter.GuildInvitedPlayerId)
                {
                    entity.SafeDispatch(WorldMessage.GUILD_JOIN_ERROR_REFUSED_LOCAL());
                    distantCharacter.SafeDispatch(WorldMessage.GUILD_JOIN_ERROR_REFUSED_DISTANT(entity.Name));
                }
                else
                {
                    entity.SafeDispatch(WorldMessage.GUILD_JOIN_ERROR_REFUSED_LOCAL());
                    distantCharacter.SafeDispatch(WorldMessage.GUILD_JOIN_ERROR_REFUSED_LOCAL());
                }
                distantCharacter.GuildInvitedPlayerId = -1;
                distantCharacter.GuildInviterPlayerId = -1;
            }

            entity.GuildInvitedPlayerId = -1;
            entity.GuildInviterPlayerId = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GuildJoinAccept(CharacterEntity entity, string message)
        {
            // not being invited ?
            if (entity.GuildInviterPlayerId == -1)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var distantCharacter = EntityManager.Instance.GetCharacter(entity.GuildInviterPlayerId);

            entity.GuildInvitedPlayerId = -1;
            entity.GuildInviterPlayerId = -1;

            // should never happend
            if (distantCharacter == null)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            distantCharacter.GuildInvitedPlayerId = -1;
            distantCharacter.GuildInviterPlayerId = -1;
            distantCharacter.SafeDispatch(WorldMessage.GUILD_JOIN_ACCEPTED_DISTANT(entity.Name));

            distantCharacter.CharacterGuild.Guild.MemberJoin(entity);

            entity.SafeDispatch(WorldMessage.GUILD_JOIN_ACCEPTED_LOCAL());
            entity.SafeDispatch(WorldMessage.GUILD_JOIN_CLOSE());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GuildJoinInvite(CharacterEntity entity, string message)
        {
            // not in guild
            if (entity.CharacterGuild == null)
            {
                entity.SafeDispatch(WorldMessage.GUILD_JOIN_ERROR_UNKNOW());
                return;
            }

            var distantCharacterName = message.Substring(3);

            // if disconnected or fake
            var distantCharacter = EntityManager.Instance.GetCharacter(distantCharacterName);
            if (distantCharacter == null)
            {
                entity.SafeDispatch(WorldMessage.GUILD_JOIN_ERROR_UNKNOW());
                return;
            }

            // if in guild or already being invited or even inviting
            if (distantCharacter.CharacterGuild != null)
            {
                entity.SafeDispatch(WorldMessage.GUILD_JOIN_ERROR_ALREADY_IN_GUILD());
                return;
            }

            // if being invited or inviting
            if (entity.GuildInvitedPlayerId != -1 ||
                entity.GuildInviterPlayerId != -1 || 
                distantCharacter.GuildInvitedPlayerId != -1 || 
                distantCharacter.GuildInviterPlayerId != -1)
            {
                entity.SafeDispatch(WorldMessage.GUILD_JOIN_ERROR_OCCUPIED());
                return;
            }

            if (!entity.CharacterGuild.HasRight(GuildRightEnum.INVITE))
            {
                entity.SafeDispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_GUILD_NOT_ENOUGH_RIGHTS));
                return;
            }

            entity.GuildInvitedPlayerId = distantCharacter.Id;
            distantCharacter.GuildInviterPlayerId = entity.Id;

            entity.SafeDispatch(WorldMessage.GUILD_JOIN_REQUEST_LOCAL(distantCharacterName));
            distantCharacter.SafeDispatch(WorldMessage.GUILD_JOIN_REQUEST_DISTANT(entity.Id, entity.Name, entity.CharacterGuild.Guild.Name));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GuildGeneralInformations(CharacterEntity entity, string message)
        {
            if (entity.CharacterGuild == null)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            entity.CharacterGuild.SendGeneralInformations();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GuildMembersInformations(CharacterEntity entity, string message)
        {
            if (entity.CharacterGuild == null)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            entity.CharacterGuild.SendMembersInformations();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void PartyInvite(CharacterEntity entity, string message)
        {
            var distantCharacterName = message.Substring(2);

            // if disconnected or fake
            var distantCharacter = EntityManager.Instance.GetCharacter(distantCharacterName);
            if(distantCharacter == null)
            {
                entity.SafeDispatch(WorldMessage.PARTY_INVITE_ERROR_PLAYER_OFFLINE(distantCharacterName));
                return;
            }

            // if in party or being invited or even inviting
            if(distantCharacter.PartyId != -1 || distantCharacter.PartyInvitedPlayerId != -1 || distantCharacter.PartyInviterPlayerId != -1)
            {
                entity.SafeDispatch(WorldMessage.PARTY_INVITE_ERROR_ALREADY_IN_PARTY());
                return;
            }
                                                           
            // if being invited or inviting
            if (entity.PartyInvitedPlayerId != -1 || entity.PartyInviterPlayerId != -1)
            {
                entity.SafeDispatch(WorldMessage.PARTY_INVITE_ERROR_ALREADY_IN_PARTY());
                return;
            }

            // if party full
            var party = PartyManager.Instance.GetParty(entity.PartyId);
            if(party != null)  
            {         
                if(party.MemberCount > 7)
                {
                    entity.SafeDispatch(WorldMessage.PARTY_INVITE_ERROR_FULL());
                    return;
                }
            }

            entity.PartyInvitedPlayerId = distantCharacter.Id;
            distantCharacter.PartyInviterPlayerId = entity.Id;

            message = WorldMessage.PARTY_INVITE_SUCCESS(entity.Name, distantCharacterName);

            entity.SafeDispatch(message);
            distantCharacter.SafeDispatch(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        public void PartyRefuse(CharacterEntity entity, string message)
        {
            // if not being invited and not even inviting
            if (entity.PartyInvitedPlayerId == -1 && entity.PartyInviterPlayerId == -1)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            CharacterEntity distantCharacter = null;

            // get the remote character
            if (entity.PartyInvitedPlayerId != -1)
                distantCharacter = EntityManager.Instance.GetCharacter(entity.PartyInvitedPlayerId);
            else
                distantCharacter = EntityManager.Instance.GetCharacter(entity.PartyInviterPlayerId);

            // be safe even if this should never happend
            if (distantCharacter != null)
            {
                distantCharacter.PartyInvitedPlayerId = -1;
                distantCharacter.PartyInviterPlayerId = -1;
                distantCharacter.SafeDispatch(WorldMessage.PARTY_REFUSE());
            }

            entity.PartyInvitedPlayerId = -1;
            entity.PartyInviterPlayerId = -1;
            entity.SafeDispatch(WorldMessage.PARTY_REFUSE());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void PartyAccept(CharacterEntity entity, string message)
        {
            // not being invited ?
            if (entity.PartyInviterPlayerId == -1)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var distantCharacter = EntityManager.Instance.GetCharacter(entity.PartyInviterPlayerId);

            entity.PartyInvitedPlayerId = -1;
            entity.PartyInviterPlayerId = -1;

            // should never happend
            if(distantCharacter == null)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
            
            distantCharacter.PartyInvitedPlayerId = -1;
            distantCharacter.PartyInviterPlayerId = -1;
            
            distantCharacter.SafeDispatch(WorldMessage.PARTY_REFUSE());

            // already in party so we add the new one
            if(distantCharacter.PartyId != -1)
            {
                var party = PartyManager.Instance.GetParty(distantCharacter.PartyId);
                if(party == null)
                {
                    entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                party.AddMember(entity);
                return;
            }

            // create new party
            PartyManager.Instance.CreateParty(distantCharacter, entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void PartyLeave(CharacterEntity entity, string message)
        {
            // not in party ?
            if (entity.PartyId == -1)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            // null party should never happend but be sure
            var party = PartyManager.Instance.GetParty(entity.PartyId);
            if(party == null)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            // wants to leave
            if (message == "PV")
            {
                party.RemoveMember(entity);
                return;
            }

            long kickedPlayerId = -1;
            if(!long.TryParse(message.Substring(2), out kickedPlayerId))
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            party.KickMember(entity, kickedPlayerId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BoostStats(CharacterEntity entity, string message)
        {
            var statId = 0;

            if (!int.TryParse(message.Substring(2), out statId))
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if (!_statById.ContainsKey(statId))
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            entity.AddMessage(() =>
            {
                var effect = _statById[statId];
                var actualValue = entity.Statistics.GetEffect(effect).Base;
                var boostValue = statId == 11 && entity.Breed == CharacterBreedEnum.BREED_SACRIEUR ? 2 : 1;
                var requiredPoint = GenericStats.GetRequiredStatsPoint(entity.Breed, statId, actualValue);

                if (entity.CaractPoint < requiredPoint)
                {
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                entity.CaractPoint -= requiredPoint;

                switch (effect)
                {
                    case EffectEnum.AddStrength:
                        entity.DatabaseRecord.Strength += boostValue;
                        break;

                    case EffectEnum.AddVitality:
                        entity.DatabaseRecord.Vitality += boostValue;
                        break;

                    case EffectEnum.AddWisdom:
                        entity.DatabaseRecord.Wisdom += boostValue;
                        break;

                    case EffectEnum.AddIntelligence:
                        entity.DatabaseRecord.Intelligence += boostValue;
                        break;

                    case EffectEnum.AddAgility:
                        entity.DatabaseRecord.Agility += boostValue;
                        break;
                }

                entity.Statistics.AddBase(effect, boostValue);
                entity.Dispatch(WorldMessage.ACCOUNT_STATS(entity));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BasicPong(CharacterEntity entity, string message)
        {
            entity.SafeDispatch(WorldMessage.BASIC_PONG());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BasicQPong(CharacterEntity entity, string message)
        {
            entity.SafeDispatch(WorldMessage.BASIC_QPONG());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BasicDate(CharacterEntity entity, string message)
        {
            entity.SafeDispatch(WorldMessage.BASIC_DATE());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BasicTime(CharacterEntity entity, string message)
        {
            entity.SafeDispatch(WorldMessage.BASIC_TIME());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BasicMessage(CharacterEntity entity, string message)
        {
            var messageData = message.Substring(2).Split('|');
            var channel = messageData[0];
            var messageContent = messageData[1];

            if (channel.Length == 1)
            {
                entity.AddMessage(() =>
                    {
                        entity.DispatchChatMessage((ChatChannelEnum)channel[0], messageContent);
                    });
            }
            else
            {
                var remoteEntity = EntityManager.Instance.GetCharacter(channel);

                if (remoteEntity == null)
                {
                    entity.SafeDispatch(WorldMessage.CHAT_MESSAGE_ERROR_PLAYER_OFFLINE());
                    return;
                }

                entity.AddMessage(() =>
                {
                    entity.DispatchChatMessage(ChatChannelEnum.CHANNEL_PRIVATE_SEND, messageContent, remoteEntity);
                });

                remoteEntity.AddMessage(() =>
                {
                    remoteEntity.DispatchChatMessage(ChatChannelEnum.CHANNEL_PRIVATE_RECEIVE, messageContent, entity);
                });
            }
        }
    }
}
