using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebreak.App.Website.Models.Website
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WebDbMgr : DbManager<WebDbMgr>
    {
        /// <summary>
        /// Load all data from the database.
        /// </summary>
        /// <param name="connectionString"></param>
        public override void LoadAll(string connectionString)
        {
            base.AddRepository(ConfigVariableRepository.Instance);

            base.LoadAll(connectionString);
        }
    }
}