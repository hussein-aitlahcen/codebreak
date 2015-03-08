using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebreak.App.Website.Controllers
{
    [Authorize]
    public class ChatController : WrappedController
    {
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        //[OutputCache(Duration = GENERIC_CACHE_DURATION)]
        public ActionResult IndexContent()
        {
            return PartialView();
        }
    }
}