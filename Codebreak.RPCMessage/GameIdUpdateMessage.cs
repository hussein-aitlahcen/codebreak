using Codebreak.RPCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.RPCMessage
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
