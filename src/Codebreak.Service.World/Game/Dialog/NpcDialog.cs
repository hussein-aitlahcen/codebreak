using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Condition;
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
        public const string BANK_COST = "%bankCost%";
        public const string NAME = "%name%";

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
        private IEnumerable<NpcResponseDAO> m_possibleResponses;

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
            m_possibleResponses = CurrentQuestion.ResponseList;
            Character.Dispatch(WorldMessage.DIALOG_QUESTION(CurrentQuestion.Id, ApplyParameter(), m_possibleResponses.Select(response => response.Id)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseId"></param>
        public void ProcessResponse(int responseId)
        {
            var response = m_possibleResponses
                .First(entry => entry.Id == responseId);
            if(response == null || !ConditionParser.Instance.Check(response.Conditions, Character))
            {
                Character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }
            foreach(var action in response.ActionsList)
                ActionEffectManager.Instance.ApplyEffect(Character, action.Effect, action.Parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        private string ApplyParameter()
        {
            switch(CurrentQuestion.Params)
            {
                case BANK_COST:
                    return Character.Bank.Items.GroupBy(item => item.TemplateId).Count().ToString();

                case NAME:
                    return Character.Name;
            }
            return "";
        }
    }
}
