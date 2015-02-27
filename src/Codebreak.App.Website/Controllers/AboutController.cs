using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebreak.App.Website.Controllers
{
    public class AboutController : WrappedController
    {
        public ActionResult Team()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}