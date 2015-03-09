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
        public ConfigVariableRepository()
            : base(true)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            var result = base.Load("upper(configvariable.key) = upper(@Key)", new { Key = key });
            if (result == null)
                return "UNKNOW_VARIABLE";
            return result.Value;
        }
    }
}