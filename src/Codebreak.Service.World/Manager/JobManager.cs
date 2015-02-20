using Codebreak.Framework.Generic;
using Codebreak.Service.World.Game.Job;
using Codebreak.Service.World.Game.Job.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class JobManager : Singleton<JobManager>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, JobTemplate> m_templateById;

        /// <summary>
        /// 
        /// </summary>
        public JobManager()
        {
            m_templateById = new Dictionary<int, JobTemplate>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            m_templateById.Add((int)JobIdEnum.JOB_BASE, 
                new JobTemplate(JobIdEnum.JOB_BASE,
                    JobIdEnum.JOB_NONE, 
                    new BasicSkill(SkillIdEnum.SKILL_ACCEDER), 
                    new BasicSkill(SkillIdEnum.SKILL_ACHETER), 
                    new BasicSkill(SkillIdEnum.SKILL_ACHETER_1),
                    new BasicSkill(SkillIdEnum.SKILL_ACTIONNER),
                    new BasicSkill(SkillIdEnum.SKILL_CONCASSER_DES_RESSOURCES),
                    new BasicSkill(SkillIdEnum.SKILL_CONSULTER),
                    new BasicSkill(SkillIdEnum.SKILL_DEVERROUILLER),
                    new BasicSkill(SkillIdEnum.SKILL_DEVERROUILLER_1),
                    new BasicSkill(SkillIdEnum.SKILL_ENTRER),
                    new BasicSkill(SkillIdEnum.SKILL_EPLUCHER),
                    new BasicSkill(SkillIdEnum.SKILL_FOUILLER),
                    new BasicSkill(SkillIdEnum.SKILL_INVOQUER_UNE_FEE),
                    new BasicSkill(SkillIdEnum.SKILL_JOUER),
                    new BasicSkill(SkillIdEnum.SKILL_MODIFIER_LE_PRIX_DE_VENTE),
                    new BasicSkill(SkillIdEnum.SKILL_MODIFIER_LE_PRIX_DE_VENTE_1),
                    new BasicSkill(SkillIdEnum.SKILL_OUVRIR),
                    new BasicSkill(SkillIdEnum.SKILL_PUISER),
                    new BasicSkill(SkillIdEnum.SKILL_RAMASSER),
                    new BasicSkill(SkillIdEnum.SKILL_PECHER_KOINKOIN),
                    new BasicSkill(SkillIdEnum.SKILL_SAUVEGARDER),
                    new BasicSkill(SkillIdEnum.SKILL_SE_REGENERER),
                    new BasicSkill(SkillIdEnum.SKILL_SE_REGENERER_1),
                    new BasicSkill(SkillIdEnum.SKILL_SE_FAIRE_TRANSPORTER),
                    new BasicSkill(SkillIdEnum.SKILL_SORTIR),
                    new BasicSkill(SkillIdEnum.SKILL_UTILISER),
                    new BasicSkill(SkillIdEnum.SKILL_UTILISER_ETABLI),
                    new BasicSkill(SkillIdEnum.SKILL_UTILISER_ZAAP),
                    new BasicSkill(SkillIdEnum.SKILL_VENDRE),
                    new BasicSkill(SkillIdEnum.SKILL_VENDRE_1),
                    new BasicSkill(SkillIdEnum.SKILL_VERROUILLER),
                    new BasicSkill(SkillIdEnum.SKILL_VERROUILLER_1),
                    new BasicSkill(SkillIdEnum.SKILL_SE_RENDRE_A_INCARNAM)
                ));

            m_templateById.Add((int)JobIdEnum.JOB_BUCHERON, new JobTemplate(
                    JobIdEnum.JOB_BUCHERON,
                    JobIdEnum.JOB_NONE,
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_1, 10, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_2, 20, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_3, 30, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_4, 35, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_5, 35, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_6, 40, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_7, 50, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_8, 50, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_9, 60, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_10, 70, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_11, 75, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_12, 80, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_13, 80, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_14, 90, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_15, 100, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_16, 100, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON])
                ));

            m_templateById.Add((int)JobIdEnum.JOB_PAYSAN, new JobTemplate(
                    JobIdEnum.JOB_PAYSAN,
                    JobIdEnum.JOB_NONE,
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_1, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_2, 20, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_3, 30, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_4, 40, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_5, 40, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_6, 50, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_7, 60, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_8, 100, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_EGRENER, 60, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_MOUDRE, 60, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN])                
                ));

            m_templateById.Add((int)JobIdEnum.JOB_PECHEUR, new JobTemplate(
                    JobIdEnum.JOB_PECHEUR,
                    JobIdEnum.JOB_NONE,
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_1, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_2, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_3, 10, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_4, 20, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_5, 40, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_6, 50, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_7, 70, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_8, 80, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_9, 80, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_10, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR])
                ));
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists(int id)
        {
            return m_templateById.ContainsKey(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JobTemplate GetById(int id)
        {
            return m_templateById[id];
        }
    }
}
