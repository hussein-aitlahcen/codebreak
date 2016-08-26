using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class QuestRepository : Repository<QuestRepository, QuestDAO>
    {
        private Dictionary<int, QuestDAO> m_questById;

        public QuestRepository()
        {
            m_questById = new Dictionary<int, QuestDAO>();
        }

        public QuestDAO GetById(int questId)
        {
            return m_questById[questId];
        }

        public override void OnObjectAdded(QuestDAO obj)
        {
            m_questById.Add(obj.Id, obj);
        }

        public override void UpdateAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }
        public override void DeleteAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }
        public override void InsertAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public sealed class QuestStepRepository : Repository<QuestStepRepository, QuestStepDAO>
    {
        private Dictionary<int, QuestStepDAO> m_stepById;

        public QuestStepRepository()
        {
            m_stepById = new Dictionary<int, QuestStepDAO>();
        }

        public QuestStepDAO GetById(int stepId)
        {
            return m_stepById[stepId];
        }

        public override void OnObjectAdded(QuestStepDAO obj)
        {
            m_stepById.Add(obj.Id, obj);
            QuestRepository
                .Instance
                .GetById(obj.QuestId)
                .Steps
                .Add(obj);
        }

        public override void UpdateAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }
        public override void DeleteAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }
        public override void InsertAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public sealed class QuestObjectiveRepository : Repository<QuestObjectiveRepository, QuestObjectiveDAO>
    {
        public override void OnObjectAdded(QuestObjectiveDAO obj)
        {
            QuestStepRepository
                .Instance
                .GetById(obj.StepId)
                .Objectives
                .Add(obj);
        }

        public override void UpdateAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }
        public override void DeleteAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }
        public override void InsertAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }
    }
}
