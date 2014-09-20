using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Manager;
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
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        private CharacterEntity _leader;
        private Dictionary<long, CharacterEntity> _memberById;

        /// <summary>
        /// 
        /// </summary>
        public int MemberCount
        {
            get
            {
                return _memberById.Count;
            }
        }
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="master"></param>
        /// <param name="member"></param>
        public PartyInstance(long id, CharacterEntity master, CharacterEntity member)
        {
            Id = id;
            _memberById = new Dictionary<long, CharacterEntity>();
            _leader = master;

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
            if (member.Id != _leader.Id || member.Id == memberId)
            {
                member.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            // exists ?
            if (!_memberById.ContainsKey(memberId))
            {
                member.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            // get the fuck out
            RemoveMember(_memberById[memberId], member.Id.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        public void AddMember(CharacterEntity member)
        {
            // new player just joined
            base.Dispatch(WorldMessage.PARTY_MEMBER_LIST(member));

            _memberById.Add(member.Id, member);
            AddHandler(member.SafeDispatch);

            // set party and send members list
            member.PartyId = Id;
            member.SafeDispatch(WorldMessage.PARTY_CREATE_SUCCESS(_leader.Name));
            member.SafeDispatch(WorldMessage.PARTY_SET_LEADER(_leader.Id));
            member.SafeDispatch(WorldMessage.PARTY_MEMBER_LIST(_memberById.Values.ToArray()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        public void RemoveMember(CharacterEntity member, string kickerId = "")
        {
            if (_memberById.ContainsKey(member.Id))
            {
                member.PartyId = -1;
                _memberById.Remove(member.Id);
                RemoveHandler(member.SafeDispatch);

                base.Dispatch(WorldMessage.PARTY_MEMBER_LEFT(member.Id));

                member.SafeDispatch(WorldMessage.PARTY_LEAVE(kickerId));

                if (_memberById.Count == 1)
                    Destroy();
                else if (member.Id == _leader.Id)
                {
                    _leader = _memberById.First().Value;

                    base.Dispatch(WorldMessage.PARTY_SET_LEADER(_leader.Id));
                }
            }
        }
    

        /// <summary>
        /// 
        /// </summary>
        public void Destroy()
        {
            base.Dispatch(WorldMessage.PARTY_LEAVE());

            foreach(var member in _memberById.Values)
            {
                member.PartyId = -1;
            }
                    
            _leader = null;
            _memberById.Clear();
            _memberById = null;

            PartyManager.Instance.RemoveParty(this);
        }
    }
}
