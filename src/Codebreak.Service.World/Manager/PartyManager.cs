using Codebreak.Framework.Generic;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Party;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PartyManager : Singleton<PartyManager>
    {
        /// <summary>
        /// 
        /// </summary>
        private long m_nextPartyId;
        private Dictionary<long, PartyInstance> m_partyById;

        /// <summary>
        /// 
        /// </summary>
        public PartyManager()
        {
            m_partyById = new Dictionary<long, PartyInstance>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void PartyMessage(long partyId, string message)
        {
            WorldService.Instance.AddMessage(() =>
                {
                    var party = GetParty(partyId);
                    if(party != null)                    
                        party.Dispatch(message);                    
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        public void PartyLeave(CharacterEntity character)
        {
            if (m_partyById.ContainsKey(character.PartyId))
                m_partyById[character.PartyId].RemoveMember(character);
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateParty(CharacterEntity master, CharacterEntity member)
        {
            var party = new PartyInstance(m_nextPartyId++, master, member);

            m_partyById.Add(party.Id, party);

            WorldService.Instance.AddUpdatable(party);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partyId"></param>
        public void RemoveParty(PartyInstance instance)
        {
            m_partyById.Remove(instance.Id);

            WorldService.Instance.RemoveUpdatable(instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partyId"></param>
        /// <returns></returns>
        public PartyInstance GetParty(long partyId)
        {
            if (m_partyById.ContainsKey(partyId))
                return m_partyById[partyId];
            return null;
        }
    }
}
