using Codebreak.App.Website.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Codebreak.App.Website.Controllers
{
    public class CommunityController : WrappedController
    {
        private static object LadderLock = new object();

        public ActionResult Forum()
        {
            return RedirectPermanent(GetConfigurationVariable("link"));
        }

        public ActionResult Ladder()
        {
            return View();
        }

        [ChildActionOnly]
        [OutputCache(Duration = (int)(LadderManager.UPDATE_INTERVAL * 0.001) + 10)]
        public ActionResult LadderContent()
        {
            lock (LadderLock)
            {
                LadderManager.Instance.TryUpdate();
                ViewBag.Ladder = LadderManager.Instance.Entries;
                ViewBag.LastUpdate = LadderManager.Instance.LastUpdate.ToString("HH:mm:ss");
                ViewBag.NextUpdate = LadderManager.Instance.NextUpdate.ToString("HH:mm:ss");
            }

            return PartialView();
        }
    }
}