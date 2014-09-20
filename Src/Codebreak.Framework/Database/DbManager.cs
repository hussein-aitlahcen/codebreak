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
        private List<IRepository> _repositories;
   
        /// <summary>
        /// 
        /// </summary>
        public DbManager()
        {
            _repositories = new List<IRepository>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadAll(string connectionString)
        {
            SqlManager.Instance.Initialize(connectionString);

            foreach (var repository in _repositories)
            {
                Logger.Info(repository.GetType().Name + " : loading...");
                repository.Initialize();
                Logger.Info(repository.GetType().Name + " : " + repository.ObjectCount + " record loaded.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public void AddRepository(IRepository repository)
        {
            _repositories.Add(repository);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateAll()
        {
            foreach(var repository in _repositories)
            {
                repository.UpdateAll();
            }
        }
    }
}
