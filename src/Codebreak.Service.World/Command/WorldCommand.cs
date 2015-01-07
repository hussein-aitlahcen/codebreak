using Codebreak.Framework.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Command
{
    public sealed class WorldCommand : Command<WorldCommandContext>
    {
        private static readonly string[] m_aliases = { "world", "w" };

        public override string[] Aliases
        {
            get { return m_aliases; }
        }

        public override string Description
        {
            get { return "World management commands."; }
        }

        protected override void Process(WorldCommandContext context)
        {
            base.Process(context);
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class SaveWorldCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "save"
            };

            public override string[] Aliases
            {
                get
                {
                    return _aliases;
                }
            }

            public override string Description
            {
                get
                {
                    return "Save the world.";
                }
            }

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
                WorldService.Instance.SaveWorld();
            }
        }
    }
}
