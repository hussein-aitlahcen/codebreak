using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Configuration
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ConfigurableAttribute : Attribute
    {
        public string Name
        {
            get; 
            private set;
        }

        public ConfigurableAttribute(string name)
        {
            Name = name;
        }
    }
}
