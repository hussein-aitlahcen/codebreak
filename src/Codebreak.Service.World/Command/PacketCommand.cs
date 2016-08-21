using Codebreak.Framework.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Command
{
    public sealed class PacketCommand : Command<WorldCommandContext>
    {
        private static readonly string[] m_aliases = { "packet" };

        public override string[] Aliases => m_aliases;

        public override string Description => "Packet management commands.";

        protected override void Process(WorldCommandContext context)
        {
            base.Process(context);
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class PacketSendCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "send"
            };

            public override string[] Aliases => _aliases;

            public override string Description => "Send a packet. Arguments : %rawString%";

            protected override bool CanExecute(WorldCommandContext context)
            {
                //if (context.Character.Power < 1)
                //{
                //    context.Character.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("You're not admin, your attempt was registered"));
                //    return false;
                //}

                return true;
            }

            protected override void Process(WorldCommandContext context)
            {
                context.Character.Dispatch(context.TextCommandArgument.NextWord());
            }
        }
    }
}
