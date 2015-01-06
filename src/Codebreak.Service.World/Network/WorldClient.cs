using Codebreak.Framework.Network;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Manager;

namespace Codebreak.Service.World.Network
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WorldClient : DofusClient<WorldClient>
    {
        /// <summary>
        /// 
        /// </summary>
        public AccountTicket Account
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CharacterDAO> Characters
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private CharacterEntity _currentCharacter;

        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity CurrentCharacter
        {
            get
            {
                return _currentCharacter;
            }
            set
            {
                if (_currentCharacter != null)
                {
                    _currentCharacter.RemoveHandler(Send);
                    _currentCharacter.KickEvent -= base.Disconnect;
                }
                _currentCharacter = value;
                if (_currentCharacter != null)
                {
                    _currentCharacter.AddHandler(Send);
                    _currentCharacter.KickEvent += base.Disconnect;
                }
            }
        }
    }
}
