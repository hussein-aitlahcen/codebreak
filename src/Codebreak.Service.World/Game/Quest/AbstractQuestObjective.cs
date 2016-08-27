using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Quest.Impl;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Quest
{
    public enum QuestObjectiveType
    {
        GENERIC = 0,
        FIND_NPC = 1,
        KILL_MONSTER = 6
    }

    public abstract class AbstractQuestObjective
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(AbstractQuestObjective));

        public int Id => m_record.Id;
        public int StepId => m_record.StepId;
        public int Type => m_record.Type;
        public int X => m_record.X;
        public int Y => m_record.Y;

        protected QuestObjectiveDAO m_record;

        protected AbstractQuestObjective(QuestObjectiveDAO record)
        {
            m_record = record;
        }

        public abstract bool Done(string value);

        public static AbstractQuestObjective FromRecord(QuestObjectiveDAO record)
        {
            switch ((QuestObjectiveType)record.Type)
            {
                case QuestObjectiveType.GENERIC: return new GenericObjective(record);
                case QuestObjectiveType.FIND_NPC: return new FindNpcObjective(record);
                case QuestObjectiveType.KILL_MONSTER: return new KillMonsterObjective(record);
            }
            throw new Exception("AbstractQuestObjective::FromType unknow typeId=" + record.Type);
        }
    }
}
