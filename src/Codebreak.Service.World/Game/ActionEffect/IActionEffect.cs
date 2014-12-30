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
    public interface IActionEffect
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
        bool ProcessItem(EntityBase entity, InventoryItemDAO item, GenericStats.GenericEffect effect, long targetId, int targetCell);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool Process(EntityBase entity, Dictionary<string, string> parameters);
    }
}
