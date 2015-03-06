using Codebreak.App.Website.Models.Authservice;
using Codebreak.App.Website.Models.Website;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebreak.App.Website.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    //[Compress]
    public abstract class WrappedController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        public static ILog Logger = LogManager.GetLogger(typeof(WrappedController));

        /// <summary>
        /// 
        /// </summary>
        public const int GENERIC_CACHE_DURATION = 60 * 60;

        /// <summary>
        /// 
        /// </summary>
        protected virtual new AccountTicket User
        {
            get
            {
                return base.User as AccountTicket;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetConfigurationVariable(string key)
        {
            return ConfigVariableRepository.Instance.GetValue
                (ControllerContext.RouteData.Values["Controller"] 
                + "_" + Request.RequestContext.RouteData.Values["Action"]
                + "_" + key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public string GetRedirectUrl(string returnUrl = "")
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return Url.Action("Index", "Home");
            return returnUrl;
        }
    }
}