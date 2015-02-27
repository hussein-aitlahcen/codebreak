using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebreak.App.Website.Controllers
{
    public class CommunityController : WrappedController
    {
        public ActionResult Forum()
        {
            return Redirect(GetConfigurationVariable("link"));
        }

        public ActionResult Ladder()
        {
            return View();
        }
    }
}