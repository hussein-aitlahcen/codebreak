using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Game.Action;
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
    public sealed class NpcDialogReplyEffect : ActionEffectBase<NpcDialogReplyEffect>
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
            ((GameNpcDialogAction)entity.CurrentAction).Dialog.SendQuestion(NpcQuestionRepository.Instance.GetById(int.Parse(parameters["questionId"])));

            return true;
        }
    }
}
