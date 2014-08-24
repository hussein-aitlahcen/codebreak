using Codebreak.Framework.Configuration;
using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repositories;
using Codebreak.Service.World.Game.Database.Repositories;
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
            base.AddRepository(GuildRepository.Instance);
            base.AddRepository(CharacterGuildRepository.Instance);
            base.AddRepository(CharacterAlignmentRepository.Instance);
            base.AddRepository(CharacterRepository.Instance);
            base.AddRepository(MapTriggerRepository.Instance);
            base.AddRepository(MapRepository.Instance);
            base.AddRepository(NpcTemplateRepository.Instance);
            base.AddRepository(NpcInstanceRepository.Instance);
            base.AddRepository(SortsRepository.Instance);

            base.LoadAll(DbConnection);
        }

        /// <summary>
        /// 
        /// </summary>
        public void BeginTransaction()
        {
            SqlManager.Instance.BeginTransaction();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CommitTransaction()
        {
            SqlManager.Instance.CommitTransaction();
        }
    }
}
