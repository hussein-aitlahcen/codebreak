using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.ActionEffect;
using Codebreak.Service.World.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Quest
{
    public sealed class QuestStep
    {
        private QuestStepDAO m_record;

        public int Id => m_record.Id;
        public int Order => m_record.Order;
        public string Name => m_record.Name;
        public string Description => m_record.Description;
        public ActionList ActionsList => m_record.ActionsList;

        public List<AbstractQuestObjective> Objectives { get; }

        public QuestStep(QuestStepDAO record)
        {
            m_record = record;
            Objectives = record.Objectives.Select(objectiveRecord => AbstractQuestObjective.FromRecord(objectiveRecord)).ToList();

            QuestManager.Instance.AddStep(this);
        }
    }
}
