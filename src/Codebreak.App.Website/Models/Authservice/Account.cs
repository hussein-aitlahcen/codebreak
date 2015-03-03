using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Codebreak.App.Website.Models.Authservice
{
    /// <summary>
    /// 
    /// </summary>
    [Table("account")]
    public sealed class Account : DataAccessObject<Account>
    { 
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Pseudo
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Power
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Question
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Response
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreationDate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastConnectionDate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string LastConnectionIP
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime RemainingSubscription
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Banned
        {
            get;
            set;
        }
    }
}