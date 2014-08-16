using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("MapTrigger")]
    public sealed class MapTriggerDAO : DataAccessObject<MapTriggerDAO>
    {
        public int MapId
        {
            get;
            set;
        }
        public int CellId
        {
            get;
            set;
        }
        public int NewMap
        {
            get;
            set;
        }
        public int NewCell
        {
            get;
            set;
        }
    }
}
