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
            _guildById.Add(instance.Id, instance);
            _guildByName.Add(instance.Name.ToLower(), instance);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool Create(CharacterEntity character, string name, int backgroundId, int backgroundColor, int symbolId, int symbolColor)
        {
            var record = new GuildDAO()
            {
                Name = name,
                BackgroundId = backgroundId,
                BackgroundColor = backgroundColor,
                SymbolId = symbolId,
                SymbolColor = symbolColor,
                Level = 1,
                BoostPoint = 0,
                Experience = 0,
            };

            var stats = GuildStatistics.Create(record);

            record.Stats = stats.Serialize();

            if (!GuildRepository.Instance.Insert(record))            
                return false;

            var instance = new GuildInstance(record);
            instance.MemberBoss(character);
            AddInstance(new GuildInstance(record));

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exists(string name)
        {
            return _guildByName.ContainsKey(name.ToLower());
        }
    }
}
