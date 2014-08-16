using Codebreak.Framework.Database;
using Codebreak.WorldService.World.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("NpcInstance")]
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
