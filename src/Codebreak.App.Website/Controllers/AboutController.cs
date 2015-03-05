using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Codebreak.App.Website.Controllers
{
    public class AboutController : WrappedController
    {
        [OutputCache(Duration = GENERIC_CACHE_DURATION)]
        public ActionResult Team()
        {
            return View();
        }

        [OutputCache(Duration = GENERIC_CACHE_DURATION)]
        public ActionResult Contact()
        {
            return View();
        }
    }
}