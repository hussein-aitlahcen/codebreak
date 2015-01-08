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
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GameCreation(CharacterEntity entity, string message)
        {
            entity.AddMessage(() =>
            {
                entity.FrameManager.RemoveFrame(GameCreationFrame.Instance);
                entity.FrameManager.AddFrame(GameInformationFrame.Instance);
                var map = entity.Map;
                if(map == null)
                {
                    entity.ServerKick("Map inconnue.");
                    return;
                }
                entity.CachedBuffer = true;
                entity.Dispatch(WorldMessage.GAME_CREATION_SUCCESS());
                if (entity.HasGameAction(Game.Action.GameActionTypeEnum.FIGHT))
                    entity.Dispatch(WorldMessage.GAME_DATA_MAP(entity.Fight.Map.Id, entity.Fight.Map.CreateTime, entity.Fight.Map.DataKey));
                else                
                    entity.Dispatch(WorldMessage.GAME_DATA_MAP(map.Id, map.CreateTime, map.DataKey));
                
                entity.Dispatch(WorldMessage.ACCOUNT_STATS((CharacterEntity)entity));
                entity.CachedBuffer = false;
            });
        }
    }
}
