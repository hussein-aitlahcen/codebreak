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
    public abstract class Updatable : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(typeof(Updatable));

        /// <summary>
        /// 
        /// </summary>
        private LockFreeQueue<Action> m_messagesQueue;
        private List<Updatable> m_subUpdatableObjects;
        private List<UpdatableTimer> m_timerList;

        /// <summary>
        /// 
        /// </summary>
        public long UpdateTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int MessageCount
        {
            get
            {
                return m_messagesQueue.Count + m_subUpdatableObjects.Sum(updatable => updatable.MessageCount);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Updatable()
        {
            m_messagesQueue = new LockFreeQueue<Action>();
            m_subUpdatableObjects = new List<Updatable>();
            m_timerList = new List<UpdatableTimer>();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            foreach (var subUpdatableObj in m_subUpdatableObjects)
                subUpdatableObj.Dispose();
            m_subUpdatableObjects.Clear();
            m_messagesQueue.Clear();
            m_timerList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearMessages()
        {
            AddMessage(() =>
                {
                    m_messagesQueue.Clear();
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(Action message)
        {
            m_messagesQueue.Enqueue(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatable"></param>
        public void AddUpdatable(Updatable updatable)
        {
            AddMessage(() =>
                {
                    m_subUpdatableObjects.Add(updatable);
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
                m_subUpdatableObjects.Remove(updatable);
            });
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
        /// <param name="delay"></param>
        /// <param name="callback"></param>
        /// <param name="oneshot"></param>
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
        /// <param name="updateDelta"></param>
        public virtual void Update(long updateDelta)
        {
            UpdateTime += updateDelta;
            
            foreach (var timer in m_timerList)
            {
                if ((UpdateTime - timer.LastActivated) >= timer.Delay)
                {
                    try
                    {
                        timer.Tick(UpdateTime);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("TaskQueue[" + GetType().Name + "] timer update failed [" + timer.GetType().Name + "] : " + ex.ToString());
                    }
                    if (timer.OneShot)
                    {
                        RemoveTimer(timer);
                    }
                }
            }
            
            foreach (var updatableObject in m_subUpdatableObjects)
            {
                try
                {
                    updatableObject.Update(updateDelta);
                }
                catch (Exception ex)
                {
                    Logger.Error("UpdatableObject[" + GetType().Name + "] update failed : " + ex.ToString()); 
                }
            }

            Action msg = null;
            while (m_messagesQueue.TryDequeue(out msg))
            {
                try
                {
                    msg();
                }
                catch (Exception ex)
                {
                    Logger.Error("UpdatableObject[" + GetType().Name + "] message failed to process : " + ex.ToString());
                }
            }
        }
    }
}
