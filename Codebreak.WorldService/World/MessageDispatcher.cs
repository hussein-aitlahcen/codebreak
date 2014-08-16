using Codebreak.Framework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World
{
    public class MessageDispatcher : Updatable
    {
        private event Action<string> OnMessage;
        private bool _cached = false;
        private StringBuilder _cachedBuffer = null;
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
        public virtual void AddHandler(Action<string> method)
        {
            OnMessage += method;
        }

        public virtual void RemoveHandler(Action<string> method)
        {
            OnMessage -= method;
        }

        public virtual void Dispatch(string message)
        {
            if(CachedBuffer)
            {
                _cachedBuffer.Append(message + (char)0x00);
            }
            else if (OnMessage != null)
            {
                OnMessage(message);
            }
        }
    }
}
