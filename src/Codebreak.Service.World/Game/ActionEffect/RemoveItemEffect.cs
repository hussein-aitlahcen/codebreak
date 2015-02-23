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
    public sealed class RemoveItemEffect : ActionEffectBase<RemoveItemEffect>
    {
        /// <summary>
        /// SHOULD NEVER BE CALLED EXCEPT IF WE CREATE A NEW ITEM WITH THAT ACTION
        /// </summary>
        /// <param name="character"></param>
        /// <param name="item"></param>
        /// <param name="effect"></param>
        /// <param name="targetId"></param>
        /// <param name="targetCell"></param>
        /// <returns></returns>
        public override bool ProcessItem(Entity.CharacterEntity character, Database.Structure.InventoryItemDAO item, Stats.GenericEffect effect, long targetId, int targetCell)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool Process(Entity.CharacterEntity character, Dictionary<string, string> parameters)
        {
            var templateId = int.Parse(parameters["templateId"]);

            var item = character.Inventory.Items.Find(entry => entry.TemplateId == templateId);
            if (item == null)
                return false;

            character.Inventory.RemoveItem(item.Id);

            return true;
        }
    }
}
