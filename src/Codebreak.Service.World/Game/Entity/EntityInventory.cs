using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EntityInventory : InventoryBag
    {
        /// <summary>
        /// 
        /// </summary>
        public override List<InventoryItemDAO> Items
        {
            get 
            {
                return m_items;
            }
        }

        private List<InventoryItemDAO> m_items;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public EntityInventory(EntityBase entity)
            : base(entity)
        {
            m_items = new List<InventoryItemDAO>();
            m_items.AddRange(Database.Repository.InventoryItemRepository.Instance.GetByOwner((int)entity.Type, entity.Id));            
        }
    }
}
