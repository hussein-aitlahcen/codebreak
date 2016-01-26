using Codebreak.Service.World.Game.Entity;
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
    public sealed class OpenBankEffect : ActionEffectBase<OpenBankEffect>
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
        public override bool ProcessItem(CharacterEntity character, Database.Structure.ItemDAO item, Stats.GenericEffect effect, long targetId, int targetCell)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool Process(CharacterEntity character, Dictionary<string, string> parameters)
        {
            if(!character.CanGameAction(Action.GameActionTypeEnum.EXCHANGE))
            {
                character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_YOU_ARE_AWAY));
                return false;
            }

            var taxe = character.Bank.Items.GroupBy(item => item.TemplateId).Count();
            if(character.Inventory.Kamas < taxe)
            {
                if (character.Bank.Kamas < taxe)
                {
                    character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_NOT_ENOUGH_KAMAS, taxe));
                    return false;
                }

                character.Bank.SubKamas(taxe);
            }
            else
            {
                character.Inventory.SubKamas(taxe);
            }

            character.CachedBuffer = true;
            character.ExchangeStorage(character.Bank);
            character.Dispatch(WorldMessage.IM_INFO_MESSAGE(InformationEnum.INFO_KAMAS_LOST, taxe));
            character.CachedBuffer = false;

            return true;
        }
    }
}
