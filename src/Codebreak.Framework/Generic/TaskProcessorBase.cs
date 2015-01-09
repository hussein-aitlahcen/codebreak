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
    public abstract class TaskProcessorBase
    {
        /// <summary>
        /// 
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(typeof(TaskProcessorBase));

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
        private Stopwatch _queueTimer;
        private LockFreeQueue<Action> _messageQueue;
        private List<Updatable> _updatableObjects;
        private List<UpdatableTimer> _timerList;
        private bool _running;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateInterval"></param>
        public TaskProcessorBase(string name, int updateInterval = 1)
        {
            UpdateInterval = updateInterval;
            Name = name;

            _running = false;
            _messageQueue = new LockFreeQueue<Action>();
            _updatableObjects = new List<Updatable>();
            _timerList = new List<UpdatableTimer>();
            _queueTimer = new Stopwatch();

            Start();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            _running = true;
            _queueTimer.Start();
            
            Task.Factory.StartNewDelayed(UpdateInterval, InternalStart);
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
        /// <param name="dispather"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public void AddLinkedMessages(params System.Action[] messages)
        {
            AddMessage(() =>
            {
                messages[0]();
                if (messages.Length > 1)
                    AddLinkedMessages(1, messages);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispather"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public void AddLinkedMessages(int index = 0, params System.Action[] messages)
        {
            AddMessage(() =>
            {
                messages[index]();
                if (messages.Length > ++index)
                    AddLinkedMessages(index, messages);
            });
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
                _timerList.Add(new UpdatableTimer(delay, callback, oneshot));
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timer"></param>
        public void AddTimer(UpdatableTimer timer)
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
        public void RemoveTimer(UpdatableTimer timer)
        {
            AddMessage(() =>
            {
                _timerList.Remove(timer);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        private void InternalStart()
        {
            Logger.Info("TaskQueue[" + Name + "] started.");

            Thread.CurrentThread.Name = Name;

            InternalUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InternalUpdate()
        {       
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
                    catch (Exception ex)
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
            var updateLagged = timeStop - timeStart > UpdateInterval;
            var nextDelay = 0;
            if (!updateLagged)
                nextDelay = (int)((timeStart + UpdateInterval) - timeStop);

            if (_running)            
                Task.Factory.StartNewDelayed(nextDelay, InternalUpdate);            
        }
    }
}
