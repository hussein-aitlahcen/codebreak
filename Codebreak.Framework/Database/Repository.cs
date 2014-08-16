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
    public interface IRepository
    {
        /// <summary>
        /// 
        /// </summary>
        void Initialize();

        /// <summary>
        /// 
        /// </summary>
        void UpdateAll();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Repository<TRepository, TDataObject> : Singleton<TRepository>, IRepository
        where TDataObject : DataAccessObject<TDataObject>, new()
        where TRepository : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        private static string TableName = SqlMapperExtensions.GetTableName(typeof(TDataObject));

        /// <summary>
        /// 
        /// </summary>
        protected List<TDataObject> _dataObjects;

        /// <summary>
        /// 
        /// </summary>
        public Repository()
        {
            _dataObjects = new List<TDataObject>();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize()
        {
            _dataObjects.AddRange(SqlManager.Instance.Connection.Query<TDataObject>("select * from " + TableName));

            foreach (var obj in _dataObjects)
                OnObjectAdded(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual TDataObject Load(string query, dynamic param = null)
        {
            return LoadMultiple(query, (object)param).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual List<TDataObject> LoadMultiple(string query, dynamic param = null)
        {
            var objects = SqlManager.Instance.Connection.Query<TDataObject>("select * from " + TableName + " where " + query, (object)param);
            foreach (var obj in objects)
            {
                if (obj != null)
                {
                    _dataObjects.Add(obj);

                    OnObjectAdded(obj);
                }
            }
            return objects.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Update(TDataObject obj)
        {
            return SqlManager.Instance.Connection.Update<TDataObject>(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool Remove(TDataObject obj)
        {
            var result = SqlManager.Instance.Remove<TDataObject>(obj);
            if (result)
            {
                _dataObjects.Remove(obj);

                OnObjectRemoved(obj);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool Insert(TDataObject obj)
        {
            var result = SqlManager.Instance.Insert<TDataObject>(obj);
            if (result)
            {
                _dataObjects.Add(obj);
                OnObjectAdded(obj);
            }
            return result;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TDataObject Find(Predicate<TDataObject> match)
        {
            return _dataObjects.Find(match);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="math"></param>
        /// <returns></returns>
        public IEnumerable<TDataObject> FindAll(Predicate<TDataObject> match)
        {
            return _dataObjects.FindAll(match);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TDataObject> GetAll()
        {
            return _dataObjects;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void UpdateAll()
        {
            foreach (var obj in _dataObjects)
            {
                Update(obj);
            }
        }

        public abstract void OnObjectAdded(TDataObject obj);
        public abstract void OnObjectRemoved(TDataObject obj);
    }
}
