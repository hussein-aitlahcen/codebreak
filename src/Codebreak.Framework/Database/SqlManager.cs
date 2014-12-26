using Codebreak.Framework.Generic;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

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
        private string _connectionString;

        /// <summary>
        /// 
        /// </summary>
        public MySqlConnection CreateConnection()
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public void Initialize(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string query, object param = null) where T : DataAccessObject<T>, new()
        {
            using (var connection = CreateConnection())
            {
                return connection.Query<T>(query, param);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject"></param>
        public bool Insert<T>(T dataObject) where T : DataAccessObject<T>, new()
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    return connection.Insert<T>(dataObject) != 0;
                }
                catch(Exception ex)
                {
                    Logger.Error("Fatal errror while inserting in database : " + ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObjects"></param>
        /// <returns></returns>
        public bool Insert<T>(IEnumerable<T> dataObjects) where T : DataAccessObject<T>, new()
        {
            using(var connection = CreateConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Insert<T>(dataObjects, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Logger.Error("Fatal errror while inserting in database : " + ex.Message);
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public bool Remove<T>(T dataObject) where T : DataAccessObject<T>, new()
        {
            using (var connection = CreateConnection())
            {
                return connection.Delete<T>(dataObject);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public bool Remove<T>(IEnumerable<T> dataObjects) where T : DataAccessObject<T>, new()
        {
            using (var connection = CreateConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Delete<T>(dataObjects, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Logger.Error("Fatal errror while deleting in database : " + ex.Message);
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObjects"></param>
        /// <returns></returns>
        public void Update<T>(IEnumerable<T> dataObjects) where T : DataAccessObject<T>, new()
        {
            using (var connection = CreateConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Update<T>(dataObjects, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Logger.Error("Fatal errror while updating database : " + ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject"></param>
        public bool Update<T>(T dataObject) where T : DataAccessObject<T>, new()
        {
            using (var connection = CreateConnection())
            {
                return connection.Update<T>(dataObject);
            }
        }       
    }
}
