using System;
using Codebreak.Framework.Configuration;
using Codebreak.Framework.Network;
using Codebreak.Service.Auth.Network;

namespace Codebreak.Service.Auth.Frames
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class VersionFrame : AbstractNetworkFrame<VersionFrame, AuthClient, string>
    {
        /// <summary>
        /// 
        /// </summary>
        [Configurable("ClientVersion")]
        public static string ClientVersion = "1.29.1";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Action<AuthClient, string> GetHandler(string message)
        {
            return HandleVersion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
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
