using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repositories;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structures
{
    /// <summary>
    /// 
    /// </summary>
    [Table("TaxCollector")]
    [ImplementPropertyChanged]
    public sealed class TaxCollectorDAO : DataAccessObject<TaxCollectorDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public long GuildId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public long OwnerId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int FirstName
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Name
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Skin
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int SkinSize
        { 
            get; 
            set; 
        }
        /// <summary>
        /// 
        /// </summary>
        public int MapId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int CellId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public long Kamas
        {
            get;
            set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private List<InventoryItemDAO> _items;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<InventoryItemDAO> GetItems()
        {
            if (_items == null)
                _items = InventoryItemRepository.Instance.GetByOwner(Id);
            return _items;
        }
    }
}
