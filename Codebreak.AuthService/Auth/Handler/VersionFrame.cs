using Codebreak.Framework.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.AuthService.Auth.Handler
{
    public sealed class VersionFrame : FrameBase<VersionFrame, AuthClient, string>
    {
        public override Action<AuthClient, string> GetHandler(string message)
        {
            return HandleVersion;
        }

        private void HandleVersion(AuthClient client, string message)
        {
            client.FrameManager.RemoveFrame(VersionFrame.Instance);

            if(message != AuthConfig.CLIENT_VERSION)
            {
                client.Send(AuthMessage.PROTOCOL_REQUIRED());
                return;
            }

            client.FrameManager.AddFrame(AuthentificationFrame.Instance);
        }
    }
}
