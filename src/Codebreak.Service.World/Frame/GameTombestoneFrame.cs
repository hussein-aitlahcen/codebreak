using Codebreak.Framework.Network;
using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Frame
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameTombestoneFrame : FrameBase<GameTombestoneFrame, CharacterEntity, string>
    {  /// <summary>
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
                case 'G':
                    switch (message[1])
                    {
                        case 'F':
                            return FreeSoul;
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
        public void FreeSoul(CharacterEntity character, string message)
        {
            character.FrameManager.RemoveFrame(GameTombestoneFrame.Instance);

            character.AddMessage(character.FreeSoul);
        }
    }
}
