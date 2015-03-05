using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebreak.App.Website.Controllers
{
    public class LayoutController : WrappedController
    {
        [ChildActionOnly]
        [OutputCache(Duration = GENERIC_CACHE_DURATION)]
        public ActionResult Header()
        {
            return PartialView();
        }

        [ChildActionOnly]
        [OutputCache(Duration = GENERIC_CACHE_DURATION)]
        public ActionResult Footer()
        {
            return PartialView();
        }
    }
}