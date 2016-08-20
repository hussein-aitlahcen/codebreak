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
                    new BasicSkill(SkillIdEnum.SKILL_ACHETER_ENCLOS),
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
                            459, 2539, 2540, 2543, 6868, 7653, 7654, 7655, 7656, 7657, 7658, 7659, 7660, 7661, 7662, 7663, 7664, 7665, 7666, 7667, 7668, 7669, 7670, 7671, 7672, 8078
                        })
                ));

            m_templateById.Add((int)JobIdEnum.JOB_FORGEUR_EPEES, new JobTemplate(
                JobIdEnum.JOB_FORGEUR_EPEES,
                JobIdEnum.JOB_NONE,
                new CraftSkill(SkillIdEnum.SKILL_FORGER_UNE_EPEE, 1, new int[]
                    {
                        40, 42, 43, 44, 46, 47, 48, 49, 51, 53, 54, 55, 58, 59, 60, 61, 62, 65, 66, 67, 202, 327, 328, 338, 339, 352, 353, 354, 357, 358, 620, 819, 820, 821, 822, 823, 824, 825, 826, 827, 928, 1636, 1637, 1638, 1639, 1640, 1641, 1650, 1651, 2415, 2440, 2544, 2637, 2638, 2639, 4241, 4242, 4382, 6750, 6751, 6752, 6821, 6957, 7102, 7192, 7193, 7195, 7198, 7199, 7200, 7201, 7202, 7203, 7257, 8094, 8095, 8292, 8450, 8596, 8604, 8605, 8695, 8930, 8931, 8932, 8993, 9469
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_FORGEUR_EPEES]),
                new CraftSkill(SkillIdEnum.SKILL_REPARER_UNE_EPEE, 20, new int[]
                    {
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_FORGEUR_EPEES])
                ));

            m_templateById.Add((int)JobIdEnum.JOB_FORGEUR_DE_DAGUES, new JobTemplate(
                JobIdEnum.JOB_FORGEUR_DE_DAGUES,
                JobIdEnum.JOB_NONE,
                new CraftSkill(SkillIdEnum.SKILL_FORGER_UNE_DAGUE, 1, new int[]
                    {
                        94, 95, 96, 97, 205, 206, 207, 208, 211, 212, 213, 214, 218, 219, 220, 340, 341, 491, 498, 499, 500, 579, 893, 894, 895, 896, 897, 898, 899, 900, 919, 1026, 1027, 1028, 1029, 1030, 1031, 1032, 1033, 1034, 1035, 1036, 1037, 1038, 1039, 1040, 1041, 1369, 1370, 1371, 1372, 1373, 1374, 1504, 1563, 1564, 1565, 3647, 3648, 3649, 3650, 3651, 6516, 6517, 6924, 6981, 6982, 6983, 6984, 7188, 7190, 7191, 7256, 7493, 7495, 8092, 8414, 8444, 8598, 8599, 8926, 8927, 8928, 8929, 9137, 9176, 9974
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_FORGEUR_DE_DAGUES]),
                new CraftSkill(SkillIdEnum.SKILL_REPARER_UNE_DAGUE, 20, new int[]
                    {
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_FORGEUR_DE_DAGUES])
                ));

            m_templateById.Add((int)JobIdEnum.JOB_FORGEUR_DE_PELLES, new JobTemplate(
                JobIdEnum.JOB_FORGEUR_DE_PELLES,
                JobIdEnum.JOB_NONE,
                new CraftSkill(SkillIdEnum.SKILL_FORGER_UNE_PELLE, 1, new int[]
                    {
                        150, 151, 152, 153, 238, 239, 240, 241, 244, 245, 246, 247, 250, 251, 293, 294, 295, 296, 345, 347, 482, 483, 484, 1042, 1043, 1044, 1045, 1046, 1047, 1048, 1049, 1050, 1051, 1052, 1053, 1054, 1055, 1056, 1057, 1058, 1059, 1426, 1427, 1429, 1430, 1431, 1432, 1433, 1435, 1436, 3356, 6526, 6527, 6528, 6535, 6536, 6537, 6538, 6539, 6540, 7213, 8417, 8419, 8420, 8613, 8614, 8935, 8936, 8937, 9468
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_FORGEUR_DE_PELLES]),
                new CraftSkill(SkillIdEnum.SKILL_REPARER_UNE_PELLE, 20, new int[]
                    {
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_FORGEUR_DE_PELLES])
                ));

            m_templateById.Add((int)JobIdEnum.JOB_FORGEUR_DE_HACHES, new JobTemplate(
                JobIdEnum.JOB_FORGEUR_DE_HACHES,
                JobIdEnum.JOB_NONE,
                new CraftSkill(SkillIdEnum.SKILL_FORGER_UNE_HACHE, 1, new int[]
                    {
                        454, 456, 478, 502, 515, 673, 674, 675, 676, 771, 782, 923, 927, 1375, 1376, 1377, 1378, 2587, 2589, 2590, 2592, 2593, 2595, 2597, 2600, 2601, 2603, 2604, 2606, 2608, 2612, 2614, 2615, 2616, 6914, 7208, 7209, 7210, 7211, 7212, 7255, 8099, 8100, 8101, 8130, 8274, 8293, 8617, 8618, 8827, 8868, 8933, 8934, 9138
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_FORGEUR_DE_HACHES]),
                new CraftSkill(SkillIdEnum.SKILL_REPARER_UNE_HACHE, 20, new int[]
                    {
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_FORGEUR_DE_HACHES])
                ));

            m_templateById.Add((int)JobIdEnum.JOB_FORGEUR_DE_MARTEAUX, new JobTemplate(
                JobIdEnum.JOB_FORGEUR_DE_MARTEAUX,
                JobIdEnum.JOB_NONE,
                new CraftSkill(SkillIdEnum.SKILL_FORGER_UN_MARTEAU, 1, new int[]
                    {
                       144, 145, 146, 147, 221, 222, 223, 224, 227, 228, 229, 230, 233, 234, 235, 236, 237, 271, 342, 455, 493, 494, 495, 496, 922, 1060, 1061, 1062, 1063, 1064, 1065, 1066, 1067, 1068, 1069, 1070, 1071, 1072, 1073, 1074, 1075, 1076, 1077, 1078, 1079, 1080, 1081, 1082, 1083, 1084, 1085, 1379, 1383, 1393, 1399, 1403, 1404, 1479, 1520, 1539, 1560, 1561, 1562, 2416, 3357, 6503, 6504, 6505, 6507, 6508, 6509, 6510, 6511, 6512, 6513, 7098, 7153, 7154, 7155, 7156, 7157, 7158, 7159, 7197, 7650, 7882, 8096, 8097, 8148, 8294, 8615, 8616, 8825, 8826, 8833, 8878, 9117, 9961, 9972
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_FORGEUR_DE_MARTEAUX]),
                new CraftSkill(SkillIdEnum.SKILL_REPARER_UN_MARTEAU, 20, new int[]
                    {
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_FORGEUR_DE_MARTEAUX])
                ));

            m_templateById.Add((int)JobIdEnum.JOB_FORGEUR_DE_BOUCLIERS, new JobTemplate(
                JobIdEnum.JOB_FORGEUR_DE_BOUCLIERS,
                JobIdEnum.JOB_NONE,
                new CraftSkill(SkillIdEnum.SKILL_FORGER_UN_BOUCLIER, 1, new int[]
                    {
                       8855, 9001, 9002, 9003, 9004, 9005, 9006, 9007, 9008, 9009, 9010, 9011, 9012, 9013, 9014, 9015, 9016, 9017, 9018, 9019, 9020, 9021, 9022, 9023, 9024, 9025, 9026, 9027, 9028, 9029, 9030
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_FORGEUR_DE_BOUCLIERS])
                ));

            m_templateById.Add((int)JobIdEnum.JOB_SCULPTEUR_ARCS, new JobTemplate(
                JobIdEnum.JOB_SCULPTEUR_ARCS,
                JobIdEnum.JOB_NONE,
                new CraftSkill(SkillIdEnum.SKILL_SCULPTER_UN_ARC, 1, new int[]
                    {
                        88, 89, 90, 91, 264, 265, 266, 267, 270, 272, 273, 329, 331, 332, 337, 420, 462, 592, 640, 828, 829, 1092, 1093, 1094, 1095, 1096, 1097, 1098, 1112, 1113, 1114, 1115, 1116, 1117, 1118, 1119, 1120, 1121, 1122, 1123, 1124, 1125, 1126, 1127, 1128, 1350, 1351, 1352, 1353, 1354, 1355, 1620, 6445, 6446, 6451, 6452, 6453, 6484, 6485, 6491, 7160, 7161, 7162, 7163, 7164, 7187, 7194, 8005, 8103, 8104, 8295, 8609, 8610, 8864, 8924, 8925, 9134
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_SCULPTEUR_ARCS]),
                new CraftSkill(SkillIdEnum.SKILL_REPARER_UN_ARC, 20, new int[]
                    {
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_SCULPTEUR_ARCS])
                ));

            m_templateById.Add((int)JobIdEnum.JOB_SCULPTEUR_DE_BATONS, new JobTemplate(
                JobIdEnum.JOB_SCULPTEUR_DE_BATONS,
                JobIdEnum.JOB_NONE,
                new CraftSkill(SkillIdEnum.SKILL_SCULPTER_UN_BATON, 1, new int[]
                    {
                        138, 139, 140, 182, 183, 184, 185, 188, 194, 200, 201, 204, 317, 318, 319, 320, 335, 336, 436, 619, 657, 658, 742, 791, 1109, 1110, 1111, 1147, 1148, 1149, 1150, 1151, 1152, 1153, 1154, 1157, 1161, 1162, 1163, 1164, 1363, 1364, 1365, 1366, 1367, 1368, 2066, 2417, 2640, 3652, 6442, 6454, 6492, 6521, 6522, 6523, 6524, 6525, 6725, 6762, 6769, 6792, 7176, 7178, 7181, 7182, 7183, 7184, 7185, 7189, 7204, 7205, 7206, 7207, 7254, 8090, 8091, 8275, 8297, 8300, 8607, 8608, 8836, 8837, 8838, 8849, 8850, 9136
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_SCULPTEUR_DE_BATONS]),
                new CraftSkill(SkillIdEnum.SKILL_REPARER_UN_BATON, 20, new int[]
                    {
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_SCULPTEUR_DE_BATONS])
                ));

            m_templateById.Add((int)JobIdEnum.JOB_SCULPTEUR_DE_BAGUETTES, new JobTemplate(
                JobIdEnum.JOB_SCULPTEUR_DE_BAGUETTES,
                JobIdEnum.JOB_NONE,
                new CraftSkill(SkillIdEnum.SKILL_SCULPTER_UNE_BAGUETTE, 1, new int[]
                    {
                        132, 133, 134, 162, 163, 164, 165, 168, 174, 180, 181, 333, 334, 469, 830, 831, 832, 833, 834, 835, 1101, 1102, 1103, 1104, 1105, 1107, 1108, 1131, 1132, 1133, 1134, 1135, 1136, 1137, 1138, 1139, 1140, 1143, 1144, 1145, 1146, 1356, 1357, 1359, 1360, 1361, 1362, 1503, 1739, 5999, 6438, 6439, 6440, 6494, 6495, 6496, 6497, 6518, 6519, 6520, 7103, 7110, 7165, 7167, 7168, 7169, 7170, 7171, 7172, 7173, 7368, 8088, 8089, 8110, 8118, 8296, 8611, 8612, 8834, 8835, 9135
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_SCULPTEUR_DE_BAGUETTES]),
                new CraftSkill(SkillIdEnum.SKILL_REPARER_UNE_BAGUETTE, 20, new int[]
                    {
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_SCULPTEUR_DE_BAGUETTES])
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
                    new CraftSkill(SkillIdEnum.SKILL_EGRENER, 1, new int[]
                    {
                        744, 754, 758, 759, 772, 773, 774, 775, 776, 777, 779, 781, 790, 932, 933, 934, 935, 936, 937, 945, 946, 947, 948, 952, 953, 954, 955, 956, 957, 1500, 1693, 1695, 1910, 2061, 2380, 2381, 2382, 2383, 2385, 2386, 2387, 2412, 2413, 2414, 2445, 2446, 2473, 2489, 2532, 2534, 2547, 2629, 6449, 6720, 6756, 6757, 6763, 6775, 6795, 6922, 6927, 6931, 6940, 6941, 6942, 6943, 6954, 6993, 6994, 6995, 7137, 7138, 7174, 7230, 7231, 7232, 7233, 7340, 7515, 7552, 7884, 8007, 8112, 8117, 8134, 8181, 8231, 8232, 8233, 8234, 8235, 8236, 8265, 8279, 8280, 8281, 8286, 8302, 8366, 8443, 8452, 8458, 8464, 8472, 8639, 8640, 8641, 8642, 8643, 8644, 8645, 8646, 8647, 8648, 8649, 8650, 8818, 8819, 8866, 8867, 8876, 8919, 9141, 9142, 9180, 10846
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN]),
                    new CraftSkill(SkillIdEnum.SKILL_MOUDRE, 1, new int[]
                    {
                        285, 389, 390, 396, 397, 399, 529, 530, 531, 534, 535, 582, 583, 586, 587, 690, 2019, 2022, 2027, 2030, 2033, 2037, 6672, 7068
                    }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PAYSAN])
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
                    new HarvestSkill(SkillIdEnum.SKILL_PECHER_SARDINEBRILLANTE_1, 80, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR]),
                    new CraftSkill(SkillIdEnum.SKILL_VIDER_POISSON, 1, new int[]
                        {
                            1751, 1755, 1758, 1760, 1761, 1763, 1780, 1781, 1783, 1785, 1787, 1789, 1791, 1793, 1795, 1797, 1798, 1800, 1802, 1804, 1806, 1808, 1845, 1848, 1851, 1852, 1854, 1976
                        }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_PECHEUR])
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
                   new HarvestSkill(SkillIdEnum.SKILL_CUEILLIR_PANDOUILLE, 50, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_ALCHIMISTE]),
                   new CraftSkill(SkillIdEnum.SKILL_PREPARER_UNE_POTION, 1, new int[]
                       {
                            282, 283, 286, 548, 580, 793, 1182, 1183, 1333, 1335, 1337, 1338, 1340, 1341, 1342, 1343, 1345, 1346, 1347, 1348, 1405, 1406, 1461, 1462, 1542, 1587, 1588, 1589, 1590, 1591, 1592, 1593, 1594, 1595, 1596, 1597, 1598, 1599, 1600, 1601, 1642, 1643, 1644, 1645, 1646, 1647, 1648, 1649, 1686, 1687, 1688, 1689, 1692, 1712, 1713, 1741, 1742, 2183, 2184, 2185, 2186, 6964, 6965, 7314, 7315, 7316, 7317, 7318, 7319, 7320, 7321, 7322, 7323, 7324, 7325, 7326, 7327, 7328, 7329, 7421, 7498, 7505, 7506, 7507, 7652, 8106, 8883, 8913, 9035, 9038, 9040, 9041, 9869, 9870, 9882, 9883, 9884, 9885, 9886, 9887, 9888, 9897, 9898, 9899, 9900, 10208, 10209, 10210, 10211, 10212, 10213, 10214, 10215, 10216, 10217, 10218, 10219, 10750, 10751, 10752, 10753, 10754, 10755, 10756, 10757, 10758, 10759, 10760, 10761, 10762, 10763, 10764, 10765, 10766, 10767, 10768, 10769, 10770, 10771, 10772, 10773, 10774, 10775, 10776, 10777, 10778, 10779, 10780, 10781, 10782, 10783, 10809, 10811, 10883, 10884, 10885, 10886, 11010, 11036, 11037, 11038, 11039, 11040
                       }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_ALCHIMISTE])
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
                   new HarvestSkill(SkillIdEnum.SKILL_COLLECTER_SILICATE, 50, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_MINEUR]),
                   new CraftSkill(SkillIdEnum.SKILL_FONDRE, 1, new int[]
                       {
                            745, 746, 747, 748, 749, 750, 2529, 2538, 2541, 6457, 6458, 7035, 7036
                       }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_MINEUR]),
                   new CraftSkill(SkillIdEnum.SKILL_POLIR_UNE_PIERRE, 1, new int[]
                       {
                            315, 316, 465, 466, 467, 7026, 7027, 7028, 7461, 7462, 7463, 7464, 7465, 7466, 7467, 7468, 7469, 7470, 7471, 7472, 7473, 7474, 7475, 7476, 7477, 7478, 7479, 7480, 7481, 7482, 7483, 7484, 7485, 7486, 7487, 7488, 7489, 7490, 7491, 7492, 7508, 8377, 9941
                       }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_MINEUR])
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
                  }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_TAILLEUR]),
                  new CraftSkill(SkillIdEnum.SKILL_FILER, 1, new int[] { })
              ));

            m_templateById.Add((int)JobIdEnum.JOB_CORDONNIER, new JobTemplate(
                   JobIdEnum.JOB_CORDONNIER,
                   JobIdEnum.JOB_NONE,
                   new CraftSkill(SkillIdEnum.SKILL_CONFECTIONNER_DES_BOTTES, 1, new int[]
                   {
                       127, 128, 129, 130, 297, 298, 299, 348, 733, 769, 770, 784, 837, 861, 862, 863, 864, 888, 889, 890, 891, 892, 902, 903, 904, 905, 906, 907, 908, 909, 910, 912, 924, 1621, 1622, 1623, 1624, 1665, 1666, 2063, 2372, 2373, 2374, 2375, 2384, 2400, 2421, 2422, 2423, 2435, 2442, 2470, 2476, 2530, 2545, 3207, 6470, 6471, 6493, 6721, 6744, 6753, 6754, 6755, 6774, 6794, 6825, 6909, 6933, 6953, 7104, 7107, 7214, 7215, 7216, 7217, 7218, 7219, 7242, 7243, 7244, 7245, 7513, 7514, 7554, 7883, 8006, 8111, 8122, 8128, 8146, 8225, 8226, 8227, 8228, 8229, 8230, 8264, 8276, 8277, 8278, 8291, 8301, 8446, 8456, 8462, 8467, 8471, 8663, 8664, 8665, 8666, 8667, 8668, 8669, 8670, 8713, 8726, 8727, 8728, 8858, 8861, 8869, 9139, 9140, 9158, 9159, 9160, 9161, 9162, 9163, 9164, 9165, 9166, 9182
                   }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_CORDONNIER]),
                   new CraftSkill(SkillIdEnum.SKILL_CONFECTIONNER_UNE_CEINTURE, 1, new int[]
                   {
                       156, 203, 252, 253, 254, 255, 256, 257, 258, 259, 260, 261, 262, 263, 356, 734, 860, 1487, 1667, 1668, 1669, 1700, 1701, 1720, 2233, 2367, 2368, 2369, 2370, 2371, 2403, 2427, 2428, 2429, 2444, 2471, 2477, 2537, 2655, 2677, 2678, 2681, 2683, 2685, 2687, 2688, 2689, 2710, 2803, 2804, 2807, 2808, 2809, 2810, 2811, 3206, 6450, 6498, 6724, 6745, 6758, 6759, 6776, 6796, 6831, 6908, 6912, 6925, 6929, 6935, 6948, 6949, 6950, 6951, 6955, 7139, 7140, 7238, 7239, 7240, 7241, 7559, 7885, 7902, 8008, 8113, 8119, 8124, 8132, 8152, 8237, 8238, 8239, 8240, 8241, 8242, 8266, 8282, 8283, 8288, 8303, 8447, 8455, 8461, 8468, 8473, 8651, 8652, 8653, 8654, 8655, 8656, 8657, 8658, 8659, 8660, 8661, 8662, 8856, 8862, 8870, 8871, 8873, 9143, 9144, 9145, 9146, 9167, 9168, 9169, 9170, 9171, 9172, 9173, 9174, 9175, 9183, 9366
                   }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_CORDONNIER])
               ));

            m_templateById.Add((int)JobIdEnum.JOB_BIJOUTIER, new JobTemplate(
                   JobIdEnum.JOB_BIJOUTIER,
                   JobIdEnum.JOB_NONE,
                   new CraftSkill(SkillIdEnum.SKILL_CREER_UN_ANNEAU, 1, new int[]
                   {
                       100, 101, 102, 103, 109, 110, 111, 112, 118, 119, 120, 121, 278, 346, 355, 359, 732, 767, 768, 785, 787, 836, 841, 849, 850, 851, 852, 1087, 1493, 1494, 1495, 1496, 1497, 1498, 1499, 1559, 1602, 1656, 1657, 2418, 2419, 2420, 2441, 2469, 2475, 3203, 6463, 6464, 6465, 6467, 6468, 6469, 6506, 6722, 6732, 6743, 6748, 6749, 6767, 6791, 6819, 6910, 6911, 6915, 6919, 6920, 6928, 6944, 6945, 6946, 6947, 6956, 6961, 6996, 6997, 6998, 7116, 7117, 7118, 7119, 7120, 7121, 7122, 7123, 7128, 7131, 7132, 7246, 7247, 7248, 7249, 7341, 7555, 7881, 8004, 8109, 8121, 8126, 8136, 8149, 8219, 8220, 8221, 8222, 8223, 8224, 8263, 8269, 8270, 8271, 8289, 8299, 8448, 8454, 8460, 8466, 8470, 8714, 8715, 8716, 8717, 8718, 8719, 8720, 8721, 8722, 8723, 8724, 8725, 8859, 8860, 8865, 8872, 8877, 8879, 8881, 8991, 9122, 9123, 9124, 9125, 9126, 9127, 9128, 9129, 9131, 9132, 9133, 9148, 9177, 9178
                   }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BIJOUTIER]),
                   new CraftSkill(SkillIdEnum.SKILL_CREER_UNE_AMULETTE, 1, new int[]
                   {
                       39, 68, 69, 70, 74, 75, 76, 77, 81, 82, 83, 84, 157, 158, 159, 160, 161, 279, 280, 323, 324, 325, 326, 458, 610, 616, 617, 766, 783, 786, 867, 916, 917, 1330, 1331, 1474, 1476, 1477, 1478, 1480, 1481, 1482, 1483, 1484, 1485, 1486, 1489, 1490, 1491, 1619, 1661, 1662, 2388, 2389, 2390, 2391, 2392, 2393, 2394, 2395, 2396, 2397, 2398, 2399, 2424, 2425, 2426, 2443, 2472, 2478, 2498, 4381, 4684, 5122, 6443, 6444, 6459, 6460, 6461, 6462, 6466, 6499, 6723, 6731, 6742, 6746, 6747, 6766, 6789, 6817, 6918, 6923, 6934, 6999, 7000, 7001, 7002, 7003, 7012, 7106, 7136, 7221, 7250, 7251, 7252, 7253, 7342, 7880, 8003, 8108, 8120, 8123, 8129, 8150, 8213, 8214, 8215, 8216, 8217, 8218, 8262, 8268, 8272, 8273, 8290, 8298, 8445, 8453, 8459, 8465, 8469, 8863, 8874, 8880, 9130, 9149, 9150, 9151, 9152, 9153, 9154, 9155, 9156, 9157, 9179, 9463, 9464, 10836
                   }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BIJOUTIER])
               ));

            m_templateById.Add((int)JobIdEnum.JOB_BOULANGER, new JobTemplate(
                    JobIdEnum.JOB_BOULANGER,
                    JobIdEnum.JOB_NONE,
                    new CraftSkill(SkillIdEnum.SKILL_CUIRE_DU_PAIN, 1, new int[]
                        {
                            468, 520, 521, 522, 524, 526, 527, 528, 536, 539, 692, 1737, 1738, 2020, 2024, 2025, 2028, 2031, 2038, 2635, 2636, 10079
                        }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BOULANGER])
                ));

            m_templateById.Add((int)JobIdEnum.JOB_BRICOLEUR, new JobTemplate(
                    JobIdEnum.JOB_BRICOLEUR,
                    JobIdEnum.JOB_NONE,
                    new CraftSkill(SkillIdEnum.SKILL_BRICOLER, 1, new int[]
                        {
                            7590,7591,7592,7593,7594,7595,7596,7597,7598,7599,7600,7601,7602,7603,7604,7607,7608,7609,7610,7611,7612,7613,7614,7615,7616,7617,7618,7619,7620,7621,7622,7623,7624,7626,7627,7629,7635,7636,7637,7673,7674,7675,7676,7677,7678,7679,7682,7683,7684,7685,7686,7687,7688,7689,7690,7691,7692,7693,7694,7695,7696,7697,7698,7699,7700,7733,7734,7735,7736,7737,7738,7739,7740,7741,7742,7743,7744,7745,7746,7755,7756,7757,7758,7759,7760,7761,7762,7763,7764,7765,7766,7767,7768,7769,7770,7771,7772,7773,7774,7775,7776,7777,7778,7779,7780,7781,7782,7783,7784,7785,7786,7787,7788,7789,7790,7791,7792,7793,7794,7795,7796,7797,8990
                        }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BRICOLEUR]),
                    new CraftSkill(SkillIdEnum.SKILL_CONFECTIONNER_UNE_CLEF, 1, new int[]
                        {
                            1568,1569,1570,6884,7309,7310,7311,7312,7511,7557,7908,7918,7924,7926,7927,8135,8139,8142,8143,8156,8307,8320,8342,8343,8436,8437,8438,8439,8917,9247,9248,9254
                        }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BRICOLEUR])
                ));

            m_templateById.Add((int)JobIdEnum.JOB_CHASSEUR, new JobTemplate(
                    JobIdEnum.JOB_CHASSEUR,
                    JobIdEnum.JOB_NONE,
                    new CraftSkill(SkillIdEnum.SKILL_PREPARER, 1, new int[]
                        {
                            1987,1988,1989,1990,1991,1992,1993,1994,1995,1997,1998,1999,2000,2001,2002,2003,2004,2005,2006,2007,2008,2009,2010,2014,2015,2016,2017,8501,8502,8503
                        }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_CHASSEUR])
                ));

            m_templateById.Add((int)JobIdEnum.JOB_BOUCHER, new JobTemplate(
                    JobIdEnum.JOB_BOUCHER,
                    JobIdEnum.JOB_NONE,
                    new CraftSkill(SkillIdEnum.SKILL_PREPARER_UNE_VIANDE, 1, new int[]
                        {
                            1947,1948,1949,1950,1951,1952,1953,1954,1955,1956,1957,1958,1959,1960,1961,1962,1963,1964,1965,1966,1967,1968,1969,1970,1971,1972,2013,7509,7536,7537,7538,7539,7540,7541,7542,7543,7544,7545,7546,7547,7548,7549,7550,7551,7721,7722,7723,7724,7726,7727,7728,7729,7730,7731,7893,7894,8016,8017,8020,8171,8172,8173,8174,8504,8505,8506,8524,8565,8679,8706,8885,9595,9616,9661,9665,9666,9671,9786,10108,10109,10658,10867,10868,10869,10870,10986
                        }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_BOUCHER])
                ));

            m_templateById.Add((int)JobIdEnum.JOB_POISSONNIER, new JobTemplate(
                    JobIdEnum.JOB_POISSONNIER,
                    JobIdEnum.JOB_NONE,
                    new CraftSkill(SkillIdEnum.SKILL_PREPARER_UN_POISSON, 1, new int[]
                        {
                            1752,1753,1756,1764,1765,1766,1767,1768,1769,1809,1810,1811,1812,1813,1814,1815,1816,1817,1818,1819,1820,1821,1822,1823,1824,1825,1826,1827,1828,1829,1830,1831,1832,1833,1834,1835,1836,1837,1838,1839,1840,1859
                        }, WorldConfig.JOB_TOOLS[JobIdEnum.JOB_POISSONNIER])
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
            if (m_templateById.ContainsKey(id))
                return m_templateById[id];
            return null;
        }
    }
}
