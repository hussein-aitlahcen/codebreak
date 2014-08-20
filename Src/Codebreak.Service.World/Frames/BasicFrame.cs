using System;
using System.Collections.Generic;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Game.Stats;
using Codebreak.Service.World.Database.Structures;

namespace Codebreak.Service.World.Frames
{
    public sealed class BasicFrame : FrameBase<BasicFrame, EntityBase, string>
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
        public override Action<EntityBase, string> GetHandler(string message)
        {
            if (message.Length < 2)
                return null;

            switch (message[0])
            {
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
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void PartyInvite(EntityBase entity, string message)
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
                                               
            var character = (CharacterEntity)entity;
            
            // if being invited or inviting
            if(character.PartyInvitedPlayerId != -1 || character.PartyInviterPlayerId != -1)
            {
                entity.SafeDispatch(WorldMessage.PARTY_INVITE_ERROR_ALREADY_IN_PARTY());
                return;
            }

            // if party full
            var party = PartyManager.Instance.GetParty(character.PartyId);
            if(party != null)  
            {         
                if(party.MemberCount > 7)
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void PartyRefuse(EntityBase entity, string message)
        {
            var character = (CharacterEntity)entity;

            // if not being invited and not even inviting
            if(character.PartyInvitedPlayerId == -1 && character.PartyInviterPlayerId == -1)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            CharacterEntity distantCharacter = null;

            // get the remote character
            if (character.PartyInvitedPlayerId != -1)
                distantCharacter = EntityManager.Instance.GetCharacter(character.PartyInvitedPlayerId);
            else
                distantCharacter = EntityManager.Instance.GetCharacter(character.PartyInviterPlayerId);

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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void PartyAccept(EntityBase entity, string message)
        {
            var character = (CharacterEntity)entity;

            // not being invited ?
            if(character.PartyInviterPlayerId == -1)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var distantCharacter = EntityManager.Instance.GetCharacter(character.PartyInviterPlayerId);

            // should never happend
            if(distantCharacter == null)
            {
                character.PartyInviterPlayerId = -1;
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
            
            character.PartyInvitedPlayerId = -1;
            character.PartyInviterPlayerId = -1;
            distantCharacter.PartyInvitedPlayerId = -1;
            distantCharacter.PartyInviterPlayerId = -1;
            
            distantCharacter.SafeDispatch(WorldMessage.PARTY_REFUSE());

            // already in party so we add the new one
            if(distantCharacter.PartyId != -1)
            {
                var party = PartyManager.Instance.GetParty(distantCharacter.PartyId);
                if(party == null)
                {
                    character.PartyInviterPlayerId = -1;
                    character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }

                party.AddMember(character);
                return;
            }

            // create new party
            PartyManager.Instance.CreateParty(distantCharacter, character);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void PartyLeave(EntityBase entity, string message)
        {
            var character = (CharacterEntity)entity;

            // not in party ?
            if(character.PartyId == -1)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            // null party should never happend but be sure
            var party = PartyManager.Instance.GetParty(character.PartyId);
            if(party == null)
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
            if(!long.TryParse(message.Substring(2), out kickedPlayerId))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            party.KickMember(character, kickedPlayerId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BoostStats(EntityBase entity, string message)
        {
            var character = (CharacterEntity)entity;
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
                var boostValue = statId == 11 && character.Breed == CharacterBreedEnum.BREED_SACRIEUR ? 2 : 1;
                var requiredPoint = GenericStats.GetRequiredStatsPoint(character.Breed, statId, actualValue);

                if (character.CaractPoint < requiredPoint)
                {
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
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
                }

                character.Statistics.AddBase(effect, boostValue);
                character.Dispatch(WorldMessage.ACCOUNT_STATS(character));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BasicPong(EntityBase entity, string message)
        {
            entity.SafeDispatch(WorldMessage.BASIC_PONG());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BasicQPong(EntityBase entity, string message)
        {
            entity.SafeDispatch(WorldMessage.BASIC_QPONG());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BasicDate(EntityBase entity, string message)
        {
            entity.SafeDispatch(WorldMessage.BASIC_DATE());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BasicTime(EntityBase entity, string message)
        {
            entity.SafeDispatch(WorldMessage.BASIC_TIME());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BasicMessage(EntityBase entity, string message)
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
