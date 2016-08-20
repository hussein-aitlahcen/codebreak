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
            switch (skill.Id)
            {
                case SkillIdEnum.SKILL_COUPER_BAMBOU:
                case SkillIdEnum.SKILL_COUPER_BAMBOUSACRE:
                case SkillIdEnum.SKILL_COUPER_BAMBOUSOMBRE:
                case SkillIdEnum.SKILL_COUPER_BOMBU:
                case SkillIdEnum.SKILL_COUPER_CHARME:
                case SkillIdEnum.SKILL_COUPER_CHATAIGNER:
                case SkillIdEnum.SKILL_COUPER_CHENE:
                case SkillIdEnum.SKILL_COUPER_EBENE:
                case SkillIdEnum.SKILL_COUPER_ERABLE:
                case SkillIdEnum.SKILL_COUPER_FRENE:
                case SkillIdEnum.SKILL_COUPER_IF:
                case SkillIdEnum.SKILL_COUPER_KALIPTUS:
                case SkillIdEnum.SKILL_COUPER_MERISIER:
                case SkillIdEnum.SKILL_COUPER_NOYER:
                case SkillIdEnum.SKILL_COUPER_OLIVIOLET:
                case SkillIdEnum.SKILL_COUPER_ORME:
                case SkillIdEnum.SKILL_PECHER_GOUJON:
                case SkillIdEnum.SKILL_PECHER_TRUITE:
                case SkillIdEnum.SKILL_PECHER_POISSONCHATON:
                case SkillIdEnum.SKILL_PECHER_BROCHET:
                case SkillIdEnum.SKILL_PECHER_GREUVETTE:
                case SkillIdEnum.SKILL_PECHER_CRABESOURIMI:
                case SkillIdEnum.SKILL_PECHER_POISSONPANE:
                case SkillIdEnum.SKILL_PECHER_SARDINEBRILLANTE:
                case SkillIdEnum.SKILL_PECHER_PICHONEUDCOMPET:
                case SkillIdEnum.SKILL_PECHER_KRALAMOURE:
                case SkillIdEnum.SKILL_PECHER_SARDINEBRILLANTE_1:
                case SkillIdEnum.SKILL_FAUCHER_BLE:
                case SkillIdEnum.SKILL_FAUCHER_HOUBLON:
                case SkillIdEnum.SKILL_FAUCHER_LIN:
                case SkillIdEnum.SKILL_FAUCHER_SEIGLE:
                case SkillIdEnum.SKILL_FAUCHER_ORGE:
                case SkillIdEnum.SKILL_FAUCHER_CHANVRE:
                case SkillIdEnum.SKILL_FAUCHER_AVOINE:
                case SkillIdEnum.SKILL_FAUCHER_MALT:
                case SkillIdEnum.SKILL_FAUCHER_RIZ:
                case SkillIdEnum.SKILL_CUEILLIR_LIN:
                case SkillIdEnum.SKILL_CUEILLIR_CHANVRE:
                case SkillIdEnum.SKILL_CUEILLIR_TREFLE:
                case SkillIdEnum.SKILL_CUEILLIR_MENTHE:
                case SkillIdEnum.SKILL_CUEILLIR_ORCHIDEE:
                case SkillIdEnum.SKILL_CUEILLIR_EDELWEISS:
                case SkillIdEnum.SKILL_CUEILLIR_PANDOUILLE:
                case SkillIdEnum.SKILL_COLLECTER_FER:
                case SkillIdEnum.SKILL_COLLECTER_CUIVRE:
                case SkillIdEnum.SKILL_COLLECTER_BRONZE:
                case SkillIdEnum.SKILL_COLLECTER_KOBALTE:
                case SkillIdEnum.SKILL_COLLECTER_ARGENT:
                case SkillIdEnum.SKILL_COLLECTER_OR:
                case SkillIdEnum.SKILL_COLLECTER_BAUXITE:
                case SkillIdEnum.SKILL_COLLECTER_ETAIN:
                case SkillIdEnum.SKILL_COLLECTER_MANGANESE:
                case SkillIdEnum.SKILL_COLLECTER_DOLOMITE:
                case SkillIdEnum.SKILL_COLLECTER_SILICATE:
                    Harvest(character, skill.Id);
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

            if (!IsActive)
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

            var exprienceWin = m_quantityFarmed * Experience;

            m_currentHarvester.CachedBuffer = true;
            m_currentHarvester.Inventory.AddItem(GeneratedTemplate.Create(m_quantityFarmed));
            m_currentHarvester.CharacterJobs.AddExperience(m_currentJob, exprienceWin);
            m_currentHarvester.Dispatch(WorldMessage.INTERACTIVE_FARMED_QUANTITY(m_currentHarvester.Id, m_quantityFarmed));
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
