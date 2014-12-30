using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
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
        private Dictionary<long, GuildDAO> m_guildById;
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
    }
}
