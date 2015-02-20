using Codebreak.Framework.Database;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Job;
using Codebreak.Service.World.Manager;
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
    [Table("characterjob")]
    public sealed class CharacterJobDAO : DataAccessObject<CharacterJobDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int JobId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public long ExperienceFloorNext
        {
            get
            {
                var next = ExperienceManager.Instance.GetFloor(Level + 1, ExperienceTypeEnum.JOB);
                if (next == -1)
                    return Experience;
                return next;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public long ExperienceFloorCurrent
        {
            get
            {
                return ExperienceManager.Instance.GetFloor(Level, ExperienceTypeEnum.JOB);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        public int Level
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long Experience
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Options
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public JobTemplate Template
        {
            get
            {
                if (m_template == null) 
                    m_template = JobManager.Instance.GetById(JobId);
                return m_template;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private JobTemplate m_template;
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasSkill(CharacterEntity character, int skillId)
        {
            return Template.HasSkill(character, skillId, Level);
        }
    }
}
