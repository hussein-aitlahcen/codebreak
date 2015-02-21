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
    public sealed class MonsterSpawnRepository : Repository<MonsterSpawnRepository, MonsterSpawnDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<MonsterSpawnDAO> GetById(SpawnTypeEnum type, int id)
        {
            return base.FindAll(spawn => spawn.Type == type && spawn.ZoneId == id);
        }


        public override void UpdateAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }

        public override void DeleteAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }

        public override void InsertAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }
    }
 
    /// <summary>
    /// 
    /// </summary>
    public sealed class MonsterSuperRaceRepository : Repository<MonsterSuperRaceRepository, MonsterSuperRaceDAO>
    {

        public override void UpdateAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }

        public override void DeleteAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }

        public override void InsertAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class MonsterRaceRepository : Repository<MonsterRaceRepository, MonsterRaceDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, MonsterRaceDAO> m_raceById;

        /// <summary>
        /// 
        /// </summary>
        public MonsterRaceRepository()
        {
            m_raceById = new Dictionary<int, MonsterRaceDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="race"></param>
        public override void OnObjectAdded(MonsterRaceDAO race)
        {
            m_raceById.Add(race.Id, race);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MonsterRaceDAO GetById(int id)
        {
            return m_raceById[id];
        }


        public override void UpdateAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }

        public override void DeleteAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }

        public override void InsertAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }
    }

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
        /// <param name="monster"></param>
        public override void OnObjectAdded(MonsterDAO monster)
        {
            m_monsterById.Add(monster.Id, monster);

            MonsterRaceRepository.Instance.GetById(monster.Race).Monsters.Add(monster);
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
        
        public override void UpdateAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }

        public override void DeleteAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }

        public override void InsertAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }
    }
}
