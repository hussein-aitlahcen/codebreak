using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Network;
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
    public sealed class AddItemEffect : ActionEffectBase<AddItemEffect>
    {
        /// <summary>
        /// 
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
            var itemId = int.Parse(parameters["itemId"]);
            var template = ItemTemplateRepository.Instance.GetById(itemId);
            if (template == null)
                return false;

            character.CachedBuffer = true;
            character.Inventory.AddItem(template.Create());
            character.Dispatch(WorldMessage.SERVER_INFO_MESSAGE("Item " + template.Name + " added in your inventory."));
            character.CachedBuffer = false;

            return true;
        }
    }
}
