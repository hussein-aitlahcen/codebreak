using System;
using Codebreak.Framework.Command;
using Codebreak.Service.World.Database.Structures;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Database.Repository;
using Codebreak.Service.World.Game.Entity;

namespace Codebreak.Service.World.Commands
{
    public sealed class CharacterCommand : Command<WorldCommandContext>
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

        public sealed class LevelUpSubCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases =
            {
                "levelup"
            };

            public override string[] Aliases
            {
                get { return _aliases; }
            }

            public override string Description
            {
                get { return "Command to level up"; }
            }

            protected override void Process(WorldCommandContext context)
            {
                int level;
                if (Int32.TryParse(context.TextCommandArgument.NextWord(), out level))
                {
                    if (level > context.Entity.Level)
                    {
                        var character = context.Entity as CharacterEntity;
                        if (character != null)
                        {
                            while (level > character.Level)
                            {
                                character.LevelUp();
                            }
                            character.Dispatch(WorldMessage.CHARACTER_NEW_LEVEL(character.Level));
                            character.Dispatch(WorldMessage.SPELLS_LIST(character.Spells));
                            character.Dispatch(WorldMessage.ACCOUNT_STATS(character));
                            character.Dispatch(WorldMessage.CHAT_MESSAGE(
                                ChatChannelEnum.CHANNEL_ADMIN, 
                                0, 
                                "Server",
                                "Now you are stronger"
                                ));
                        }
                    }
                }
            }
        }

        public sealed class ItemSubCommand : SubCommand<WorldCommandContext>
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
                    var itemTemplate = ItemTemplateRepository.Instance.GetTemplate(idTemplate);
                    if (itemTemplate != null)
                    {
                        var instance = itemTemplate.CreateItem(1, ItemSlotEnum.SLOT_INVENTORY, true);
                        if (instance != null)
                        {
                            context.Entity.Inventory.AddItem(instance);
                            context.Entity.Dispatch(WorldMessage.CHAT_MESSAGE(
                                ChatChannelEnum.CHANNEL_ADMIN,
                                0,
                                "Server",
                                String.Format("Item {0} - `{1}` added in your inventory", itemTemplate.Id, itemTemplate.Name)
                                ));
                        }
                    }
                }
            }
        }
    }
}
