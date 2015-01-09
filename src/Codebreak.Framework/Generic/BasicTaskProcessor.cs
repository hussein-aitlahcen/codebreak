using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Generic
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BasicTaskProcessor : TaskProcessorBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="updateInterval"></param>
        public BasicTaskProcessor(string name, int updateInterval = 10)
            : base(name, updateInterval)
        {
        }
    }
}
