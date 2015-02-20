using Codebreak.Framework.Network;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Frame
{
    public sealed class SocialFrame : FrameBase<SocialFrame, CharacterEntity, string>
    {
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
                case 'F':
                    switch (message[1])
                    {
                        case 'L':
                            return FriendsList;

                        case 'A':
                            return AddFriend;

                        case 'D':
                            return RemoveFriend;

                        case 'O':
                            return FriendNotifyChange;
                    }
                    break;

                case 'i':
                    switch(message[1])
                    {
                        case 'L':
                            return EnnemiesList;

                        case 'A':
                            return AddEnnemy;

                        case 'D':
                            return RemoveEnnemy;
                    }
                    break;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void EnnemiesList(CharacterEntity character, string message)
        {
            character.SafeDispatch(WorldMessage.ENNEMIES_LIST(character.Account.Pseudo, character.Ennemies));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void RemoveEnnemy(CharacterEntity character, string message)
        {
            var pseudo = message.Substring(3);

            WorldService.Instance.AddMessage(() =>
                {
                    var relation = character.Ennemies.FirstOrDefault(ennemy => ennemy.Pseudo == pseudo);
                    if (relation == null)
                    {
                        character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    SocialRelationRepository.Instance.Removed(relation);

                    character.SafeDispatch(WorldMessage.ENNEMY_DELETE_SUCCESS());
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void AddEnnemy(CharacterEntity character, string message)
        {
            var name = message.Substring(2);

            WorldService.Instance.AddMessage(() =>
                {
                    var target = EntityManager.Instance.GetCharacterByName(name);
                    if (target == null || target.Account == null)
                    {
                        character.SafeDispatch(WorldMessage.ENNEMY_ADD_ERROR_UNKNOW_PLAYER());
                        return;
                    }

                    if (target.Id == character.Id)
                    {
                        character.SafeDispatch(WorldMessage.ENNEMY_ADD_ERROR_YOURSELF());
                        return;
                    }

                    if (character.Friends.Any(friend => friend.Pseudo == target.Account.Pseudo))
                    {
                        character.SafeDispatch(WorldMessage.FRIEND_ADD_ERROR_ALREADY());
                        return;
                    }

                    if (character.Ennemies.Any(ennemy => ennemy.Pseudo == target.Account.Pseudo))
                    {
                        character.SafeDispatch(WorldMessage.ENNEMY_ADD_ERROR_ALREADY());
                        return;
                    }

                    if (character.Ennemies.Count() >= WorldConfig.MAX_ENNEMIES)
                    {
                        character.SafeDispatch(WorldMessage.ENNEMY_ADD_ERROR_FULL());
                        return;
                    }

                    var relation = SocialRelationRepository.Instance.Create(character.AccountId, target.Account.Pseudo, (int)SocialRelationTypeEnum.TYPE_ENNEMY);

                    character.SafeDispatch(WorldMessage.ENNEMY_ADD(character.Account.Pseudo, relation));
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void FriendNotifyChange(CharacterEntity character, string message)
        {
            WorldService.Instance.AddMessage(() => character.NotifyOnFriendConnection = message[2] == '+');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void FriendsList(CharacterEntity character, string message)
        {
            WorldService.Instance.AddMessage(() => character.SafeDispatch(WorldMessage.FRIENDS_LIST(character.Account.Pseudo, character.Friends)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void RemoveFriend(CharacterEntity character, string message)
        {
            var pseudo = message.Substring(3);

            WorldService.Instance.AddMessage(() =>
                {
                    var relation = character.Friends.FirstOrDefault(friend => friend.Pseudo == pseudo);
                    if (relation == null)
                    {
                        character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    SocialRelationRepository.Instance.Removed(relation);

                    character.SafeDispatch(WorldMessage.FRIEND_DELETE_SUCCESS());
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void AddFriend(CharacterEntity character, string message)
        {
            var name = message.Substring(2);

            WorldService.Instance.AddMessage(() =>
                {
                    var target = EntityManager.Instance.GetCharacterByName(name);
                    if (target == null || target.Account == null)
                    {
                        character.SafeDispatch(WorldMessage.FRIEND_ADD_ERROR_UNKNOW_PLAYER());
                        return;
                    }

                    if (target.Id == character.Id)
                    {
                        character.SafeDispatch(WorldMessage.FRIEND_ADD_ERROR_YOURSELF());
                        return;
                    }

                    if (character.Friends.Any(friend => friend.Pseudo == target.Account.Pseudo))
                    {
                        character.SafeDispatch(WorldMessage.FRIEND_ADD_ERROR_ALREADY());
                        return;
                    }

                    if (character.Ennemies.Any(ennemy => ennemy.Pseudo == target.Account.Pseudo))
                    {
                        character.SafeDispatch(WorldMessage.ENNEMY_ADD_ERROR_ALREADY());
                        return;
                    }

                    if (character.Friends.Count() >= WorldConfig.MAX_FRIENDS)
                    {
                        character.SafeDispatch(WorldMessage.FRIEND_ADD_ERROR_FULL());
                        return;
                    }

                    var relation = SocialRelationRepository.Instance.Create(character.AccountId, target.Account.Pseudo, (int)SocialRelationTypeEnum.TYPE_FRIEND);

                    character.SafeDispatch(WorldMessage.FRIEND_ADD(character.Account.Pseudo, relation));
                });
        }
    }
}
