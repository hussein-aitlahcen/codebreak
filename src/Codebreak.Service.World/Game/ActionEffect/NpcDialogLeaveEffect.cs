using Codebreak.Service.World.Game.Entity;
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
    public sealed class NpcDialogLeaveEffect : ActionEffectBase<NpcDialogLeaveEffect>
    {
        /// <summary>
        /// SHOULD NEVER BE CALLED
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="item"></param>
        /// <param name="effect"></param>
        /// <param name="targetId"></param>
        /// <param name="targetCell"></param>
        /// <returns></returns>
        public override bool ProcessItem(EntityBase entity, Database.Structure.InventoryItemDAO item, Stats.GenericStats.GenericEffect effect, long targetId, int targetCell)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parameters"></param>
        public override bool Process(EntityBase entity, Dictionary<string, string> parameters)
        {
            entity.StopAction(Action.GameActionTypeEnum.NPC_DIALOG);

            return true;
        }
    }
}
