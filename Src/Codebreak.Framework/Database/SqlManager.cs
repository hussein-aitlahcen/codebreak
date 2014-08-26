using Codebreak.Framework.Generic;
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
    public sealed class SqlManager : Singleton<SqlManager>
    {
        /// <summary>
        /// 
        /// </summary>
        private SqlConnection _sqlConnection;
        
        /// <summary>
        /// 
        /// </summary>
        private SqlTransaction _sqlTransaction;

        /// <summary>
        /// 
        /// </summary>
        public static object SyncLock = new object();

        /// <summary>
        /// 
        /// </summary>
        public SqlConnection Connection
        {
            get
            {
                if (_sqlConnection == null)
                    throw new InvalidOperationException("SqlManager : not initialized.");
                return _sqlConnection;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public void Initialize(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);

            try
            {
                _sqlConnection.Open();

                Logger.Info("SqlManager connection opened : " + _sqlConnection.Database);
            }
            catch (Exception ex)
            {
                Logger.Error("SqlManager : Initialize connection faulted " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject"></param>
        public bool Insert<T>(T dataObject) where T : DataAccessObject<T>, new()
        {
            lock (SyncLock)
                return SqlMapperExtensions.Insert<T>(Connection, dataObject, _sqlTransaction) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public bool Remove<T>(T dataObject) where T : DataAccessObject<T>, new()
        {
            lock (SyncLock)
                return SqlMapperExtensions.Delete<T>(Connection, dataObject, _sqlTransaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject"></param>
        public bool Update<T>(T dataObject) where T : DataAccessObject<T>, new()
        {
            lock(SyncLock)
                return SqlMapperExtensions.Update<T>(Connection, dataObject, _sqlTransaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public void BeginTransaction()
        {
            if (_sqlTransaction != null)
                throw new InvalidOperationException("SqlManager : starting new transaction meanwhile last one is still alive.");
            _sqlTransaction = Connection.BeginTransaction();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CommitTransaction()
        {
            if (_sqlTransaction == null)
                throw new InvalidOperationException("SqlManager : trying to commit an unknow transaction.");
            _sqlTransaction.Commit();
            _sqlTransaction = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RollbackTransaction()
        {
            if (_sqlTransaction == null)
                throw new InvalidOperationException("SqlManager : trying to rollback an unknow transactio.");
            _sqlTransaction.Rollback();
            _sqlTransaction = null;
        }
    }
}
