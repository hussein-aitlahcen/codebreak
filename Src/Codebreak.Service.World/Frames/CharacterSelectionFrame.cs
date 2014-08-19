using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Database.Structures;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Database.Repository;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Manager;
using Codebreak.WorldService;

namespace Codebreak.Service.World.Frames
{
    public sealed class CharacterSelectionFrame : FrameBase<CharacterSelectionFrame, WorldClient, string>
    {
        public override Action<WorldClient, string> GetHandler(string message)
        {
            if (message.Length < 2)
                return null;

            switch (message[0])
            {
                case 'A':
                    switch (message[1])
                    {
                        case 'V':
                            return HandleRegionalVersion;

                        case 'L':
                            return HandleCharacterList;

                        case 'A': // create
                            return HandleCharacterCreate;

                        case 'S': // select
                            return HandleCharacterSelect;

                        case 'D': // delete
                            return HandleCharacterDelete;

                        case 'P': // generatename
                            return HandleCharacterNameGenerate;

                        case 'R': // reset ( dead char )
                            return HandleCharacterReset;

                        default:
                            return null;
                    }
                default:
                    return null;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void HandleRegionalVersion(WorldClient client, string message)
        {
            client.Send(WorldMessage.ACCOUNT_REGIONAL_VERSION());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void HandleCharacterList(WorldClient client, string message)
        {
            if (client.Characters == null)            
                client.Characters = CharacterRepository.Instance.GetByAccount(client.Account.Id);

            foreach(var character in client.Characters)
            {
                var alreadyConnectedEntity = EntityManager.Instance.GetCharacter(character.Id);
                if(alreadyConnectedEntity != null)
                {
                    HandleCharacterReconnect(client, alreadyConnectedEntity);
                    return;
                }
            }
            
            client.Send(WorldMessage.CHARACTER_LIST(client.Characters));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="character"></param>
        public void HandleCharacterReconnect(WorldClient client, CharacterEntity character)
        {
            client.CurrentCharacter = character;

            client.CurrentCharacter.AddMessage(() =>
            {
                client.FrameManager.RemoveFrame(CharacterSelectionFrame.Instance);
                client.CurrentCharacter.FrameManager.AddFrame(BasicFrame.Instance);
                client.CurrentCharacter.FrameManager.AddFrame(GameCreationFrame.Instance);

                client.CurrentCharacter.CachedBuffer = true;
                client.CurrentCharacter.Dispatch(WorldMessage.CHARACTER_SELECTION_SUCCESS(client.CurrentCharacter));
                client.CurrentCharacter.Dispatch(WorldMessage.SPELLS_LIST(client.CurrentCharacter.Spells));
                client.CurrentCharacter.Dispatch(WorldMessage.BASIC_DATE());
                client.CurrentCharacter.Dispatch(WorldMessage.BASIC_TIME());
                client.CurrentCharacter.Dispatch(WorldMessage.AREAS_LIST());
                client.CurrentCharacter.Dispatch(WorldMessage.SPECIALISATION_SET(client.CurrentCharacter.AlignmentId));
                client.CurrentCharacter.Dispatch(WorldMessage.CHAT_ENABLED_CHANNELS());
                client.CurrentCharacter.Dispatch(WorldMessage.ACCOUNT_RIGHTS(client.CurrentCharacter.Restriction));
                client.CurrentCharacter.Dispatch(WorldMessage.INVENTORY_WEIGHT(0, 2000));
                client.CurrentCharacter.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_SERVER_WELCOME));
                client.CurrentCharacter.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_SERVER_BETA));
                client.CurrentCharacter.CachedBuffer = false;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void HandleCharacterCreate(WorldClient client, string message)
        {
            if (client.Characters == null)
            {
                client.Send(WorldMessage.CHARACTER_CREATION_ERROR());
                return;
            }

            var infos = message.Substring(2).Split('|');

            var name = infos[0];
            var breed = byte.Parse(infos[1]);
            var sex = int.Parse(infos[2]) == 1;
            var color1 = int.Parse(infos[3]);
            var color2 = int.Parse(infos[4]);
            var color3 = int.Parse(infos[5]);

            if (client.Characters.Count > 4)
            {
                client.Send(WorldMessage.CHARACTER_CREATION_ERROR());
                return;
            }

            if (!Enum.IsDefined(typeof(CharacterBreedEnum), breed))
            {
                client.Send(WorldMessage.CHARACTER_CREATION_ERROR());
                return;
            }

            if (color1 < -1 || color2 < -1 || color3 < -1)
            {
                client.Send(WorldMessage.CHARACTER_CREATION_ERROR());
                return;
            }

            var character = new CharacterDAO()
            {
                // global
                AccountId = client.Account.Id,
                Name = name,
                Experience = 0,
                Level = WorldConfig.CHARACTER_CREATION_LEVEL,
                MaxLevel = WorldConfig.CHARACTER_CREATION_LEVEL,

                // stats
                Ap = WorldConfig.CHARACTER_CREATION_AP,
                Mp = WorldConfig.CHARACTER_CREATION_MP,
                Chance = WorldConfig.CHARACTER_CREATION_CHANCE,
                Intelligence = WorldConfig.CHARACTER_CREATION_INTELLIGENCE,
                Agility = WorldConfig.CHARACTER_CREATION_AGILITY,
                Strength = WorldConfig.CHARACTER_CREATION_STRENGTH,
                Vitality = WorldConfig.CHARACTER_CREATION_VITALITY,
                Wisdom = WorldConfig.CHARACTER_CREATION_WISDOM,
                CaracPoint = WorldConfig.CHARACTER_CREATION_CARACPOINT,
                SpellPoint = WorldConfig.CHARACTER_CREATION_SPELLPOINT,

                // life status
                Life = WorldConfig.CHARACTER_CREATION_LIFE,
                Dead = false,
                DeathCount = 0,
                Energy = WorldConfig.CHARACTER_CREATION_ENERGY,

                // position
                MapId = WorldConfig.WORLD_MAP_START,
                CellId = WorldConfig.WORLD_CELL_START,

                // restricts
                Restriction = (int)PlayerRestrictionEnum.RESTRICTION_NEW_CHARACTER,

                // skin and color, 
                Breed = breed,
                Color1 = color1,
                Color2 = color2,
                Color3 = color3,
                Skin = breed * 10 + (sex ? 1 : 0),
                SkinSize = 100,
                Sex = sex,
            };

            WorldService.Instance.AddMessage(() =>
                {
                    CharacterCreateExecute(client, character);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="record"></param>
        private void CharacterCreateExecute(WorldClient client, CharacterDAO record)
        {
            if (CharacterRepository.Instance.GetByName(record.Name) != null)
            {
                client.Send(WorldMessage.CHARACTER_CREATION_ERROR_NAME_ALREADY_EXISTS());
                return;
            }

            if (!CharacterRepository.Instance.Insert(record))
            {
                client.Send(WorldMessage.CHARACTER_CREATION_ERROR());
                return;
            }

            client.Characters.Add(record);

            WorldService.Instance.AddMessage(() =>
                {
                    client.Send(WorldMessage.CHARACTER_CREATION_SUCCESS());
                    client.Send(WorldMessage.CHARACTER_LIST(client.Characters));
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void HandleCharacterDelete(WorldClient client, string message)
        {
            if (client.Characters == null)
            {
                client.Send(WorldMessage.CHARACTER_DELETION_ERROR());
                return;
            }

            var deletionData = message.Substring(2).Split('|');
            var characterId = long.Parse(deletionData[0]);
            var character = client.Characters.Find(entry => entry.Id == characterId);

            if (character == null)
            {
                client.Send(WorldMessage.CHARACTER_DELETION_ERROR());
                return;
            }

            WorldService.Instance.AddMessage(() =>
                {
                    CharacterDeletionExecute(client, character);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="character"></param>
        private void CharacterDeletionExecute(WorldClient client, CharacterDAO character)
        {
            if (!CharacterRepository.Instance.Remove(character))
            {
                client.Send(WorldMessage.CHARACTER_DELETION_ERROR());
                return;
            }

            client.Characters.Remove(character);

            WorldService.Instance.AddMessage(() =>
                {
                    client.Send(WorldMessage.CHARACTER_LIST(client.Characters));
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void HandleCharacterNameGenerate(WorldClient client, string message)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void HandleCharacterReset(WorldClient client, string message)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void HandleCharacterSelect(WorldClient client, string message)
        {
            if (client.Characters == null)
            {
                return;
            }

            var characterId = long.Parse(message.Substring(2));
            var character = client.Characters.Find(entry => entry.Id == characterId);

            // unknow id
            if (character == null)
            {
                client.Send(WorldMessage.CHARACTER_SELECTION_ERROR());
                return;
            }

            // dead ?
            if (character.Dead)
            {
                client.Send(WorldMessage.CHARACTER_SELECTION_ERROR());
                return;
            }
            
            WorldService.Instance.AddMessage(() =>
                {
                    CharacterSelectExecute(client, character);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="character"></param>
        public void CharacterSelectExecute(WorldClient client, CharacterDAO character)
        {
            client.FrameManager.RemoveFrame(CharacterSelectionFrame.Instance);

            client.CurrentCharacter = EntityManager.Instance.CreateCharacter(character);

            WorldService.Instance.AddUpdatable(client.CurrentCharacter);

            client.CurrentCharacter.FrameManager.AddFrame(BasicFrame.Instance);
            client.CurrentCharacter.FrameManager.AddFrame(SpellFrame.Instance);
            client.CurrentCharacter.FrameManager.AddFrame(GameCreationFrame.Instance);

            client.CurrentCharacter.CachedBuffer = true;
            client.CurrentCharacter.SafeDispatch(WorldMessage.CHARACTER_SELECTION_SUCCESS(client.CurrentCharacter));
            client.CurrentCharacter.SafeDispatch(WorldMessage.SPELLS_LIST(client.CurrentCharacter.Spells));
            client.CurrentCharacter.SafeDispatch(WorldMessage.BASIC_DATE());
            client.CurrentCharacter.SafeDispatch(WorldMessage.BASIC_TIME());
            client.CurrentCharacter.SafeDispatch(WorldMessage.AREAS_LIST());
            client.CurrentCharacter.SafeDispatch(WorldMessage.SPECIALISATION_SET(client.CurrentCharacter.AlignmentId));
            client.CurrentCharacter.SafeDispatch(WorldMessage.CHAT_ENABLED_CHANNELS());
            client.CurrentCharacter.SafeDispatch(WorldMessage.ACCOUNT_RIGHTS(client.CurrentCharacter.Restriction));
            client.CurrentCharacter.SafeDispatch(WorldMessage.INVENTORY_WEIGHT(0, 2000));
            client.CurrentCharacter.SafeDispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_SERVER_WELCOME));
            client.CurrentCharacter.SafeDispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_SERVER_BETA));
            client.CurrentCharacter.SafeDispatch(WorldMessage.INFORMATION_MESSAGE
                (
                    InformationTypeEnum.INFO,
                    InformationEnum.INFO_BASIC_LAST_CONNECTION,
                    client.Account.LastConnectionTime.Year.ToString(),
                    client.Account.LastConnectionTime.Month.ToString(),
                    client.Account.LastConnectionTime.Day.ToString(),
                    client.Account.LastConnectionTime.Hour.ToString(),
                    client.Account.LastConnectionTime.Minute.ToString(),
                    client.Account.LastConnectionIP
                ));
            client.CurrentCharacter.SafeDispatch(WorldMessage.INFORMATION_MESSAGE
                (
                    InformationTypeEnum.INFO,
                    InformationEnum.INFO_BASIC_CURRENT_IP,
                    client.Ip
                ));
            client.CurrentCharacter.CachedBuffer = false;
            client.Account.LastConnectionTime = DateTime.Now;
            client.Account.LastConnectionIP = client.Ip;            
        }
    }
}
