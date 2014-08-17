using Codebreak.Framework.Network;
using Codebreak.WorldService.World.Action;
using Codebreak.WorldService.World.Entity;
using Codebreak.WorldService.World.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Handler
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
            entity.FrameManager.RemoveFrame(GameInformationFrame.Instance);

            if(entity.HasGameAction(GameActionTypeEnum.FIGHT))
            {
                entity.AddMessage(() =>
                    {
                        entity.CachedBuffer = true;
                        entity.Fight.JoinFight((FighterBase)entity, null);
                        entity.CachedBuffer = false;
                    });
                return;
            }

            WorldService.Instance.RemoveUpdatable(entity);

            entity.StartAction(Action.GameActionTypeEnum.MAP);
        }
    }
}
