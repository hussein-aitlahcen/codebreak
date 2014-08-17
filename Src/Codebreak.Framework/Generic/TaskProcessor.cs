using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Codebreak.Framework.Generic
{
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
        public bool IsRunning
        {
            get
            {
                return _running;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPaused
        {
            get
            {
                return _paused;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public volatile bool Blocked;

        /// <summary>
        /// 
        /// </summary>
        private Stopwatch _queueTimer;
        private LockFreeQueue<Action> _messageQueue;
        private List<Updatable> _updatableObjects;
        private List<Timer> _timerList;
        private volatile bool _paused;
        private volatile bool _running;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateInterval"></param>
        public TaskProcessor(string name, int updateInterval = 5)
        {
            UpdateInterval = updateInterval;
            Name = name;

            _running = false;
            _paused = true;
            _messageQueue = new LockFreeQueue<Action>();
            _updatableObjects = new List<Updatable>();
            _timerList = new List<Timer>();
            _queueTimer = new Stopwatch();

            Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            _paused = false;
            _running = true;
            _queueTimer.Start();

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
                _running = false;
                _queueTimer.Reset();
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
            _messageQueue.Enqueue(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatable"></param>
        public void AddUpdatable(Updatable updatable)
        {
            AddMessage(() =>
            {
                _updatableObjects.Add(updatable);
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
                _updatableObjects.Remove(updatable);
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
                _timerList.Add(new Timer(delay, callback, oneshot));
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
                _timerList.Add(timer);
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
                _timerList.Remove(timer);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        private void InternalUpdate(object state)
        {
            if (!_running)
                return;

            if(Blocked)
            {
                _paused = true;
                _queueTimer.Stop();

                Logger.Warn("TaskQueue[" + Name + "] paused.");

                while (Blocked)
                    Thread.Sleep(1);

                Logger.Warn("TaskQueue[" + Name + "] resumed.");

                _paused = false;
                _queueTimer.Start();
            }

            var timeStart = _queueTimer.ElapsedMilliseconds;
            var updateDelta = timeStart - LastUpdate;
            LastUpdate = timeStart;

            foreach (var timer in _timerList)
            {
                if ((LastUpdate - timer.LastActivated) >= timer.Delay)
                {
                    try
                    {
                        timer.Tick(LastUpdate);
                    }
                    catch(Exception ex)
                    {
                        Logger.Error("TaskQueue[" + Name + "] failed to update timer [" + timer.GetType().Name + "] : " + ex.ToString());
                    }
                    if (timer.OneShot)
                    {
                        RemoveTimer(timer);
                    }
                }
            }

            foreach (var updatableObject in _updatableObjects)
            {
                try
                {
                    updatableObject.Update(updateDelta);
                }
                catch (Exception ex)
                {
                    Logger.Error("TaskQueue[" + Name + "] failed to update object [" + updatableObject.GetType().Name + "] : " + ex.ToString());
                }
            }

            Action msg = null;
            while (_messageQueue.TryDequeue(out msg))
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

            var timeStop = _queueTimer.ElapsedMilliseconds;
            var updateTime = timeStop - timeStart;
            var updateLagged = updateTime >= UpdateInterval;

            if (!updateLagged)
            {
                Thread.Sleep(1 + (int)(UpdateInterval - updateTime));
            }

            ThreadPool.QueueUserWorkItem(InternalUpdate);
        }
    }
}
