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
    public sealed class NpcTemplateRepository : Repository<NpcTemplateRepository, NpcTemplateDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, NpcTemplateDAO> m_templateById;

        /// <summary>
        /// 
        /// </summary>
        public NpcTemplateRepository()
        {
            m_templateById = new Dictionary<int, NpcTemplateDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        public override void OnObjectAdded(NpcTemplateDAO template)
        {
            m_templateById.Add(template.Id, template);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        public override void OnObjectRemoved(NpcTemplateDAO template)
        {
            m_templateById.Remove(template.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NpcTemplateDAO GetById(int id)
        {
            if (m_templateById.ContainsKey(id))
                return m_templateById[id];
            return null;
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
