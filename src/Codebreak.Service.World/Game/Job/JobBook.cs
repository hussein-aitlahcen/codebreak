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
    public sealed class JobBook
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
        /// <param name="message"></param>
        public void SerializeAs_SkillListMessage(StringBuilder message)
        {
            foreach(var job in Jobs)
            {
                if (job.JobId != (int)JobIdEnum.JOB_BASE)
                {
                    job.Template.SerializeAs_SkillListMessage(m_character, job.Level, message);
                }
            }
            if (Jobs.Count > 1)
                message.Remove(message.Length - 1, 1);
        }
    }
}
