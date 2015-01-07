using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Exchange
{
    /// <summary>
    /// 
    /// </summary>
    public class StorageExchange : ExchangeBase
    {
        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity Character
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public PersistentInventory Storage
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="type"></param>
        public StorageExchange(CharacterEntity character, PersistentInventory storage, ExchangeTypeEnum type = ExchangeTypeEnum.EXCHANGE_STORAGE)
            : base(type)
        {
            Character = character;
            Storage = storage;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Create()
        {
            if(Storage.ActualUser != -1)
            {
                Character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_STORAGE_ALREADY_IN_USE));
                Character.AddMessage(() => Character.StopAction(Action.GameActionTypeEnum.EXCHANGE));
                return;
            }

            Storage.ActualUser = Character.Id;

            Character.CachedBuffer = true;
            base.Create();
            SendItemsList();
            Character.CachedBuffer = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        public override void Leave(bool success = false)
        {
            if (Storage.ActualUser == Character.Id)
            {
                base.Leave(success);
                Storage.ActualUser = -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendItemsList()
        {
            Character.Dispatch(WorldMessage.EXCHANGE_TAXCOLLECTOR_ITEMS_LIST(Storage.Items, Storage.Kamas));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public override long MoveKamas(EntityBase actor, long quantity)
        {
            Character.CachedBuffer = true;

            if(quantity < 0)
            {
                quantity = Math.Abs(quantity);
                if (quantity > Storage.Kamas)
                    quantity = Storage.Kamas;

                Storage.SubKamas(quantity);
                Character.Inventory.AddKamas(quantity);
            }
            else
            {
                if (quantity > Character.Inventory.Kamas)
                    quantity = Character.Inventory.Kamas;
                
                Storage.AddKamas(quantity);
                Character.Inventory.SubKamas(quantity);
            }

            Character.Dispatch(WorldMessage.EXCHANGE_STORAGE_KAMAS_VALUE(Storage.Kamas));
            Character.CachedBuffer = false;

            return quantity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="guid"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public override int AddItem(EntityBase actor, long guid, int quantity, long price = -1)
        {
            var item = Character.Inventory.RemoveItem(guid, quantity);
            if (item == null)
                return 0;
            
            Character.CachedBuffer = true;
            Storage.AddItem(item);
            SendItemsList();
            Character.CachedBuffer = false;

            return item.Quantity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="guid"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public override int RemoveItem(EntityBase actor, long guid, int quantity)
        {
            var item = Storage.RemoveItem(guid, quantity);
            if (item == null)
                return 0;

            Character.CachedBuffer = true;
            Character.Inventory.AddItem(item);
            SendItemsList();
            Character.CachedBuffer = false;

            return item.Quantity;
        }
    }
}
