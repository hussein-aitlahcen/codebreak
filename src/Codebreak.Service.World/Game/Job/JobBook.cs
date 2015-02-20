using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
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
        private List<CharacterJobDAO> m_jobs;

        /// <summary>
        /// 
        /// </summary>
        private CharacterEntity m_character;

        /// <summary>
        /// 
        /// </summary>
        public JobBook(CharacterEntity character)
        {
            m_jobs = CharacterJobRepository.Instance.GetByCharacterId(character.Id);
            m_character = character;

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
            // already exists
            if (m_jobs.Any(job => job.JobId == jobId))
                return;

            // will implicitly added to the list of job because it has the same reference
            CharacterJobRepository.Instance.Create(m_character.Id, jobId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public bool HasSkill(int skillId)
        {
            return m_jobs.Any(job => job.HasSkill(m_character, skillId));
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
