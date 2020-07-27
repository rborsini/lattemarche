using LatteMarche.WebApi.App_Start;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace LatteMarche.WebApi.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private static ILog log = LogManager.GetLogger(typeof(ApiExceptionFilter));

        public override void OnException(HttpActionExecutedContext filterContext)
        {
            var routeData = filterContext.ActionContext.ControllerContext.RouteData;
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];

            var message = String.Format("{0}/{1} [UNHANDLED EXCEPTION]", controllerName, actionName);

            log.Error(message);
        }

    }
}