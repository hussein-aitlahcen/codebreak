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
    [Table("configvariable")]
    public sealed class ConfigVariable : DataAccessObject<ConfigVariable>
    {        
        [Key]
        public string Key
        {
            get;
            set;
        }
        
        public string Value
        {
            get;
            set;
        }
    }
}