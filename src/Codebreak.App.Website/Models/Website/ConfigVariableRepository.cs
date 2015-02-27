using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebreak.App.Website.Models.Website
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ConfigVariableRepository : Repository<ConfigVariableRepository, ConfigVariable>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, string> m_variables;

        /// <summary>
        /// 
        /// </summary>
        public ConfigVariableRepository()
        {
            m_variables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public override void OnObjectAdded(ConfigVariable variable)
        {
            m_variables.Add(variable.Key, variable.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public override void OnObjectRemoved(ConfigVariable variable)
        {
            m_variables.Remove(variable.Key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            if (!m_variables.ContainsKey(key))
                return "UNKNOW_VARIABLE";
            return m_variables[key];
        }
    }
}