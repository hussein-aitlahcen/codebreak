using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Job;
using Codebreak.Service.World.Game.Job.Skill;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Interactive.Type
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class HarvestableResource : InteractiveObject
    {
        /// <summary>
        /// 
        /// </summary>
        public const int FRAME_NORMAL = 0;

        /// <summary>
        /// 
        /// </summary>
        public const int FRAME_FARMING = 3;
        
        /// <summary>
        /// 
        /// </summary>
        public const int FRAME_CUT = 4;

        /// <summary>
        /// 
        /// </summary>
        public const int FRAME_GROW = 5;

        /// <summary>
        /// 
        /// </summary>
        public int GeneratedTemplateId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int MinRespawnTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxRespawnTime
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int Experience
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ItemTemplateDAO GeneratedTemplate
        {
            get
            {
                if (m_generatedTemplate == null)
                    m_generatedTemplate = ItemTemplateRepository.Instance.GetById(GeneratedTemplateId);
                return m_generatedTemplate;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        private UpdatableTimer m_harvestTimer;

        /// <summary>
        /// 
        /// </summary>
        private CharacterEntity m_currentHarvester;

        /// <summary>
        /// 
        /// </summary>
        private CharacterJobDAO m_currentJob;

        /// <summary>
        /// 
        /// </summary>
        private ItemTemplateDAO m_generatedTemplate;

        /// <summary>
        /// 
        /// </summary>
        private int m_quantityFarmed;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="cellId"></param>
        /// <param name="walkThrough"></param>
        public HarvestableResource(MapInstance map, int cellId, int generatedTemplateId, int minRespawnTime, int maxRespawnTime, int experience, bool walkThrough = false)
            : base(map, cellId, walkThrough)
        {
            GeneratedTemplateId = generatedTemplateId;
            MinRespawnTime = minRespawnTime;
            MaxRespawnTime = maxRespawnTime;
            Experience = experience;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="skill"></param>
        public override void UseWithSkill(CharacterEntity character, JobSkill skill)
        {
            switch (skill.SkillId)
            {
                case SkillIdEnum.SKILL_COUPER:
                case SkillIdEnum.SKILL_COUPER_1:
                case SkillIdEnum.SKILL_COUPER_2:
                case SkillIdEnum.SKILL_COUPER_3:
                case SkillIdEnum.SKILL_COUPER_4:
                case SkillIdEnum.SKILL_COUPER_5:
                case SkillIdEnum.SKILL_COUPER_6:
                case SkillIdEnum.SKILL_COUPER_7:
                case SkillIdEnum.SKILL_COUPER_8:
                case SkillIdEnum.SKILL_COUPER_9:
                case SkillIdEnum.SKILL_COUPER_10:
                case SkillIdEnum.SKILL_COUPER_11:
                case SkillIdEnum.SKILL_COUPER_12:
                case SkillIdEnum.SKILL_COUPER_13:
                case SkillIdEnum.SKILL_COUPER_14:
                case SkillIdEnum.SKILL_COUPER_15:
                case SkillIdEnum.SKILL_COUPER_16:
                case SkillIdEnum.SKILL_FAUCHER:
                case SkillIdEnum.SKILL_FAUCHER_1:
                case SkillIdEnum.SKILL_FAUCHER_2:
                case SkillIdEnum.SKILL_FAUCHER_3:
                case SkillIdEnum.SKILL_FAUCHER_4:
                case SkillIdEnum.SKILL_FAUCHER_5:
                case SkillIdEnum.SKILL_FAUCHER_6:
                case SkillIdEnum.SKILL_FAUCHER_7:
                case SkillIdEnum.SKILL_FAUCHER_8:
                case SkillIdEnum.SKILL_FAUCHER_9:
                case SkillIdEnum.SKILL_EGRENER:
                case SkillIdEnum.SKILL_MOUDRE:
                case SkillIdEnum.SKILL_PECHER:
                case SkillIdEnum.SKILL_PECHER_1:
                case SkillIdEnum.SKILL_PECHER_2:
                case SkillIdEnum.SKILL_PECHER_3:
                case SkillIdEnum.SKILL_PECHER_4:
                case SkillIdEnum.SKILL_PECHER_5:
                case SkillIdEnum.SKILL_PECHER_6:
                case SkillIdEnum.SKILL_PECHER_7:
                case SkillIdEnum.SKILL_PECHER_8:
                case SkillIdEnum.SKILL_PECHER_9:
                    Harvest(character, skill.SkillId);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="skill"></param>
        private void Harvest(CharacterEntity character, SkillIdEnum skill)
        {
            if(!character.CanGameAction(GameActionTypeEnum.SKILL_HARVEST))
            {
                character.Dispatch(WorldMessage.IM_ERROR_MESSAGE(InformationEnum.ERROR_YOU_ARE_AWAY));
                return;
            }

            if (!m_active)
                return;

            m_currentJob = character.CharacterJobs.GetJob(skill);
            if (m_currentJob == null)            
                return;

            var duration = m_currentJob.HarvestDuration;
            m_quantityFarmed = Util.Next(m_currentJob.HarvestMinQuantity, m_currentJob.HarvestMaxQuantity);

            character.HarvestStart(this, duration);
            m_currentHarvester = character;

            Deactivate();

            m_harvestTimer = base.AddTimer(duration, StopHarvest, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void AbortHarvest()
        {
            Activate();

            m_currentHarvester = null;
            m_currentJob = null;

            base.RemoveTimer(m_harvestTimer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void StopHarvest()
        {
            m_currentHarvester.StopAction(GameActionTypeEnum.SKILL_HARVEST);

            var exprienceWin = m_quantityFarmed * Experience * WorldConfig.RATE_XP;

            m_currentHarvester.CachedBuffer = true;
            m_currentHarvester.Inventory.AddItem(GeneratedTemplate.Create(m_quantityFarmed));
            m_currentHarvester.CharacterJobs.AddExperience(m_currentJob, (long)exprienceWin);
            m_currentHarvester.Dispatch(WorldMessage.INTERACTIVE_FARMED_QUANTITY(m_currentHarvester.Id, m_quantityFarmed));
            m_currentHarvester.Dispatch(WorldMessage.IM_INFO_MESSAGE(InformationEnum.INFO_WON_JOB_XP, exprienceWin, m_currentJob.JobId));
            m_currentHarvester.Dispatch(WorldMessage.JOB_XP(m_currentJob));
            m_currentHarvester.CachedBuffer = false;

            base.UpdateFrame(FRAME_FARMING, FRAME_CUT);
            base.AddTimer(Util.Next(MinRespawnTime, MaxRespawnTime), Respawn, true);
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void Respawn()
        {
            base.UpdateFrame(FRAME_GROW, FRAME_NORMAL, true);
        }
    }
}
