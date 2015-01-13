using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class IdUpdateMessage : RPCMessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        public override int Id
        {
            get 
            {
                return (int)MessageIdEnum.WORLD_TO_AUTH_ID_UPDATE;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int GameId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameId"></param>
        public IdUpdateMessage(int gameId)
        {
            GameId = gameId;
        }

        /// <summary>
        /// 
        /// </summary>
        public IdUpdateMessage()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Deserialize()
        {
            GameId = base.ReadInt();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Serialize()
        {
            base.WriteInt(GameId);
        }
    }
}
