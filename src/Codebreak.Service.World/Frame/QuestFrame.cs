using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game.Entity;

namespace Codebreak.Service.World.Frame
{
    public sealed class QuestFrame : AbstractNetworkFrame<QuestFrame, CharacterEntity, string>
    {
        public override Action<CharacterEntity, string> GetHandler(string message)
        {
            if (message.Length < 2)
                return null;

            switch (message[0])
            {
                case 'Q':
                    switch (message[1])
                    {
                        case 'L':
                            return QuestsListRequest;

                        case 'S':
                            return QuestsStepsRequest;
                    }
                    break;
            }
            return null;
        }

        private void QuestsListRequest(CharacterEntity character, string message)
        {
            character.AddMessage(character.SendQuestsList);
        }

        private void QuestsStepsRequest(CharacterEntity character, string message)
        {
            var questId = int.Parse(message.Contains('|') ? message.Split('|')[0].Substring(2) : message.Substring(2));
            character.AddMessage(() => character.SendQuestsStepsList(questId));
        }
    }
}
