using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.ActionEffect
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AddJobEffect : AbstractActionEffect<AddJobEffect>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="item"></param>
        /// <param name="effect"></param>
        /// <param name="targetId"></param>
        /// <param name="targetCell"></param>
        /// <returns></returns>
        public override bool ProcessItem(Entity.CharacterEntity character, Database.Structure.ItemDAO item, Stats.GenericEffect effect, long targetId, int targetCell)
        {
            return Process(character, new Dictionary<string, string>() { { "jobId", effect.Value1.ToString() } });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool Process(Entity.CharacterEntity character, Dictionary<string, string> parameters)
        {
            var jobId = int.Parse(parameters["jobId"]);
                        
            var jobTemplate = JobManager.Instance.GetById(jobId);
            if(jobTemplate == null)
            {
                character.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Metier inconnu, veuillez reporter l'item sur le forum."));
                return false;
            }

            if (character.CharacterJobs.HasJob(jobId))
            {
                character.Dispatch(WorldMessage.IM_ERROR_MESSAGE(InformationEnum.ERROR_ALREADY_JOB));
                return false;
            }

            if (character.CharacterJobs.BaseJobCount > 2)
            {
                character.Dispatch(WorldMessage.IM_ERROR_MESSAGE(InformationEnum.ERROR_TOO_MUCH_JOB));
                return false;
            }

            character.CachedBuffer = true;
            character.CharacterJobs.LearnJob(jobId);
            character.Dispatch(WorldMessage.JOB_SKILL(character.CharacterJobs));
            character.Dispatch(WorldMessage.JOB_XP(character.CharacterJobs.Jobs));
            character.Dispatch(WorldMessage.IM_INFO_MESSAGE(InformationEnum.INFO_JOB_LEARNT, jobId));
            character.CachedBuffer = false;

            return true;
        }
    }
}