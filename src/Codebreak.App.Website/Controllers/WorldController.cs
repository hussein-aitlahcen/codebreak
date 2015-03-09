using Codebreak.App.Website.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebreak.App.Website.Controllers
{
    public class WorldController : WrappedController
    {
        public ActionResult UpdateConnected(int Id)
        {
            WorldManager.Instance.PlayersConnected = Id;
            Logger.Info("WorldController : players connected count updated  " + Id);

            return Content("");
        }
    }
}