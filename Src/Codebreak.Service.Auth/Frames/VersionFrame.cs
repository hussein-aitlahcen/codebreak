using System;
using Codebreak.Framework.Configuration;
using Codebreak.Framework.Network;
using Codebreak.Service.Auth.Network;

namespace Codebreak.Service.Auth.Frames
{
    public sealed class VersionFrame : FrameBase<VersionFrame, AuthClient, string>
    {
        [Configurable("ClientVersion")]
        public static string ClientVersion = "1.29.1";

        public override Action<AuthClient, string> GetHandler(string message)
        {
            return HandleVersion;
        }

        private void HandleVersion(AuthClient client, string message)
        {
            client.FrameManager.RemoveFrame(VersionFrame.Instance);

            if (message != ClientVersion)
            {
                client.Send(AuthMessage.PROTOCOL_REQUIRED());
                return;
            }

            client.FrameManager.AddFrame(AuthentificationFrame.Instance);
        }
    }
}
