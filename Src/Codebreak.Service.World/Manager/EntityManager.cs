using System.Collections.Generic;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Structures;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;

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
        public CharacterEntity CreateCharacter(CharacterDAO characterDAO)
        {
            var character = new CharacterEntity(characterDAO);
            _characterById.Add(character.Id, character);
            _characterByName.Add(character.Name.ToLower(), character);
            
            return character;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void RemoveCharacter(CharacterEntity character)
        {
            character.AddMessage(() =>
            {                
                if (character.HasGameAction(GameActionTypeEnum.FIGHT))
                {
                    if (character.CurrentAction != null)
                        character.AbortAction(character.CurrentAction.Type);
                    character.AbortAction(GameActionTypeEnum.FIGHT);
                }

                // disconnected meanwhile fighting
                if (character.HasGameAction(GameActionTypeEnum.FIGHT))
                    return;

                WorldService.Instance.AddMessage(() =>
                    {
                        _characterById.Remove(character.Id);
                        _characterByName.Remove(character.Name.ToLower());
                    });

                
                if (character.CurrentAction != null)
                    character.AbortAction(character.CurrentAction.Type, character.Id);
                if (character.HasGameAction(GameActionTypeEnum.MAP))
                    character.AbortAction(GameActionTypeEnum.MAP);
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
