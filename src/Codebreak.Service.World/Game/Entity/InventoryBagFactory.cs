using Codebreak.Framework.Generic;
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
    public sealed class InventoryBagFactory : Singleton<InventoryBagFactory>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public InventoryBag Create(EntityBase entity)
        {
            switch(entity.Type)
            {
                case EntityTypeEnum.TYPE_CHARACTER:
                    return new EntityInventory(entity, ((CharacterEntity)entity).Kamas);

                case EntityTypeEnum.TYPE_TAX_COLLECTOR:
                    return new EntityInventory(entity, ((TaxCollectorEntity)entity).Kamas);
            }

            return null;
        }
    }
}
