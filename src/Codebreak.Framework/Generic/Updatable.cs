using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Generic
{
    public abstract class Updatable : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(typeof(Updatable));

        /// <summary>
        /// 
        /// </summary>
        private LockFreeQueue<Action> _messagesQueue;
        private List<Updatable> _subUpdatableObjects;
        private List<UpdatableTimer> _timerList;

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
                return _messagesQueue.Count + _subUpdatableObjects.Sum(updatable => updatable.MessageCount);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Updatable()
        {
            _messagesQueue = new LockFreeQueue<Action>();
            _subUpdatableObjects = new List<Updatable>();
            _timerList = new List<UpdatableTimer>();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            foreach (var subUpdatableObj in _subUpdatableObjects)
                subUpdatableObj.Dispose();
            _subUpdatableObjects.Clear();
            _subUpdatableObjects = null;

            _messagesQueue.Clear();
            _messagesQueue = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearMessages()
        {
            AddMessage(() =>
                {
                    _messagesQueue.Clear();
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(Action message)
        {
            _messagesQueue.Enqueue(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatable"></param>
        public void AddUpdatable(Updatable updatable)
        {
            AddMessage(() =>
                {
                    _subUpdatableObjects.Add(updatable);
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
                _subUpdatableObjects.Remove(updatable);
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
        /// <param name="updateDelta"></param>
        public virtual void Update(long updateDelta)
        {
            UpdateTime += updateDelta;
            
            foreach (var timer in _timerList)
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

            foreach (var updatableObject in _subUpdatableObjects)
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
            while (_messagesQueue.TryDequeue(out msg))
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
