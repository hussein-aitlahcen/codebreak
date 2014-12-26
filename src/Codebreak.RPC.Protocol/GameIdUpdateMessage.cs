using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
{
    public sealed class GameIdUpdateMessage : RPCMessageBase
    {
        public override int Id
        {
            get { return (int)MessageId.WORLD_TO_AUTH_GAMEIDUPDATE; }
        }

        public int GameId
        {
            get;
            private set;
        }

        public GameIdUpdateMessage(int gameId)
        {
            GameId = gameId;
        }

        public GameIdUpdateMessage()
        {
        }

        public override void Deserialize()
        {
            GameId = base.ReadInt();
        }

        public override void Serialize()
        {
            base.WriteInt(GameId);
        }
    }
}
