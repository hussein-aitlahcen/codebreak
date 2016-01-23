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
        void Initialize(SqlManager sqlManager);

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
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        void UpdateAll(MySqlConnection connection, MySqlTransaction transaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        void DeleteAll(MySqlConnection connection, MySqlTransaction transaction);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        void InsertAll(MySqlConnection connection, MySqlTransaction transaction);
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
        public SqlManager SqlMgr
        {
            get;
            private set;
        }

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
        /// <returns></returns>
        public IEnumerable<TDataObject> All
        {
            get
            {
                lock (m_syncLock)
                    return m_dataObjects.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TDataObject> UpdateObjects
        {
            get
            {
                var objects = new List<TDataObject>();
                lock (m_syncLock)
                {
                    foreach (var obj in m_dataObjects)
                    {
                        if (obj.IsDirty && !obj.IsNew && !obj.IsDeleted)
                        {
                            obj.OnBeforeUpdate();
                            obj.IsDirty = false;
                            objects.Add(obj);
                        }
                    }
                }
                return objects;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TDataObject> InsertObjects
        {
            get
            {
                var objects = new List<TDataObject>();
                lock (m_syncLock)
                {
                    foreach (var obj in m_dataObjects)
                    {
                        if (obj.IsNew && !obj.IsDeleted)
                        {                            
                            obj.OnBeforeInsert();
                            obj.IsNew = false;
                            obj.IsDirty = false;
                            objects.Add(obj);
                        }
                    }
                }
                return objects;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TDataObject> DeleteObjects
        {
            get
            {
                var objects = new List<TDataObject>();
                lock (m_syncLock)
                {
                    foreach (var obj in m_dataObjects)
                    {
                        if (obj.IsDeleted && !obj.IsNew)
                        {
                            obj.OnBeforeDelete();
                            obj.IsDeleted = false;
                            obj.IsDirty = false;
                            objects.Add(obj);
                        }
                    }
                }
                return objects;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<TDataObject> SelectAll
        {
            get
            {
                return SqlMgr.Query<TDataObject>("select * from " + TableName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool LoadOnly
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Repository(bool loadOnly = false)
        {
            m_dataObjects = new List<TDataObject>();

            LoadOnly = loadOnly;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize(SqlManager sqlMgr)
        {
            SqlMgr = sqlMgr;

            if (!LoadOnly)
            {
                IEnumerable<TDataObject> objects = SqlMgr.Query<TDataObject>("select * from " + TableName);
                m_dataObjects.AddRange(objects);
                foreach (var obj in m_dataObjects)
                    OnObjectAdded(obj);
                DataAccessObject<TDataObject>.IsRunning = true;
                DataAccessObject<TDataObject>.SqlMgr = SqlMgr;
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
        public virtual IEnumerable<TDataObject> LoadMultiple(string query, dynamic param = null)
        {
            IEnumerable<TDataObject> objects = SqlMgr.Query<TDataObject>("select * from " + TableName + " where " + query, (object)param);

            if (!LoadOnly)
            {
                lock (m_syncLock)
                {
                    foreach (var obj in objects)
                    {
                        m_dataObjects.Add(obj);
                        OnObjectAdded(obj);
                    }
                }
            }

            return objects;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objects"></param>
        public virtual void Update(MySqlConnection connection, MySqlTransaction transaction, IEnumerable<TDataObject> objects)
        {
            if (objects.Count() > 0)
                SqlMgr.Update<TDataObject>(connection, transaction, objects);
        }  
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual void Delete(MySqlConnection connection, MySqlTransaction transaction, IEnumerable<TDataObject> objects)
        {
            if (objects.Count() > 0)
                SqlMgr.Delete<TDataObject>(connection, transaction, objects);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual void Insert(MySqlConnection connection, MySqlTransaction transaction, IEnumerable<TDataObject> objects)
        {
            if (objects.Count() > 0)
                SqlMgr.InsertWithKey<TDataObject>(connection, transaction, objects);
        }     

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool Delete(TDataObject obj)
        {
            var result = SqlMgr.Delete<TDataObject>(obj);
            if (!LoadOnly)
            {
                if (result)
                {
                    lock (m_syncLock)
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
        public virtual void Removed(IEnumerable<TDataObject> objects)
        {
            lock (m_syncLock)
                foreach (TDataObject obj in objects)
                {
                    if (obj.IsNew)
                        m_dataObjects.Remove(obj);
                    OnObjectRemoved(obj);
                    obj.IsDeleted = true;
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual void Removed(TDataObject obj)
        {
            lock (m_syncLock)
            {
                if (obj.IsNew)
                    m_dataObjects.Remove(obj);
                OnObjectRemoved(obj);
                obj.IsDeleted = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual void Created(TDataObject obj)
        {
            lock (m_syncLock)
            {
                m_dataObjects.Add(obj);
                OnObjectAdded(obj);
                obj.IsNew = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool Insert(TDataObject obj)
        {
            var result = SqlMgr.InsertWithKey<TDataObject>(obj);
            if (!LoadOnly)
            {
                if (result)
                {
                    lock (m_syncLock)
                    {
                        m_dataObjects.Add(obj);
                        OnObjectAdded(obj);
                    }
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
        public virtual void UpdateAll(MySqlConnection connection, MySqlTransaction transaction)
        {
            Update(connection, transaction, UpdateObjects);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public virtual void DeleteAll(MySqlConnection connection, MySqlTransaction transaction)
        {
            Delete(connection, transaction, DeleteObjects);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public virtual void InsertAll(MySqlConnection connection, MySqlTransaction transaction)
        {
            Insert(connection, transaction, InsertObjects);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void ImplicitDeletion(TDataObject obj)
        {
            lock (m_syncLock)
                m_dataObjects.Remove(obj);
        }
    }
}
