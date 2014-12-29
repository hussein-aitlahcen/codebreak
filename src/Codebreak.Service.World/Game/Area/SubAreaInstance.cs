using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Game.Area
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
