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
    public sealed class NpcQuestionRepository : Repository<NpcQuestionRepository, NpcQuestionDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, NpcQuestionDAO> m_questionById;

        /// <summary>
        /// 
        /// </summary>
        public NpcQuestionRepository()
        {
            m_questionById = new Dictionary<int, NpcQuestionDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public override void OnObjectAdded(NpcQuestionDAO question)
        {
            m_questionById.Add(question.Id, question);

            base.OnObjectAdded(question);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="question"></param>
        public override void OnObjectRemoved(NpcQuestionDAO question)
        {
            m_questionById.Remove(question.Id);

            base.OnObjectRemoved(question);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public NpcQuestionDAO GetById(int questionId)
        {
            return m_questionById[questionId];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public override void UpdateAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
            // NO UPDATE
        }
    }
}
