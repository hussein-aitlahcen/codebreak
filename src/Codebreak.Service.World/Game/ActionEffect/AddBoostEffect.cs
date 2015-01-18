using Codebreak.Service.World.Database.Repository;
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
    public sealed class AddBoostEffect : ActionEffectBase<AddBoostEffect>
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
            // Unknown
            if (!WorldConfig.BOOST_ITEMS.ContainsKey(item.TemplateId))
                return false;

            return Process(character, new Dictionary<string, string>() { { "item", WorldConfig.BOOST_ITEMS[item.TemplateId].ToString() } });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool Process(Entity.CharacterEntity character, Dictionary<string, string> parameters)
        {
            var itemId = int.Parse(parameters["item"]);
            var template = ItemTemplateRepository.Instance.GetById(itemId);
            if (template == null)
                return false;

            var item = template.Create(1, Database.Structure.ItemSlotEnum.SLOT_BOOST);

            character.CachedBuffer = true;
            character.Statistics.Merge(StatsType.TYPE_BOOST, item.Statistics);
            character.Inventory.AddItem(item);
            character.SendAccountStats();
            character.CachedBuffer = false;

            return true;
        }
    }
}
