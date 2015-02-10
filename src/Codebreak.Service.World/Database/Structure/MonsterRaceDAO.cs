using Codebreak.Framework.Database;
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
    [Table("monsterrace")]
    public sealed class MonsterRaceDAO : DataAccessObject<MonsterRaceDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int SuperRaceId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private List<MonsterDAO> m_monsters = new List<MonsterDAO>();

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        public List<MonsterDAO> Monsters
        {
            get
            {
                return m_monsters;
            }
        }
    }
}
