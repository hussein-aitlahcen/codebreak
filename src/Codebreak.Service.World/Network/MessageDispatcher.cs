using Codebreak.Framework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Network
{    
    /// <summary>
    /// 
    /// </summary>
    public class MessageDispatcher : Updatable
    {
        /// <summary>
        /// 
        /// </summary>
        private event Action<string> OnMessage;
        private int m_cached = 0;
        private StringBuilder m_buffer = new StringBuilder();
        
        /// <summary>
        /// 
        /// </summary>
        public bool CachedBuffer
        {
            get
            {
                return m_cached > 0;
            }
            set
            {
                m_cached += value ? 1 : -1;
                if (m_cached == 0)
                {
                    if (m_buffer.Length > 0)
                    {
                        Dispatch(m_buffer.ToString());
                        m_buffer.Clear();
                    }
                }
                if(m_cached < 0)
                    throw new InvalidOperationException("cached buffer should be >= 0");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            if (m_buffer != null)
                m_buffer.Clear();
            m_buffer = null;
            OnMessage = null;

            base.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public void AddHandler(Action<string> method)
        {
            OnMessage += method;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public virtual void SafeAddHandler(Action<string> method)
        {
            AddMessage(() =>
                {
                    OnMessage += method;
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public virtual void RemoveHandler(Action<string> method)
        {
            OnMessage -= method;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public virtual void SafeRemoveHandler(Action<string> method)
        {
            AddMessage(() =>
                {
                    OnMessage -= method;
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public virtual void Dispatch(string message)
        {
            if (CachedBuffer)
            {
                m_buffer.Append(message + (char)0x00);
            }
            else if (OnMessage != null)
            {
                OnMessage(message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public virtual void SafeDispatch(string message)
        {
            AddMessage(() =>
            {
                if (CachedBuffer)
                {
                    m_buffer.Append(message + (char)0x00);
                }
                else if (OnMessage != null)
                {
                    OnMessage(message);
                }
            });
        }
    }
}
