using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Command
{
    public abstract class CommandContext
    {
        public TextCommandArgument TextCommandArgument
        {
            get; 
            private set;
        }

        protected CommandContext(string line)
        {
            TextCommandArgument = new TextCommandArgument(line);
        }
    }
}
