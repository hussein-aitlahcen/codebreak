using System.Collections.Generic;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Structures;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Frames;

namespace Codebreak.Service.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EntityManager : Singleton<EntityManager>
    {
        private Dictionary<long, CharacterEntity> _characterById;
        private Dictionary<string, CharacterEntity> _characterByName;
        private Dictionary<long, NonPlayerCharacterEntity> _npcById;
        private long _onlinePlayer = 0;

        public EntityManager()
        {
            _characterById = new Dictionary<long, CharacterEntity>();
            _characterByName = new Dictionary<string, CharacterEntity>();
            _npcById = new Dictionary<long, NonPlayerCharacterEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="npcDAO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public NonPlayerCharacterEntity CreateNpc(NpcInstanceDAO npcDAO, long id)
        {
            var npc = new NonPlayerCharacterEntity(npcDAO, id);
            _npcById.Add(npc.Id, npc);
            return npc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterDAO"></param>
        /// <returns></returns>
        public CharacterEntity CreateCharacter(int power, CharacterDAO characterDAO)
        {
            var character = new CharacterEntity(power, characterDAO);
            var guildMember = GuildManager.Instance.GetMember(characterDAO.GetCharacterGuild().GuildId, character.Id);
            if (guildMember != null)
                guildMember.CharacterConnected(character);
            _characterById.Add(character.Id, character);
            _characterByName.Add(character.Name.ToLower(), character);
            _onlinePlayer++;
            Logger.Debug("EntityManager online players : " + _onlinePlayer);            
            return character;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void CharacterDisconnect(CharacterEntity character)
        {
            if (character.PartyId != -1)
                PartyManager.Instance.PartyLeave(character);
            if (character.PartyInvitedPlayerId != -1 || character.PartyInviterPlayerId != -1)
                BasicFrame.Instance.PartyRefuse(character, "");
            if (character.GuildInvitedPlayerId != -1 || character.GuildInviterPlayerId != -1)
                BasicFrame.Instance.GuildJoinRefuse(character, "");
                
            character.AddMessage(() =>
            {
                if (character.HasGameAction(GameActionTypeEnum.FIGHT))
                {
                    if (character.CurrentAction != null)
                        character.AbortAction(character.CurrentAction.Type);
                    character.AbortAction(GameActionTypeEnum.FIGHT);
                    return;
                }

                if (character.CharacterGuild != null)
                    character.CharacterGuild.CharacterDisconnected();
                if (character.CurrentAction != null)
                    character.AbortAction(character.CurrentAction.Type, character.Id);
                if (character.HasGameAction(GameActionTypeEnum.MAP))
                    character.AbortAction(GameActionTypeEnum.MAP);

                RemoveCharacter(character);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void RemoveCharacter(CharacterEntity character)
        {
            WorldService.Instance.AddMessage(() =>
                {
                    _onlinePlayer--;
                    _characterById.Remove(character.Id);
                    _characterByName.Remove(character.Name.ToLower());

                    character.Dispose();

                    Logger.Debug("EntityManager online players : " + _onlinePlayer);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CharacterEntity GetCharacter(long id)
        {
            if (_characterById.ContainsKey(id))
                return _characterById[id];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CharacterEntity GetCharacter(string name)
        {
            name = name.ToLower();
            if (_characterByName.ContainsKey(name))
                return _characterByName[name];
            return null;
        }
    }
}
