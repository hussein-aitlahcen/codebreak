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
    public sealed class NpcResponseRepository : Repository<NpcResponseRepository, NpcResponseDAO>
    {
        private Dictionary<int, NpcResponseDAO> m_reponseById;

        /// <summary>
        /// 
        /// </summary>
        public NpcResponseRepository()
        {
            m_reponseById = new Dictionary<int, NpcResponseDAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public override void OnObjectAdded(NpcResponseDAO response)
        {
            m_reponseById.Add(response.Id, response);

            base.OnObjectAdded(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="question"></param>
        public override void OnObjectRemoved(NpcResponseDAO response)
        {
            m_reponseById.Remove(response.Id);

            base.OnObjectRemoved(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reponseId"></param>
        /// <returns></returns>
        public NpcResponseDAO GetById(int reponseId)
        {
            return m_reponseById[reponseId];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public override void UpdateAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
            // NO UPDATE
        }
    }
}
