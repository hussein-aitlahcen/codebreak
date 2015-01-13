using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameTicketMessage : RPCMessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        public override int Id
        {
            get 
            {
                return (int)MessageIdEnum.AUTH_TO_WORLD_GAME_TICKET;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long AccountId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Pseudo
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Power
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long RemainingSubscription
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long LastConnectionDate
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string LastConnectionIP
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Ticket
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="name"></param>
        /// <param name="power"></param>
        /// <param name="remainingSub"></param>
        /// <param name="lastConnection"></param>
        /// <param name="lastIp"></param>
        /// <param name="ticket"></param>
        public GameTicketMessage(long accountId, string name, string pseudo, int power, long remainingSub, long lastConnection, string lastIp, string ticket)
        {
            AccountId = accountId;
            Name = name;
            Pseudo = pseudo;
            Power = power;
            RemainingSubscription = remainingSub;
            LastConnectionDate = lastConnection;
            LastConnectionIP = lastIp;
            Ticket = ticket;
        }

        /// <summary>
        /// 
        /// </summary>
        public GameTicketMessage()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Deserialize()
        {
            AccountId = base.ReadLong();
            Name = base.ReadString();
            Pseudo = base.ReadString();
            Power = base.ReadInt();
            RemainingSubscription = base.ReadLong();
            LastConnectionDate = base.ReadLong();
            LastConnectionIP = base.ReadString();
            Ticket = base.ReadString();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Serialize()
        {
            base.WriteLong(AccountId);
            base.WriteString(Name);
            base.WriteString(Pseudo);
            base.WriteInt(Power);
            base.WriteLong(RemainingSubscription);
            base.WriteLong(LastConnectionDate);
            base.WriteString(LastConnectionIP);
            base.WriteString(Ticket);
        }
    }
}
