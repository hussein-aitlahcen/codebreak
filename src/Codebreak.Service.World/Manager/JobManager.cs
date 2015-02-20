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
                    new BasicSkill(SkillIdEnum.SKILL_SE_RENDRE_A_INCARNAM)));
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
