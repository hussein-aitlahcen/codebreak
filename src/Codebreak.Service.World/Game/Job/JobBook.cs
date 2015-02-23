using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
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
    public sealed class JobBook : MessageDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CharacterJobDAO> Jobs
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int BaseJobCount
        {
            get
            {
                return Jobs.Count(job => job.Template.ParentJobId == JobIdEnum.JOB_NONE && job.Template.Id != JobIdEnum.JOB_BASE);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private CharacterEntity m_character;

        /// <summary>
        /// 
        /// </summary>
        public JobBook(CharacterEntity character)
        {
            Jobs = CharacterJobRepository.Instance.GetByCharacterId(character.Id);
            m_character = character;

            // JOB BASE, GOT YA
            AddJob(JobIdEnum.JOB_BASE);

            base.AddHandler(character.Dispatch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        public void LearnJob(int jobId)
        {
            AddJob(jobId);
            
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
            if (Jobs.Any(job => job.JobId == jobId))
                return;

            // will implicitly added to the list of job because it has the same reference
            CharacterJobRepository.Instance.Create(m_character.Id, jobId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public JobSkill GetSkill(int skillId)
        {
            foreach(var job in Jobs)
            {
                var skill = job.GetSkill(m_character, skillId);
                if (skill != null)
                    return skill;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public bool HasSkill(int skillId)
        {
            return Jobs.Any(job => job.HasSkill(m_character, skillId));
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toolId"></param>
        /// <returns></returns>
        public void ToolEquipped(int templatId)
        {
            foreach (var job in Jobs)
                if (job.Template.HasTool(templatId))
                {
                    m_character.SafeDispatch(WorldMessage.JOB_TOOL_EQUIPPED(job.JobId.ToString()));
                    return;
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public CharacterJobDAO GetJob(SkillIdEnum skill)
        {
            return Jobs.Find(job => job.HasSkill(m_character, (int)skill));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public bool HasJob(int jobId)
        {
            return Jobs.Any(job => job.JobId == jobId);
        }
              

        /// <summary>
        /// 
        /// </summary>
        /// <param name="experience"></param>
        public void AddExperience(CharacterJobDAO job, long experience)
        {
            job.Experience += (long)(experience * WorldConfig.RATE_XP);

            var currentLevel = job.Level;

            while (job.Experience > job.ExperienceFloorNext)
                job.Level++;

            if (job.Level != currentLevel)
            {
                base.Dispatch(WorldMessage.JOB_NEW_LEVEL(job.JobId, job.Level));
                base.Dispatch(WorldMessage.JOB_SKILL(job));
            }

            base.Dispatch(WorldMessage.IM_INFO_MESSAGE(InformationEnum.INFO_WON_JOB_XP, experience, job.JobId));
            base.Dispatch(WorldMessage.JOB_XP(job));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SerializeAs_SkillListMessage(StringBuilder message)
        {
            foreach(var job in Jobs)
            {
                if (job.JobId != (int)JobIdEnum.JOB_BASE)
                {
                    job.Template.SerializeAs_SkillListMessage(job, message);
                }
            }
            if (Jobs.Count > 1)
                message.Remove(message.Length - 1, 1);
        }
    }
}
