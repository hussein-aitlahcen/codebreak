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
        private long _nextPartyId;
        private Dictionary<long, PartyInstance> _partyById;

        /// <summary>
        /// 
        /// </summary>
        public PartyManager()
        {
            _partyById = new Dictionary<long, PartyInstance>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void PartyMessage(long partyId, long entityId, string entityName, string message)
        {
            WorldService.Instance.AddMessage(() =>
                {
                    var party = GetParty(partyId);
                    if(party != null)                    
                        party.Dispatch(WorldMessage.CHAT_MESSAGE(ChatChannelEnum.CHANNEL_GROUP, entityId, entityName, message));                    
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        public void PartyLeave(CharacterEntity character)
        {
            if (_partyById.ContainsKey(character.PartyId))
                _partyById[character.PartyId].RemoveMember(character);
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateParty(CharacterEntity master, CharacterEntity member)
        {
            var party = new PartyInstance(_nextPartyId++, master, member);

            _partyById.Add(party.Id, party);

            WorldService.Instance.AddUpdatable(party);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partyId"></param>
        public void RemoveParty(PartyInstance instance)
        {
            _partyById.Remove(instance.Id);

            WorldService.Instance.RemoveUpdatable(instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partyId"></param>
        /// <returns></returns>
        public PartyInstance GetParty(long partyId)
        {
            if (_partyById.ContainsKey(partyId))
                return _partyById[partyId];
            return null;
        }
    }
}
