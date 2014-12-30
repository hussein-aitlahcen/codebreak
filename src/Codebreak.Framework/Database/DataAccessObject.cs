using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Database
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// 
    public abstract class DataAccessObject<T> : INotifyPropertyChanged
        where T : DataAccessObject<T>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public static bool IsRunning
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        [Write(false)]
        [DoNotNotify]
        public bool IsDirty
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        static DataAccessObject()
        {
            IsRunning = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public DataAccessObject()
        {
            IsDirty = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (IsRunning)            
                IsDirty = true;            
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnBeforeUpdate()
        {
        }
    }
}
