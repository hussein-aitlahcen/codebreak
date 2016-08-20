using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Guild;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Mount
{
    public sealed class Paddock 
    {
        public int MapId
        {
            get { return m_record.MapId; }
        }
        public int GuildId
        {
            get { return m_record.GuildId; }
            set { m_record.GuildId = value; }
        }
        public long DefaultPrice
        {
            get { return m_record.DefaultPrice; }
        }
        public long Price
        {
            get { return m_record.Price; }
            set { m_record.Price = value; }
        }
        public int MountPlace
        {
            get { return m_record.MountPlace; }
        }
        public int ItemPlace
        {
            get { return m_record.ItemPlace; }
        }
        public GuildInstance Guild
        {
            get
            {
                if (m_guild == null || m_guild.Id != GuildId)
                    m_guild = GuildManager.Instance.GetGuild(GuildId);
                return m_guild;
            }
        }

        public bool OnSale => GuildId == -2;
        public bool Public => GuildId == -1;

        private GuildInstance m_guild;
        private PaddockDAO m_record;
                  
        public Paddock(PaddockDAO record)
        {
            m_record = record;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void SendInformations(AbstractEntity entity)
        {
            var guildName = string.Empty;
            var guildEmblem = string.Empty;
            if(Guild != null)
            {
                guildName = Guild.Name;
                guildEmblem = Guild.Emblem;
            }
            // Map -2 to 0
            var guildId = GuildId == -2 ? 0 : GuildId;
            entity.Dispatch(WorldMessage.PADDOCK_INFORMATIONS(guildId, Price, MountPlace, ItemPlace, guildName, guildEmblem));
        }
    }
}
