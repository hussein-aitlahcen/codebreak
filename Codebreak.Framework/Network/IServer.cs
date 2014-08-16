using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Network
{
    public interface IServer<TClient>
    {
        void Send(TClient client, byte[] data);
        void Disconnect(TClient client);
    }
}
