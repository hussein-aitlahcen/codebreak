using Codebreak.Framework.Configuration;
using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Game.Database.Repository;
using Codebreak.WorldService;

namespace Codebreak.Service.World.Database
{
    public sealed class WorldDbMgr : DbManager<WorldDbMgr>
    {
        [Configurable("DbConnection")]
        public static string DbConnection = "Data Source=SMARKEN;Initial Catalog=codebreak_world;Integrated Security=True;Pooling=False";

        public void Initialize()
        {
            base.AddRepository(ExperienceTemplateRepository.Instance);
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

            base.LoadAll(DbConnection);
        }
    }
}
