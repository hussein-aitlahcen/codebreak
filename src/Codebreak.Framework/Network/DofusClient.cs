using Codebreak.Framework.IO;
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
    /// <typeparam name="T"></typeparam>
    public abstract class DofusClient<T> : TcpClientBase<T>
        where T : DofusClient<T>, new()
    {      
        /// <summary>
        /// 
        /// </summary>
        public FrameManager<T, string> FrameManager
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int CumulatedPacketInOneSecond
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long LastPacketTime
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private BinaryQueue m_messageQueue;

        /// <summary>
        /// 
        /// </summary>
        public DofusClient()
        {
            m_messageQueue = new BinaryQueue();
            FrameManager = new FrameManager<T, string>((T)this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<string> Receive(byte[] buffer, int offset, int count)
        {
            for (int i = offset; i < offset + count; i++)
            {
                if (buffer[i] == 0x00)
                {
                    if (Environment.TickCount - LastPacketTime < 1000)
                    {
                        CumulatedPacketInOneSecond++;
                        if (CumulatedPacketInOneSecond > 15)
                        {
                            Logger.Warn("Client kicked due to packet spam : " + Ip);
                            Disconnect();
                            break;
                        }
                    }
                    else
                    {
                        CumulatedPacketInOneSecond = 1;
                        LastPacketTime = Environment.TickCount;
                    }

                    yield return Encoding.UTF8.GetString(m_messageQueue.ReadBytes(m_messageQueue.Count));
                }
                else
                {
                    if (buffer[i] != '\n')
                    {
                        m_messageQueue.WriteByte(buffer[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public virtual void Send(string message)
        {
            message = message + (char)0x00; // delimiter

            Logger.Debug("Server : " + message);

            base.Send(Encoding.UTF8.GetBytes(message));
        }
    }
}
