using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GuildRepository : Repository<GuildRepository, GuildDAO>
    {
        private Dictionary<long, GuildDAO> _guildById;
        private Dictionary<string, GuildDAO> _guildByName;

        /// <summary>
        /// 
        /// </summary>
        public GuildRepository()
        {
            _guildById = new Dictionary<long, GuildDAO>();
            _guildByName = new Dictionary<string, GuildDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GuildDAO GetByName(string name)
        {
            name = name.ToLower();
            if (_guildByName.ContainsKey(name))
                return _guildByName[name];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GuildDAO GetById(long id)
        {
            if (_guildById.ContainsKey(id))
                return _guildById[id];
            return null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guild"></param>
        public override void OnObjectAdded(GuildDAO guild)
        {
            _guildById.Add(guild.Id, guild);
            _guildByName.Add(guild.Name.ToLower(), guild);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public override void OnObjectRemoved(GuildDAO guild)
        {
            _guildById.Remove(guild.Id);
            _guildByName.Remove(guild.Name.ToLower());
        }
    }
}
