using Codebreak.Service.World.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Job
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class JobBook
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, JobTemplate> m_templateById;

        /// <summary>
        /// 
        /// </summary>
        public JobBook()
        {
            m_templateById = new Dictionary<int, JobTemplate>();

            // JOB BASE, GOT YA
            AddJob(JobIdEnum.JOB_BASE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        public void AddJob(JobIdEnum jobId)
        {
            AddJob((int)jobId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId"></param>
        public void AddJob(int jobId)
        {
            m_templateById.Add(jobId, JobManager.Instance.GetById(jobId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public bool HasSkill(int skillId)
        {
            return m_templateById.Values.Any(job => job.HasSkill(skillId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public bool HasSkill(SkillIdEnum id)
        {
            return HasSkill((int)id);
        }
    }
}
