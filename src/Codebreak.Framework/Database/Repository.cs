using Codebreak.Framework.Generic;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        void UpdateAll(MySqlConnection connection, MySqlTransaction transaction);
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
        protected object m_syncLock = new object();

        /// <summary>
        /// 
        /// </summary>
        protected List<TDataObject> m_dataObjects;

        /// <summary>
        /// 
        /// </summary>
        public int ObjectCount
        {
            get
            {
                return m_dataObjects.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Repository()
        {
            m_dataObjects = new List<TDataObject>();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize()
        {
            IEnumerable<TDataObject> objects = SqlManager.Instance.Query<TDataObject>("select * from " + TableName);

            lock (m_syncLock)
            {
                m_dataObjects.AddRange(objects);
                foreach (var obj in m_dataObjects)
                        OnObjectAdded(obj);
            }

            DataAccessObject<TDataObject>.IsRunning = true;
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
        public virtual IEnumerable<TDataObject> LoadMultiple(string query, dynamic param = null)
        {
            IEnumerable<TDataObject> objects = SqlManager.Instance.Query<TDataObject>("select * from " + TableName + " where " + query, (object)param);

            lock (m_syncLock)
            {
                foreach (var obj in objects)
                {
                    m_dataObjects.Add(obj);
                    OnObjectAdded(obj);
                }
            }

            return objects;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Update(TDataObject obj)
        {
            return SqlManager.Instance.Update<TDataObject>(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objects"></param>
        public virtual void Update(IEnumerable<TDataObject> objects)
        {
            if(objects.Count() > 0)
                SqlManager.Instance.Update<TDataObject>(objects);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objects"></param>
        public virtual void Update(MySqlConnection connection, MySqlTransaction transaction, IEnumerable<TDataObject> objects)
        {
            if (objects.Count() > 0)
                SqlManager.Instance.Update<TDataObject>(connection, transaction, objects);
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
                lock (m_syncLock)
                {
                    m_dataObjects.Remove(obj);
                    OnObjectRemoved(obj);
                }
            }
            return result;                   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool Remove(IEnumerable<TDataObject> objects)
        {
            var result = SqlManager.Instance.Remove<TDataObject>(objects);
            if (result)
            {
                lock (m_syncLock)
                {
                    foreach (var obj in objects)
                    {
                        m_dataObjects.Remove(obj);
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
            var result = SqlManager.Instance.Insert<TDataObject>(obj);
            if (result)
            {
                lock (m_syncLock)
                {
                    m_dataObjects.Add(obj);
                    OnObjectAdded(obj);
                }
            }
            return result;            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool InsertWithKey(TDataObject obj)
        {
            var result = SqlManager.Instance.InsertWithKey<TDataObject>(obj);
            if (result)
            {
                lock (m_syncLock)
                {
                    m_dataObjects.Add(obj);
                    OnObjectAdded(obj);
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool InsertWithKey(IEnumerable<TDataObject> objects)
        {
            var result = SqlManager.Instance.InsertWithKey<TDataObject>(objects);
            if (result)
            {
                lock (m_syncLock)
                {
                    m_dataObjects.AddRange(objects);
                    foreach (var obj in objects)
                        OnObjectAdded(obj);
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool Insert(IEnumerable<TDataObject> objects)
        {
            var result = SqlManager.Instance.Insert<TDataObject>(objects);
            if (result)
            {
                lock (m_syncLock)
                {
                    m_dataObjects.AddRange(objects);
                    foreach (var obj in objects)
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
            lock(m_syncLock)
                return m_dataObjects.Find(match);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="math"></param>
        /// <returns></returns>
        public IEnumerable<TDataObject> FindAll(Predicate<TDataObject> match)
        {
            lock (m_syncLock)
                return m_dataObjects.FindAll(match);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TDataObject> GetAll()
        {
            lock (m_syncLock)
                return m_dataObjects.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void UpdateAll()
        {          
            Update(GetDirtyObjects().ToList());            
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void UpdateAll(MySqlConnection connection, MySqlTransaction transaction)
        {
            Update(connection, transaction,GetDirtyObjects().ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerable<TDataObject> GetDirtyObjects()
        {
            lock (m_syncLock)
            {
                foreach (var obj in m_dataObjects)
                {
                    if (obj.IsDirty)
                    {
                        obj.OnBeforeUpdate();
                        obj.IsDirty = false;
                        yield return obj;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public virtual void OnObjectAdded(TDataObject obj)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public virtual void OnObjectRemoved(TDataObject obj)
        {
        }
    }
}
