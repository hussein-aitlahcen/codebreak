using System.Collections.Generic;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Structures;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Frames;
using Codebreak.Service.World.Game.Guild;

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
        private Dictionary<long, TaxCollectorEntity> _taxCollectorById;
        private long _onlinePlayers = 0;

        /// <summary>
        /// 
        /// </summary>
        public EntityManager()
        {
            _characterById = new Dictionary<long, CharacterEntity>();
            _characterByName = new Dictionary<string, CharacterEntity>();
            _npcById = new Dictionary<long, NonPlayerCharacterEntity>();
            _taxCollectorById = new Dictionary<long, TaxCollectorEntity>();
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
            npc.StartAction(GameActionTypeEnum.MAP);
            _npcById.Add(npc.Id, npc);
            return npc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="taxCollectorDAO"></param>
        /// <returns></returns>
        public TaxCollectorEntity CreateTaxCollector(GuildInstance guild, TaxCollectorDAO taxCollectorDAO)
        {
            var taxCollector = new TaxCollectorEntity(guild, taxCollectorDAO);
            taxCollector.StartAction(GameActionTypeEnum.MAP);
            _taxCollectorById.Add(taxCollector.Id, taxCollector);
            return taxCollector;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollector"></param>
        public void RemoveTaxCollector(TaxCollectorEntity taxCollector)
        {
            _taxCollectorById.Remove(taxCollector.Id);
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
            _onlinePlayers++;
            Logger.Debug("EntityManager online players : " + _onlinePlayers);            
            return character;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void CharacterDisconnected(CharacterEntity character)
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

                if (character.CurrentAction != null)
                    character.AbortAction(character.CurrentAction.Type, character.Id);
                if (character.HasGameAction(GameActionTypeEnum.MAP))
                    character.AbortAction(GameActionTypeEnum.MAP);
                if (character.CharacterGuild != null)
                    character.CharacterGuild.CharacterDisconnected();

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
                    _onlinePlayers--;
                    _characterById.Remove(character.Id);
                    _characterByName.Remove(character.Name.ToLower());

                    character.Dispose();

                    Logger.Debug("EntityManager online players : " + _onlinePlayers);
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
