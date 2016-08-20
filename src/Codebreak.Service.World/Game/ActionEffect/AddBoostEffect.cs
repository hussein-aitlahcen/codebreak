using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Stats;
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
        public override bool ProcessItem(Entity.CharacterEntity character, Database.Structure.ItemDAO item, Stats.GenericEffect effect, long targetId, int targetCell)
        {
            // Unknown
            if (!WorldConfig.BOOST_ITEMS.ContainsKey(item.TemplateId))
                return false;

            return Process(character, new Dictionary<string, string>() { { "itemId", WorldConfig.BOOST_ITEMS[item.TemplateId].ToString() } });
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

            ItemSlotEnum slot = ItemSlotEnum.SLOT_INVENTORY;
            switch ((ItemTypeEnum)template.Type)
            {
                case ItemTypeEnum.TYPE_TRANSFORM:
                    slot = ItemSlotEnum.SLOT_BOOST_MUTATION;
                    break;

                case ItemTypeEnum.TYPE_PERSO_SUIVEUR:
                    slot = ItemSlotEnum.SLOT_BOOST_FOLLOWER;
                    break;

                case ItemTypeEnum.TYPE_BENEDICTION:
                    if (character.Inventory.Items.Any(entry => entry.Slot == ItemSlotEnum.SLOT_BOOST_BENEDICTION))                    
                        slot = ItemSlotEnum.SLOT_BOOST_BENEDICTION_1;                    
                    else                    
                        slot = ItemSlotEnum.SLOT_BOOST_BENEDICTION;                    
                    break;

                case ItemTypeEnum.TYPE_MALEDICTION:
                    if (character.Inventory.Items.Any(entry => entry.Slot == ItemSlotEnum.SLOT_BOOST_MALEDICTION))
                        slot = ItemSlotEnum.SLOT_BOOST_MALEDICTION_1;   
                    else
                        slot = ItemSlotEnum.SLOT_BOOST_MALEDICTION;
                    break;

                case ItemTypeEnum.TYPE_RP_BUFF:
                    slot = ItemSlotEnum.SLOT_BOOST_ROLEPLAY_BUFF;
                    break;

                case ItemTypeEnum.TYPE_BOOST_FOOD:
                    slot = ItemSlotEnum.SLOT_BOOST_FOOD;
                    break;
            }

            if(character.Inventory.Items.Any(entry => entry.Slot == slot || entry.TemplateId == itemId))
            {
                character.Dispatch(WorldMessage.IM_ERROR_MESSAGE(InformationEnum.ERROR_CONDITIONS_UNSATISFIED));
                return false;
            }

            var item = template.Create(1, slot);

            character.CachedBuffer = true;
            character.Statistics.Merge(StatsType.TYPE_BOOST, item.Statistics);
            character.Inventory.AddItem(item);
            character.SendAccountStats();
            character.CachedBuffer = false;

            return true;
        }
    }
}
