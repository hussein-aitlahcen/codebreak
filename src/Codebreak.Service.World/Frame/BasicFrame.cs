using System;
using System.Linq;
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
using Codebreak.Service.World.Command;

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
        private Dictionary<int, EffectEnum> m_statById = new Dictionary<int, EffectEnum>()
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
                case 'c':
                    switch(message[1])
                    {
                        case 'C':
                            return ChatChannelEnable;
                    }
                    break;

                case 'g':
                    switch (message[1])
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
                            switch (message[2])
                            {
                                case 'J': // join tax collector fight
                                    return GuildTaxCollectorJoin;
                                case 'V': // leave tax collector fight
                                    return GuildTaxCollectorLeave;
                            }
                            break;
                        case 'I':
                            switch (message[2])
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
                                    if (message.Length > 3)
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

                        case 'W': // ParyLocalize
                            return PartyLocalize;

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
                        case 'A':
                            return BasicCommand;

                        case 'D':
                            return BasicDate;

                        case 'T':
                            return BasicTime;

                        case 'M':
                            return BasicMessage;

                        case 'Y':
                            switch(message[2])
                            {
                                case 'A':
                                    return BasicAway;
                            }
                            break;
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
        private void PartyLocalize(CharacterEntity character, string message)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void ChatChannelEnable(CharacterEntity character, string message)
        {
            var enabled = message[2] == '+';
            var channel = (ChatChannelEnum)message[3];
            character.SafeDispatch(WorldMessage.CHAT_CHANNEL(enabled, channel));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void BasicAway(CharacterEntity character, string message)
        {
            character.AddMessage(character.SetAway);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void BasicCommand(CharacterEntity character, string message)
        {
            if (character.Account.Power < 1)
            {
                character.SafeDispatch(WorldMessage.SERVER_ERROR_MESSAGE("Not enought rights, attempt registered."));
                Logger.Error("BasicFrame::BasicCommand player trying to use an admin command : " + character.Name + " -> " + message);
                return;
            }

            var command = message.Substring(2);

            character.AddMessage(() =>
                {
                    if (!WorldService.Instance.CommandManager.Execute(new WorldCommandContext(character, command)))
                        character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Unknow command, type help to lists them all."));
                    else
                        Logger.Info("[CONSOLE COMMAND] name=" + character.Name + " ip=" + character.Ip + " command=" + command);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GuildTaxCollectorRemove(CharacterEntity character, string message)
        {
            long taxCollectorId = -1;
            if (!long.TryParse(message.Substring(2), out taxCollectorId))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.AddMessage(() =>
                {
                    if (!character.HasGameAction(GameActionTypeEnum.MAP))
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    var taxCollector = character.Map.GetEntity(taxCollectorId) as TaxCollectorEntity;
                    if (taxCollector == null)
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    if (character.GuildMember == null)
                    {
                        character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    character.GuildMember.RemoveTaxCollector(taxCollector);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GuildTaxCollectorInterfaceLeave(CharacterEntity character, string message)
        {
            if (character.GuildMember == null)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.GuildMember.TaxCollectorsInterfaceLeave();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        public void GuildTaxCollectorLeave(CharacterEntity character, string message)
        {           
            character.AddMessage(() =>
                {
                    if (character.GuildMember == null)
                    {
                        character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

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
            if (character.GuildMember == null)
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

            character.GuildMember.TaxCollectorJoin(taxCollectorId);
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

                    if (entity.GuildMember == null)
                    {
                        entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    entity.GuildMember.HireTaxCollector();
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GuildBoostStats(CharacterEntity character, string message)
        {
            if (character.GuildMember == null)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.GuildMember.BoostGuildStats(message[2]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GuildBoostSpell(CharacterEntity character, string message)
        {
            if (character.GuildMember == null)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var spellId = -1;
            if (!int.TryParse(message.Substring(2), out spellId))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.GuildMember.BoostGuildSpell(spellId);
        }  
        
        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GuildTaxCollectorsList(CharacterEntity character, string message)
        {
            if (character.GuildMember == null)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
            
            character.GuildMember.SendTaxCollectorsList();
            character.GuildMember.TaxCollectorsInterfaceJoin();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GuildBoostInformations(CharacterEntity character, string message)
        {
            if (character.GuildMember == null)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.GuildMember.SendBoostInformations();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GuildCreationRequest(CharacterEntity character, string message)
        {       
            var guildData = message.Substring(2).Split('|');
            var backId = int.Parse(guildData[0]);
            var backColor = int.Parse(guildData[1]);
            var symbolId = int.Parse(guildData[2]);
            var symbolColor = int.Parse(guildData[3]);
            var name = guildData[4];

            character.AddMessage(() =>
            {
                if (character.GuildMember != null)
                {
                    character.SafeDispatch(WorldMessage.GUILD_CREATION_ERROR_ALREADY_IN_GUILD());
                    return;
                }

                if (!character.HasGameAction(GameActionTypeEnum.GUILD_CREATE))
                {
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                WorldService.Instance.AddMessage(() =>
                    {
                        if(GuildManager.Instance.Exists(name))
                        {
                            character.SafeDispatch(WorldMessage.GUILD_CREATION_ERROR_NAME_ALREADY_EXISTS());
                            return;
                        }

                        if (!GuildManager.Instance.Create(character, name, backId, backColor, symbolId, symbolColor))
                        {
                            character.SafeDispatch(WorldMessage.SERVER_ERROR_MESSAGE("Unable to create the guild, unknow error."));
                            return;
                        }

                        character.SafeDispatch(WorldMessage.GUILD_CREATION_SUCCESS());
                        character.GuildMember.SendGuildStats();
                        character.RefreshOnMap();

                        character.AddMessage(() =>
                            {
                                character.StopAction(GameActionTypeEnum.GUILD_CREATE);
                            });
                    });
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GuildCreationLeave(CharacterEntity character, string message)
        {
            character.AddMessage(() =>
                {
                    if (!character.HasGameAction(GameActionTypeEnum.GUILD_CREATE))
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }
                    character.StopAction(GameActionTypeEnum.GUILD_CREATE);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GuildKick(CharacterEntity character, string message)
        {
            if(character.GuildMember == null)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.GuildMember.MemberKick(message.Substring(2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GuildProfilUpdate(CharacterEntity character, string message)
        {
            if (character.GuildMember == null)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var messageData = message.Substring(2).Split('|');
            var profilId = long.Parse(messageData[0]);
            var rank = int.Parse(messageData[1]);
            var xpSharePercent = int.Parse(messageData[2]);
            var power = int.Parse(messageData[3]);

            character.GuildMember.MemberProfilUpdate(profilId, rank, xpSharePercent, power);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        public void GuildJoinRefuse(CharacterEntity character, string message)
        {
            WorldService.Instance.AddMessage(() =>
                {
                    // if not being invited and not even inviting
                    if (character.GuildInvitedPlayerId == -1 && character.GuildInviterPlayerId == -1)
                    {
                        character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    CharacterEntity distantCharacter = null;

                    // get the remote character
                    if (character.GuildInvitedPlayerId != -1)
                        distantCharacter = EntityManager.Instance.GetCharacterById(character.GuildInvitedPlayerId);
                    else
                        distantCharacter = EntityManager.Instance.GetCharacterById(character.GuildInviterPlayerId);

                    // be safe even if this should never happend
                    if (distantCharacter != null)
                    {
                        if (character.Id == distantCharacter.GuildInvitedPlayerId)
                        {
                            character.SafeDispatch(WorldMessage.GUILD_JOIN_ERROR_REFUSED_LOCAL());
                            distantCharacter.SafeDispatch(WorldMessage.GUILD_JOIN_ERROR_REFUSED_DISTANT(character.Name));
                        }
                        else
                        {
                            character.SafeDispatch(WorldMessage.GUILD_JOIN_ERROR_REFUSED_LOCAL());
                            distantCharacter.SafeDispatch(WorldMessage.GUILD_JOIN_ERROR_REFUSED_LOCAL());
                        }
                        distantCharacter.GuildInvitedPlayerId = -1;
                        distantCharacter.GuildInviterPlayerId = -1;
                    }
                    character.GuildInvitedPlayerId = -1;
                    character.GuildInviterPlayerId = -1;
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GuildJoinAccept(CharacterEntity character, string message)
        {
            WorldService.Instance.AddMessage(() =>
                {
                    // not being invited ?
                    if (character.GuildInviterPlayerId == -1)
                    {
                        character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    var distantCharacter = EntityManager.Instance.GetCharacterById(character.GuildInviterPlayerId);

                    character.GuildInvitedPlayerId = -1;
                    character.GuildInviterPlayerId = -1;

                    // should never happend
                    if (distantCharacter == null)
                    {
                        character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    distantCharacter.GuildInvitedPlayerId = -1;
                    distantCharacter.GuildInviterPlayerId = -1;
                    distantCharacter.SafeDispatch(WorldMessage.GUILD_JOIN_ACCEPTED_DISTANT(character.Name));

                    distantCharacter.GuildMember.Guild.MemberJoin(character);

                    character.SafeDispatch(WorldMessage.GUILD_JOIN_ACCEPTED_LOCAL());
                    character.SafeDispatch(WorldMessage.GUILD_JOIN_CLOSE());
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GuildJoinInvite(CharacterEntity character, string message)
        {            
            WorldService.Instance.AddMessage(() =>
                {
                    // not in guild
                    if (character.GuildMember == null)
                    {
                        character.SafeDispatch(WorldMessage.GUILD_JOIN_ERROR_UNKNOW());
                        return;
                    }

                    var distantCharacterName = message.Substring(3);

                    // if disconnected or fake
                    var distantCharacter = EntityManager.Instance.GetCharacterByName(distantCharacterName);
                    if (distantCharacter == null)
                    {
                        character.SafeDispatch(WorldMessage.GUILD_JOIN_ERROR_UNKNOW());
                        return;
                    }

                    // if in guild or already being invited or even inviting
                    if (distantCharacter.GuildMember != null)
                    {
                        character.SafeDispatch(WorldMessage.GUILD_JOIN_ERROR_ALREADY_IN_GUILD());
                        return;
                    }

                    // if being invited or inviting
                    if (character.GuildInvitedPlayerId != -1 ||
                        character.GuildInviterPlayerId != -1 ||
                        distantCharacter.GuildInvitedPlayerId != -1 ||
                        distantCharacter.GuildInviterPlayerId != -1)
                    {
                        character.SafeDispatch(WorldMessage.GUILD_JOIN_ERROR_OCCUPIED());
                        return;
                    }

                    if (!character.GuildMember.HasRight(GuildRightEnum.INVITE))
                    {
                        character.SafeDispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_GUILD_NOT_ENOUGH_RIGHTS));
                        return;
                    }

                    character.GuildInvitedPlayerId = distantCharacter.Id;
                    distantCharacter.GuildInviterPlayerId = character.Id;

                    character.SafeDispatch(WorldMessage.GUILD_JOIN_REQUEST_LOCAL(distantCharacterName));
                    distantCharacter.SafeDispatch(WorldMessage.GUILD_JOIN_REQUEST_DISTANT(character.Id, character.Name, character.GuildMember.Guild.Name));
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GuildGeneralInformations(CharacterEntity character, string message)
        {
            if (character.GuildMember == null)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.GuildMember.SendGeneralInformations();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GuildMembersInformations(CharacterEntity character, string message)
        {
            if (character.GuildMember == null)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.GuildMember.SendMembersInformations();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void PartyInvite(CharacterEntity character, string message)
        {
            WorldService.Instance.AddMessage(() =>
                {
                    var distantCharacterName = message.Substring(2);

                    // if disconnected or fake
                    var distantCharacter = EntityManager.Instance.GetCharacterByName(distantCharacterName);
                    if (distantCharacter == null)
                    {
                        character.SafeDispatch(WorldMessage.PARTY_INVITE_ERROR_PLAYER_OFFLINE(distantCharacterName));
                        return;
                    }

                    // if in party or being invited or even inviting
                    if (distantCharacter.PartyId != -1 || distantCharacter.PartyInvitedPlayerId != -1 || distantCharacter.PartyInviterPlayerId != -1)
                    {
                        character.SafeDispatch(WorldMessage.PARTY_INVITE_ERROR_ALREADY_IN_PARTY());
                        return;
                    }

                    // if being invited or inviting
                    if (character.PartyInvitedPlayerId != -1 || character.PartyInviterPlayerId != -1)
                    {
                        character.SafeDispatch(WorldMessage.PARTY_INVITE_ERROR_ALREADY_IN_PARTY());
                        return;
                    }

                    // if party full
                    var party = PartyManager.Instance.GetParty(character.PartyId);
                    if (party != null)
                    {
                        if (party.MemberCount > 7)
                        {
                            character.SafeDispatch(WorldMessage.PARTY_INVITE_ERROR_FULL());
                            return;
                        }
                    }

                    character.PartyInvitedPlayerId = distantCharacter.Id;
                    distantCharacter.PartyInviterPlayerId = character.Id;

                    message = WorldMessage.PARTY_INVITE_SUCCESS(character.Name, distantCharacterName);

                    character.SafeDispatch(message);
                    distantCharacter.SafeDispatch(message);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        public void PartyRefuse(CharacterEntity character, string message)
        {           
            WorldService.Instance.AddMessage(() =>
                {
                    // if not being invited and not even inviting
                    if (character.PartyInvitedPlayerId == -1 && character.PartyInviterPlayerId == -1)
                    {
                        character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    CharacterEntity distantCharacter = null;

                    // get the remote character
                    if (character.PartyInvitedPlayerId != -1)
                        distantCharacter = EntityManager.Instance.GetCharacterById(character.PartyInvitedPlayerId);
                    else
                        distantCharacter = EntityManager.Instance.GetCharacterById(character.PartyInviterPlayerId);

                    // be safe even if this should never happend
                    if (distantCharacter != null)
                    {
                        distantCharacter.PartyInvitedPlayerId = -1;
                        distantCharacter.PartyInviterPlayerId = -1;
                        distantCharacter.SafeDispatch(WorldMessage.PARTY_REFUSE());
                    }

                    character.PartyInvitedPlayerId = -1;
                    character.PartyInviterPlayerId = -1;
                    character.SafeDispatch(WorldMessage.PARTY_REFUSE());
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void PartyAccept(CharacterEntity character, string message)
        {
            WorldService.Instance.AddMessage(() =>
            {
                // not being invited ?
                if (character.PartyInviterPlayerId == -1)
                {
                    character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                var distantCharacter = EntityManager.Instance.GetCharacterById(character.PartyInviterPlayerId);

                character.PartyInvitedPlayerId = -1;
                character.PartyInviterPlayerId = -1;

                // should never happend
                if (distantCharacter == null)
                {
                    character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                distantCharacter.PartyInvitedPlayerId = -1;
                distantCharacter.PartyInviterPlayerId = -1;

                distantCharacter.SafeDispatch(WorldMessage.PARTY_REFUSE());

                // already in party so we add the new one
                if (distantCharacter.PartyId != -1)
                {
                    var party = PartyManager.Instance.GetParty(distantCharacter.PartyId);
                    if (party == null)
                    {
                        character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    party.AddMember(character);
                    return;
                }

                // create new party
                PartyManager.Instance.CreateParty(distantCharacter, character);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void PartyLeave(CharacterEntity character, string message)
        {
            WorldService.Instance.AddMessage(() =>
            {
                // not in party ?
                if (character.PartyId == -1)
                {
                    character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                // null party should never happend but be sure
                var party = PartyManager.Instance.GetParty(character.PartyId);
                if (party == null)
                {
                    character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                // wants to leave
                if (message == "PV")
                {
                    party.RemoveMember(character);
                    return;
                }

                long kickedPlayerId = -1;
                if (!long.TryParse(message.Substring(2), out kickedPlayerId))
                {
                    character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                party.KickMember(character, kickedPlayerId);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void BoostStats(CharacterEntity character, string message)
        {
            var statId = 0;

            if (!int.TryParse(message.Substring(2), out statId))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            if (!m_statById.ContainsKey(statId))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            character.AddMessage(() =>
            {
                var effect = m_statById[statId];
                var actualValue = character.Statistics.GetEffect(effect).Base;
                var boostValue = statId == 11 && character.Breed == CharacterBreedEnum.BREED_SACRIEUR ? 2 : 1;
                var requiredPoint = GenericStats.GetRequiredStatsPoint(character.Breed, statId, actualValue);

                if (character.CaractPoint < requiredPoint)
                {
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                character.CaractPoint -= requiredPoint;

                switch (effect)
                {
                    case EffectEnum.AddStrength:
                        character.DatabaseRecord.Strength += boostValue;
                        break;

                    case EffectEnum.AddVitality:
                        character.DatabaseRecord.Vitality += boostValue;
                        break;

                    case EffectEnum.AddWisdom:
                        character.DatabaseRecord.Wisdom += boostValue;
                        break;

                    case EffectEnum.AddIntelligence:
                        character.DatabaseRecord.Intelligence += boostValue;
                        break;

                    case EffectEnum.AddAgility:
                        character.DatabaseRecord.Agility += boostValue;
                        break;
                        
                    case EffectEnum.AddChance:
                        character.DatabaseRecord.Chance += boostValue;
                        break;
                }

                character.Statistics.AddBase(effect, boostValue);
                character.Dispatch(WorldMessage.ACCOUNT_STATS(character));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void BasicPong(CharacterEntity character, string message)
        {
            character.SafeDispatch(WorldMessage.BASIC_PONG());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void BasicQPong(CharacterEntity character, string message)
        {
            character.SafeDispatch(WorldMessage.BASIC_QPONG());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void BasicDate(CharacterEntity character, string message)
        {
            character.SafeDispatch(WorldMessage.BASIC_DATE());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void BasicTime(CharacterEntity character, string message)
        {
            character.SafeDispatch(WorldMessage.BASIC_TIME());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void BasicMessage(CharacterEntity character, string message)
        {
            var messageData = message.Substring(2).Split('|');
            var channel = messageData[0];
            var messageContent = messageData[1];

            if (channel.Length == 1)
            {
                if (messageData.Length > 2)                
                    messageContent = messageContent + "|" + messageData[2];              
                if(!Enum.IsDefined(typeof(ChatChannelEnum), channel))
                {
                    character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }
                character.AddMessage(() => character.DispatchChatMessage((ChatChannelEnum)channel[0], messageContent));                
            }
            else
            {
                WorldService.Instance.AddMessage(() =>
                    {
                        var remoteEntity = EntityManager.Instance.GetCharacterByName(channel);
                        if (remoteEntity == null)
                        {
                            character.SafeDispatch(WorldMessage.CHAT_MESSAGE_ERROR_PLAYER_OFFLINE());
                            return;
                        }

                        character.AddMessage(() =>
                        {
                            if (character.DispatchChatMessage(ChatChannelEnum.CHANNEL_PRIVATE_SEND, messageContent, remoteEntity))
                            {
                                remoteEntity.AddMessage(() => remoteEntity.DispatchChatMessage(ChatChannelEnum.CHANNEL_PRIVATE_RECEIVE, messageContent, character));
                            }
                        });
                    });
            }
        }
    }
}
