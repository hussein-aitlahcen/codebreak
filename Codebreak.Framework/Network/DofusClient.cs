using Codebreak.Framework.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Network
{
    public abstract class DofusClient<T> : TcpClientBase<T>
        where T : DofusClient<T>, new()
    {
        private BinaryQueue _messageQueue;

        public FrameManager<T, string> FrameManager
        {
            get;
            private set;
        }

        public DofusClient()
        {
            _messageQueue = new BinaryQueue();

            FrameManager = new FrameManager<T, string>((T)this);
        }

        public IEnumerable<string> Receive(byte[] buffer, int offset, int count)
        {
            for (int i = offset; i < offset + count; i++)
            {
                if (buffer[i] == 0x00)
                {
                    yield return Encoding.Default.GetString(_messageQueue.ReadBytes(_messageQueue.Count));
                }
                else
                {
                    if (buffer[i] != '\n')
                    {
                        _messageQueue.WriteByte(buffer[i]);
                    }
                }
            }
        }

        public void Send(string message)
        {
            message = message + (char)0x00; // delimiter

            Logger.Debug("Server : " + message);

            base.Send(Encoding.Default.GetBytes(message));
        }
    }
}
