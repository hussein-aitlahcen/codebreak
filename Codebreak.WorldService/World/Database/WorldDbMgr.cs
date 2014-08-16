using Codebreak.Framework.Database;
using Codebreak.WorldService.World.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Database
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WorldDbMgr : DbManager<WorldDbMgr>
    {
        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            base.AddRepository(SubAreaRepository.Instance);
            base.AddRepository(AreaRepository.Instance);
            base.AddRepository(SuperAreaRepository.Instance);
            base.AddRepository(ItemTemplateRepository.Instance);
            base.AddRepository(InventoryItemRepository.Instance);
            base.AddRepository(SpellBookEntryRepository.Instance);
            base.AddRepository(CharacterAlignmentRepository.Instance);
            base.AddRepository(CharacterRepository.Instance);
            base.AddRepository(MapTriggerRepository.Instance);
            base.AddRepository(MapRepository.Instance);
            base.AddRepository(NpcTemplateRepository.Instance);
            base.AddRepository(NpcInstanceRepository.Instance);

            base.LoadAll(WorldConfig.DB_CONNECTION);
        }
    }
}
