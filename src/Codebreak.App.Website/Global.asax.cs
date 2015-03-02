using Codebreak.App.Website.Models.Authservice;
using Codebreak.App.Website.Models.Website;
using Codebreak.App.Website.Models.Worldservice;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Codebreak.App.Website
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            WebConfig.Instance.Initialize(Server);
            WebDbMgr.Instance.LoadAll(WebConfig.WEB_DB_CONNECTION_STRING);
            AuthDbMgr.Instance.LoadAll(WebConfig.AUTH_DB_CONNECTION_STRING);
            WorldDbMgr.Instance.LoadAll(WebConfig.WORLD_DB_CONNECTION_STRING);
        }
    }
}