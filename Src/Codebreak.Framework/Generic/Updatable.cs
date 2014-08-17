using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Generic
{
    public abstract class Updatable
    {
        /// <summary>
        /// 
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(typeof(TaskQueue));

        /// <summary>
        /// 
        /// </summary>
        private LockFreeQueue<Action> messageQueue = new LockFreeQueue<Action>();
        private List<Updatable> subUpdatableObject = new List<Updatable>();

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
        public void ClearMessages()
        {
            messageQueue.Clear();
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
                    subUpdatableObject.Add(updatable);
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
                subUpdatableObject.Remove(updatable);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateDelta"></param>
        public virtual void Update(long updateDelta)
        {
            UpdateTime += updateDelta;
            
            foreach (var updatableObject in subUpdatableObject)
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
            while (messageQueue.TryDequeue(out msg))
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
