using Codebreak.WorldService.World.Database.Structure;
using Codebreak.WorldService.World.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Area
{
    public sealed class AreaInstance : MessageDispatcher
    {
        private AreaDAO _areaRecord;
        private SuperAreaInstance _superArea;
        
        public SuperAreaInstance SuperArea
        {
            get
            {
                if (_superArea == null)
                    _superArea = AreaManager.Instance.GetSuperArea(_areaRecord.SuperAreaId);
                return _superArea;
            }
        }

        public AreaInstance(AreaDAO record)
        {
            _areaRecord = record;
        }
    }
}
