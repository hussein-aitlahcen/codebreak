using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{
    [Table("auctionhouseallowedtype")]
    public sealed class AuctionHouseAllowedTypeDAO : DataAccessObject<AuctionHouseAllowedTypeDAO>
    {
        [Key]
        public int AuctionHouseId
        {
            get;
            set;
        }

        [Key]
        public int TemplateId
        {
            get;
            set;
        }
    }
}
