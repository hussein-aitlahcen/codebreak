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
        /// <param name="jobLevel"></param>
        /// <returns></returns>
        public static int GeneratedMinQuantity(int jobLevel)
        {
            return 1 + (int)Math.Floor((double)jobLevel / 5) + 6 * (int)Math.Floor((double)jobLevel / 100);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobLevel"></param>
        /// <returns></returns>
        public static int GeneratedMaxQuantity(int jobLevel)
        {
            return GeneratedMinQuantity(jobLevel) + 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobLevel"></param>
        /// <returns></returns>
        public static int HarvestTime(int jobLevel)
        {
            return Math.Max(2000, (int)(1000 * (10 - Math.Round(0.1 * (jobLevel - 1), 1))));
        }

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
        public long ExperienceFloorCurrent => ExperienceManager.Instance.GetFloor(Level, ExperienceTypeEnum.JOB);


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="skillId"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public JobSkill GetSkill(CharacterEntity character, int skillId)
        {
            return Template.GetSkill(character, skillId, Level);
        }

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public int HarvestMinQuantity => GeneratedMinQuantity(Level);

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public int HarvestMaxQuantity => GeneratedMaxQuantity(Level);

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public int HarvestDuration => HarvestTime(Level);

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public int CraftMaxCase
        {
            get
            {
                if (Level < 10) return 2;
                else if (Level < 20) return 3;
                else if (Level < 40) return 4;
                else if (Level < 60) return 5;
                else if (Level < 80) return 6;
                else if (Level < 100) return 7;
                else return 8;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caseCount"></param>
        /// <returns></returns>
        public long CraftExperience(int caseCount)
        {
            if(Level >= 100)
                return 0;

            switch(caseCount)
            {
                case 1: if (Level < 10) return 1; break;
                case 2: if (Level < 60) return 10; break;
                case 3: if (Level > 9 && Level < 80) return 25; break;
                case 4: if (Level > 19) return 50; break;
                case 5: if (Level > 39) return 100; break;
                case 6: if (Level > 59) return 250; break;
                case 7: if (Level > 79) return 500; break;
                case 8: if (Level > 99) return 1000; break;
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public int CraftSuccessPercent(int caseCount)
        {
            var maxCase = CraftMaxCase;
            if (maxCase - caseCount > 2) return 100;
            else return (Level / 2) + 50;
        }
    }
}
