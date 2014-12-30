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
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ActionEffectBase<T> : Singleton<T>, IActionEffect
        where T : ActionEffectBase<T>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="item"></param>
        /// <param name="effect"></param>
        /// <param name="targetId"></param>
        /// <param name="targetCell"></param>
        /// <returns></returns>
        public abstract bool ProcessItem(EntityBase entity, InventoryItemDAO item, GenericStats.GenericEffect effect, long targetId, int targetCell);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parameters"></param>
        public abstract bool Process(EntityBase entity, Dictionary<string, string> parameters);
    }
}
