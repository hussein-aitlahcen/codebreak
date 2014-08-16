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
        private SqlConnection sqlConnection;
        
        /// <summary>
        /// 
        /// </summary>
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// 
        /// </summary>
        public SqlConnection Connection
        {
            get
            {
                if (sqlConnection == null)
                    throw new InvalidOperationException("SqlManager : not initialized.");
                return sqlConnection;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public void Initialize(string connectionString)
        {
            sqlConnection = new SqlConnection(connectionString);

            try
            {
                sqlConnection.Open();

                Logger.Info("SqlManager connection opened : " + sqlConnection.Database);
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
            return SqlMapperExtensions.Insert<T>(Connection, dataObject) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public bool Remove<T>(T dataObject) where T : DataAccessObject<T>, new()
        {
            return SqlMapperExtensions.Delete<T>(Connection, dataObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject"></param>
        public bool Update<T>(T dataObject) where T : DataAccessObject<T>, new()
        {
            return SqlMapperExtensions.Update<T>(Connection, dataObject);
        }

        /// <summary>
        /// 
        /// </summary>
        public void BeginTransaction()
        {
            if (sqlTransaction != null)
                throw new InvalidOperationException("SqlManager : starting new transaction meanwhile last one is still alive.");
            sqlTransaction = Connection.BeginTransaction();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CommitTransaction()
        {
            if (sqlTransaction == null)
                throw new InvalidOperationException("SqlManager : trying to commit unknow transaction.");
            sqlTransaction.Commit();
            sqlTransaction = null;
        }
    }
}
