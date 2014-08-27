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
        protected static ILog Logger = LogManager.GetLogger(typeof(TaskQueue));

        /// <summary>
        /// 
        /// </summary>
        private LockFreeQueue<Action> _messagesQueue = new LockFreeQueue<Action>();
        private List<Updatable> _subUpdatableObjects = new List<Updatable>();

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
        public virtual void Dispose()
        {
            foreach (var subUpdatableObj in _subUpdatableObjects)
                subUpdatableObj.Dispose();
            _subUpdatableObjects.Clear();

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
        /// <param name="updateDelta"></param>
        public virtual void Update(long updateDelta)
        {
            UpdateTime += updateDelta;
            
            foreach (var updatableObject in _subUpdatableObjects)
            {
                try
                {
                    updatableObject.Update(updateDelta);
                }
                catch (Exception ex)
                {
                    Logger.Error("UpdatableObject[" + GetType().Name + "] failed to update : " + ex.ToString()); 
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
                    Logger.Error("Updatable message failed to process : " + ex.ToString());
                }
            }
        }
    }
}
