using System;
using Codebreak.Framework.Database;

namespace Codebreak.Service.Auth.Database.Structures
{
    [Table("Account")]
    public sealed class AccountDAO : DataAccessObject<AccountDAO>
    {
        [Key]
        public long Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Pseudo
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public int Power
        {
            get;
            set;
        }
        public DateTime CreationDate
        {
            get;
            set;
        }
        public DateTime LastConnectionDate
        {
            get;
            set;
        }
        public string LastConnectionIP
        {
            get;
            set;
        }
        public DateTime RemainingSubscription
        {
            get;
            set;
        }
        public bool Banned
        {
            get;
            set;
        }
    }
}
