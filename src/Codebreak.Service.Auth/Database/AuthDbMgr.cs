using System;
using Codebreak.Framework.Configuration;
using Codebreak.Framework.Database;
using Codebreak.Service.Auth.Database.Repository;

namespace Codebreak.Service.Auth.Database
{
    public sealed class AuthDbMgr : DbManager<AuthDbMgr>
    {
        [Configurable("DbConnection")]
		public static string DbConnection = "Server=localhost;Database=codebreak_auth;Uid=root;Pwd=;";

        public void Initialize()
        {
            base.AddRepository(AccountRepository.Instance);
            base.LoadAll(DbConnection);
        }
    }
}
