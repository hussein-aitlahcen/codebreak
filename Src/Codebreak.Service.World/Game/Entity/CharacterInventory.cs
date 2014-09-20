using Codebreak.Service.World.Database.Structures;
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
    public sealed class CharacterInventory : InventoryBag
    {
        /// <summary>
        /// 
        /// </summary>
        public override long Kamas
        {
            get
            {
                return _character.Kamas;
            }
            set
            {
                _character.Kamas = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override List<InventoryItemDAO> Items
        {
            get 
            { 
                return _character.Items;
            }
        }

        private CharacterEntity _character;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public CharacterInventory(CharacterEntity character)
            : base(character)
        {
            _character = character;

            foreach (var item in Items)            
                if (item.IsEquiped())                
                    Entity.Statistics.Merge(item.GetStatistics());               
        }
    }
}
