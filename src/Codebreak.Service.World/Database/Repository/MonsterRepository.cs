using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MonsterRepository : Repository<MonsterRepository, MonsterDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, MonsterDAO> m_monsterById;

        /// <summary>
        /// 
        /// </summary>
        public MonsterRepository()
        {
            m_monsterById = new Dictionary<int, MonsterDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MonsterDAO GetById(int id)
        {
            if (m_monsterById.ContainsKey(id))
                return m_monsterById[id];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monster"></param>
        public override void OnObjectAdded(MonsterDAO monster)
        {
            m_monsterById.Add(monster.Id, monster);
        }
    }
}
