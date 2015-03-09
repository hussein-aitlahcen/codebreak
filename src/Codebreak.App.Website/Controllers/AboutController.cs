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
        public ActionResult Team()
        {
            return View();
        }

        [ChildActionOnly]
        [OutputCache(Duration = GENERIC_CACHE_DURATION)]
        public ActionResult TeamContent()
        {
            return PartialView();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [ChildActionOnly]
        [OutputCache(Duration = GENERIC_CACHE_DURATION)]
        public ActionResult ContactContent()
        {
            return PartialView();
        }
    }
}