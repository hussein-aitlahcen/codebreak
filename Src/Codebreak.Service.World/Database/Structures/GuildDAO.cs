using Codebreak.Framework.Database;
using Codebreak.Service.World.Game.Guild;
using Codebreak.Service.World.Game.Stats;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structures
{
    /// <summary>
    /// 
    /// </summary>
    [Table("guild")]
    [ImplementPropertyChanged]
    public sealed class GuildDAO : DataAccessObject<GuildDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int SymbolId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int SymbolColor
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int BackgroundId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int BackgroundColor
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Level
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public long Experience
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] Stats
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int BoostPoint
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private GuildStatistics _statistics;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GuildStatistics GetStatistics()
        {
            if (_statistics == null)
                _statistics = GuildStatistics.Deserialize(Stats);
            return _statistics;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnBeforeUpdate()
        {
            if (_statistics != null)
                Stats = _statistics.Serialize();
        }
    }
}
