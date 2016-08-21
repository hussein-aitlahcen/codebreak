using Codebreak.Service.World.Game.Action;
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
    public sealed class TeleportEffect : AbstractActionEffect<TeleportEffect>
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
            var mapId = int.Parse(parameters["mapId"]);
            var cellId = int.Parse(parameters["cellId"]);

            // Not on map, maybe offline teleport or end fight teleport.
            if(!character.HasGameAction(GameActionTypeEnum.MAP) && character.CurrentAction == null)
            {
                character.MapId = mapId;
                character.CellId = cellId;

                return true;
            }

            if(!character.CanGameAction(GameActionTypeEnum.MAP_TELEPORT))
            {
                character.Dispatch(WorldMessage.IM_ERROR_MESSAGE(InformationEnum.ERROR_YOU_ARE_AWAY));
                return false;
            }

            character.Teleport(mapId, cellId);

            return true;
        }
    }
}
