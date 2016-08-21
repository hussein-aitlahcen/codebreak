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
    public sealed class AddKamasEffect : AbstractActionEffect<AddKamasEffect>
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
            return Process(character, new Dictionary<string, string> { { "kamas", effect.RandomJet.ToString() } } );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool Process(Entity.CharacterEntity character, Dictionary<string, string> parameters)
        {
            var kamas = long.Parse(parameters["kamas"]);

            character.CachedBuffer = true;
            character.Inventory.AddKamas(kamas);
            character.Dispatch(WorldMessage.IM_INFO_MESSAGE(InformationEnum.INFO_KAMAS_WON, kamas));
            character.CachedBuffer = false;

            return true;
        }
    }
}
