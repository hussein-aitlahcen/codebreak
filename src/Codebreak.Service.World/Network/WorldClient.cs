using Codebreak.Framework.Network;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Manager;
using Codebreak.Framework.Util;

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
        private CharacterEntity m_currentCharacter;

        /// <summary>
        /// 
        /// </summary>
        public CharacterEntity CurrentCharacter
        {
            get
            {
                return m_currentCharacter;
            }
            set
            {
                if (m_currentCharacter != null)
                {
                    m_currentCharacter.RemoveHandler(Send);
                    m_currentCharacter.KickEvent -= Disconnect;
                }
                m_currentCharacter = value;
                if (m_currentCharacter != null)
                {
                    m_currentCharacter.Ip = Ip;
                    m_currentCharacter.AddHandler(Send);
                    m_currentCharacter.KickEvent += Disconnect;
                }
            }
        }
    }
}
