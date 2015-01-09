using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
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
    public sealed class AddLifeEffect : ActionEffectBase<AddLifeEffect>
    {
        const int EMOTE_EAT_BREAD = 17;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="item"></param>
        /// <param name="effect"></param>
        /// <param name="targetId"></param>
        /// <param name="targetCell"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool ProcessItem(EntityBase entity, InventoryItemDAO item, GenericStats.GenericEffect effect, long targetId, int targetCell)
        {            
            if(targetId != -1)
            {
                entity = entity.Map.GetEntity(targetId);
                if (entity == null)
                    return false;
            }

            switch((ItemTypeEnum)item.Template.Type)
            {
                case ItemTypeEnum.TYPE_PAIN:
                    entity.EmoteUse(EMOTE_EAT_BREAD);
                    break;
                    
                default:
                    break;
            }

            return Process(entity, new Dictionary<string, string>() { { "life", effect.Items.ToString() } });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parameters"></param>
        public override bool Process(EntityBase entity, Dictionary<string, string> parameters)
        {
            var heal = int.Parse(parameters["life"]);
            if (entity.Life + heal > entity.MaxLife)
                heal = entity.MaxLife - entity.Life;
            entity.Life += heal;

            if (entity.Type == EntityTypeEnum.TYPE_CHARACTER)
            {
                entity.Dispatch(WorldMessage.ACCOUNT_STATS((CharacterEntity)entity));
                entity.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_LIFE_RECOVERED, heal));
            }

            return true;
        }
    }
}
