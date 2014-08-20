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
        int ObjectCount
        {
            get;
        }

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
        private static object _syncLock = new object();

        /// <summary>
        /// 
        /// </summary>
        protected List<TDataObject> _dataObjects;

        /// <summary>
        /// 
        /// </summary>
        public int ObjectCount
        {
            get
            {
                return _dataObjects.Count;
            }
        }

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
            IEnumerable<TDataObject> objects = new List<TDataObject>();

            lock (SqlManager.SyncLock)
                objects = SqlManager.Instance.Connection.Query<TDataObject>("select * from " + TableName);

            lock (_syncLock)
            {
                _dataObjects.AddRange(objects);
                foreach (var obj in _dataObjects)
                        OnObjectAdded(obj);
            }
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
            IEnumerable<TDataObject> objects = new List<TDataObject>();

            lock(SqlManager.SyncLock)
                objects = SqlManager.Instance.Connection.Query<TDataObject>("select * from " + TableName + " where " + query, (object)param);

            foreach (var obj in objects)
            {
                if (obj != null)
                {
                    lock (_syncLock)
                    {
                        _dataObjects.Add(obj);
                        OnObjectAdded(obj);
                    }
                }
            }

            return objects.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Update(TDataObject obj)
        {
            lock(SqlManager.SyncLock)
                return SqlManager.Instance.Connection.Update<TDataObject>(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool Remove(TDataObject obj)
        {
            var result = false;
            lock (SqlManager.SyncLock)
                result = SqlManager.Instance.Remove<TDataObject>(obj);

            if (result)
            {
                lock (_syncLock)
                {
                    if (_dataObjects.Contains(obj))
                    {
                        _dataObjects.Remove(obj);
                        OnObjectRemoved(obj);
                    }
                }
            }
            return result;                   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool Insert(TDataObject obj)
        {
            var result = false;
            lock (SqlManager.Instance)            
                result = SqlManager.Instance.Insert<TDataObject>(obj);

            if (result)
            {
                lock (_syncLock)
                {
                    _dataObjects.Add(obj);
                    OnObjectAdded(obj);
                }
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
            lock(_syncLock)
                return _dataObjects.Find(match);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="math"></param>
        /// <returns></returns>
        public IEnumerable<TDataObject> FindAll(Predicate<TDataObject> match)
        {
            lock (_syncLock)
                return _dataObjects.FindAll(match);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TDataObject> GetAll()
        {
            lock (_syncLock)
                return _dataObjects.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void UpdateAll()
        {
            lock (_syncLock)
            {
                foreach (var obj in _dataObjects)
                {
                    Update(obj);
                }
            }
        }

        public virtual void OnObjectAdded(TDataObject obj)
        {
        }

        public virtual void OnObjectRemoved(TDataObject obj)
        {
        }
    }
}
