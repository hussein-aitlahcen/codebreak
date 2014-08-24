using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Database.Repositories
{
    public sealed class NpcTemplateRepository : Repository<NpcTemplateRepository, NpcTemplateDAO>
    {
        private Dictionary<int, NpcTemplateDAO> _templateById;

        public NpcTemplateRepository()
        {
            _templateById = new Dictionary<int, NpcTemplateDAO>();
        }

        public override void OnObjectAdded(NpcTemplateDAO template)
        {
            _templateById.Add(template.Id, template);
        }

        public override void OnObjectRemoved(NpcTemplateDAO template)
        {
            _templateById.Remove(template.Id);
        }

        public NpcTemplateDAO GetTemplate(int id)
        {
            if (_templateById.ContainsKey(id))
                return _templateById[id];
            return null;
        }
    }
}
