using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Repositories;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Guild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GuildManager : Singleton<GuildManager>
    {
        private Dictionary<long, GuildInstance> _guildById;
        private Dictionary<string, GuildInstance> _guildByName;

        /// <summary>
        /// 
        /// </summary>
        public GuildManager()
        {
            _guildById = new Dictionary<long, GuildInstance>();
            _guildByName = new Dictionary<string, GuildInstance>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {            
            foreach(var guild in GuildRepository.Instance.GetAll())
            {
                var instance = new GuildInstance(guild);

                WorldService.Instance.AddUpdatable(instance);
                _guildById.Add(guild.Id, instance);
                _guildByName.Add(guild.Name.ToLower(), instance);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guildId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public GuildMember GetMember(long guildId, long memberId)
        {
            if (!_guildById.ContainsKey(guildId))
                return null;
            return _guildById[guildId].GetMember(memberId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guildId"></param>
        /// <returns></returns>
        public GuildInstance GetGuild(long guildId)
        {
            if (_guildById.ContainsKey(guildId))
                return _guildById[guildId];
            return null;
        }
    }
}
