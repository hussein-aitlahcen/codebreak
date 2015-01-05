using Codebreak.Framework.Command;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Command
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class HelpCommand : Command<WorldCommandContext>
    {
        private static readonly string[] m_aliases = { "help", "h" };

        public override string[] Aliases
        {
            get { return m_aliases; }
        }

        public override string Description
        {
            get { return "Lists the available commands."; }
        }

        protected override void Process(WorldCommandContext context)
        {
            StringBuilder message = new StringBuilder();
            foreach(var command in WorldService.Instance.CommandManager.Commands)  
                if(command.GetType().BaseType != typeof(SubCommand<WorldCommandContext>))
                    command.Serialize(message);
            context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE(message.ToString()));
        }
    }
}
