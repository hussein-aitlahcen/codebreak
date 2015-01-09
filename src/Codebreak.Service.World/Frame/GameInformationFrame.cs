using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight;

namespace Codebreak.Service.World.Frame
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameInformationFrame : FrameBase<GameInformationFrame, CharacterEntity, string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Action<CharacterEntity, string> GetHandler(string message)
        {
            if (message == "GI")
                return GameInformation;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void GameInformation(CharacterEntity character, string message)
        {
            character.AddMessage(() =>
                {
                    character.FrameManager.RemoveFrame(GameInformationFrame.Instance);

                    if (character.HasGameAction(GameActionTypeEnum.FIGHT))
                    {
                        character.Fight.SendFightJoinInfos(character);
                        return;
                    }

                    WorldService.Instance.RemoveUpdatable(character);

                    character.StartAction(Game.Action.GameActionTypeEnum.MAP);
                });
        }
    }
}
