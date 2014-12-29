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
    public sealed class AddLifeEffect : ActionEffectBase<AddLifeEffect>
    {
        const int EMOTE_EAT_BREAD = 17;

        public override bool Process(EntityBase entity, InventoryItemDAO item, GenericStats.GenericEffect effect, long targetId, int targetCell, Dictionary<string, string> parameters = null)
        {
            var heal = effect.Items;
            if (entity.Life + heal > entity.MaxLife)
                heal = entity.MaxLife - entity.Life;

            entity.Life += heal;

            if(entity.Type == EntityTypeEnum.TYPE_CHARACTER)
                entity.Dispatch(WorldMessage.ACCOUNT_STATS((CharacterEntity)entity));

            switch((ItemTypeEnum)item.GetTemplate().Type)
            {
                case ItemTypeEnum.TYPE_PAIN:
                    entity.Map.Dispatch(WorldMessage.EMOTE_PLAY(entity.Id, EMOTE_EAT_BREAD));
                    break;
                    
                default:
                    break;
            }

            return true;
        }
    }
}
