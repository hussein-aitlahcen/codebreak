using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Frame
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameCreationFrame : FrameBase<GameCreationFrame, CharacterEntity, string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Action<CharacterEntity, string> GetHandler(string message)
        {
            if(message.StartsWith("GC"))
                return GameCreation;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GameCreation(CharacterEntity character, string message)
        {
            character.AddMessage(() =>
            {
                character.FrameManager.RemoveFrame(GameCreationFrame.Instance);
                character.FrameManager.AddFrame(GameInformationFrame.Instance);
                var map = character.Map;
                if(map == null)
                {
                    character.MapId = WorldConfig.GetStartMap(character.Breed);
                    character.CellId = WorldConfig.GetStartCell(character.Breed);
                    map = character.Map;
                }
                character.CachedBuffer = true;
                character.Dispatch(WorldMessage.GAME_CREATION_SUCCESS());
                if (character.HasGameAction(Game.Action.GameActionTypeEnum.FIGHT))
                    character.Dispatch(WorldMessage.GAME_DATA_MAP(character.Fight.Map.Id, character.Fight.Map.CreateTime, character.Fight.Map.DataKey));
                else
                {
                    character.Dispatch(WorldMessage.GAME_DATA_MAP(map.Id, map.CreateTime, map.DataKey));             
                }
                character.Dispatch(WorldMessage.ACCOUNT_STATS((CharacterEntity)character));
                character.CachedBuffer = false;
            });
        }
    }
}
