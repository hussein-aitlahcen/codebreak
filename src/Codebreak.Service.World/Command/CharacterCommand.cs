using System;
using System.Text;
using Codebreak.Framework.Command;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.Game.Spell;

namespace Codebreak.Service.World.Command
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
            get { return "Character management commands.";  }
        }

        protected override bool CanExecute(WorldCommandContext context)
        {
            return true;
        }

        protected override void Process(WorldCommandContext context)
        {
            context.Character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
        }

        public sealed class SizeCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "size"  
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
                get { return "Change your character skin. Arguments : %skinId%"; }
            }

            protected override bool CanExecute(WorldCommandContext context)
            {
                return base.CanExecute(context);
            }

            protected override void Process(WorldCommandContext context)
            {
                int size = 0;
                if (Int32.TryParse(context.TextCommandArgument.NextWord(), out size))
                {
                    context.Character.SkinSizeBase = size;
                    context.Character.RefreshOnMap();
                }
                else
                {
                    context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Command format : character size %size%"));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class EffectCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "effect"  
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
                get { return "Apply a specified effect to your character. Arguments : %effectId"; }
            }

            protected override bool CanExecute(WorldCommandContext context)
            {
                return base.CanExecute(context);
            }

            protected override void Process(WorldCommandContext context)
            {
                int effectId = 0;
                if (!Int32.TryParse(context.TextCommandArgument.NextWord(), out effectId))
                {
                    context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Command format : character effect %effectId%"));
                    return;
                }

                ActionEffectManager.Instance.ApplyEffect(context.Character, (EffectEnum)effectId, null);
            }
        }

        public sealed class MorphCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "skin"  
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
                get { return "Modify the player skin."; }
            }

            protected override bool CanExecute(WorldCommandContext context)
            {
                return base.CanExecute(context);
            }

            protected override void Process(WorldCommandContext context)
            {
                int skinId = 0;
                if(Int32.TryParse(context.TextCommandArgument.NextWord(), out skinId))
                {
                    context.Character.SkinBase = skinId;
                    context.Character.RefreshOnMap();
                }
                else
                {
                    context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Command format : character morph %skinId%"));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class WarnCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "warn"  
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
                get { return "Warn a player. Arguments : %playerName%"; }
            }

            protected override bool CanExecute(WorldCommandContext context)
            {
                return base.CanExecute(context);
            }

            protected override void Process(WorldCommandContext context)
            {
                var characterName = context.TextCommandArgument.NextWord();
                var reason = context.TextCommandArgument.NextToEnd();

                WorldService.Instance.AddMessage(() =>
                {
                    var character = EntityManager.Instance.GetCharacterByName(characterName);
                    if (character == null)
                    {
                        context.Character.SafeDispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Player not found."));
                        return;
                    }

                    character.SafeDispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.INFO, InformationEnum.INFO_BASIC_WARNING_BEFORE_SANCTION, reason));
                    context.Character.SafeDispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Player warned."));
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class AlignmentResetCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "alignmentreset"  
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
                get { return "Reset your character alignment."; }
            }

            protected override bool CanExecute(WorldCommandContext context)
            {
                return base.CanExecute(context);
            }

            protected override void Process(WorldCommandContext context)
            {
                context.Character.ResetAlignment();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class AlignmentCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "alignment"  
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
                get { return "Set a new alignment to your character. Arguments : %alignmentId%"; }
            }

            protected override bool CanExecute(WorldCommandContext context)
            {
                return base.CanExecute(context);
            }

            protected override void Process(WorldCommandContext context)
            {
                var alignmentId = -1;
                if(!Int32.TryParse(context.TextCommandArgument.NextWord(), out alignmentId))
                {
                    context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Command format : character alignment %alignementId%"));
                    return;
                }
                context.Character.SetAlignment(alignmentId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class AddHonorCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "addhonor"  
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
                get { return "Will honor your character. Arguments : %honorValue%"; }
            }

            protected override bool CanExecute(WorldCommandContext context)
            {
                return base.CanExecute(context);
            }

            protected override void Process(WorldCommandContext context)
            {
                var honorValue = -1;
                if(!Int32.TryParse(context.TextCommandArgument.NextWord(), out honorValue))
                {
                    context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Command format : character addhonor %honorValue%"));
                    return;
                }

                context.Character.AddHonor(honorValue);                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class KickCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "kick"  
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
                get { return "Kick a player. Arguments : %playerName% %reason%"; }
            }

            protected override bool CanExecute(WorldCommandContext context)
            {
                return base.CanExecute(context);
            }

            protected override void Process(WorldCommandContext context)
            {
                var characterName = context.TextCommandArgument.NextWord();
                var reason = context.TextCommandArgument.NextToEnd();

                WorldService.Instance.AddMessage(() =>
                {
                    var character = EntityManager.Instance.GetCharacterByName(characterName);
                    if (character == null)
                    {
                        context.Character.SafeDispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Player not found."));
                        return;
                    }

                    if(character.Power >= context.Character.Power)
                    {
                        context.Character.SafeDispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("This player is a god, god cannot be kicked. In addition, he will be noticed."));
                        character.SafeDispatch(WorldMessage.SERVER_ERROR_MESSAGE("Player " + context.Character.Name + " tried to kick you."));
                        return;
                    }

                    character.SafeKick(context.Character.Name, reason);
                    context.Character.SafeDispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Player kicked successfully."));
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
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
                get { return "Open a guild creation panel"; }
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
                    context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Unable to start a guild creation in your actual state."));
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public sealed class TeleportMeCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "teleportme"
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
                    return "Teleport a player to your location. Arguments : %playerName%";
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
                string characterName = context.TextCommandArgument.NextWord();
                WorldService.Instance.AddMessage(() =>
                {
                    var character = EntityManager.Instance.GetCharacterByName(characterName);
                    if (character == null)
                    {
                        context.Character.SafeDispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Player not found."));
                        return;
                    }

                    var mapId = context.Character.MapId;
                    var cellId = context.Character.CellId;

                    character.AddMessage(() =>
                    {
                        if (!character.CanGameAction(Game.Action.GameActionTypeEnum.MAP_TELEPORT))
                        {
                            context.Character.SafeDispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Unable to teleport remote player due to his actual state."));
                            return;
                        }

                        character.Teleport(mapId, cellId);
                        context.Character.SafeDispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Player teleported successfully."));
                    });
                });         
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class TeleportToCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "teleportto"
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
                    return "Teleport yourself to a player location. Arguments : %playerName%";
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
                string characterName = context.TextCommandArgument.NextWord();                
                WorldService.Instance.AddMessage(() => 
                    {
                        var character = EntityManager.Instance.GetCharacterByName(characterName);
                        if(character == null)
                        {
                            context.Character.SafeDispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Player not found."));
                            return;
                        }

                        var mapId = character.MapId;
                        var cellId = character.CellId;

                        context.Character.AddMessage(() =>
                            {
                                if (!context.Character.CanGameAction(Game.Action.GameActionTypeEnum.MAP_TELEPORT))
                                {
                                    context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Unable to teleport yourself in your actual state."));
                                    return;
                                }

                                context.Character.Teleport(mapId, cellId);
                            });
                    });                
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
                    return "Teleport yourself at the desired location. Arguments : %mapId% %cellId%";
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
                        int cellId;
                        if (Int32.TryParse(context.TextCommandArgument.NextWord(), out cellId))
                        {
                            var cell = context.Character.Map.GetCell(cellId);
                            if (cell == null || !cell.Walkable)
                            {                                
                                context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Null cell or not walkable"));
                                return;
                            }

                            if (cellId != -1)
                            {
                                if (context.Character.CanGameAction(Game.Action.GameActionTypeEnum.MAP_TELEPORT))
                                {
                                    context.Character.Teleport(mapId, cellId);
                                }
                                else
                                {
                                    context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Unable to teleport yourself in your actual state."));
                                }
                            }
                            else
                            {
                                context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("No cell available to be teleported on"));
                            }
                        }
                        else
                        {
                            context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Command format : character teleport %mapId% %cellId%"));
                        }
                    }
                    else
                    {
                        context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Unknow mapId"));
                    }
                }
                else
                {
                    context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Command format : character teleport %mapId% %cellId%"));
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
                get { return "Levelup your character. Arguments : %level%"; }
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
                        context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("You are now level " + level));
                    }
                    else
                    {
                        context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("New level should be higher than yours"));
                    }
                }
                else
                {
                    context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Command format : character levelup %level%"));
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
                get { return "Add an item in your invotentory, with max jet. Arguments : %templateId%"; }
            }

            protected override void Process(WorldCommandContext context)
            {
                int idTemplate;
                if (Int32.TryParse(context.TextCommandArgument.NextWord(), out idTemplate))
                {
                    var itemTemplate = ItemTemplateRepository.Instance.GetById(idTemplate);
                    if (itemTemplate != null)
                    {
                        var instance = itemTemplate.Create(1, ItemSlotEnum.SLOT_INVENTORY, true);
                        if (instance != null)
                        {
                            context.Character.Inventory.AddItem(instance);
                            context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE(
                                String.Format("Item {0} added in your inventory", itemTemplate.Id, itemTemplate.Name)
                                ));
                        }
                    }
                    else
                    {
                        context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Unknow templateId"));
                    }
                }
                else
                {
                    context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Command format : character item %templateId%"));
                }
            }
        }
    }
}
