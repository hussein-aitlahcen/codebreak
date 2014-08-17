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
    public abstract class TaskProcessor<T> : Singleton<T>
        where T : class, new()
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
        public long LastUpdate
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
        private LockFreeQueue<Action> messageQueue;
        private List<Updatable> updatableObjects;
        private List<Timer> timerList;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateInterval"></param>
        public TaskProcessor(string name, int updateInterval = 1)
        {
            UpdateInterval = updateInterval;
            Name = name;

            messageQueue = new LockFreeQueue<Action>();
            updatableObjects = new List<Updatable>();
            timerList = new List<Timer>();
            queueTimer = new Stopwatch();

            Start();
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
            AddMessage(() =>
            {
                Running = false;
                queueTimer.Reset();
                LastUpdate = 0;

                Logger.Info("TaskQueue[" + Name + "] stopped.");
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(Action message)
        {
            messageQueue.Enqueue(message);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatable"></param>
        public void AddUpdatable(Updatable updatable)
        {
            AddMessage(() =>
            {
                updatableObjects.Add(updatable);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatable"></param>
        public void RemoveUpdatable(Updatable updatable)
        {
            AddMessage(() =>
            {
                updatableObjects.Remove(updatable);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="callback"></param>
        /// <param name="oneshot"></param>
        public void AddTimer(int delay, Action callback, bool oneshot = false)
        {
            AddMessage(() =>
                {
                    timerList.Add(new Timer(delay, callback, oneshot));
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timer"></param>
        public void AddTimer(Timer timer)
        {
            AddMessage(() =>
                {
                    timerList.Add(timer);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timer"></param>
        public void RemoveTimer(Timer timer)
        {
            AddMessage(() =>
                {
                    timerList.Remove(timer);
                });
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
                var updateDelta = timeStart - LastUpdate;
                LastUpdate = timeStart;
                
                foreach(var timer in timerList)
                {
                    if((LastUpdate - timer.LastActivated) >= timer.Delay)
                    {
                        timer.Tick(LastUpdate);
                        if(timer.OneShot)
                        {
                            RemoveTimer(timer);
                        }
                    }
                }

                foreach (var updatableObject in updatableObjects)
                {
                    try
                    {
                        updatableObject.Update(updateDelta);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("TaskQueue[" + Name + "] failed to update UpdatableObject[" + GetType().Name + "] : " + ex.ToString());
                    }
                }

                Action msg = null;
                while (messageQueue.TryDequeue(out msg))
                {
                    try
                    {
                        msg();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("TaskQueue[" + Name + "] failed to process message: " + ex.ToString());
                    }
                }

                var timeStop = queueTimer.ElapsedMilliseconds;
                var updateTime = timeStop - timeStart;
                var updateLagged = updateTime >= UpdateInterval;

                if (!updateLagged)
                {
                    Thread.Sleep(1 + (int)(UpdateInterval - updateTime));
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
