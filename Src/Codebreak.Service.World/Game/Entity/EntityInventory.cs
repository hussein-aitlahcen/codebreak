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
        public override long Kamas
        {
            get;
            set;
        }

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
        public EntityInventory(EntityBase entity, long kamas)
            : base(entity)
        {
            m_items = new List<InventoryItemDAO>();
            m_items.AddRange(Database.Repository.InventoryItemRepository.Instance.GetByOwner((int)entity.Type, entity.Id));
            foreach (var item in Items)            
                if (item.IsEquiped())                
                    Entity.Statistics.Merge(item.GetStatistics());               
        }
    }
}
