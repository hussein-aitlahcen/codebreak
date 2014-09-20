using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Generic
{
    public sealed class UpdatableTimer
    {
        private static ILog Logger = LogManager.GetLogger(typeof(UpdatableTimer));

        private Action _callback;

        public long LastActivated
        {
            get;
            private set;
        }

        public int Delay
        {
            get;
            private set;
        }

        public bool OneShot
        {
            get;
            private set;
        }

        public UpdatableTimer(int delay, Action callback, bool oneshot = false)
        {
            Delay = delay;
            _callback = callback;
            OneShot = oneshot;
        }

        public void Tick(long currentTime)
        {
            try
            {
                _callback();
            }
            catch(Exception ex)
            {
                Logger.Error("Error while processing timer callback : " + ex.ToString());
            }
            finally
            {
                LastActivated = currentTime;
            }
        }
    }
}
