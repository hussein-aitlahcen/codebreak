using Codebreak.Framework.Database;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    public enum SocialRelationTypeEnum
    {
        TYPE_FRIEND = 0,
        TYPE_ENNEMY = 1,
    }

    /// <summary>
    /// 
    /// </summary>
    [Table("socialrelation")]
    public sealed class SocialRelationDAO : DataAccessObject<SocialRelationDAO>
    {
        [Key]
        public long AccountId
        {
            get;
            set;
        }

        [Key]
        public string Pseudo
        {
            get;
            set;
        }

        public int TypeId
        {
            get;
            set;
        }
        
        [Write(false)]
        [DoNotNotify]
        public SocialRelationTypeEnum Type
        {
            get
            {
                return (SocialRelationTypeEnum)TypeId;
            }
        }
    }
}
