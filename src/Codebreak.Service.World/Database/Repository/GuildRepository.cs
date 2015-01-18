using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Guild;
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
    public sealed class GuildRepository : Repository<GuildRepository, GuildDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        public long NextGuildId
        {
            get
            {
                lock (m_syncLock)
                    return m_nextGuildId++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private long m_nextGuildId;


        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, GuildDAO> m_guildById;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, GuildDAO> m_guildByName;

        /// <summary>
        /// 
        /// </summary>
        public GuildRepository()
        {
            m_guildById = new Dictionary<long, GuildDAO>();
            m_guildByName = new Dictionary<string, GuildDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GuildDAO GetByName(string name)
        {
            name = name.ToLower();
            if (m_guildByName.ContainsKey(name))
                return m_guildByName[name];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GuildDAO GetById(long id)
        {
            if (m_guildById.ContainsKey(id))
                return m_guildById[id];
            return null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guild"></param>
        public override void OnObjectAdded(GuildDAO guild)
        {
            m_guildById.Add(guild.Id, guild);
            m_guildByName.Add(guild.Name.ToLower(), guild);

            if (guild.Id >= m_nextGuildId)
                m_nextGuildId = guild.Id + 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public override void OnObjectRemoved(GuildDAO guild)
        {
            m_guildById.Remove(guild.Id);
            m_guildByName.Remove(guild.Name.ToLower());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="backgroundId"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="symbolId"></param>
        /// <param name="symbolColor"></param>
        /// <returns></returns>
        public GuildDAO Create(string name, int backgroundId, int backgroundColor, int symbolId, int symbolColor)
        {
            var instance = new GuildDAO()
            {
                Id = NextGuildId,
                Name = name,
                BackgroundId = backgroundId,
                BackgroundColor = backgroundColor,
                SymbolId = symbolId,
                SymbolColor = symbolColor,
                Level = 1,
                BoostPoint = 0,
                Experience = 0,
            };

            var stats = GuildStatistics.Create(instance);

            instance.Stats = stats.Serialize();

            base.Created(instance);

            return instance;
        }
    }
}
