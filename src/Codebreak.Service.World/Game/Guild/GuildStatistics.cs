using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Game.Stats;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Guild
{
    /// <summary>
    /// 
    /// </summary>
    [ProtoContract(ImplicitFields=ImplicitFields.AllPublic)]
    public sealed class GuildStatistics
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static GuildStatistics Deserialize(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return Serializer.Deserialize<GuildStatistics>(stream);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stats"></param>
        /// <returns></returns>
        public byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize<GuildStatistics>(stream, this);

                return stream.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static GuildStatistics Create(GuildDAO guild)
        {
            return new GuildStatistics
            {
                Spells = GuildSpellBook.Create(),
                BaseStatistics = new GenericStats(guild),
                MaxTaxcollector = 1
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public GuildSpellBook Spells
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public GenericStats BaseStatistics
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxTaxcollector
        {
            get;
            set;
        }
    }
}
