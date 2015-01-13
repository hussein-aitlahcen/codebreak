using System;

namespace Codebreak.RPC.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum MessageIdEnum : int
    {
        WORLD_TO_AUTH_CREDENTIAL,
        AUTH_TO_WORLD_CREDENTIAL_RESULT,
        WORLD_TO_AUTH_ID_UPDATE,
        WORLD_TO_AUTH_STATE_UPDATE,
        AUTH_TO_WORLD_GAME_TICKET,
        WORLD_TO_AUTH_ACCOUNT_DISCONNECTED,
        WORLD_TO_AUTH_ACCOUNT_CONNECTED_LIST,
    }
}
