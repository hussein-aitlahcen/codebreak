using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("craftentry")]
    public sealed class CraftEntryDAO : DataAccessObject<CraftEntryDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int TemplateId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int RequiredId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int RequiredQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private ItemTemplateDAO m_requiredTemplate;

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public ItemTemplateDAO RequiredTemplate
        {
            get
            {
                if (m_requiredTemplate == null)
                    m_requiredTemplate = ItemTemplateRepository.Instance.GetById(RequiredId);
                return m_requiredTemplate;
            }
        }
    }
}
