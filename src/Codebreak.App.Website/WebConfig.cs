using Codebreak.Framework.Configuration;
using Codebreak.Framework.Configuration.Providers;
using Codebreak.Framework.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Codebreak.App.Website
{
    /// <summary>
    /// Configuration file, overrided by config.json load
    /// </summary>
    public sealed class WebConfig : Singleton<WebConfig>
    {
        /// <summary>
        /// Configuration file.
        /// </summary>
        public const string CONFIG_PATH = "~/App_Data/config.json";

        /// <summary>
        /// Website database connection string
        /// </summary>
        [Configurable]
        public static string WEB_DB_CONNECTION_STRING = "Database=codebreak_web;Server=localhost;Uid=root;Pwd=;";

        /// <summary>
        /// Authservice database connection string
        /// </summary>
        [Configurable]
        public static string AUTH_DB_CONNECTION_STRING = "";

        /// <summary>
        /// Worldservice database connection string
        /// </summary>
        [Configurable]
        public static string WORLD_DB_CONNECTION_STRING = "";

        /// <summary>
        /// Configuration manager.
        /// </summary>
        private ConfigurationManager m_configMgr;

        /// <summary>
        /// 
        /// </summary>
        public WebConfig()
        {
            m_configMgr = new ConfigurationManager();
            m_configMgr.RegisterAttributes();
        }

        /// <summary>
        /// Initialize configurations by loading the config.json file in ~/app_data/.
        /// </summary>
        public void Initialize(HttpServerUtility server)
        {
            m_configMgr.Add(new JsonConfigurationProvider(server.MapPath(CONFIG_PATH)), true);
            m_configMgr.Load();
        }
    }
}