using Codebreak.Service.World.Database.Structures;
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
            var instance = new GuildStatistics();
            instance.Spells = GuildSpellBook.Create();
            instance.BaseStatistics = new GenericStats(guild);
            instance.MaxTaxcollector = 1;
            return instance;
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
            private set;
        }
    }
}
