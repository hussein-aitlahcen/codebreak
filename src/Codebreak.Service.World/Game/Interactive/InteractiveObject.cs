using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Job;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Interactive
{
    /// <summary>
    /// 
    /// </summary>
    public enum InteractiveObjectIdEnum
    {
        INTERACTIVE_ALEMBIC = 62,
        INTERACTIVE_ORME = 30,
        INTERACTIVE_ERABLE = 31,
        INTERACTIVE_PLAN_DE_TRAVAIL = 95,
        INTERACTIVE_DOLOMITE = 113,
        INTERACTIVE_MACHINE_A_COUDRE = 86,
        INTERACTIVE_TABLE_MAGIQUE = 118,
        INTERACTIVE_CHARME = 32,
        INTERACTIVE_NOYER = 34,
        INTERACTIVE_PLAN_DE_TRAVAIL_1 = 96,
        INTERACTIVE_MARMITE = 60,
        INTERACTIVE_COFFRE = 85,
        INTERACTIVE_ATELIER_MAGIQUE = 117,
        INTERACTIVE_EDELWEISS = 61,
        INTERACTIVE_FRENE = 1,
        INTERACTIVE_EBENE = 29,
        INTERACTIVE_ATELIER_DE_BRICOLAGE = 122,
        INTERACTIVE_PICHON = 100,
        INTERACTIVE_ORCHIDEE_FREYESQUE = 68,
        INTERACTIVE_LISTE_DES_ARTISANS = 119,
        INTERACTIVE_MACHINE_A_COUDRE_1 = 58,
        INTERACTIVE_COTON = 82,
        INTERACTIVE_FROMENT = 63,
        INTERACTIVE_POISSONS_GEANTS_MER = 81,
        INTERACTIVE_MOULE = 27,
        INTERACTIVE_EPEAUTRE = 64,
        INTERACTIVE_SCIE = 2,
        INTERACTIVE_IF = 28,
        INTERACTIVE_BLE = 38,
        INTERACTIVE_KALIPTUS = 121,
        INTERACTIVE_ENCLUME = 57,
        INTERACTIVE_ENCLOS = 120,
        INTERACTIVE_PLAN_DE_TRAVAIL_2 = 94,
        INTERACTIVE_MERISIER = 35,
        INTERACTIVE_MANGANESE = 54,
        INTERACTIVE_POUBELLE = 105,
        INTERACTIVE_PIERRE_CUIVREE = 53,
        INTERACTIVE_SORGHO = 65,
        INTERACTIVE_CHATAIGNIER = 33,
        INTERACTIVE_POISSON_DE_FRIGOST = 132,
        INTERACTIVE_MACHINE_A_COUDRE_MAGIQUE = 116,
        INTERACTIVE_CONCASSEUR_MUNSTER = 93,
        INTERACTIVE_ARGENT = 24,
        INTERACTIVE_ETABLI_EN_BOIS = 88,
        INTERACTIVE_BRONZE = 55,
        INTERACTIVE_PIERRE_DE_BAUXITE = 26,
        INTERACTIVE_SILICATE = 114,
        INTERACTIVE_LEVIER = 127,
        INTERACTIVE_TREFLE_A_5_FEUILLES = 67,
        INTERACTIVE_BAMBOU = 108,
        INTERACTIVE_GROS_POISSONS_RIVIERE = 76,
        INTERACTIVE_AVOINE = 45,
        INTERACTIVE_STATUE_DE_CLASSE = 128,
        INTERACTIVE_POISSONS_RIVIERE = 74,
        INTERACTIVE_CHANVRE = 46,
        INTERACTIVE_OR = 25,
        INTERACTIVE_SOURCE_DE_JOUVENCE = 56,
        INTERACTIVE_CHAUDRON = 15,
        INTERACTIVE_POISSONS_GEANTS_RIVIERE = 79,
        INTERACTIVE_ENCLUME_A_BOUCLIERS = 107,
        INTERACTIVE_ORGE = 43,
        INTERACTIVE_ZAAP = 16,
        INTERACTIVE_TAS_DE_PATATES = 48,
        INTERACTIVE_PANDOUILLE = 112,
        INTERACTIVE_TRUITE_VASEUSE = 80,
        INTERACTIVE_PWOULPE = 73,
        INTERACTIVE_MEULE = 41,
        INTERACTIVE_PLAN_DE_TRAVAIL_3 = 97,
        INTERACTIVE_PORTE = 70,
        INTERACTIVE_ZAAPI = 106,
        INTERACTIVE_FILEUSE = 83,
        INTERACTIVE_PERCE_NEIGE = 131,
        INTERACTIVE_MALT = 47,
        INTERACTIVE_PETITS_POISSONS_RIVIERE = 75,
        INTERACTIVE_MENTHE_SAUVAGE = 66,
        INTERACTIVE_ETABLI_PYROTECHNIQUE = 103,
        INTERACTIVE_BOMBU = 98,
        INTERACTIVE_ATELIER = 12,
        INTERACTIVE_SEIGLE = 44,
        INTERACTIVE_FER = 17,
        INTERACTIVE_ETABLI = 13,
        INTERACTIVE_POISSONS_MER = 77,
        INTERACTIVE_MACHINE_DE_FORCE = 102,
        INTERACTIVE_PUITS = 84,
        INTERACTIVE_PIERRE_DE_KOBALTE = 37,
        INTERACTIVE_OLIVIOLET = 101,
        INTERACTIVE_CHENE = 8,
        INTERACTIVE_MOULIN = 40,
        INTERACTIVE_OMBRE_ETRANGE = 99,
        INTERACTIVE_KOINKOIN = 104,
        INTERACTIVE_SOMOON_AGRESSIF = 72,
        INTERACTIVE_OBSIDIENNE = 135,
        INTERACTIVE_TABLE_A_PATATES = 49,
        INTERACTIVE_PETITS_POISSONS_MER = 71,
        INTERACTIVE_FOUR = 22,
        INTERACTIVE_FROSTIZZ = 134,
        INTERACTIVE_GROS_POISSONS_MER = 78,
        INTERACTIVE_ETAIN = 52,
        INTERACTIVE_LIN = 42,
        INTERACTIVE_ALAMBIC = 90,
        INTERACTIVE_RIZ = 111,
        INTERACTIVE_TABLE_DE_CONFECTION = 11,
        INTERACTIVE_CONCASSEUR = 50,
        INTERACTIVE_HOUBLON = 39,
        INTERACTIVE_BAMBOU_SOMBRE = 109,
        INTERACTIVE_ENCLUME_MAGIQUE = 92,
        INTERACTIVE_TREMBLE = 133,
        INTERACTIVE_BAMBOU_SACRE = 110,        
        INTERACTIVE_MORTIER_ET_PILON = 69,
        INTERACTIVE_PHOENIX = 200,
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class InteractiveObject : MessageDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
        public bool CanWalkThrough
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public MapInstance Map
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int CellId
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsActive
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        protected int m_frameId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cellId"></param>
        public InteractiveObject(MapInstance map, int cellId, bool canWalkThrough = false)
        {
            CanWalkThrough = canWalkThrough;
            Map = map;
            CellId = cellId;

            // STARTS ON ACTIVE STATE ? YA NIGGAE
            IsActive = true;
            m_frameId = 0;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentFrameId"></param>
        /// <param name="nextFrameId"></param>
        /// <param name="activated"></param>
        public void UpdateFrame(int currentFrameId, int nextFrameId, bool activated = false)
        {
            IsActive = activated;
            m_frameId = currentFrameId;

            SendUpdate();

            m_frameId = nextFrameId;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendUpdate()
        {
            base.Dispatch(WorldMessage.INTERACTIVE_DATA_FRAME(CellId, m_frameId, IsActive));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Activate()
        {
            IsActive = true;
            SendUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
            SendUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SerializeAs_InteractiveListMessage(StringBuilder message)
        {
            message.Append(CellId).Append(';');
            message.Append(m_frameId).Append(';');
            message.Append(IsActive ? '1' : '0').Append('|');
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="skillId"></param>
        public virtual void UseWithSkill(CharacterEntity character, JobSkill skill)
        {
            character.Dispatch(WorldMessage.SERVER_INFO_MESSAGE("Cet objet interactif est en cours de developpement."));
        }
    }
}
