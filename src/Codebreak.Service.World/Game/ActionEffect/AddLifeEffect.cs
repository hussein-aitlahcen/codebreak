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
        const int EMOTE_EAT_REST = 17;

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
        public override bool ProcessItem(CharacterEntity character, InventoryItemDAO item, GenericEffect effect, long targetId, int targetCell)
        {            
            if(targetId != -1)
            {
                var entity = character.Map.GetEntity(targetId);
                character = entity as CharacterEntity;
                if (character == null)
                    return false;
            }

            switch((ItemTypeEnum)item.Template.Type)
            {
                case ItemTypeEnum.TYPE_PAIN:
                    character.EmoteUse(EMOTE_EAT_REST);
                    break;
                    
                default:
                    break;
            }
                        
            return Process(character, new Dictionary<string, string>() { { "life", effect.RandomJet.ToString() } });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parameters"></param>
        public override bool Process(CharacterEntity character, Dictionary<string, string> parameters)
        {
            if (character.Life == character.MaxLife)
                return false;

            var heal = int.Parse(parameters["life"]);
            if (character.Life + heal > character.MaxLife)
                heal = character.MaxLife - character.Life;

            character.CachedBuffer = true; 
            character.Life += heal;
            character.SendAccountStats();
            character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_LIFE_RECOVERED, heal));
            character.CachedBuffer = false;

            return true;
        }
    }
}
