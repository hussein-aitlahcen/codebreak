using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Command
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CommandContext
    {
        /// <summary>
        /// 
        /// </summary>
        public TextCommandArgument TextCommandArgument
        {
            get; 
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        protected CommandContext(string line)
        {
            TextCommandArgument = new TextCommandArgument(line);
        }
    }
}
