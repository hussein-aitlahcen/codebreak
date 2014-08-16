using Codebreak.WorldService.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Area
{
    public sealed  class SuperAreaInstance : MessageDispatcher
    {
        private SuperAreaDAO _superAreaRecord;

        public SuperAreaInstance(SuperAreaDAO record)
        {
            _superAreaRecord = record;
        }
    }
}
