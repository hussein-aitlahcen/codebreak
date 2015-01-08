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
        private bool m_cached = false;
        private StringBuilder m_buffer = null;
        
        /// <summary>
        /// 
        /// </summary>
        public bool CachedBuffer
        {
            get
            {
                return m_cached;
            }
            set
            {
                if (value != m_cached)
                {
                    m_cached = value;

                    if (m_cached)
                    {
                        if (m_buffer == null)
                            m_buffer = new StringBuilder();
                    }
                    else
                    {
                        Dispatch(m_buffer.ToString());
                        m_buffer.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            if (m_buffer != null)
                m_buffer.Clear();
            OnMessage = null;
            m_buffer = null;
            base.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public virtual void AddHandler(Action<string> method)
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
