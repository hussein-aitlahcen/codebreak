using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;

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

        private NpcTemplateDAO m_template;

        [Write(false)]
        public NpcTemplateDAO Template
        {
            get
            {
                if (m_template == null)
                    m_template = NpcTemplateRepository.Instance.GetTemplate(TemplateId);
                return m_template;
            }
        }
    }
}
