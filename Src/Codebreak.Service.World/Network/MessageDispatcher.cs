using Codebreak.Framework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game
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
        private bool _cached = false;
        private StringBuilder _cachedBuffer = null;
        
        /// <summary>
        /// 
        /// </summary>
        public bool CachedBuffer
        {
            get
            {
                return _cached;
            }
            set
            {
                if (value != _cached)
                {
                    _cached = value;

                    if (_cached)
                    {
                        if (_cachedBuffer == null)
                            _cachedBuffer = new StringBuilder();
                    }
                    else
                    {
                        Dispatch(_cachedBuffer.ToString());
                        _cachedBuffer.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            if (_cachedBuffer != null)
                _cachedBuffer.Clear();
            OnMessage = null;
            _cachedBuffer = null;
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
                _cachedBuffer.Append(message + (char)0x00);
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
                    _cachedBuffer.Append(message + (char)0x00);
                }
                else if (OnMessage != null)
                {
                    OnMessage(message);
                }
            });
        }
    }
}
