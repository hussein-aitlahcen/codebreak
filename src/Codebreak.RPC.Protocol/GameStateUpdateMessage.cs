using Codebreak.RPC.Service;

namespace Codebreak.RPC.Protocol
{
    public sealed class GameStateUpdateMessage : RPCMessageBase
    {
        public override int Id
        {
            get { return (int)MessageId.WORLD_TO_AUTH_GAMESTATEUPDATE; }
        }

        public GameState State
        {
            get;
            private set;
        }
        
        public GameStateUpdateMessage(GameState state)
        {
            State = state;
        }

        public GameStateUpdateMessage()
        {
        }

        public override void Deserialize()
        {
            State = (GameState)base.ReadInt();
        }

        public override void Serialize()
        {
            base.WriteInt((int)State);
        }
    }
}
