using Codebreak.Framework.Database;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{    
    [Table("paddockinstance")]
    [ImplementPropertyChanged]
    public sealed class PaddockDAO : DataAccessObject<PaddockDAO>
    {
        [Key]
        public int MapId { get; set; }
        public int GuildId { get; set; }
        public long DefaultPrice { get; set; }
        public long Price { get; set; }
        public int MountPlace { get; set; }
        public int ItemPlace { get; set; }
    }
}
