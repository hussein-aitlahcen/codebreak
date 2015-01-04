using Codebreak.Framework.Generic;
using Codebreak.Service.World.Game.Job;
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
                    SkillIdEnum.SKILL_ACCEDER, 
                    SkillIdEnum.SKILL_ACHETER, 
                    SkillIdEnum.SKILL_ACHETER_1,
                    SkillIdEnum.SKILL_ACTIONNER,
                    SkillIdEnum.SKILL_CONCASSER_DES_RESSOURCES,
                    SkillIdEnum.SKILL_CONSULTER,
                    SkillIdEnum.SKILL_DEVERROUILLER,
                    SkillIdEnum.SKILL_DEVERROUILLER_1,
                    SkillIdEnum.SKILL_ENTRER,
                    SkillIdEnum.SKILL_EPLUCHER,
                    SkillIdEnum.SKILL_FOUILLER,
                    SkillIdEnum.SKILL_INVOQUER_UNE_FEE,
                    SkillIdEnum.SKILL_JOUER,
                    SkillIdEnum.SKILL_MODIFIER_LE_PRIX_DE_VENTE,
                    SkillIdEnum.SKILL_MODIFIER_LE_PRIX_DE_VENTE_1,
                    SkillIdEnum.SKILL_OUVRIR,
                    SkillIdEnum.SKILL_PUISER,
                    SkillIdEnum.SKILL_RAMASSER,
                    SkillIdEnum.SKILL_PECHER_KOINKOIN,
                    SkillIdEnum.SKILL_SAUVEGARDER,
                    SkillIdEnum.SKILL_SE_REGENERER,
                    SkillIdEnum.SKILL_SE_REGENERER_1,
                    SkillIdEnum.SKILL_SE_FAIRE_TRANSPORTER,
                    SkillIdEnum.SKILL_SORTIR,
                    SkillIdEnum.SKILL_UTILISER,
                    SkillIdEnum.SKILL_UTILISER_ETABLI,
                    SkillIdEnum.SKILL_UTILISER_ZAAP,
                    SkillIdEnum.SKILL_VENDRE,
                    SkillIdEnum.SKILL_VENDRE_1,
                    SkillIdEnum.SKILL_VERROUILLER,
                    SkillIdEnum.SKILL_VERROUILLER_1));
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
