using Codebreak.Framework.Database;

namespace Codebreak.Service.World.Database.Structures
{
    /// <summary>
    /// 
    /// </summary>
    [Table("maptrigger")]
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
