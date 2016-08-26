using Codebreak.Framework.Database;
using Codebreak.Service.World.Game.ActionEffect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{
    [Table("queststep")]
    public sealed class QuestStepDAO : DataAccessObject<QuestStepDAO>
    {
        [Key]
        public int Id { get; set; }
        public int QuestId { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Actions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private ActionList m_actions;

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public ActionList ActionsList
        {
            get
            {
                if (m_actions == null)                
                    m_actions = ActionList.Deserialize(Actions);                
                return m_actions;
            }
        }

        public List<QuestObjectiveDAO> Objectives { get; } = new List<QuestObjectiveDAO>();
    }
}
