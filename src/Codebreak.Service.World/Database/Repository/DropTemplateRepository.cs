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
    public sealed class DropTemplateRepository : Repository<DropTemplateRepository, DropTemplateDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="drop"></param>
        public override void OnObjectAdded(DropTemplateDAO drop)
        {
            if(drop.Monster != null)
            {
                drop.Monster.AddDrop(drop);
            }
        }

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
