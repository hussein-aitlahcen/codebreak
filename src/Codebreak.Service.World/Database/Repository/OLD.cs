using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SortsRepository : Repository<SortsRepository, sorts>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public override void UpdateAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
            // NO UPDATE
        }
    }
}
