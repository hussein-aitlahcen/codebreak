using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StateUpdateMessage : AbstractRcpMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public override int Id
        {
            get 
            { 
                return (int)MessageIdEnum.WORLD_TO_AUTH_STATE_UPDATE; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public GameStateEnum State
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public StateUpdateMessage(GameStateEnum state)
        {
            State = state;
        }

        /// <summary>
        /// 
        /// </summary>
        public StateUpdateMessage()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Deserialize()
        {
            State = (GameStateEnum)base.ReadInt();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Serialize()
        {
            base.WriteInt((int)State);
        }
    }
}
