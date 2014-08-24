using Codebreak.Framework.Generic;
using Codebreak.Service.World.Game.Database.Repositories;

namespace Codebreak.Service.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public enum ExperienceTypeEnum
    {
        CHARACTER,
        JOB,
        MOUNT,
        AGGRESSION
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class ExperienceManager : Singleton<ExperienceManager>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public long GetFloor(int level, ExperienceTypeEnum type)
        {
            var template = ExperienceTemplateRepository.Instance.GetByLevel(level);
            if (template == null)
                return -1;
            switch(type)
            {
                case ExperienceTypeEnum.CHARACTER:
                    return template.Character;
                case ExperienceTypeEnum.AGGRESSION:
                    return template.Aggression;
                case ExperienceTypeEnum.JOB:
                    return template.Job;
                case ExperienceTypeEnum.MOUNT:
                    return template.Mount;
                default:
                    return -1;
            }
        }
    }
}
