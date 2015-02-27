using Codebreak.App.Website.Models.Website;
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
    public abstract class WrappedController : Controller
    {
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
    }
}