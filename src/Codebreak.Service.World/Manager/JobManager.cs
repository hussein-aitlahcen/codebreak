using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
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
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_FRENE, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_CHENE, 30, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_IF, 50, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_EBENE, 70, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_ORME, 90, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_ERABLE, 40, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_CHARME, 80, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_CHATAIGNER, 10, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_NOYER, 20, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_MERISIER, 60, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_BOMBU, 35, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_OLIVIOLET, 35, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_BAMBOU, 50, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_BAMBOUSOMBRE, 80, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_BAMBOUSACRE, 100, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new HarvestSkill(SkillIdEnum.SKILL_COUPER_KALIPTUS, 75, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BUCHERON]),
                    new CraftSkill(SkillIdEnum.SKILL_SCIER, 1, new int[]
                        {
                        })
                ));

            m_templateById.Add((int)JobIdEnum.JOB_FORGEUR_EPEES, new JobTemplate(
                JobIdEnum.JOB_FORGEUR_EPEES,
                JobIdEnum.JOB_NONE,
                new CraftSkill(SkillIdEnum.SKILL_FORGER_UNE_EPEE, 1, new int[]
                    {
                        40, 42, 43, 44, 46, 47, 48, 49, 51, 53, 54, 55, 58, 59, 60, 61, 62, 65, 66, 67, 202, 327, 328, 338, 339, 352, 353, 354, 357, 358, 620, 819, 820, 821, 822, 823, 824, 825, 826, 827, 928, 1636, 1637, 1638, 1639, 1640, 1641, 1650, 1651, 2415, 2440, 2544, 2637, 2638, 2639, 4241, 4242, 4382, 6750, 6751, 6752, 6821, 6957, 7102, 7192, 7193, 7195, 7198, 7199, 7200, 7201, 7202, 7203, 7257, 8094, 8095, 8292, 8450, 8596, 8604, 8605, 8695, 8930, 8931, 8932, 8993, 9469
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_FORGEUR_EPEES])
                ));

            m_templateById.Add((int)JobIdEnum.JOB_PAYSAN, new JobTemplate(
                    JobIdEnum.JOB_PAYSAN,
                    JobIdEnum.JOB_NONE,
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_BLE, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_HOUBLON, 30, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_LIN, 40, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_SEIGLE, 50, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_ORGE, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_CHANVRE, 40, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_AVOINE, 20, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_MALT, 60, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_FAUCHER_RIZ, 50, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_EGRENER, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new HarvestSkill(SkillIdEnum.SKILL_MOUDRE, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN])                   
                ));

            m_templateById.Add((int)JobIdEnum.JOB_PECHEUR, new JobTemplate(
                    JobIdEnum.JOB_PECHEUR,
                    JobIdEnum.JOB_NONE,
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_GOUJON, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_TRUITE, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_POISSONCHATON, 40, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_BROCHET, 10, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_GREUVETTE, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_CRABESOURIMI, 20, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_POISSONPANE, 50, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_SARDINEBRILLANTE, 80, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_PICHONEUDCOMPET, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_KRALAMOURE, 80, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_SARDINEBRILLANTE_1, 80, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]) 
                ));

             m_templateById.Add((int)JobIdEnum.JOB_ALCHIMISTE, new JobTemplate(
                    JobIdEnum.JOB_ALCHIMISTE,
                    JobIdEnum.JOB_NONE,
                    new HarvestSkill(SkillIdEnum.SKILL_CUEILLIR_LIN, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_ALCHIMISTE]),
                    new HarvestSkill(SkillIdEnum.SKILL_CUEILLIR_CHANVRE, 10, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_ALCHIMISTE]),
                    new HarvestSkill(SkillIdEnum.SKILL_CUEILLIR_TREFLE, 20, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_ALCHIMISTE]),
                    new HarvestSkill(SkillIdEnum.SKILL_CUEILLIR_MENTHE, 30, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_ALCHIMISTE]),
                    new HarvestSkill(SkillIdEnum.SKILL_CUEILLIR_ORCHIDEE, 40, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_ALCHIMISTE]),
                    new HarvestSkill(SkillIdEnum.SKILL_CUEILLIR_EDELWEISS, 50, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_ALCHIMISTE]),
                    new HarvestSkill(SkillIdEnum.SKILL_CUEILLIR_PANDOUILLE, 50, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_ALCHIMISTE])
                ));
            
             m_templateById.Add((int)JobIdEnum.JOB_MINEUR, new JobTemplate(
                    JobIdEnum.JOB_MINEUR,
                    JobIdEnum.JOB_NONE,
                    new HarvestSkill(SkillIdEnum.SKILL_COLLECTER_FER, 1, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_MINEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_COLLECTER_CUIVRE, 10, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_MINEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_COLLECTER_BRONZE, 20, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_MINEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_COLLECTER_KOBALTE, 30, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_MINEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_COLLECTER_ARGENT, 60, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_MINEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_COLLECTER_OR, 80, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_MINEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_COLLECTER_BAUXITE, 70, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_MINEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_COLLECTER_ETAIN, 50, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_MINEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_COLLECTER_MANGANESE, 40, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_MINEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_COLLECTER_DOLOMITE, 100, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_MINEUR]),
                    new HarvestSkill(SkillIdEnum.SKILL_COLLECTER_SILICATE, 50, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_MINEUR])
                ));

             m_templateById.Add((int)JobIdEnum.JOB_TAILLEUR, new JobTemplate(
                   JobIdEnum.JOB_TAILLEUR,
                   JobIdEnum.JOB_NONE,
                   new CraftSkill(SkillIdEnum.SKILL_COUDRE_UN_CHAPEAU, 1, new int[] 
                   {
                       629, 696, 698, 699, 701, 702, 703, 704, 705, 706, 707, 708, 709, 710, 711, 712, 940, 941, 942, 943, 944, 949, 1904, 1908, 2070, 2094, 2095, 2096, 2097, 2104, 2409, 2410, 2411, 2438, 2447, 2474, 2485, 2531, 2535, 2546, 2641, 6472, 6473, 6474, 6477, 6481, 6482, 6483, 6500, 6719, 6760, 6761, 6764, 6778, 6797, 6834, 6913, 6917, 6921, 6926, 6930, 6936, 6937, 6938, 6939, 6952, 6988, 6989, 6990, 6991, 6992, 7056, 7058, 7109, 7141, 7142, 7143, 7144, 7145, 7146, 7150, 7151, 7152, 7177, 7226, 7227, 7228, 7229, 7339, 7516, 7553, 7680, 7886, 7921, 8009, 8114, 8116, 8125, 8133, 8147, 8163, 8243, 8244, 8245, 8246, 8247, 8248, 8260, 8267, 8284, 8285, 8287, 8304, 8330, 8331, 8441, 8442, 8451, 8457, 8463, 8474, 8530, 8569, 8619, 8628, 8629, 8630, 8631, 8632, 8633, 8634, 8635, 8636, 8637, 8638, 8699, 8704, 8820, 8821, 8822, 8823, 8824, 8829, 8839, 8840, 8841, 8842, 8843, 8844, 8845, 8846, 8847, 8848, 8918, 8989, 9147, 9181, 9394, 9395, 9461
                   }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_TAILLEUR]),
                   new CraftSkill(SkillIdEnum.SKILL_COUDRE_UNE_CAPE, 1, new int[]
                   { 
                       744, 754, 758, 759, 772, 773, 774, 775, 776, 777, 779, 781, 790, 932, 933, 934, 935, 936, 937, 945, 946, 947, 948, 952, 953, 954, 955, 956, 957, 1500, 1693, 1695, 1910, 2061, 2380, 2381, 2382, 2383, 2385, 2386, 2387, 2412, 2413, 2414, 2445, 2446, 2473, 2489, 2532, 2534, 2547, 2629, 6449, 6720, 6756, 6757, 6763, 6775, 6795, 6922, 6927, 6931, 6940, 6941, 6942, 6943, 6954, 6993, 6994, 6995, 7137, 7138, 7174, 7230, 7231, 7232, 7233, 7340, 7515, 7552, 7884, 8007, 8112, 8117, 8134, 8181, 8231, 8232, 8233, 8234, 8235, 8236, 8265, 8279, 8280, 8281, 8286, 8302, 8366, 8443, 8452, 8458, 8464, 8472, 8639, 8640, 8641, 8642, 8643, 8644, 8645, 8646, 8647, 8648, 8649, 8650, 8818, 8819, 8866, 8867, 8876, 8919, 9141, 9142, 9180, 10846 
                   }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_TAILLEUR]),
                   new CraftSkill(SkillIdEnum.SKILL_COUDRE_UN_SAC, 1, new int[] 
                   { 
                       1697, 1698, 1699, 1702, 1703, 1704, 1705, 1707, 6501, 6830, 6916, 7108, 8131 
                   }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_TAILLEUR])
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
            if(m_templateById.ContainsKey(id))
                return m_templateById[id];
            return null;
        }
    }
}
