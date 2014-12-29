using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Dialog
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NpcDialog 
    {
        /// <summary>
        /// 
        /// </summary>
        private CharacterEntity Character
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private NonPlayerCharacterEntity Npc
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public NpcQuestionDAO CurrentQuestion
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="npc"></param>
        public NpcDialog(CharacterEntity character, NonPlayerCharacterEntity npc)
        {
            Character = character;
            Npc = npc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="question"></param>
        public void SendQuestion(NpcQuestionDAO question)
        {
            CurrentQuestion = question;
            Character.Dispatch(WorldMessage.DIALOG_QUESTION(CurrentQuestion.Id, "", CurrentQuestion.GetResponses().Select(response => response.Id)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseId"></param>
        public void ProcessResponse(int responseId)
        {
            var response = CurrentQuestion.GetResponses().Find(entry => entry.Id == responseId);
            if(response == null)
            {
                Character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            foreach(var action in response.GetActions())
            {
                ActionEffectManager.Instance.ApplyEffect(Character, action.Key, action.Value);
            }
        }
    }
}
