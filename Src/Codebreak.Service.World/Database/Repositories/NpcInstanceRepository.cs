using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NpcInstanceRepository : Repository<NpcInstanceRepository, NpcInstanceDAO>
    {
        public override void OnObjectAdded(NpcInstanceDAO instance)
        {
        }

        public override void OnObjectRemoved(NpcInstanceDAO instance)
        {
        }
    }
}
