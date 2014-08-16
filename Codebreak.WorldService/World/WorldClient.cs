using Codebreak.Framework.Network;
using Codebreak.WorldService.World.Database.Structure;
using Codebreak.WorldService.World.Entity;
using Codebreak.WorldService.World.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World
{
    public sealed class WorldClient : DofusClient<WorldClient>
    {
        public AccountTicket Account
        {
            get;
            set;
        }

        public List<CharacterDAO> Characters
        {
            get;
            set;
        }

        private CharacterEntity _currentCharacter;
        public CharacterEntity CurrentCharacter
        {
            get
            {
                return _currentCharacter;
            }
            set
            {
                if (_currentCharacter != null)
                    _currentCharacter.RemoveHandler(Send);
                _currentCharacter = value;
                if(_currentCharacter != null)
                    _currentCharacter.AddHandler(Send);
            }
        }
    }
}
