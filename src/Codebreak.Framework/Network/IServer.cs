using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Network
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    public interface IServer<TClient>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="data"></param>
        void Send(TClient client, byte[] data);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        void Disconnect(TClient client);
    }
}
