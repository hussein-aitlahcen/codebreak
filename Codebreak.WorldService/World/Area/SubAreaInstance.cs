using Codebreak.WorldService.World.Database.Structure;
using Codebreak.WorldService.World.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Area
{
    public sealed class SubAreaInstance : MessageDispatcher
    {
        private SubAreaDAO _subAreaRecord;
        private AreaInstance _area;

        public AreaInstance Area
        {
            get
            {
                if (_area == null)
                    _area = AreaManager.Instance.GetArea(_subAreaRecord.AreaId);
                return _area;
            }
        }

        public SubAreaInstance(SubAreaDAO record)
        {
            _subAreaRecord = record;
        }
    }
}
