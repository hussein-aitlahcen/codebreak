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
    public sealed class ExperienceTemplateRepository : Repository<ExperienceTemplateRepository, ExperienceTemplateDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, ExperienceTemplateDAO> m_experienceByLevel;

        /// <summary>
        /// 
        /// </summary>
        public ExperienceTemplateRepository()
        {
            m_experienceByLevel = new Dictionary<int, ExperienceTemplateDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public ExperienceTemplateDAO GetByLevel(int level)
        {
            if(m_experienceByLevel.ContainsKey(level))
                return m_experienceByLevel[level];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="experienceTemplate"></param>
        public override void OnObjectAdded(ExperienceTemplateDAO experienceTemplate)
        {
            m_experienceByLevel.Add(experienceTemplate.Level, experienceTemplate);

            base.OnObjectAdded(experienceTemplate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public override void OnObjectRemoved(ExperienceTemplateDAO experienceTemplate)
        {
            m_experienceByLevel.Remove(experienceTemplate.Level);

            base.OnObjectRemoved(experienceTemplate);
        }
    }
}
