using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Codebreak.App.Website.Controllers
{
    public class HomeController : WrappedController
    {
        [OutputCache(Duration = GENERIC_CACHE_DURATION)]
        public ActionResult Index()
        {
            return View();
        }
    }
}
