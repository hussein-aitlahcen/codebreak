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
    [Table("droptemplate")]
    public sealed class DropTemplateDAO : DataAccessObject<DropTemplateDAO>
    {
        [Key]
        public int Id
        {
            get;
            set;
        }
        public int MonsterId
        {
            get;
            set;
        }
        public string MonsterName
        {
            get;
            set;
        }
        public int TemplateId
        {
            get;
            set;
        }
        public int PPThreshold
        {
            get;
            set;
        }
        public int Max
        {
            get;
            set;
        }
        public double Rate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private MonsterDAO m_monster;
        [Write(false)]
        public MonsterDAO Monster
        {
            get
            {
                if (m_monster == null)
                    m_monster = MonsterRepository.Instance.GetById(MonsterId);
                return m_monster;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private ItemTemplateDAO m_item;
        [Write(false)]
        public ItemTemplateDAO ItemTemplate
        {
            get
            {
                if (m_item == null)
                    m_item = ItemTemplateRepository.Instance.GetById(TemplateId);
                return m_item;
            }
        }
    }
}
