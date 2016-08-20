using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class InventoryItemRepository : Repository<InventoryItemRepository, ItemDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        public long NextItemId
        {
            get
            {
                lock (m_syncLock)
                    return m_nextItemId++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private long m_nextItemId;

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public override void OnObjectAdded(ItemDAO item)
        {
            if (item.Id >= m_nextItemId)
                m_nextItemId = item.Id + 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public ItemDAO GetById(long itemId)
        {
            return Find(item => item.Id == itemId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerType"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public IEnumerable<ItemDAO> GetByOwner(int ownerType, long ownerId)
        {
            return FindAll(item => item.OwnerType == ownerType && item.OwnerId == ownerId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        public void EntityRemoved(int type, long id)
        {
            base.Removed(base.FindAll(item => item.OwnerType == type && item.OwnerId == id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public override void InsertAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
            lock (m_syncLock)
                m_dataObjects.RemoveAll(item => item.IsNew && item.OwnerId == -1);

            base.InsertAll(connection, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public override void DeleteAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
            lock (m_syncLock)
                m_dataObjects.ForEach(item =>
                {
                    if (item.OwnerId == -1)
                        item.IsDeleted = true;
                });

            base.DeleteAll(connection, transaction);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ItemDAO Create(int templateId, long ownerId, int quantity, GenericStats statistics, ItemSlotEnum slot = ItemSlotEnum.SLOT_INVENTORY)
        {
            var instance = new ItemDAO();
            instance.Id = NextItemId;
            instance.OwnerId = -1;
            instance.TemplateId = templateId;
            instance.Quantity = quantity;
            instance.Effects = statistics.Serialize();
            instance.StringEffects = statistics.ToItemStats();
            instance.SlotId = (int)slot;

            base.Created(instance);

            return instance;
        }
    }
}
