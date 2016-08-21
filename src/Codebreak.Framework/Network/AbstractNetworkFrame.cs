using Codebreak.Framework.Generic;
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
    /// <typeparam name="TMessage"></typeparam>
    public interface IFrame<TClient, TMessage>
    {
        bool Process(TClient client, TMessage message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TFrame"></typeparam>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public abstract class AbstractNetworkFrame<TFrame, TClient, TMessage> : Singleton<TFrame>, IFrame<TClient, TMessage>
        where TFrame : AbstractNetworkFrame<TFrame, TClient, TMessage>, new()
    {
        public abstract Action<TClient, TMessage> GetHandler(TMessage message);

        public bool Process(TClient client, TMessage message)
        {
            var handler = GetHandler(message);
            if(handler != null)
            {
                try
                {
                    handler(client, message);
                }
                catch(Exception ex)
                {
                    Logger.Error("Frame handler error :  " + ex.ToString());
                }
                return true;
            }
            return false;
        }
    }
}
