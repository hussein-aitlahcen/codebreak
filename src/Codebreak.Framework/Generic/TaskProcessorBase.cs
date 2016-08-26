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
                return m_running;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        private Stopwatch m_queueTimer;
        private LockFreeQueue<Action> m_messageQueue;
        private List<Updatable> m_updatableObjects;
        private List<UpdatableTimer> m_timerList;
        private bool m_running;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateInterval"></param>
        public TaskProcessorBase(string name, int updateInterval = 10)
        {
            UpdateInterval = updateInterval;
            Name = name;

            m_running = false;
            m_messageQueue = new LockFreeQueue<Action>();
            m_updatableObjects = new List<Updatable>();
            m_timerList = new List<UpdatableTimer>();
            m_queueTimer = new Stopwatch();

            Start();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            m_running = true;
            m_queueTimer.Start();
            
            Task.Factory.StartNewDelayed(UpdateInterval, InternalStart);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            AddMessage(() =>
            {
                m_running = false;
                m_queueTimer.Reset();
                LastUpdate = 0;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(Action message)
        {
            m_messageQueue.Enqueue(message);
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
                m_updatableObjects.Add(updatable);
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
                m_updatableObjects.Remove(updatable);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="callback"></param>
        public UpdatableTimer AddTimer(int delay, Action callback, bool oneshot = false)
        {
            var timer = new UpdatableTimer(delay, callback, oneshot);
            AddTimer(timer);
            return timer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timer"></param>
        public void AddTimer(UpdatableTimer timer)
        {
            AddMessage(() =>
            {
                timer.LastActivated = LastUpdate;
                m_timerList.Add(timer);
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
                m_timerList.Remove(timer);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        private void InternalStart()
        {            
            InternalUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InternalUpdate()
        {
            var timeStart = m_queueTimer.ElapsedMilliseconds;
            var updateDelta = timeStart - LastUpdate;
            LastUpdate = timeStart;

            foreach (var timer in m_timerList)
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

            foreach (var updatableObject in m_updatableObjects)
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
            while (m_messageQueue.TryDequeue(out msg))
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

            var timeStop = m_queueTimer.ElapsedMilliseconds;
            var updateLagged = timeStop - timeStart > UpdateInterval;
            var nextDelay = 0;
            if (!updateLagged)
                nextDelay = (int)((timeStart + UpdateInterval) - timeStop);

            if (m_running)            
                Task.Factory.StartNewDelayed(nextDelay, InternalUpdate);            
        }
    }
}
