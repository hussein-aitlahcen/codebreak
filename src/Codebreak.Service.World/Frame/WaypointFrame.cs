using Codebreak.Framework.Network;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Frame
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WaypointFrame: FrameBase<WaypointFrame, CharacterEntity, string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Action<CharacterEntity, string> GetHandler(string message)
        {
            switch(message[1])
            {
                case 'U': return WaypointUse;
                case 'V': return WaypointLeave; 
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void WaypointLeave(CharacterEntity character, string message)
        {
            character.AddMessage(() =>
                {
                    character.StopAction(GameActionTypeEnum.WAYPOINT);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void WaypointUse(CharacterEntity character, string message)
        {
            int mapId = int.Parse(message.Substring(2));

            character.AddMessage(() =>
            {
                var waypointAction = character.CurrentAction as GameWaypointAction;
                if(waypointAction == null)
                {
                    character.Dispatch(WorldMessage.WAYPOINT_USE_ERROR());
                    return;
                }

                if(mapId == character.MapId)
                {
                    character.Dispatch(WorldMessage.WAYPOINT_USE_ERROR());
                    return;
                }

                var waypoint = WaypointManager.Instance.GetByMapId(mapId);
                if(waypoint == null)
                {
                    character.Dispatch(WorldMessage.WAYPOINT_USE_ERROR());
                    return;
                }
                
                var price = 10 * (Math.Abs(waypoint.Map.X - character.Map.X) + Math.Abs(waypoint.Map.Y - character.Map.Y) - 1);
                if(character.Inventory.Kamas < price)
                {
                    character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_NOT_ENOUGH_KAMAS, price));
                    return;
                }

                character.CachedBuffer = true;
                character.Inventory.SubKamas(price);
                character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_KAMAS_LOST, price));
                character.StopAction(GameActionTypeEnum.WAYPOINT);
                character.CachedBuffer = false;

                var nearestCell = waypoint.Map.GetNearestCell(waypoint.CellId);
                if (nearestCell == -1)
                    nearestCell = waypoint.CellId;

                character.Teleport(waypoint.Map.Id, nearestCell);
            });
        }
    }
}
