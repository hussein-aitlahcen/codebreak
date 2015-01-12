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
    public sealed class ItemTemplateRepository : Repository<ItemTemplateRepository, ItemTemplateDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, ItemTemplateDAO> m_templateById;

        /// <summary>
        /// 
        /// </summary>
        public ItemTemplateRepository()
        {
            m_templateById = new Dictionary<int, ItemTemplateDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        public override void OnObjectAdded(ItemTemplateDAO template)
        {
            m_templateById.Add(template.Id, template);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        public override void OnObjectRemoved(ItemTemplateDAO template)
        {
            m_templateById.Remove(template.Id);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public ItemTemplateDAO GetById(int templateId)
        {
            if(m_templateById.ContainsKey(templateId))
                return m_templateById[templateId];
            return null;
        }

        public override void UpdateAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
            // NO UPDATE
        }
    }
}
