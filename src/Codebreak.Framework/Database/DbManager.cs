using Codebreak.Framework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Database
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DbManager<T> : Singleton<T>
        where T : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        private List<IRepository> m_repositories;
   
        /// <summary>
        /// 
        /// </summary>
        public DbManager()
        {
            m_repositories = new List<IRepository>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadAll(string connectionString)
        {
            SqlManager.Instance.Initialize(connectionString);

            try
            {
                foreach (var repository in m_repositories)
                {
                    Logger.Info(repository.GetType().Name + " : loading...");
                    repository.Initialize();
                    Logger.Info(repository.GetType().Name + " : " + repository.ObjectCount + " record loaded.");
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Logger.Error("Fatal error while loading database : " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public void AddRepository(IRepository repository)
        {
            m_repositories.Add(repository);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateAll()
        {
            foreach(var repository in m_repositories)
            {
                repository.UpdateAll();
            }
        }
    }
}
