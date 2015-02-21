using System.Collections.Generic;
using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;

namespace Codebreak.Service.World.Database.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SuperAreaRepository : Repository<SuperAreaRepository, SuperAreaDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, SuperAreaDAO> m_superAreaById;

        /// <summary>
        /// 
        /// </summary>
        public SuperAreaRepository()
        {
            m_superAreaById = new Dictionary<int, SuperAreaDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="superArea"></param>
        public override void OnObjectAdded(SuperAreaDAO superArea)
        {
            m_superAreaById.Add(superArea.Id, superArea);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="superArea"></param>
        public override void OnObjectRemoved(SuperAreaDAO superArea)
        {
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
    public sealed class AreaRepository : Repository<AreaRepository, AreaDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, AreaDAO> m_areaById;

        /// <summary>
        /// 
        /// </summary>
        public AreaRepository()
        {
            m_areaById = new Dictionary<int, AreaDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="area"></param>
        public override void OnObjectAdded(AreaDAO area)
        {
            m_areaById.Add(area.Id, area);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="area"></param>
        public override void OnObjectRemoved(AreaDAO area)
        {
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
    public sealed class SubAreaRepository : Repository<SubAreaRepository, SubAreaDAO>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, SubAreaDAO> m_subAreaById;

        /// <summary>
        /// 
        /// </summary>
        public SubAreaRepository()
        {
            m_subAreaById = new Dictionary<int, SubAreaDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subArea"></param>
        public override void OnObjectAdded(SubAreaDAO subArea)
        {
			m_subAreaById.Add (subArea.Id, subArea);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subArea"></param>
        public override void OnObjectRemoved(SubAreaDAO subArea)
        {
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
