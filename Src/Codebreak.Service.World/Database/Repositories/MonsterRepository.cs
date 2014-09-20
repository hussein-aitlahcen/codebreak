using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MonsterRepository : Repository<MonsterRepository, MonsterDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, MonsterDAO> _monsterById;

        /// <summary>
        /// 
        /// </summary>
        public MonsterRepository()
        {
            _monsterById = new Dictionary<int, MonsterDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MonsterDAO GetById(int id)
        {
            if (_monsterById.ContainsKey(id))
                return _monsterById[id];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monster"></param>
        public override void OnObjectAdded(MonsterDAO monster)
        {
            _monsterById.Add(monster.Id, monster);
        }
    }
}
