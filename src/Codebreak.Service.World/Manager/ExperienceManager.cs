using System.Linq;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Repository;

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
        PVP,
        LIVING,
        GUILD,
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class ExperienceManager : Singleton<ExperienceManager>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="experience"></param>
        /// <returns></returns>
        public int GetLevel(ExperienceTypeEnum type, long experience)
        {
            int x = 1;
            while(GetFloor(x, type) < experience)
            {
                x++;
            }
            return x;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="experience"></param>
        /// <returns></returns>
        public long GetFlootCurrent(ExperienceTypeEnum type, long experience)
            => GetFloor(GetLevel(type, experience), type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="experience"></param>
        /// <returns></returns>
        public long GetFloorNext(ExperienceTypeEnum type, long experience)
            => GetFloor(GetLevel(type, experience) + 1, type);

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
                case ExperienceTypeEnum.PVP:
                    return template.Pvp;
                case ExperienceTypeEnum.JOB:
                    return template.Job;
                case ExperienceTypeEnum.MOUNT:
                    return template.Mount;
                case ExperienceTypeEnum.GUILD:
                    return template.Guild;
                case ExperienceTypeEnum.LIVING:
                    return template.Living;
                default:
                    return -1;
            }
        }
    }
}
