using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameStateUpdateMessage : RPCMessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        public override int Id
        {
            get 
            { 
                return (int)MessageIdEnum.WORLD_TO_AUTH_GAMESTATEUPDATE; 
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
        public GameStateUpdateMessage(GameStateEnum state)
        {
            State = state;
        }

        /// <summary>
        /// 
        /// </summary>
        public GameStateUpdateMessage()
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
