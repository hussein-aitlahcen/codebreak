using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Database.Repository
{
    public sealed class ItemTemplateRepository : Repository<ItemTemplateRepository, ItemTemplateDAO>
    {
        private Dictionary<int, ItemTemplateDAO> _templateById;

        public ItemTemplateRepository()
        {
            _templateById = new Dictionary<int, ItemTemplateDAO>();
        }

        public override void OnObjectAdded(ItemTemplateDAO template)
        {
            _templateById.Add(template.Id, template);
        }

        public override void OnObjectRemoved(ItemTemplateDAO template)
        {
            _templateById.Remove(template.Id);
        }
        
        public ItemTemplateDAO GetTemplate(int templateId)
        {
            if(_templateById.ContainsKey(templateId))
                return _templateById[templateId];
            return null;
        }
    }
}
