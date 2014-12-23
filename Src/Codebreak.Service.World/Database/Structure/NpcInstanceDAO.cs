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

        private NpcTemplateDAO _template;

        public NpcTemplateDAO GetTemplate()
        {
            if (_template == null)
                _template = NpcTemplateRepository.Instance.GetTemplate(TemplateId);
            return _template;
        }
    }
}
