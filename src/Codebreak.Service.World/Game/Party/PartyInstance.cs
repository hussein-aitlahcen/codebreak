using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Party
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PartyInstance : MessageDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public int MemberCount => m_memberById.Count;

        /// <summary>
        /// 
        /// </summary>
        private CharacterEntity m_leader;
        private Dictionary<long, CharacterEntity> m_memberById;
                        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="master"></param>
        /// <param name="member"></param>
        public PartyInstance(long id, CharacterEntity master, CharacterEntity member)
        {
            Id = id;
            m_memberById = new Dictionary<long, CharacterEntity>();
            m_leader = master;
            
            AddMember(master);
            AddMember(member);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="memberId"></param>
        public void KickMember(CharacterEntity member, long memberId)
        {
            // only the leader can kick someone
            if (member.Id != m_leader.Id || member.Id == memberId)
            {
                member.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            // exists ?
            if (!m_memberById.ContainsKey(memberId))
            {
                member.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            // get the fuck out
            RemoveMember(m_memberById[memberId], member.Id.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        public void AddMember(CharacterEntity member)
        {
            // new player just joined
            Dispatch(WorldMessage.PARTY_MEMBER_LIST(member));

            m_memberById.Add(member.Id, member);
            AddHandler(member.SafeDispatch);

            // set party and send members list
            member.PartyId = Id;
            member.SafeDispatch(WorldMessage.PARTY_CREATE_SUCCESS(m_leader.Name));
            member.SafeDispatch(WorldMessage.PARTY_SET_LEADER(m_leader.Id));
            member.SafeDispatch(WorldMessage.PARTY_MEMBER_LIST(m_memberById.Values.ToArray())); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="kickerId"></param>
        public void RemoveMember(CharacterEntity member, string kickerId = "")
        {
            if (m_memberById.ContainsKey(member.Id))
            {
                member.PartyId = -1;
                m_memberById.Remove(member.Id);
                RemoveHandler(member.SafeDispatch);

                Dispatch(WorldMessage.PARTY_MEMBER_LEFT(member.Id));

                member.SafeDispatch(WorldMessage.PARTY_LEAVE(kickerId));

                if (m_memberById.Count == 1)
                {
                    Dispose();
                }
                else if (member.Id == m_leader.Id)
                {
                    m_leader = m_memberById.First().Value;
                    Dispatch(WorldMessage.PARTY_SET_LEADER(m_leader.Id));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            Dispatch(WorldMessage.PARTY_LEAVE());

            foreach (var member in m_memberById.Values)
                member.PartyId = -1;
            
            m_memberById.Clear();
            m_memberById = null;
            m_leader = null;

            PartyManager.Instance.RemoveParty(this);

            base.Dispose();
        }    
    }
}
