using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;
using PropertyChanged;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("npcinstance")]
    public sealed class NpcInstanceDAO : DataAccessObject<NpcInstanceDAO>
    {
        [Key]
        public int Id
        {
            get;
            set;
        }
        public int MapId
        {
            get;
            set;
        }
        public int TemplateId
        {
            get;
            set;
        }
        public int CellId
        {
            get;
            set;
        }
        public int Orientation
        {
            get;
            set;
        }
        public int QuestionId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private NpcTemplateDAO m_template;

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        [DoNotNotify]
        public NpcTemplateDAO Template
        {
            get
            {
                if (m_template == null || m_template.Id != TemplateId)
                    m_template = NpcTemplateRepository.Instance.GetById(TemplateId);
                return m_template;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private MapTemplateDAO m_map;

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        [DoNotNotify]
        public MapTemplateDAO Map
        {
            get
            {
                if (m_map == null || m_map.Id != MapId)
                    m_map = MapRepository.Instance.GetById(MapId);
                return m_map;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private NpcQuestionDAO m_question;

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        [DoNotNotify]
        public NpcQuestionDAO Question
        {
            get
            {
                if (m_question == null || m_question.Id != QuestionId)
                    m_question = NpcQuestionRepository.Instance.GetById(QuestionId);
                return m_question;
            }
        }
    }
}
