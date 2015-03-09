using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebreak.App.Website.Controllers
{
    public abstract class BaseViewPage : WebViewPage
    {
        public virtual new AccountTicket User
        {
            get
            {
                return base.User as AccountTicket;
            }
        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual new AccountTicket User
        {
            get
            {
                return base.User as AccountTicket;
            }
        }
    }
}