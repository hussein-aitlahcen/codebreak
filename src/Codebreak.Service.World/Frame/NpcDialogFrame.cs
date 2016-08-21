using Codebreak.Framework.Network;
using Codebreak.Service.World.Game.Action;
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
    public sealed class NpcDialogFrame : AbstractNetworkFrame<NpcDialogFrame, CharacterEntity, string>
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
                case 'D':
                    switch (message[1])
                    {
                        case 'R':
                            return DialogReply;

                        case 'V':
                            return DialogLeave;

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
        /// <param name="character"></param>
        /// <param name="message"></param>
        public void DialogReply(CharacterEntity character, string message)
        {
            var dialogData = message.Substring(2).Split('|');
            var questionId = int.Parse(dialogData[0]);
            var responseId = int.Parse(dialogData[1]);

            character.AddMessage(() =>
                {
                    ((GameNpcDialogAction)character.CurrentAction).Dialog.ProcessResponse(responseId);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        public void DialogLeave(CharacterEntity character, string message)
        {
            character.AddMessage(() =>
                {
                    character.StopAction(GameActionTypeEnum.NPC_DIALOG);
                });
        }
    }
}
