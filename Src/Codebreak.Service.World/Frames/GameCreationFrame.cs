using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Entity;

namespace Codebreak.Service.World.Frames
{
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
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GameCreation(CharacterEntity entity, string message)
        {
            entity.AddMessage(() =>
            {
                entity.FrameManager.RemoveFrame(GameCreationFrame.Instance);
                entity.FrameManager.AddFrame(GameInformationFrame.Instance);
                var map = entity.Map;
                entity.CachedBuffer = true;
                entity.Dispatch(WorldMessage.GAME_CREATION_SUCCESS());
                entity.Dispatch(WorldMessage.GAME_DATA_MAP(map.Id, map.CreateTime, map.DataKey));
                entity.Dispatch(WorldMessage.ACCOUNT_STATS((CharacterEntity)entity));
                entity.CachedBuffer = false;
            });
        }
    }
}
