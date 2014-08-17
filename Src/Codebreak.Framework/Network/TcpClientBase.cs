using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Network
{
    public abstract class TcpClientBase<T> 
        where T : TcpClientBase<T>, new()
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(T));

        public TcpClientBase()
        {
            Id = -1;
        }


        public int Id
        {
            get;
            set;
        }

        public Socket Socket
        {
            get;
            set;
        }

        public IServer<T> Server
        {
            get;
            set;
        }

        public string Ip
        {
            get;
            set;
        }

        public void Send(byte[] data)
        {
            Server.Send((T)this, data);
        }

        public void Disconnect()
        {
            Server.Disconnect((T)this);
        }
    }
}
