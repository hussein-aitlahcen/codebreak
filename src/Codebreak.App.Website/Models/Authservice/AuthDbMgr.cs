using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebreak.App.Website.Models.Authservice
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthDbMgr : DbManager<AuthDbMgr>
    {
        /// <summary>
        /// Load all data from the database.
        /// </summary>
        /// <param name="connectionString"></param>
        public override void LoadAll(string connectionString)
        {
            base.LoadAll(connectionString);
        }
    }
}