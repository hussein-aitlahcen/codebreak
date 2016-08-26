using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Game.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Manager
{
    public sealed class QuestManager : Singleton<QuestManager>
    {
        private Dictionary<int, Quest> m_questById;
        private Dictionary<int, QuestStep> m_stepById;

        public QuestManager()
        {
            m_questById = new Dictionary<int, Quest>();
            m_stepById = new Dictionary<int, Game.Quest.QuestStep>();
        }

        public void Initialize()
        {
            foreach(var questDAO in QuestRepository.Instance.All)
            {
                m_questById.Add(questDAO.Id, new Quest(questDAO));
            }
        }

        public void AddStep(QuestStep step)
        {
            m_stepById.Add(step.Id, step);
        }

        public Quest GetQuest(int questId)
        {
            return m_questById[questId];
        }

        public QuestStep GetStep(int stepId)
        {
            return m_stepById[stepId];
        }
    }
}
