using System;
using Codebreak.Framework.Command;
using Codebreak.Service.World.Database.Structures;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Database.Repository;
using Codebreak.Service.World.Game.Entity;

namespace Codebreak.Service.World.Commands
{
    public class CharacterCommand : Command<WorldCommandContext>
    {
        private readonly string[] _aliases =
        {
            "character"
        };

        public override string[] Aliases { get { return _aliases; } }

        public override string Description
        {
            get { return "Command that manages everything about the character connected";  }
        }

        protected override bool CanExecute(WorldCommandContext context)
        {
            return true;

        }

        public class ItemSubCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases =
            {
                "item"
            };

            public override string[] Aliases
            {
                get { return _aliases; }
            }

            public override string Description
            {
                get { return "Command to add an item"; }
            }

            protected override void Process(WorldCommandContext context)
            {
                int idTemplate;
                if (Int32.TryParse(context.TextCommandArgument.NextWord(), out idTemplate))
                {
                    Console.WriteLine(idTemplate);
                    var itemTemplate = ItemTemplateRepository.Instance.GetTemplate(idTemplate);
                    if (itemTemplate != null)
                    {
                        var instance = itemTemplate.CreateItem(1, ItemSlotEnum.SLOT_INVENTORY, true);
                        if (instance != null)
                        {
                            context.Entity.Inventory.AddItem(instance);
                            context.Entity.DispatchChatMessage(
                                ChatChannelEnum.CHANNEL_ADMIN,
                                String.Format("Item {0} - `{1}` added in your inventory", itemTemplate.Id, itemTemplate.Name)
                                );
                        }
                    }
                }
            }
        }
    }
}
