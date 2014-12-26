using Codebreak.Framework.Network;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Manager;

namespace Codebreak.Service.World.Game
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
