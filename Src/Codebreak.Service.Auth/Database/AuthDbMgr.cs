using Codebreak.Framework.Configuration;
using Codebreak.Framework.Database;
using Codebreak.Service.Auth.Database.Repositories;

namespace Codebreak.Service.Auth.Database
{
    public sealed class AuthDbMgr : DbManager<AuthDbMgr>
    {
        [Configurable("DbConnection")]
        public static string DbConnection = "Data Source=SMARKEN;Initial Catalog=codebreak_auth;Integrated Security=True;Pooling=False";

        public void Initialize()
        {
            base.AddRepository(AccountRepository.Instance);
            base.LoadAll(DbConnection);
        }
    }
}
