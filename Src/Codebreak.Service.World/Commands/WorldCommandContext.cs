using Codebreak.Framework.Command;
using Codebreak.Service.World.Game.Entity;

namespace Codebreak.Service.World.Commands
{
    public class WorldCommandContext : CommandContext
    {
        public WorldCommandContext(CharacterEntity character, string line) 
            : base(line)
        {
            Character = character;
        }

        public CharacterEntity Character
        {
            get; 
            set;
        }
    }
}
