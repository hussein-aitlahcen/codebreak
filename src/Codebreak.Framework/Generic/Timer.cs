using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Generic
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UpdatableTimer
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(UpdatableTimer));

        /// <summary>
        /// 
        /// </summary>
        private readonly Action m_callback;

        /// <summary>
        /// 
        /// </summary>
        public long LastActivated
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Delay
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool OneShot
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="callback"></param>
        /// <param name="oneshot"></param>
        public UpdatableTimer(int delay, Action callback, bool oneshot = false)
        {
            Delay = delay;
            m_callback = callback;
            OneShot = oneshot;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentTime"></param>
        public void Tick(long currentTime)
        {
            try
            {
                m_callback();
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
