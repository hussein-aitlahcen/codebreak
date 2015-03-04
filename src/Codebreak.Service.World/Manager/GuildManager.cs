using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Guild;
using Codebreak.Service.World.Game.Stats;
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
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, GuildInstance> m_guildById;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, GuildInstance> m_guildByName;

        /// <summary>
        /// 
        /// </summary>
        public GuildManager()
        {
            m_guildById = new Dictionary<long, GuildInstance>();
            m_guildByName = new Dictionary<string, GuildInstance>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {            
            foreach(var guild in GuildRepository.Instance.All)
            {
                AddInstance(new GuildInstance(guild));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        private void AddInstance(GuildInstance instance)
        {
            WorldService.Instance.AddUpdatable(instance);
            m_guildById.Add(instance.Id, instance);
            m_guildByName.Add(instance.Name.ToLower(), instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guildId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public GuildMember GetMember(long guildId, long memberId)
        {
            if (!m_guildById.ContainsKey(guildId))
                return null;
            return m_guildById[guildId].GetMember(memberId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guildId"></param>
        /// <returns></returns>
        public GuildInstance GetGuild(long guildId)
        {
            if (m_guildById.ContainsKey(guildId))
                return m_guildById[guildId];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool Create(CharacterEntity character, string name, int backgroundId, int backgroundColor, int symbolId, int symbolColor)
        {
            AddInstance(new GuildInstance(GuildRepository.Instance.Create(name, backgroundId, backgroundColor, symbolId, symbolColor), character));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guild"></param>
        public void Destroy(GuildInstance guild)
        {
            m_guildById.Remove(guild.Id);
            m_guildByName.Remove(guild.Name.ToLower());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exists(string name)
        {
            return m_guildByName.ContainsKey(name.ToLower());
        }
    }
}
