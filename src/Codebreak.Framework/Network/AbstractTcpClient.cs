using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Network
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractTcpClient<T> 
        where T : AbstractTcpClient<T>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(typeof(T));

        /// <summary>
        /// 
        /// </summary>
        public AbstractTcpClient()
        {
            Id = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Socket Socket
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IServer<T> Server
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Ip
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void Send(byte[] data)
        {
            Server.Send((T)this, data);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Disconnect()
        {
            Server.Disconnect((T)this);
        }
    }
}
