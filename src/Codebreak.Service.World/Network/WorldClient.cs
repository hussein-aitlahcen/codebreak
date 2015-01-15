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
        public bool Cypher
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
                    m_currentCharacter.KickEvent -= base.Disconnect;
                }
                m_currentCharacter = value;
                if (m_currentCharacter != null)
                {
                    m_currentCharacter.Ip = Ip;
                    m_currentCharacter.AddHandler(Send);
                    m_currentCharacter.KickEvent += base.Disconnect;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void Send(string message)
        {
            if (Cypher)
            {
                StringBuilder realMessage = new StringBuilder();
                foreach (var packet in message.Split(new char[] { (char)0x00 }))                
                    realMessage.Append(Util.PrepareData(packet)).Append((char)0x00);                
                message = realMessage.ToString();
            }
            base.Send(message);
        }
    }
}
