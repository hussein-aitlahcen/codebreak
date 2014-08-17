using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Database.Repository
{
    public sealed class MapRepository : Repository<MapRepository, MapTemplateDAO>
    {
        private Dictionary<int, MapTemplateDAO> _mapById;

        public MapRepository()
        {
            _mapById = new Dictionary<int, MapTemplateDAO>();
        }

        public override void OnObjectAdded(MapTemplateDAO map)
        {
            _mapById.Add(map.Id, map);
        }

        public override void OnObjectRemoved(MapTemplateDAO map)
        {
            _mapById.Remove(map.Id);
        }

        public List<MapTemplateDAO> GetMaps()
        {
            return _dataObjects;
        }
    }
}
