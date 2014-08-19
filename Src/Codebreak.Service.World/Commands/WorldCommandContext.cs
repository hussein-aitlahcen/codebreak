using Codebreak.Framework.Command;
using Codebreak.Service.World.Game.Entity;

namespace Codebreak.Service.World.Commands
{
    public class WorldCommandContext : CommandContext
    {
        public WorldCommandContext(EntityBase entity, string line) : base(line)
        {
            Entity = entity;
        }

        public EntityBase Entity
        {
            get; 
            set;
        }
    }
}
