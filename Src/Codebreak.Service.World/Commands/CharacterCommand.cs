using System;
using Codebreak.Framework.Command;
using Codebreak.Service.World.Database.Structures;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Database.Repositories;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Manager;

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
            get { return "Command that manages everything about the connected character";  }
        }

        protected override bool CanExecute(WorldCommandContext context)
        {
            return true;
        }

        protected override void Process(WorldCommandContext context)
        {
            context.Character.Dispatch(WorldMessage.BASIC_NO_OPERATION()); // nothing to do
        }

        public sealed class GuildCreateCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "guild"  
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
                get { return "Command destined to create a new guild"; }
            }

            protected override bool CanExecute(WorldCommandContext context)
            {
                return base.CanExecute(context);
            }

            protected override void Process(WorldCommandContext context)
            {
                if (context.Character.CanGameAction(Game.Action.GameActionTypeEnum.GUILD_CREATE))
                {
                    context.Character.GuildCreationOpen();
                }
                else
                {
                    context.Character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_YOU_ARE_AWAY));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class SpawnMonsterCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "monster"
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
                    return "Spawn monster command that can only be used by admins.";
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
                context.Character.Map.SpawnMonsters(context.Character.CellId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class TeleportCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "teleport"
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
                    return "Teleport command that can only be used by admins.";
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
                int mapId;
                if(Int32.TryParse(context.TextCommandArgument.NextWord(), out mapId))
                {
                    var map = MapManager.Instance.GetById(mapId);
                    if (map != null)
                    {
                        var cellId = map.RandomTeleportCell;
                        if (cellId != -1)
                        {
                            if (context.Character.CanGameAction(Game.Action.GameActionTypeEnum.MAP_TELEPORT))
                            {
                                context.Character.Teleport(mapId, cellId);
                            }
                            else
                            {
                                context.Character.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_YOU_ARE_AWAY));
                            }
                        }
                        else
                        {
                            context.Character.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("No cell available to be teleported on"));
                        }
                    }
                    else
                    {
                        context.Character.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Unknow mapId"));
                    }
                }
                else
                {
                    context.Character.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Command format : .character teleport mapId"));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class LevelUpSubCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases =
            {
                "level"
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
                    if (level > context.Character.Level)
                    {
                        while (level > context.Character.Level)
                        {
                            context.Character.LevelUp();
                        }
                        context.Character.Dispatch(WorldMessage.CHARACTER_NEW_LEVEL(context.Character.Level));
                        context.Character.Dispatch(WorldMessage.SPELLS_LIST(context.Character.Spells));
                        context.Character.Dispatch(WorldMessage.ACCOUNT_STATS(context.Character));
                        context.Character.Dispatch(WorldMessage.SERVER_INFO_MESSAGE("You are now level " + level));
                    }
                    else
                    {
                        context.Character.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("New level should be higher than yours"));
                    }
                }
                else
                {
                    context.Character.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Command format : .character levelup level"));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
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
                            context.Character.Inventory.AddItem(instance);
                            context.Character.Dispatch(WorldMessage.SERVER_INFO_MESSAGE(
                                String.Format("Item {0} - `{1}` added in your inventory", itemTemplate.Id, itemTemplate.Name)
                                ));
                        }
                    }
                    else
                    {
                        context.Character.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Unknow templateId"));
                    }
                }
                else
                {
                    context.Character.Dispatch(WorldMessage.SERVER_ERROR_MESSAGE("Command format : .character item templateId"));
                }
            }
        }
    }
}
