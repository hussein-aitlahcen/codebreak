using Codebreak.AuthService.Auth.Database.Repository;
using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.AuthService.Auth.Database
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuthDbMgr : DbManager<AuthDbMgr>
    {
        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            base.AddRepository(AccountRepository.Instance);
            
            base.LoadAll(AuthConfig.DB_CONNECTION);
        }
    }
}
