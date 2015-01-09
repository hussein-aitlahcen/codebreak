using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.Manager;

namespace Codebreak.Service.World.Frame
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class InventoryFrame : FrameBase<InventoryFrame, CharacterEntity, string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Action<CharacterEntity, string> GetHandler(string message)
        {
            if (message.Length < 2)
                return null;

            switch(message[0])
            {
                case 'O':
                    switch (message[1])
                    {
                        case 'M':
                            return ObjectMove;

                        case 'U':
                            return ObjectUse;

                        case 'd':
                            return ObjectDelete;

                        default:
                            return null;
                    }

                default:
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void ObjectMove(CharacterEntity character, string message)
        {            
            var data = message.Substring(2).Split('|');

            long itemId = -1;
            if(!long.TryParse(data[0], out itemId))
            {
                character.SafeDispatch(WorldMessage.OBJECT_MOVE_ERROR());
                return;
            }

            int slotId = -1;
            if(!int.TryParse(data[1], out slotId))
            {
                character.SafeDispatch(WorldMessage.OBJECT_MOVE_ERROR());
                return;
            }

            int quantity = 1;
            if(data.Length > 2)
            {
                if (!int.TryParse(data[2], out quantity))
                {
                    character.SafeDispatch(WorldMessage.OBJECT_MOVE_ERROR());
                    return;
                }
            }

            if(!Enum.IsDefined(typeof(ItemSlotEnum), slotId))
            {
                character.SafeDispatch(WorldMessage.OBJECT_MOVE_ERROR());
                return;
            }

            character.AddMessage(() =>
                {
                    var item = character.Inventory.Items.Find(x => x.Id == itemId);
                    if(item == null)
                    {
                        character.Dispatch(WorldMessage.OBJECT_MOVE_ERROR());
                        return;
                    }

                    character.Inventory.MoveItem(item, (ItemSlotEnum)slotId, quantity);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void ObjectUse(CharacterEntity character, string message)
        {
            var data = message.Substring(2);
            var useData = data.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            long itemId = -1;
            if (!long.TryParse(useData[0], out itemId))
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            long targetId = -1;
            if (useData.Length > 1)
            {
                if (!long.TryParse(useData[1], out targetId))
                {
                    character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }
            }

            int targetCell = -1;
            if (useData.Length > 2)
            {
                if (!int.TryParse(useData[2], out targetCell))
                {
                    character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }
            }

            character.AddMessage(() => 
                {
                    ActionEffectManager.Instance.ApplyEffects(character, itemId, targetId, targetCell);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void ObjectDelete(CharacterEntity character, string message)
        {
           
        }
    }
}
