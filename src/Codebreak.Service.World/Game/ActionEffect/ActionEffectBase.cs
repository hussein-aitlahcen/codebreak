using Codebreak.Framework.Generic;
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
    public abstract class ActionEffectBase<T> : Singleton<T>, IActionEffect
        where T : ActionEffectBase<T>, new()
    {
        public abstract bool Process(EntityBase entity, InventoryItemDAO item, GenericStats.GenericEffect effect, long targetId, int targetCell, Dictionary<string, string> parameters = null);
    }
}
