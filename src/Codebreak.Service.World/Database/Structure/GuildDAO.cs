using Codebreak.Framework.Database;
using Codebreak.Service.World.Game.Guild;
using Codebreak.Service.World.Game.Stats;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
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
        private GuildStatistics m_statistics;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Write(false)]
        [DoNotNotify]
        public GuildStatistics Statistics
        {
            get
            {
                if (m_statistics == null)
                    m_statistics = GuildStatistics.Deserialize(Stats);
                return m_statistics;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnBeforeUpdate()
        {
            if (m_statistics != null)
                Stats = m_statistics.Serialize();
        }
    }
}
