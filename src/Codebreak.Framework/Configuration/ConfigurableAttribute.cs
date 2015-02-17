using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ConfigurableAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get; 
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public ConfigurableAttribute(string name = "")
        {
            Name = name;
        }
    }
}
