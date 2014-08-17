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
        private List<IRepository> repositories = new List<IRepository>();
   
        /// <summary>
        /// 
        /// </summary>
        public void LoadAll(string connectionString)
        {
            SqlManager.Instance.Initialize(connectionString);

            foreach (var repository in repositories)
            {               
                Logger.Info("Database loading : " + repository.GetType().Name);
                repository.Initialize();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public void AddRepository(IRepository repository)
        {
            repositories.Add(repository);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateAll()
        {
            foreach(var repository in repositories)
            {
                repository.UpdateAll();
            }
        }
    }
}
