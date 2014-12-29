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
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void ObjectMove(CharacterEntity entity, string message)
        {            
            var data = message.Substring(2).Split('|');

            long itemId = -1;
            if(!long.TryParse(data[0], out itemId))
            {
                entity.SafeDispatch(WorldMessage.OBJECT_MOVE_ERROR());
                return;
            }

            int slotId = -1;
            if(!int.TryParse(data[1], out slotId))
            {
                entity.SafeDispatch(WorldMessage.OBJECT_MOVE_ERROR());
                return;
            }

            if(!Enum.IsDefined(typeof(ItemSlotEnum), slotId))
            {
                entity.SafeDispatch(WorldMessage.OBJECT_MOVE_ERROR());
                return;
            }

            entity.AddMessage(() =>
                {
                    var item = entity.Inventory.Items.Find(x => x.Id == itemId);
                    if(item == null)
                    {
                        entity.Dispatch(WorldMessage.OBJECT_MOVE_ERROR());
                        return;
                    }

                    entity.Inventory.MoveItem(item, (ItemSlotEnum)slotId);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void ObjectUse(CharacterEntity entity, string message)
        {
            var data = message.Substring(2);
            var useData = data.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            long itemId = -1;
            if (!long.TryParse(useData[0], out itemId))
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            long targetId = -1;
            if (useData.Length > 1)
            {
                if (!long.TryParse(useData[1], out targetId))
                {
                    entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }
            }

            int targetCell = -1;
            if (useData.Length > 2)
            {
                if (!int.TryParse(useData[2], out targetCell))
                {
                    entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                    return;
                }
            }

            entity.AddMessage(() => 
                {
                    var item = entity.Inventory.RemoveItem(itemId);
                    if(item == null)
                    {
                        entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    ActionEffectManager.Instance.ApplyEffects(entity, item, targetId, targetCell);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void ObjectDelete(CharacterEntity entity, string message)
        {
           
        }
    }
}
