using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ExperienceTemplateRepository : Repository<ExperienceTemplateRepository, ExperienceTemplateDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, ExperienceTemplateDAO> _experienceByLevel;

        /// <summary>
        /// 
        /// </summary>
        public ExperienceTemplateRepository()
        {
            _experienceByLevel = new Dictionary<int, ExperienceTemplateDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public ExperienceTemplateDAO GetByLevel(int level)
        {
            if(_experienceByLevel.ContainsKey(level))
                return _experienceByLevel[level];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="experienceTemplate"></param>
        public override void OnObjectAdded(ExperienceTemplateDAO experienceTemplate)
        {
            _experienceByLevel.Add(experienceTemplate.Level, experienceTemplate);

            base.OnObjectAdded(experienceTemplate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public override void OnObjectRemoved(ExperienceTemplateDAO experienceTemplate)
        {
            _experienceByLevel.Remove(experienceTemplate.Level);

            base.OnObjectRemoved(experienceTemplate);
        }
    }
}
