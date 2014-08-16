using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Codebreak.Framework.Generic
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskQueue : Updatable
    {
        /// <summary>
        /// 
        /// </summary>
        public int UpdateInterval
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Running
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        private Stopwatch queueTimer;

        /// <summary>
        /// 
        /// </summary>
        private long lastUpdate;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateInterval"></param>
        public TaskQueue(string name, int updateInterval)
        {
            UpdateInterval = updateInterval;
            Name = name;

            queueTimer = new Stopwatch();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Running = true;
            queueTimer.Start();
            ThreadPool.QueueUserWorkItem(InternalUpdate);

            Logger.Info("TaskQueue[" + Name + "] started.");
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            Running = false;
            queueTimer.Stop();
            lastUpdate = 0;
            ClearMessages();

            Logger.Info("TaskQueue[" + Name + "] stopped.");
        }

        /// <summary>
        /// 
        /// </summary>
        private void InternalUpdate(object state)
        {
            if (!Running)
                return;

            try
            {
                var timeStart = queueTimer.ElapsedMilliseconds;
                var updateDelta = timeStart - lastUpdate;
                lastUpdate = timeStart;

                base.Update(updateDelta);

                var timeStop = queueTimer.ElapsedMilliseconds;
                var updateTime = timeStop - timeStart;
                var updateLagged = updateTime >= UpdateInterval;

                if (!updateLagged)
                {
                    Thread.Sleep((int)(UpdateInterval - updateTime));
                }

                ThreadPool.QueueUserWorkItem(InternalUpdate);
            }
            catch (Exception ex)
            {
                Logger.Error("TaskQueue[" + Name + "] unknow error : " + ex.ToString());
            }
        }
    }
}
