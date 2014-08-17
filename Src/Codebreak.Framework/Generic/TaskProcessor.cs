using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Codebreak.Framework.Generic
{
    public abstract class TaskProcessor<T> : Singleton<T>
        where T : class, new()
    {
        public int UpdateInterval
        {
            get;
            private set;
        }

        public long LastUpdate
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public bool Running
        {
            get;
            private set;
        }

        private Stopwatch queueTimer;
        
        private LockFreeQueue<Action> messageQueue;
        private List<Updatable> updatableObjects;
        private List<Timer> timerList;

        protected TaskProcessor(string name, int updateInterval = 1)
        {
            UpdateInterval = updateInterval;
            Name = name;

            messageQueue = new LockFreeQueue<Action>();
            updatableObjects = new List<Updatable>();
            timerList = new List<Timer>();
            queueTimer = new Stopwatch();

            StartTaskProcessor();
        }


        public void StartTaskProcessor()
        {
            Running = true;
            queueTimer.Start();
            ThreadPool.QueueUserWorkItem(InternalUpdate);

            Logger.Info("TaskQueue[" + Name + "] started.");
        }

        public void StopTaskProcessor()
        {
            AddMessage(() =>
            {
                Running = false;
                queueTimer.Reset();
                LastUpdate = 0;

                Logger.Info("TaskQueue[" + Name + "] stopped.");
            });
        }

        public void AddMessage(Action message)
        {
            messageQueue.Enqueue(message);
        }

        public void AddUpdatable(Updatable updatable)
        {
            AddMessage(() => updatableObjects.Add(updatable));
        }

        public void RemoveUpdatable(Updatable updatable)
        {
            AddMessage(() => updatableObjects.Remove(updatable));
        }

        public void AddTimer(int delay, Action callback, bool oneshot = false)
        {
            AddMessage(() => timerList.Add(new Timer(delay, callback, oneshot)));
        }

        public void AddTimer(Timer timer)
        {
            AddMessage(() => timerList.Add(timer));
        }

        public void RemoveTimer(Timer timer)
        {
            AddMessage(() => timerList.Remove(timer));
        }

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
