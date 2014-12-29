using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.ActionEffect
{
    public interface IActionEffect
    {
        bool Process(EntityBase entity, InventoryItemDAO item, GenericStats.GenericEffect effect, long targetId, int targetCell, Dictionary<string, string> parameters = null);
    }
}
