using Codebreak.Framework.Command;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Fight;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
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
        public sealed class AddFightCellCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "addfightcell"
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
                    return "Add a fight cell to the map.";
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
                var team = int.Parse(context.TextCommandArgument.NextWord());
                var mapTemplate = MapTemplateRepository.Instance.GetById(context.Character.MapId);
                if(team == 0)
                    mapTemplate.FightTeam0Cells.Add(context.Character.CellId);
                else if(team == 1)
                    mapTemplate.FightTeam1Cells.Add(context.Character.CellId);

                context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("FightCell added."));  
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class AddFightActionCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "addfightaction"
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
                    return "Add a fight action to the map.";
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
                var nextMap = int.Parse(context.TextCommandArgument.NextWord());
                var nextCell = int.Parse(context.TextCommandArgument.NextWord());

                new FightActionDAO()
                {
                    ZoneType = (int)ZoneTypeEnum.TYPE_MAP,
                    ZoneId = context.Character.MapId,
                    FightType = (int)FightTypeEnum.TYPE_PVM,
                    FightState = (int)FightStateEnum.STATE_ENDED,
                    Conditions = "",
                    Actions = "2005:mapId=" + nextMap + ",cellId=" + nextCell
                }.Insert();

                context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("FightAction added."));  
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class AddStaticMonsterCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "addstaticmonster"
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
                    return "Add a monster spawn to the map.";
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
                var gradeId = int.Parse(context.TextCommandArgument.NextWord());
                new MonsterSpawnDAO()
                {
                    ZoneType = (int)ZoneTypeEnum.TYPE_MAP,
                    ZoneId = (int)context.Character.MapId,
                    GradeId = gradeId,
                    Probability = 1,
                }.Insert();

                context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("MonsterSpawn added."));  
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class SaveMapCommand : SubCommand<WorldCommandContext>
        {
            private readonly string[] _aliases = 
            {
                "savemap"
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
                    return "Save the map.";
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
                MapTemplateRepository.Instance.GetById(context.Character.MapId).Update();

                context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("Map saved."));  
            }
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
                
                context.Character.Dispatch(WorldMessage.BASIC_CONSOLE_MESSAGE("World saved."));                              
            }
        }
    }
}
