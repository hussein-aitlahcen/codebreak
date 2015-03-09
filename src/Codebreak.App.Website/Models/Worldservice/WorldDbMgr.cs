using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebreak.App.Website.Models.Worldservice
{
    /// <summary>
    /// 
    /// </summary>
    public class WorldDbMgr : DbManager<WorldDbMgr>
    {
        /// <summary>
        /// Load all data from the database.
        /// </summary>
        /// <param name="connectionString"></param>
        public override void LoadAll(string connectionString)
        {
            base.AddRepository(CharacterRepository.Instance);

            base.LoadAll(connectionString);
        }
    }
}