using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight;

namespace Codebreak.Service.World.Frames
{
    public sealed class GameInformationFrame : FrameBase<GameInformationFrame, EntityBase, string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Action<EntityBase, string> GetHandler(string message)
        {
            if (message == "GI")
                return GameInformation;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void GameInformation(EntityBase entity, string message)
        {
            entity.AddMessage(() =>
                {
                    entity.FrameManager.RemoveFrame(GameInformationFrame.Instance);

                    if (entity.HasGameAction(GameActionTypeEnum.FIGHT))
                    {
                        entity.CachedBuffer = true;
                        entity.Fight.JoinFight((FighterBase)entity, null);
                        entity.CachedBuffer = false;
                        return;
                    }

                    WorldService.Instance.RemoveUpdatable(entity);

                    entity.StartAction(Game.Action.GameActionTypeEnum.MAP);
                });
        }
    }
}
